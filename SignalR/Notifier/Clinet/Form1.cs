using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Domas.DAP.ADF.NotifierClient;
using Domas.DAP.ADF.NotifierDeploy;

namespace Clinet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.textBox2.Text = "http://localhost";
            //this.textBox3.Text = "wangchunleird";
        }
        NotifierClient client = new NotifierClient();
        private void button1_Click(object sender, EventArgs e)
        {
            var url = this.textBox2.Text;

            client.StartListen("MyHub", url, Domas.DAP.ADF.Cookie.CookieManger.GetUriCookieContainer(new Uri(url)), CallBack);
            this.button1.Enabled = false;
        }
        private void CallBack(NotifierDTO dto)
        {
            textBox1.Invoke((Action)(() => textBox1.AppendText(string.Format("{0}:" + dto.MessageId, DateTime.Now.ToString()))));
            
            textBox1.Invoke((Action)(() => textBox1.AppendText(Environment.NewLine)));

            textBox1.Invoke((Action)(() => textBox1.ScrollToCaret()));
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            client.Stop();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var method = this.textBox3.Text;
        }
    }
}
