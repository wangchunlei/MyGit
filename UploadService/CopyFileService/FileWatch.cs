using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using Lanxum.Domas.Extension.LogManager;
using System.Threading;

namespace CopyFileService
{

    public class FileWatch
    {
        private List<FileSystemWatcher> _FileSystemWatcherList = null;
        private ILogger log = null;
        private dynamic txtBox;
        private dynamic mainOwner;
        public FileWatch()
        {
            _FileSystemWatcherList = new List<FileSystemWatcher>();
        }
        public FileWatch(dynamic textbox, dynamic owner)
        {
            txtBox = textbox;
            mainOwner = owner;
            _FileSystemWatcherList = new List<FileSystemWatcher>();
        }
        /// <summary>
        /// 开始监控文件夹
        /// </summary>
        public void Start()
        {
            log = LogManager.GetLogger("FileWatch.Start()");
            log.Info("准备开始监控文件夹");
            txtBox.AppendText("准备开始监控文件夹" + System.Environment.NewLine);
            FileSystemWatcher watcher = new FileSystemWatcher();
            string emfFilePath = string.Empty;
            if (string.IsNullOrEmpty(emfFilePath))
            {
                emfFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "test");

                if (!Directory.Exists(emfFilePath))
                {
                    Directory.CreateDirectory(emfFilePath);
                }
                else
                {
                    Directory.GetFiles(emfFilePath).ToList().ForEach(c =>
                        { if (c != null) File.Delete(c); });
                }
            }
            log.Info("监控的文件夹为：" + emfFilePath);
            txtBox.AppendText("监控的文件夹为：" + emfFilePath + System.Environment.NewLine);
            watcher.Path = emfFilePath;
            //监控文件的上次访问、上次写入、文件名、文件目录、文件大小；
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size;
            //仅仅监控emf文件
            watcher.Filter = "*.xml";
            watcher.Created += new FileSystemEventHandler(watcher_Created);
            watcher.Deleted += new FileSystemEventHandler(watcher_Deleted);
            watcher.EnableRaisingEvents = true; //设置为true则触发删除和deleted事件；
        }

        void watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            log = LogManager.GetLogger("watcher_Deleted");
            string info = "PCL File" + e.FullPath + " Time:" + DateTime.Now + " " + e.ChangeType;
            log.Info(info);

            txtBox.Dispatcher.Invoke((Action)(() => txtBox.AppendText(info)));
        }

        void watcher_Created(object sender, FileSystemEventArgs e)
        {
            log = LogManager.GetLogger("监控文件(创建)开始");
            string info = "PCL File" + e.FullPath + " " + e.ChangeType;
            log.Info(info);
            txtBox.Dispatcher.Invoke((Action)(() => txtBox.AppendText(info)));
            string strPath = e.FullPath.Substring(0, e.FullPath.LastIndexOf(".xml"));
            info = "PCL XML File:" + strPath;
            log.Info(info);
            txtBox.Dispatcher.Invoke((Action)(() => txtBox.AppendText(info)));
            PCL pcl = new PCL();
            pcl.PCLDataPath = strPath;
            pcl.PCLXMLPath = e.FullPath;
            log.Info("准备启动进程处理PCL文件");

            txtBox.Dispatcher.Invoke((Action)(() => txtBox.AppendText("准备启动进程处理PCL文件")));
            Thread thread = new Thread(new ParameterizedThreadStart(ProcessPCL));
            thread.Start(pcl);
            log.Info("已经启动处理PCL文件进程");
            txtBox.Dispatcher.Invoke((Action)(() => txtBox.AppendText("已经启动处理PCL文件进程")));
        }
        public void ProcessPCL(object pcl)
        {
            AnayMeteData(pcl);
            log.Info("准备开始上传PCL文件");

            txtBox.Dispatcher.Invoke((Action)(() => txtBox.AppendText("准备开始上传PCL文件")));
            UploadFile(((PCL)pcl).PCLDataPath);
            string info = ((PCL)pcl).PCLDataPath + "PCL文件上传成功";
            log.Info(info);
            txtBox.Dispatcher.Invoke((Action)(() => txtBox.AppendText(info)));
            log.Info("准备开始上传PCL XML文件");

            txtBox.Dispatcher.Invoke((Action)(() => txtBox.AppendText("准备开始上传PCL XML文件")));

            UploadFile(((PCL)pcl).PCLXMLPath);

            info = ((PCL)pcl).PCLXMLPath + "XML文件上传成功";
            log.Info(info);
            txtBox.Dispatcher.Invoke((Action)(() => txtBox.AppendText(info)));
        }

        private void AnayMeteData(object emf)
        {
            log = LogManager.GetLogger("AnayMeteData");
            string vritualPrinterName = "";
            bool isSucess = true;
            //1. 通过xml文件获取打印机名称
            do
            {
                Thread.Sleep(1000);
                try
                {
                    vritualPrinterName = XmlFileReader.GetNodeInnerText(((PCL)emf).PCLXMLPath, "/Attribute/PortName");
                    isSucess = true;
                }
                catch (Exception ex)
                {
                    isSucess = false;
                    log.Error("获取PortName出错" + ex.ToString());
                    txtBox.Dispatcher.Invoke((Action)(() => txtBox.AppendText("获取PortName出错" + ex.ToString())));
                }
            }
            while (System.IO.File.Exists(((PCL)emf).PCLXMLPath) == false || isSucess == false);
        }

        public void UploadFile(string filePath)
        {
            string filename = System.IO.Path.GetFileName(filePath);
            int bytesRead = 0;
            byte[] filebyte = new byte[1024 * 80];
            bool isFirstChunk = false;
            bool isLastChunk = false;
            bool result = true;
            long byteupload = 0;
            long startPosition = 0;
            long filelen = 0;
            UploadWeb.UploadSoapClient uploadClient = new UploadWeb.UploadSoapClient();

            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    filelen = fs.Length;
                    while ((bytesRead = fs.Read(filebyte, 0, filebyte.Length)) != 0 && byteupload < fs.Length)
                    {
                        isFirstChunk = byteupload == 0;
                        byteupload += bytesRead;
                        if (byteupload >= fs.Length)
                        {
                            isLastChunk = true;
                            byte[] lastbyte = new byte[bytesRead];
                            fs.Position = byteupload - bytesRead;
                            startPosition = byteupload - bytesRead;
                            bytesRead = fs.Read(lastbyte, 0, lastbyte.Length);
                            result = uploadClient.UploadFileBybyte(filename, lastbyte, isFirstChunk, isLastChunk);
                        }
                        else
                        {
                            result = uploadClient.UploadFileBybyte(filename, filebyte, isFirstChunk, isLastChunk);
                            startPosition += bytesRead;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                txtBox.Dispatcher.Invoke((Action)(() => txtBox.AppendText("上传文件出错" + ex.Message)));
            }
            finally
            {
                if (Path.GetExtension(filePath) == ".pcl")
                {
                    mainOwner.Dispatcher.Invoke((Action)(() => mainOwner.UpProcessBar()));
                }
            }
        }

        public void Stop()
        {
            foreach (FileSystemWatcher fileSystemWatcher in _FileSystemWatcherList)
            {
                fileSystemWatcher.EnableRaisingEvents = false;
            }
        }
    }
}
