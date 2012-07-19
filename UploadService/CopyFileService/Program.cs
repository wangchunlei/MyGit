using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace CopyFileService
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ServiceBase[] ServicesToRun;
                // 同一进程中可以运行多个用户服务。若要将
                // 另一个服务添加到此进程中，请更改下行以
                // 创建另一个服务对象。例如，
                //
                //   ServicesToRun = new ServiceBase[] {new Service1(), new MySecondUserService()};
                //
                ServicesToRun = new ServiceBase[] 
                    { 
                        new FileMonitor() 
                    };
                ServiceBase.Run(ServicesToRun);

            }
            else
            {
            Begin:
                Console.WriteLine("服务正在启动,按任意键开始...");
                Console.ReadKey();
                FileMonitor sv = null;
                try
                {
                    sv = new FileMonitor();
                    sv.StartSv();
                    Console.WriteLine("服务启动完成...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("服务启动失败...");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.GetType().ToString());
                    Console.WriteLine(ex.StackTrace.ToString());
                    sv.Stop();
                    goto Begin;
                }
                Console.Read();
            }
        }
    }
}
