using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Lanxum.Domas.Extension.LogManager;

namespace CopyFileService
{
    partial class FileMonitor : ServiceBase
    {
        public FileMonitor()
        {
            InitializeComponent();
        }

        public void StopSv()
        {
            this.OnStop();
        }

        public void StartSv()
        {
            OnStart(null);
        }

        public static FileWatch g_FilePathWatch = null;
        private static ILogger logger = null;

        protected override void OnStart(string[] args)
        {
            logger = LogManager.GetLogger("文件监控服务启动");
            logger.Info("文件监控服务准备启动");
            g_FilePathWatch = new FileWatch();
            g_FilePathWatch.Start();
            logger.Info("文件监控服务启动完毕");
            // TODO: Add code here to start your service.
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}
