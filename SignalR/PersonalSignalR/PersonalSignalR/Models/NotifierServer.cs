using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace PersonalSignalR.Models
{
    [HubName("NotifierServer")]
    public class NotifierServer : Hub
    {
       
        public override Task OnDisconnected()
        {
            
            return null;
        }

        public override Task OnConnected()
        {
            
            return null;
        }
        public void SetClientAgent(string clientAgent)
        {
            
        }
       
        public override Task OnReconnected()
        {

            return null;
        }

        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
