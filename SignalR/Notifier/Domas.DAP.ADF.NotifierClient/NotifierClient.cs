using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Domas.DAP.ADF.NotifierDeploy;
using SignalR.Client.Hubs;

namespace Domas.DAP.ADF.NotifierClient
{
    public class NotifierClient
    {
        public static void StartListen(string hubName, string baseUrl, Action<NotifierDTO> callback)
        {
            var connection = new HubConnection(baseUrl);

            var hubProxy = connection.CreateProxy(hubName);

            hubProxy.On("addMessage", callback);

            connection.Start().Wait();
        }

        public static void StartListen(string hubName, string baseUrl, CookieContainer cookieContainer, Action<NotifierDTO> callback)
        {
            var connection = new HubConnection(baseUrl);
            connection.CookieContainer = cookieContainer;
            var hubProxy = connection.CreateProxy(hubName);

            hubProxy.On("addMessage", callback);
            connection.Start().Wait();
        }
    }
}
