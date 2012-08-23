using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR;

namespace Test1.Models
{
    public class Notifier
    {
        public static void GroupNotify(string group,string message)
        {
            var context = GlobalHost.ConnectionManager.GetConnectionContext<MyConnection>();
            
        }
    }
    public class MyConnection : PersistentConnection
    {
        private static Dictionary<string, string> clients;
        protected override System.Threading.Tasks.Task OnReceivedAsync(IRequest request, string connectionId, string data)
        {
            foreach (var client in clients)
            {
                if (client.Key==connectionId)
                {
                    continue;
                }
                Connection.Send(client.Key, data + DateTime.Now.ToLongTimeString());
            }
            return null;
            // return Connection.Send(connectionId, data + DateTime.Now.ToLongTimeString());
            //return Connection.Broadcast(data+DateTime.Now.ToLongTimeString());
        }

        protected override Connection CreateConnection(string connectionId, IEnumerable<string> groups, IRequest request)
        {

            return base.CreateConnection(connectionId, groups, request);
        }

        protected override System.Threading.Tasks.Task OnConnectedAsync(IRequest request, string connectionId)
        {
            if (clients == null)
            {
                clients = new Dictionary<string, string>();
            }
            clients.Add(connectionId, request.User.Identity.Name);
            return base.OnConnectedAsync(request, connectionId);
        }

        protected override System.Threading.Tasks.Task OnDisconnectAsync(string connectionId)
        {
            if (clients != null && clients.ContainsKey(connectionId))
            {
                clients.Remove(connectionId);
            }
            return base.OnDisconnectAsync(connectionId);
        }
    }
}