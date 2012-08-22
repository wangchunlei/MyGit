using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PokeIn.Comet;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 100; i++)
            {
                var thread = new Thread(delegate()
                 {
                     var clientInstance = new ClientInstance();
                     var client = new DesktopClient(clientInstance, "http://localhost:8006/host.PokeIn", "");
                     client.Connect();
                 });
                thread.Start();
                Thread.Sleep(1000);
                Console.WriteLine("");
            }
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
