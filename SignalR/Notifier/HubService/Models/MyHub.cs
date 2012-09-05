using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Hubs;

namespace HubService.Models
{
    public class MyHub : Hub, IDisconnect, IConnected
    {
        public void InvokeByClient(string message)
        {
            
        }
    }
}