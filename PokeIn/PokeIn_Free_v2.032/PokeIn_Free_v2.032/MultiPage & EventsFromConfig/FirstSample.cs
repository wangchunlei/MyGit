namespace EventsFromConfig
{
    using System;
    using System.Collections.Generic;
    using PokeIn;
    using PokeIn.Comet;

    public partial class FirstSample:IDisposable
    {
        private string clientId = "";
        public FirstSample(string _clientId)
        {
            clientId = _clientId;
            CometWorker.Groups.PinClientID(clientId, "SampleClass");
        }

        //OnClientDisconnected
        public void Dispose()
        {
            //Do not send a message to the client here, because its already disconnected    
            //If you want to send a message to client and do some actions on its disconnection phase, use PokeIn.OnClose from the client side
        }

        public void GetServerTimeAsString()
        {
            CometWorker.SendToClient(clientId, JSON.Method("s", DateTime.Now.ToLongTimeString()));
        }

        public void SendToAll(string message)
        {
            CometWorker.SendToAll(JSON.Method("s", message));
        }

        public void SendOnlyToFirst(string message)
        {
            CometWorker.Groups.Send("SampleClass", JSON.Method("s", message));
        }
    }
}