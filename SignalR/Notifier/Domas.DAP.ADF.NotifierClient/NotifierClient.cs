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
        private HubConnection connection;
        private IHubProxy hubProxy = null;
        private Domas.DAP.ADF.LogManager.ILogger logger;

        public void Stop()
        {
            this.connection.Stop();
        }
        public void InvokeServer(string method)
        {
            hubProxy.Invoke(method);
        }
        public void StartListen(string hubName, string baseUrl, CookieContainer cookieContainer, Action<NotifierDTO> callback)
        {
            logger = Domas.DAP.ADF.LogManager.LogManager.GetLogger("NotifierClinet");
            connection = new HubConnection(baseUrl);

            var auth_cookie = cookieContainer.GetCookies(new Uri(baseUrl))[".ASPXAUTH"];
            if (auth_cookie == null)
            {
               // throw new Exception("请登录");
            }
            var user_cookie = cookieContainer.GetCookies(new Uri(baseUrl))["UserInfo"];
            var userDto = Domas.DAP.ADF.Cookie.CookieManger.DecryCookie(user_cookie.Value);
            if (userDto != null)
            {
                connection.ConnectionId = userDto.LoginID;
            }

            connection.CookieContainer = cookieContainer;
            hubProxy = connection.CreateProxy(hubName);

            hubProxy.On<NotifierDTO>("addMessage", (data) =>
                {
                    lock (connection)
                    {
                        if (connection.State==SignalR.Client.ConnectionState.Disconnected)
                        {
                            connection.Start().Wait();
                        }
                    }
                    hubProxy.Invoke("ServerCallback", data.MessageId).ContinueWith(task =>
                        {
                            if (task.IsFaulted)
                            {
                                logger.Error("Task 报错");
                            }
                            else
                            {
                                callback(data);
                            }
                        }).Wait();
                });
            connection.Start().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        throw new Exception("连接服务器失败");
                    }
                    else
                    {
                        ////登录成功
                        //hubProxy.Invoke<string>("ServerCallback", "").ContinueWith(invoke =>
                        //    {
                        //        if (invoke.IsFaulted)
                        //        {
                        //            logger.Error("Login invoke error");
                        //        }
                        //    });
                    }
                }).Wait();
        }
    }
}
