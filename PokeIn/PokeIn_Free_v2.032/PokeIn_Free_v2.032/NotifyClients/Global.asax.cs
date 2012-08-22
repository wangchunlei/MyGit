using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace NotifyClients
{
    using System.Net;

    using PokeIn.Comet;

    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //Faster & Secure Transfers
            CometSettings.Base64EncyrptedTransfers = true;

            //Reduce the load
            CometSettings.ChunkedMode = true;

            //Since we are not going to develop a DesktopClient for this sample better keep it false (Security)
            CometSettings.AcceptDesktopClients = false;

            //Activate WebSocket feature
            CometSettings.SocketPort = 36969;
            CometSettings.SocketServerIP = IPAddress.Parse("127.0.0.1");

            CometWorker.OnClientConnected += new DefineClassObjects(CometWorker_OnClientConnected);
            CometWorker.OnClientCreated += new ClientCreatedDelegate(CometWorker_OnClientCreated);
            CometWorker.OnClientDisconnected += new ClientDisconnectedDelegate(CometWorker_OnClientDisconnected);

            //Start the notifer
            Notifier.Start();
        }

        void CometWorker_OnClientDisconnected(string clientId)
        {
            //PokeIn already cleans the individual object instances of that client
        }

        void CometWorker_OnClientCreated(string clientId)
        {
            //client is created and ready. You can start sending messages
        }

        void CometWorker_OnClientConnected(ConnectionDetails details, ref Dictionary<string, object> classList)
        {
            classList.Add("MyPokeInClass", new MyPokeInClass(details.ClientId));
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}