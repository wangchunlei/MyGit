using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PokeIn;
using PokeIn.Comet;

namespace PokeIn_Silverlight.Web
{
    public class Dummy:IDisposable
    {
        public string ClientId;
        public Dummy(string clientId)
        {
            ClientId = clientId;
        }

        public void Dispose()
        {
            //Do something here
        }

        public void ServerTime()
        {
            CometWorker.SendToClient(ClientId, JSON.Method("TimeReceived", DateTime.Now.ToLongTimeString()));
        }
    }
}