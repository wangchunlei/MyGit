﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Hubs;

namespace MultiEditWithSignalR.SignalR
{
    [HubName("blogHub")]
    public class BlogHub : Hub
    {
        /// <summary>
        /// The method called from SignalR client (JS)
        /// </summary>
        /// <param name="message">String Data</param>
        /// <param name="sessnId">Session ID: Used in this sample to track users</param>
        /// <param name="append">A boolean value used in the example indicating
        /// if incoming value should be appended or overwritten when sent back to
        /// other clients</param>
        public void Send(string message, string sessnId)
        {
            Clients.addMessage(message, sessnId);
        }
    }
}