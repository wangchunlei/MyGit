using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Hubs;

namespace Test1.SignalR
{
    [HubName("blogHub")]
    public class BlogHub : Hub
    {
        public void Send(string message,string sessnId)
        {
            Clients.addMessage(message, sessnId);

            Clients.Test("aaaaa" + DateTime.Now.ToLongTimeString());
        }
    }
}