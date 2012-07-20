using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CopyFileService;
using MahApps.Metro.Controls;

namespace ClientTestTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            this.textBox1.TextChanged += textBox1_TextChanged;
            this.textBox3.PreviewTextInput += textBox3_PreviewTextInput;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.textBox2.AppendText("文件监听完成，请等待上传结束");
        }

        public void UpProcessBar()
        {
            currCount++;
            var precent = (currCount * 100) / recyCount;
            this.progressBar1.Value = precent;
            if (precent>=100)
            {
                MessageBox.Show("上传结束");
            }
        }
        void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.textBox1.LineCount == 0)
            {
                return;
            }
            var text = this.textBox1.GetLineText(this.textBox1.LineCount - 1);
            if (string.IsNullOrEmpty(text) || text == "\n")
            {
                return;
            }

            if (text.Contains("出错"))
            {
                this.textBox2.AppendText(text + System.Environment.NewLine);
            }
            this.textBox1.AppendText(System.Environment.NewLine);
            this.textBox1.ScrollToEnd();
        }
        private int recyCount = 0;
        private int currCount = 0;

        void textBox3_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !AreAllValidNumericChars(e.Text);
            base.OnPreviewTextInput(e);
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            dynamic arg = e.Argument;
            var raise = 100 / arg;
            for (int i = 1; i <= arg; i++)
            {
                if (worker.CancellationPending)
                {
                    break;
                }
                var thread = new Thread(CopyFile);

                thread.Start(i);
                Thread.Sleep(500);
            }
        }
        public void CopyFile(object count)
        {
            var cn = System.Environment.MachineName;
            var date = DateTime.Now;
            var newFileName = string.Format("{0}-{1}-{2}", cn, date.ToString("yyyyMMddHHmmss"), count.ToString());

            System.IO.File.Copy("test.pcl", "test\\" + newFileName + ".pcl");
            System.IO.File.Copy("test.pcl.xml", "test\\" + newFileName + ".pcl.xml");
        }
        private BackgroundWorker worker;

        FileWatch watch;
        private void StartService()
        {
            watch = new FileWatch(this.textBox1, this);
            watch.Start();
            recyCount = string.IsNullOrEmpty(this.textBox3.Text) ? 0 : int.Parse(textBox3.Text);

            worker.RunWorkerAsync(recyCount);
        }


        private bool AreAllValidNumericChars(string str)
        {
            foreach (char c in str)
            {
                if (!Char.IsNumber(c)) return false;
            }

            return true;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            StartService();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            watch.Stop();
            worker.CancelAsync();
        }
    }
}
