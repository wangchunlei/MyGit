using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PokeIn_Silverlight
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            HtmlPage.RegisterScriptableObject("Page", this);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.busyIndicator1.IsBusy = true;
            HtmlPage.Window.Eval("Dummy.ServerTime();");
        }

        [ScriptableMember()]
        public void Connected()
        {
            this.button1.IsEnabled = true;
            button2.IsEnabled = true;
            this.textBox1.Text = "Connected!";
        }

        [ScriptableMember()]
        public void TimeReceived(string time)
        {
            this.textBox1.Text = time;
            this.busyIndicator1.IsBusy = false;
        }

        private void image1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HtmlPage.Window.Navigate(new Uri("http://pokein.com"));
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (button2.Content.ToString() == "Disconnect")
            {
                button2.Content = "Connect";
                HtmlPage.Window.Eval("PokeIn.Close();");
                this.textBox1.Text = "Disconnected!";
            }
            else
            {
                button1.IsEnabled = false;
                button2.Content = "Disconnect";
                this.textBox1.Text = "Connecting...";
                button2.IsEnabled = false;
                HtmlPage.Window.Eval("PokeIn.ReConnect();");
            }
        }
    }
}
