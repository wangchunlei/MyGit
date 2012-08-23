using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domas.DAP.ADF.NotifierDeploy;
using SignalR.Hubs;
using SignalR;

namespace Domas.DAP.ADF.Notifier
{
    public class NotifierServer : Hub, IDisconnect, IConnected
    {
        private static Dictionary<string, string> _clientUsers;
        public static void Send(NotifierDeploy.NotifierDTO message, string userAccount)
        {
            if (_clientUsers == null || _clientUsers.Count == 0)
            {
                return;
            }
            var users = _clientUsers.Where(c => c.Value == userAccount);
            var context = GlobalHost.ConnectionManager.GetHubContext<NotifierServer>();
            foreach (var user in users)
            {
                context.Clients[user.Key].addMessage(message);
            }
        }

        public Task Disconnect()
        {
            if (_clientUsers != null && _clientUsers.ContainsKey(Context.ConnectionId))
            {
                _clientUsers.Remove(Context.ConnectionId);
            }

            return null;
        }

        public Task Connect()
        {
            if (_clientUsers == null)
            {
                _clientUsers = new Dictionary<string, string>();
            }
            _clientUsers.Add(Context.ConnectionId, Context.RequestCookies["user"].Value);
            return null;
        }

        public Task Reconnect(IEnumerable<string> groups)
        {
            return null;
        }
    }
}
