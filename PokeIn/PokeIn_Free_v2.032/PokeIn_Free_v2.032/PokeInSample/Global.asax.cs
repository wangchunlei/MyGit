using System;
using System.Collections.Generic;
using PokeIn.Comet;

namespace PokeInSample
{
    using System.Net;

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //Chunked mode allows multiple message using same listener request
            CometSettings.ChunkedMode = true;

            //You must define a listener for below event
            CometWorker.OnClientConnected += new DefineClassObjects(CometWorker_OnClientConnected);
          
            //This listener is optional, if you need your clients use same clientId after page refresh etc.
            //this is the option
            CometWorker.OnReConnectionDecision += new DecisionDelegate(CometWorker_OnReConnectionDecision); 

            //WebSocket Server IP
            CometSettings.SocketServerIP = IPAddress.Parse("127.0.0.1");

            //WebSocket Port
            CometSettings.SocketPort = 33333;
        }

        static void CometWorker_OnReConnectionDecision(ConnectionDetails details, ref bool accepted)
        {
            //You may check the parameters from "details" object to decide accept 
            //reconnection to same client id and its objects or not
            accepted = true;
        }

        static void CometWorker_OnClientConnected(ConnectionDetails details, ref Dictionary<string, object> classList)
        {
            classList.Add("Dummy", new Dummy(details.ClientId)); 
        }

        //!!!!!!!!!Important!!!!
        //Below definition must be commented out to use PokeInHandler with OnReConnectionDecision feature
        //If you need below function defined, you have to change PokeInHandler definition inside web.config
        //to PokeInHandlerNoSession

        //When you activate PokeInHandlerNoSession, OnReconnectionDecision will not work. 
        /*
        protected void Session_Start(object sender, EventArgs e)
        {

        }
        */

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }
         
        protected void Session_End(object sender, EventArgs e)
        {

        }
        
        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}