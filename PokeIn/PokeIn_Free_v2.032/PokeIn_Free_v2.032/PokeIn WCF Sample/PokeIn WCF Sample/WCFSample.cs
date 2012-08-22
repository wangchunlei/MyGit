using System;
using System.ServiceModel;

namespace PokeIn_WCF_Sample
{
    public class WCFSample
    {
        static WCFSample()
        {
            PokeIn.Comet.CometSettings.ClientTimeout = 0;//unlimited 

            /*
             * Please read the documentation for this parameter.
             * 
             * To test locally, TRUE assigned
             * 
             * if you assign false or remove assignment below. Every new client window on same browser will close another.
             */
            PokeIn.Comet.CometSettings.MultiWindowsForSameSession = true;
        }

        internal string ClientId;
        public WCFSample(string clientId)
        {
            ClientId = clientId;
        }

        public void GetWCFServerTime()
        {
            try
            {
                localhost.PokeInWCF proxy = new localhost.PokeInWCF();
                proxy.Timeout = 1500;

                PokeIn.Comet.CometWorker.SendToClient(ClientId, "WCFInfo('" + proxy.TestMethod(ClientId) + "');");
            }
            catch
            {
                PokeIn.Comet.CometWorker.SendToClient(ClientId, "WCFInfo('<font color=\\\'red\\\'>Run WCF Server Project</font>');");
            }
        }
    }
}