using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PokeIn;
using PokeIn.Comet;

namespace MVC3_Razor_CS.Models
{
    public class SampleClass:IDisposable
    {
        string _clientID;
        public SampleClass(string clientId)
        {
            _clientID = clientId;
        }

        public void Dispose()
        {
            //Do whatever you want here : PokeIn is going to call this method when user disconnectes
        }

        public void GetServerTime()
        {
            string msg = JSON.Method("ServerTime", DateTime.Now);
            CometWorker.SendToClient(_clientID, msg);
        }
    }
}