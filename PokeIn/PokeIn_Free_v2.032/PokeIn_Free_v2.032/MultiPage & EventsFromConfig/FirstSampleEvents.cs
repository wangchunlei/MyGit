namespace EventsFromConfig
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using PokeIn;
    using PokeIn.Comet;

    public partial class FirstSample:IDisposable
    {
        //Parameterless class constructor is a mendatory part of CustomEvent hoster classes 
        public FirstSample()
        {

        }

        //!!!!!!!!!!!!!!!!!EVENT LISTENERS MUST BE STATIC (SHARED for VB.NET)

        //First OnClientConnected Event Handler Called By Default.aspx
        public static void OnClientConnectedToFirst(ConnectionDetails details, ref Dictionary<string, object> classList)
        {
            classList.Add("FirstClass", new FirstSample(details.ClientId));
        }

        //First OnClientCreated Event Handler Called By Default.aspx
        public static void OnClientCreatedToFirst(string ClientId)
        {
            //Do not send a message to the client during its OnClientConnected phase. 
            //Instead use this event in order to start sending messages
            CometWorker.SendToClient(ClientId, JSON.Method("s", "OnClientCreatedToFirst event is fired"));
        } 
    }
}