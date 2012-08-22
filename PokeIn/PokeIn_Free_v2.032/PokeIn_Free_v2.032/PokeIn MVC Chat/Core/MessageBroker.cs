using System.Collections.Generic;
using PokeIn.Comet;

namespace PokeInMVC_Chat.Core
{
    public class MessageBroker
    { 

        public static void OnClientConnected(ConnectionDetails details, ref Dictionary<string, object> list)
        {
            list.Add("Chat", new PubSubNotification(details.ClientId));
        }
    }
}