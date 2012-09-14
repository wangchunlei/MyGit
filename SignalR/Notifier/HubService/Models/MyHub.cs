using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using SignalR;
using SignalR.Hubs;

namespace HubService.Models
{
    public class NotifierServer : Hub
    {
        private static System.Timers.Timer aTimer;

        static NotifierServer()
        {
            aTimer = new Timer(60000);
            aTimer.Elapsed += new ElapsedEventHandler(aTimer_Elapsed);
            aTimer.Enabled = true;
            aTimer.Start();
        }

        static void aTimer_Elapsed(object sender, ElapsedEventArgs e)
        {

        }
        public static void Send(string message)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<NotifierServer>();


            
            context.Clients.addMessage123(message);

        }
        public void ServerCallback(string messageId)
        {

        }
        public Task Disconnect()
        {

            return null;
        }

        public Task Connect()
        {

            return null;
        }

        public Task Reconnect(IEnumerable<string> groups)
        {
            return null;
        }
    }
}
