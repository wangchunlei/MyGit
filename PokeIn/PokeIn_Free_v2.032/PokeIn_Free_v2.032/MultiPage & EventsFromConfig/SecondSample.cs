namespace EventsFromConfig
{
    using System;
    using System.Collections.Generic;

    using PokeIn;
    using PokeIn.Comet;

    public partial class SecondSample:IDisposable
    { 
        private string clientId = "";
        public SecondSample(string _clientId)
        {
            clientId = _clientId;
            CometWorker.Groups.PinClientID(clientId, "SecondSample");
        }

        //OnClientDisconnected
        public void Dispose()
        {
            //Do not send a message to the client here, because its already disconnected    
            //If you want to send a message to client and do some actions on its disconnection phase, use PokeIn.OnClose from the client side
        }

        public void GetServerTime()
        {
            CometWorker.SendToClient(clientId, JSON.Method("ShowTime", DateTime.Now));
        }

        public void SendToAll(string message)
        {
            CometWorker.SendToAll(JSON.Method("s", message));
        }

        public void SendOnlyToSecond(string message)
        {
            CometWorker.Groups.Send("SecondSample", JSON.Method("s", message));
        }
    }
}