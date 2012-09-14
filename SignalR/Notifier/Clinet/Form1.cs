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
using Microsoft.Win32;
using System.Diagnostics;

namespace Clinet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var url = "http://192.168.20.50";
            //var url="http://192.168.20.34";
            //var url = "http://localhost";
            this.textBox2.Text = url;
            //this.textBox3.Text = "wangchunleird";
        }
        NotifierClient client = new NotifierClient();
        private void button1_Click(object sender, EventArgs e)
        {
            var url = this.textBox2.Text;

            bool result = client.StartListen("NotifierServer", url, CallBack);
            if (!result)
            {
                MessageBox.Show("连接失败，请查看日志");
            }
            client.connection.StateChanged += new Action<SignalR.Client.StateChange>(connection_StateChanged);
            this.button1.Enabled = false;
            this.label1.Text = text;
        }
        string text = "未连接";
        void connection_StateChanged(SignalR.Client.StateChange obj)
        {
            lock (client)
            {
                switch (obj.NewState)
                {
                    case SignalR.Client.ConnectionState.Connected:
                        text = "已连接";
                        break;
                    case SignalR.Client.ConnectionState.Connecting:
                        text = "连接中";
                        break;
                    case SignalR.Client.ConnectionState.Disconnected:
                        text = "已断开";
                        break;
                    case SignalR.Client.ConnectionState.Reconnecting:
                        text = "重接中";
                        break;
                    default:

                        break;
                }
                SetLabelText(text);
            }

        }

        private void SetLabelText(string text)
        {
            this.label1.Invoke((Action)(() => this.label1.Text = text));
        }

        private void CallBack(NotifierDTO dto)
        {
            textBox1.Invoke((Action)(() => textBox1.AppendText(string.Format(@"[{0}]-{1}:", dto.SendFromId, dto.Expiration.ToShortTimeString()) + dto.Tag)));

            textBox1.Invoke((Action)(() => textBox1.AppendText(Environment.NewLine)));

            textBox1.Invoke((Action)(() => textBox1.ScrollToCaret()));
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            client.Stop();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (client != null && client.hubProxy != null)
            {
                client.hubProxy.Invoke("ClinetToClient", this.textBox3.Text);

                this.textBox3.Clear();
            }
        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.button2_Click(null, null);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            client.Stop();
            this.button1.Enabled = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            const string name = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5";
            RegistryKey subKey = Registry.LocalMachine.OpenSubKey(name);
            var version = subKey.GetValue("Version").ToString();
            var servicePack = subKey.GetValue("SP").ToString();

            string ver = string.Format(".net framework:{0} -- {1}", version, servicePack == "1" ? "SP1" : "");
            MessageBox.Show(ver);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            var path = Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, @"logs\log.txt");
            var exist = System.IO.File.Exists(path);
            MessageBox.Show(exist.ToString());

            Process.Start(path);
        }
    }
}
