/*
 * PokeIn ASP.NET Ajax Library - WCF Desktop Controller Sample
 * 
 * PokeIn 2010
 * http://pokein.com
 */
using System.Collections.Generic;
using System.ServiceModel;

namespace ClientWCF_Control
{
    public delegate void UserAddedDelegate(string clientId);
    public delegate void UserRemovedDelegate(string clientId);

    [ServiceContract]
    public class PokeInWCF
    {
        [ServiceContract]
        public class MessageFormat
        {
            public string[] Clients;
            public string Message;
        }

        public static List<string> ClientIds = new List<string>();
        public static List<MessageFormat> Messages = new List<MessageFormat>();

        public static UserAddedDelegate OnUserAdded = null;
        public static UserRemovedDelegate OnUserRemoved = null;

        [OperationContract]
        public void AddClient(string clientId)
        {
            lock (ClientIds)
            {
                if (!ClientIds.Contains(clientId))
                    ClientIds.Add(clientId);
                else
                    return;
            }
            if(OnUserAdded!=null)
                OnUserAdded(clientId);
        }

        [OperationContract]
        public void AddClients(string []clientIds)
        {
            lock (ClientIds)
            {
                ClientIds.AddRange(clientIds);
            }
            if (OnUserAdded != null)
                foreach(string clientId in clientIds)
                    OnUserAdded(clientId);
        }

        [OperationContract]
        public void RemoveClient(string clientId)
        {
            lock(ClientIds)
            {
                ClientIds.Remove(clientId);
            }
            if (OnUserRemoved != null)
                OnUserRemoved(clientId);
        }

        [OperationContract]
        public MessageFormat[] PingMessages()
        {
            MessageFormat [] list;
            lock (Messages)
            {
                list = Messages.ToArray();
                Messages.Clear();
            }
            return list;
        }

        [OperationContract]
        public bool PingAlive()
        {
            return true;
        }
    }
}
