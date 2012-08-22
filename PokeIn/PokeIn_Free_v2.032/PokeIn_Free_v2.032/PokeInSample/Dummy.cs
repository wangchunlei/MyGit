using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using PokeIn;
using PokeIn.Comet;

//Important! See Global.asax file and read the comments
namespace PokeInSample
{
    public class Dummy:IDisposable
    {
        string _clientId;  

        static Dummy()
        {
            new Thread(delegate()
            {
               while (true)
               {
                   string jsonMethod = JSON.Method("UpdateTime", DateTime.Now);
                   CometWorker.Groups.Send("TimeChannel", jsonMethod);
                   Thread.Sleep(1000);
               }
            }).Start();
        }

        public Dummy(string clientId)
        {
            _clientId = clientId; 
        }

        public void Dispose()
        { 
            // do something on exit
        }

        public void TestString(string str)
        {
            str = JSON.Tidy(str);//to make sure that the string is ready for proper transfer
            CometWorker.SendToClient(_clientId, "UpdateString('" + str + "');");
        } 

        public void GetServerTime()
        {
            string jsonMethod = JSON.Method("UpdateTime", DateTime.Now);
            CometWorker.SendToClient(_clientId, jsonMethod);
        } 

        public void SubscribeToTimeChannel()
        {
            CometWorker.Groups.PinClientID(_clientId, "TimeChannel");
        }

        public void LeaveChannel()
        {
            CometWorker.Groups.UnpinClient(_clientId);
        }
    }
}
