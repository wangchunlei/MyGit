using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Headers;
using System.Net;
using System.Text;
using Domas.Service.Base.Common;
using Domas.Service.Print.PrintTask;
using RestSharp;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Net;
using System.Linq;
using System.Collections.Generic;
namespace HttpClient
{
    class Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        static void Main(string[] args)
        {
            var client = new RestClient("http://localhost:8006/");
            //client.Authenticator = new NtlmAuthenticator(@"administrator", "Lanxum123456");
            var request = new RestRequest(Method.GET);

            //request.AddUrlSegment("api", "POAPI");
            //request.AddUrlSegment("method", "CreatePO");
            //request.AddHeader("content-type", "application/json; charset=utf-8");
            //request.AddHeader("Accept", "applicaiton/json");

            request.RequestFormat = DataFormat.Json;
            var cookieContainer = CookieManger.GetUriCookieContainer(new Uri(client.BaseUrl));
            if (cookieContainer == null || cookieContainer.Count == 0)
            {
                int i = 3;
                while (cookieContainer == null || cookieContainer.Count == 0)
                {
                    Process process = new Process();
                    process.StartInfo.FileName = "iexplore.exe";
                    process.StartInfo.Arguments = "http://localhost:8006";

                    process.Start();

                    process.WaitForExit(int.MaxValue);
                    cookieContainer = CookieManger.GetUriCookieContainer(new Uri(client.BaseUrl));
                    if (i-- == 0)
                    {
                        break;
                    }
                }
            }

            //var dto = new PODTO()
            //{
            //    DriverName = "123",
            //    FileName = "231",
            //    OrderNo = "order",
            //    CreatedOn = DateTime.Now,
            //    Createdby = "zhangzs",
            //    SafeLevel = SafeLevel.Classified,
            //    PrintBy = new Guid("86F6C414-9D34-4970-9A17-25FCE4CF563A")
            //};
            //request.AddBody(new
            //    {
            //        username = "admin",
            //        password = "admin"
            //    });

            client.CookieContainer = cookieContainer;
            var response = client.Execute(request);
            var content = response.Content;

        }


        private static void ReadCookie()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);

            var request = WebRequest.Create("http://localhost:8006/") as HttpWebRequest;

            request.CookieContainer = new CookieContainer();
            //request.ContentType = "application/x-www-form-urlencoded";

            var response = request.GetResponse();

            var cookies = request.CookieContainer.GetCookies(request.RequestUri);

            foreach (Cookie cookie in cookies)
            {
                Console.WriteLine(cookie.Name + "=" + cookie.Value);
            }
        }
        private static bool GetCookie_InternetExplorer(string strHost, string strField, ref string Value)
        {
            Value = string.Empty;
            bool fRtn = false;
            string strPath, strCookie;
            string[] fp;
            StreamReader r;
            int idx;

            try
            {
                strField = strField + "\n";
                strPath = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
                Version v = Environment.OSVersion.Version;

                //if (IsWindows7())
                //{
                strPath += @"\low";
                //}

                fp = Directory.GetFiles(strPath, "*.txt");

                foreach (string path in fp)
                {
                    idx = -1;
                    r = File.OpenText(path);
                    strCookie = r.ReadToEnd();
                    r.Close();

                    if (System.Text.RegularExpressions.Regex.IsMatch(strCookie, strHost))
                    {
                        idx = strCookie.ToUpper().IndexOf(strField.ToUpper());
                    }

                    if (-1 < idx)
                    {
                        idx += strField.Length;
                        Value = strCookie.Substring(idx, strCookie.IndexOf('\n', idx) - idx);
                        if (!Value.Equals(string.Empty))
                        {
                            fRtn = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception) //File not found, etc...
            {
                Value = string.Empty;
                fRtn = false;
            }

            return fRtn;
        }
        public static CookieContainer GetCooKie(string loginUrl)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                CookieContainer cc = new CookieContainer();
                request = (HttpWebRequest)WebRequest.Create(loginUrl);
                request.Method = "GET";

                request.AllowAutoRedirect = false;
                request.CookieContainer = cc;
                request.KeepAlive = true;

                //接收响应          
                response = (HttpWebResponse)request.GetResponse();
                response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);
                CookieCollection cook = response.Cookies;               //Cookie字符串格式      
                string strcrook = request.CookieContainer.GetCookieHeader(request.RequestUri);
                return cc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void RestClientInvoke()
        {
            var client = new RestClient("http://localhost:8006/");
            //client.Authenticator = new NtlmAuthenticator(@"administrator", "Lanxum123456");
            var request = new RestRequest(Method.GET);

            //request.AddUrlSegment("api", "POAPI");
            //request.AddUrlSegment("method", "CreatePO");
            //request.AddHeader("content-type", "application/json; charset=utf-8");
            //request.AddHeader("Accept", "applicaiton/json");

            request.RequestFormat = DataFormat.Json;


            //var dto = new PODTO()
            //{
            //    DriverName = "123",
            //    FileName = "231",
            //    OrderNo = "order",
            //    CreatedOn = DateTime.Now,
            //    Createdby = "zhangzs",
            //    SafeLevel = SafeLevel.Classified,
            //    PrintBy = new Guid("86F6C414-9D34-4970-9A17-25FCE4CF563A")
            //};
            //request.AddBody(new
            //    {
            //        username = "admin",
            //        password = "admin"
            //    });
            request.AddCookie(".ASPXAUTH",
                              "9B39C3A516F088217BCEC4E8C71849876DE6E7655FE4E934F9FD78ECBA198C527F21FED5959B4A39EE1DECEE6D7FEFA3F1B688833A1278B1B62E4BD77FC9B6FE15DBCD8A4A2CFB0CEB192FD7530EEA693552B3257FAECF175A8E478186404B8D7653599A12B246B133B23E1643878A800B9802C800D20014BBECFB944E0E8E6E");
            var response = client.Execute(request);
            var content = response.Content;

        }
        private static void RestClientInvoke1()
        {
            var client = new RestClient("http://192.168.20.34:8001/");
            client.Authenticator = new NtlmAuthenticator(@"administrator", "Lanxum123456");
            var request = new RestRequest("api/{api}/{method}?poid={id}", Method.GET);

            request.AddUrlSegment("api", "POAPI");
            request.AddUrlSegment("method", "GetPO");
            request.AddHeader("content-type", "application/json; charset=utf-8");
            request.AddHeader("Accept", "applicaiton/json");

            request.RequestFormat = DataFormat.Json;


            //var dto = new PODTO()
            //{
            //    DriverName = "123",
            //    FileName = "231",
            //    OrderNo = "order",
            //    CreatedOn = DateTime.Now,
            //    Createdby = "zhangzs",
            //    SafeLevel = SafeLevel.Classified,
            //    PrintBy = new Guid("86F6C414-9D34-4970-9A17-25FCE4CF563A")
            //};
            //request.AddBody(dto);
            request.AddParameter("id", "683F48FE-F29C-4EE6-8AA6-40DC56ACFBD1", ParameterType.UrlSegment);

            //var val = new
            //    {
            //        metadatatype = "Domas.Service.Print.PrintTask.PO",
            //        id = new Guid("683F48FE-F29C-4EE6-8AA6-40DC56ACFBD1")
            //    };
            //request.AddBody(val);
            var response = client.Execute<Domas.Service.Print.PrintTask.PODeviceDTO>(request);
            var content = response.Content;

        }
    }
}
