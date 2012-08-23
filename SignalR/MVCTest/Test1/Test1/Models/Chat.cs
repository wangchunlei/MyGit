using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SignalR;
using SignalR.Hubs;

namespace Test1.Models
{
    public class Chat : Hub, IDisconnect, IConnected
    {
        private static Dictionary<string, string> users = new Dictionary<string, string>();
        public void Send(string message)
        {
            Clients.addMessage(message + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString());
            Send1(message, @"LANXUM\wangchunleird");
        }

        private static Dictionary<string, string> ClientUsers;
        public static void Send1(string message, string userAccount)
        {
            var users = ClientUsers.Where(c => c.Value == userAccount);
            var context = GlobalHost.ConnectionManager.GetHubContext<Chat>();
            foreach (var user in users)
            {
                context.Clients[user.Key].addMessage(message);
            }
        }
        public Task Disconnect()
        {
            if (ClientUsers != null && ClientUsers.ContainsKey(Context.ConnectionId))
            {
                ClientUsers.Remove(Context.ConnectionId);
            }

            return null;
        }

        public Task Connect()
        {
            if (ClientUsers == null)
            {
                ClientUsers = new Dictionary<string, string>();
            }

            ClientUsers.Add(Context.ConnectionId, Context.User.Identity.Name);
            return null;
        }

        public Task Reconnect(IEnumerable<string> groups)
        {
            return null;
        }
    }
}