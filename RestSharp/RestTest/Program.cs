using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using RestSharp;

namespace RestTest
{
    class Program
    {
        private static string orderno = "P20121127";
        private static void WebApi()
        {
            //var client = new RestClient("http://localhost:2616/api/values/");
            //var client = Domas.DAP.ADF.AgentCommon.AgentCommon.CreateRestClient();
            //var request = new RestRequest("api/QueryManagerAPI/QueryByCase", Method.POST);
            ////var username = "wangchunleird";
            ////var password = "lx314114";
            ////request.AddParameter("username", username);
            ////request.AddParameter("password", password);
            ////request.AddParameter("password_enc", "bHgzMTQxMTQ%3D&newpassword_enc=");
            ////request.AddParameter("login", "1");
            ////request.AddParameter("login_type", "login");
            ////request.AddParameter("uri", "aHR0cDovLzE5Mi4xNjguNjAuNjUv");
            ////request.AddParameter("password_type", "normal");
            //request.RequestFormat = RestSharp.DataFormat.Json;
            //request.AddBody("value=wangcl");

            //var response = client.Execute(request);

            //var content = response.Content;
            var proxy = new Domas.Web.QueryFramework.QueryManager.QueryManagerProxy();
            proxy.QueryByCase(new Domas.DAP.ADF.Query.Case());
            Console.WriteLine("Ok");
        }
        static void Main(string[] args)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    System.Threading.Thread.Sleep(1000);
                   WebApi();
                });
            var thread = new System.Threading.Thread(WebApi);
            thread.Start();
            System.Threading.Thread.Sleep(2000);
          WebApi();
            //var url =
            //    string.Format(
            //        "http://192.168.100.254?username={0}&password={1}&password_enc={2}&login={3}&login_type={4}&uri={5}&password_type={6}",
            //        "wangchunleird", "lx314114", "bHgzMTQxMTQ%3D&newpassword_enc=", "1", "login", "aHR0cDovLzE5Mi4xNjguNjAuNjUv", "normal"
            //        );
            //foreach (var @interface in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
            //    .SelectMany(c => c.GetIPProperties().UnicastAddresses)
            //    .Where(c => c.Address.AddressFamily == AddressFamily.InterNetwork && c.IsDnsEligible))
            //{
            //    var ipendPoint = new IPEndPoint(@interface.Address, 0);
            //    Console.WriteLine(ipendPoint);

            //    var serverPoint = ServicePointManager.FindServicePoint(new Uri("http://192.168.100.254"));
            //    serverPoint.BindIPEndPointDelegate = delegate(
            //        ServicePoint servicePoint,
            //        IPEndPoint remoteEndPoint,
            //        int retryCount)
            //        {
            //            return ipendPoint;
            //        };
            //    serverPoint.ConnectionLeaseTimeout = 0;
            //    var req = WebRequest.Create(url) as HttpWebRequest;
            //    req.KeepAlive = false;
            //    req.Method = "Post";
            //    req.Timeout = 1000 * 30;
            //    //req.ReadWriteTimeout = 1000 * 10;

            //    try
            //    {
            //        using (var response = req.GetResponse())
            //        {
            //            using (var stream = response.GetResponseStream())
            //            {
            //                content = new StreamReader(stream).ReadToEnd();
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }

            //}

            Console.WriteLine("完成");
            //Console.ReadKey(false);
        }

        private static string CallAPI(string value)
        {
            //int v = int.Parse(value);
            var client = Domas.DAP.ADF.AgentCommon.AgentCommon.CreateRestClient();
            var request = new RestRequest("api/APITest/Post?orderno={orderno}&archivefileid={archivefileid}&printnum={printnum}", Method.POST);
            //request.AddHeader("Connection", "keep-alive");
            //request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddBody(new
            {
                barcodevaluelist = new List<string>
                {
                    "B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                    ,"B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001","B2012112700001"
                }
            });
            request.AddUrlSegment("orderno", orderno + value);
            request.AddUrlSegment("archivefileid", value);
            request.AddUrlSegment("printnum", "1");

            var response = client.Execute(request);

            //Console.WriteLine(response.ErrorException.GetBaseException().ToString());
            var content = response.Content;
            Console.WriteLine(content);
            return content;
        }
    }
}
