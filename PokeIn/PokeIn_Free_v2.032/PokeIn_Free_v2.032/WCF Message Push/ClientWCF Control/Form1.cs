/*
 * PokeIn ASP.NET Ajax Library - WCF Desktop Controller Sample
 * 
 * PokeIn 2010
 * http://pokein.com
 */
using System;
using System.Windows.Forms;
using System.ServiceModel.Description;
using System.ServiceModel;
using PokeIn;

namespace ClientWCF_Control
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        ServiceHost host;
        private void Form1_Load(object sender, EventArgs e)
        {
            host = new ServiceHost(typeof(PokeInWCF), new[] { new Uri("http://localhost:8090/") }); 
            ServiceMetadataBehavior behavior = new ServiceMetadataBehavior { HttpGetEnabled = true };

            host.Description.Behaviors.Add(behavior);
            host.AddServiceEndpoint(typeof(PokeInWCF), new BasicHttpBinding(), "PokeInWCF");
            host.AddServiceEndpoint(typeof(IMetadataExchange), new BasicHttpBinding(), "ENDX");
            host.Open();

            PokeInWCF.OnUserAdded = new UserAddedDelegate(UserAdded);
            PokeInWCF.OnUserRemoved = new UserRemovedDelegate(UserRemoved);
        }

        void UserAdded(string clientId)
        {
            listBox1.Items.Add(clientId);
        }

        void UserRemoved(string clientId)
        {
            listBox1.Items.Remove(clientId);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                host.Abort();
            }catch
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedItems.Count==0)
            {
                MessageBox.Show("You should select some clients to send a message");
                return;
            }
            if(textBox1.Text.Trim().Length == 0)
            {
                MessageBox.Show("You should enter a message to send");
                return;
            }
            string message = JSON.Method("ServerMessage", textBox1.Text);

            PokeInWCF.MessageFormat Mess = new PokeInWCF.MessageFormat();
            Mess.Clients = new string[listBox1.SelectedItems.Count];
            listBox1.SelectedItems.CopyTo(Mess.Clients, 0);
            Mess.Message = message;

            lock (PokeInWCF.Messages)
            {
                PokeInWCF.Messages.Add(Mess);
            }
        }
    }
}
