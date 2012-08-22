/*
 * PokeIn ASP.NET Ajax Library - WCF Desktop Controller Sample
 * 
 * PokeIn 2010
 * http://pokein.com
 */
using System;
using System.Threading;
using PokeIn.Comet;
using PokeIn_WCF_Sample.localhost;

namespace PokeIn_WCF_Sample
{
    public class WCFSample:IDisposable
    {
        internal string ClientId;
        public WCFSample(string clientId)
        {
            ClientId = clientId;
            CometWorker.SendToClient(ClientId, "UpdateServiceStatus(" + ServerConnected.ToString().ToLower() + ");");

            if (ServerConnected)
            { 
                proxy.AddClient(clientId);
            }
        } 

        public void Dispose()
        {
            if(ServerConnected)
                proxy.RemoveClient(ClientId);
        }

        //Your desktop service should be running before the web application start
        static WCFSample()
        {
            ConnectToWCF();
        }
        static bool ServerConnected = false;
        static localhost.PokeInWCF proxy = null;
        static Thread workerThread = null;
        static void ConnectToWCF()
        {
            try
            {
                proxy = new localhost.PokeInWCF();
                bool b;
                proxy.PingAlive(out b, out b);

                ServerConnected = true;

                /* ConnectToWCF method call
                 * made by static constructor or CheckMessage function?
                 * 
                 * Check, if there is an existing worker thread
                 */
                if (workerThread == null)
                {
                    workerThread = new System.Threading.Thread(CheckMessages);
                    workerThread.Start();
                }

                CometWorker.SendToAll("UpdateServiceStatus(" + ServerConnected.ToString().ToLower() + ");");

                string[] clientIds = CometWorker.GetClientIds();
                if (clientIds != null)
                    proxy.AddClients(clientIds); 
            }
            catch
            {
                //Connection Issue re-try after 1.5 secs
                Thread.Sleep(1500);
                ConnectToWCF();
            }  
        }

        static void CheckMessages()
        {
            PokeInWCFMessageFormat[] results = null;

            try
            {
                results = proxy.PingMessages();
            }
            catch(System.Net.WebException e)
            {
                ServerConnected = false;
                CometWorker.SendToAll("UpdateServiceStatus(" + ServerConnected.ToString().ToLower() + ");");
                ConnectToWCF();
            }
            if (results!=null)
            {
                if(results.Length>0)
                    foreach (PokeInWCFMessageFormat message in results)
                    {
                        CometWorker.SendToClients(message.Clients, message.Message);
                    }
            }

            //check messages every 1 second
            //You may implement a WCF instance to both side for efficiency or check out the Multiple Server sample project
            Thread.Sleep(1000);
            CheckMessages();
        }
    }
}