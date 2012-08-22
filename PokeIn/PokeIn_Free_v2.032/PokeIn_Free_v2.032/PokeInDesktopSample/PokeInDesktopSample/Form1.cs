using System;
using System.Drawing;
using System.Windows.Forms;
using PokeIn.Comet;
using SharedObjects;

namespace PokeInDesktopSample
{
    delegate void DConnectionChanged(bool active);
    delegate void DAddParam(string t);
    delegate void DUpdateString(DateTime t);
    delegate void DUpdateStatus(bool st);

    public partial class Form1 : Form
    {
        //test class to receive server calls
        DesktopTest123 test;

        //PokeIn desktop client class instance
        DesktopClient client;

        public Form1()
        {
            InitializeComponent();

            //We define our test class to pokein to let server side call the methods from this class
            test = new DesktopTest123(this);
            client = new DesktopClient(test, "http://localhost:7777/host.PokeIn", "");

            //DesktopClient events
            client.OnClientConnected += new OnConnection(client_OnClientConnected);
            client.OnClientDisconnected += new OnConnection(client_OnClientDisconnected);
            client.OnErrorReceived += new OnError(client_OnErrorReceived);
        }

        static void client_OnErrorReceived(DesktopClient c, string errorMessage)
        {
            MessageBox.Show(errorMessage);
        }

        void client_OnClientDisconnected(DesktopClient c)
        {
            //we call invoke to made change on Window due to cross-thread issues
            this.Invoke(new DConnectionChanged(ConnectionChanged), false); 
        }

        void client_OnClientConnected(DesktopClient c)
        {
            this.Invoke(new DConnectionChanged(ConnectionChanged), true); 
        }

        void ConnectionChanged(bool active)
        {
            if (active)
            {
                groupBox1.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = true;
            }
            else
            {
                groupBox1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = false;
                test.UnSubscribed();
            }
        }

        //Disconnect
        private void button3Click(object sender, EventArgs e)
        {
            client.Close();
        }

        //Connect
        private void button2Click(object sender, EventArgs e)
        {
            try
            {
                client.Connect();
            }
            catch
            {
                MessageBox.Show("Unable to connect to server. Make sure that you run WebServer solution.");
            }
        }

        //Subscribe
        private void button1Click(object sender, EventArgs e)
        {
            if(!test.subscribed)
                client.SendAsync("MyServer.Subscribe");//calls a server side method
            else
                client.SendAsync("MyServer.UnSubscribe");//calls a server side method
        }

        //Send TestClass to Server
        private void button4Click(object sender, EventArgs e)
        {
            client.SendAsync("MyServer.ClassTest", new TestClass(), DateTime.Now);//calls a server side method with a parameter
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Image im;
            client.GetImageResource("Logo", out im);
            pictureBox1.Image = (Image)im.Clone();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string str;
            client.GetTextResource("Message", out str);
            MessageBox.Show(@"Text resource from server is : " + str);
        }
    }  

}
