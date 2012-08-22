namespace EventsFromConfig
{
    using System;
    using System.Collections.Generic; 
    using PokeIn;
    using PokeIn.Comet;
     
    public partial class SecondSample:IDisposable
    {
        //Parameterless class constructor is a mendatory part of CustomEvent hoster classes 
        public SecondSample()
        {
        }

        //!!!!!!!!!!!!!!!!!EVENT LISTENERS MUST BE STATIC (SHARED for VB.NET)

        //Second OnClientConnected Event Handler Called By SamplePage.aspx
        public static void OnClientConnectedToSecond(ConnectionDetails details, ref Dictionary<string, object> classList)
        {
            classList.Add("SecondSample", new SecondSample(details.ClientId));
        }

        //Second OnClientCreated Event Handler Called By SamplePage.aspx
        public static void OnClientCreatedToSecond(string ClientId)
        {
            //Do not send a message to the client during its OnClientConnected phase. 
            //Instead use this event in order to start sending messages
            CometWorker.SendToClient(ClientId, JSON.Method("s", "OnClientCreatedToSecond event is fired"));
        }
    }
}