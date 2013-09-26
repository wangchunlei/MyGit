using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domas.DAP.ADF.NotifierDeploy;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.AspNet.SignalR.Client.Transports;

namespace SignalRClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://192.168.20.222:8088";
            var client = Domas.DAP.ADF.NotifierClient.NotifierClient.Create(url, null);
            client.StateChanged += e =>
                {
                    Console.WriteLine(e.NewState.ToString());
                };

            client.Start();
        }

        private static HubConnection connection;
        private static void StartListen(string hubName, string baseUrl, Action callback)
        {
            connection = new HubConnection(baseUrl);

            connection.Credentials = System.Net.CredentialCache.DefaultCredentials;

            var cc = new System.Net.CookieContainer();
            var cookiename = "userinfo";
            var cookievalue =
                @"3kiJ2IdvqPGaSuxB7J8Kk9l7nwFTV0mMB0gqTY5pLKePBukJ/Onh9fBBGHxWwKDxkTZfLsebSK1QgqRNmsYNrjkYriErk7N0ESyOH426P+ddVfNjYLYF39M22WG0nR/X1ZfHB2AphqnEzlPryVoirqOZa9c5ti7FC55S7O21myqOgJZigkozRj6+65SsdbfG";
            cc.Add(new Uri(baseUrl), new System.Net.Cookie(cookiename, cookievalue));

            connection.CookieContainer =
                Domas.DAP.ADF.Cookie.CookieManger.GetUriCookieContainer(new Uri(baseUrl));

            var hubProxy = connection.CreateHubProxy(hubName);

            Action reConnection = () =>
            {
                try
                {
                    lock (connection)
                    {
                        connection.Stop();
                    }
                }
                finally
                {
                    while (true)
                    {
                        Thread.Sleep(1000);
                        if (connection == null || connection.State == ConnectionState.Disconnected)
                        {

                            break;
                        }
                    }
                    StartListen(hubName, baseUrl, callback);
                }
            };
            hubProxy.On("addMessage", () => Task.Factory.StartNew(() =>
            {
                hubProxy.Invoke("ServerCallback").ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {

                    }
                    else
                    {
                        callback();
                    }
                });
            }));
            hubProxy.On("CloseClient", () =>
            {
                reConnection();
            });
            var sseTransport = new ServerSentEventsTransport();

            connection.Start(sseTransport).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    throw task.Exception;
                }
                else
                {
                    Console.WriteLine("connected.");
                    connection.Reconnected += () =>
                    {
                        reConnection();
                    };
                }
            }).Wait();


            hubProxy.Invoke("SetClientAgent", "Notifier").ContinueWith(task =>
            {
                if (task.IsFaulted)
                {

                }
                else
                {
                    Console.WriteLine("first invoke success.");
                }
            }).Wait();
        }
    }
}
