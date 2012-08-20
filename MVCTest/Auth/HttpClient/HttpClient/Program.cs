using System;
using System.Collections.Generic;
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
namespace PCTools
{
    /**/
    ///<summary>
    /// 获取Cookie的方法类。
    ///</summary>
    public class CookieManger
    {
        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetCookie(string url, string cookieName, StringBuilder cookieData, ref int size);
        public static CookieContainer GetUriCookieContainer(Uri uri)
        {
            CookieContainer cookies = null;
            //定义Cookie数据的大小。
            int datasize = 256;
            StringBuilder cookieData = new StringBuilder(datasize);
            if (!InternetGetCookie(uri.ToString(), ".ASPXAUTH", cookieData, ref datasize))
            {
                if (datasize < 0) return null;
                // 确信有足够大的空间来容纳Cookie数据。
                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookie(uri.ToString(), ".ASPXAUTH", cookieData, ref datasize)) return null;
            }
            if (cookieData.Length > 0)
            {
                cookies = new CookieContainer();
                cookies.SetCookies(uri, cookieData.ToString().Replace(';', ','));
            }
            return cookies;
        }
        public static void PrintCookies(CookieContainer cookies, Uri uri)
        {
            CookieCollection cc = cookies.GetCookies(uri);
            foreach (var cook in cc)
            {
                Console.WriteLine(cook);
            }
        }
    }
    public class Test
    {
        static void Main(string[] args)
        {
            string url = @"http://localhost:8006/";
            Uri uri = new Uri(url);
            CookieContainer cookies = CookieManger.GetUriCookieContainer(uri);
            CookieManger.PrintCookies(cookies, uri);

            Console.ReadKey();
        }
    }
}
namespace HttpClient
{
    class Program
    {
        static void Main1(string[] args)
        {
            //var httpClientHandler = new HttpClientHandler();
            //httpClientHandler.UseDefaultCredentials = true;
            //var client = new System.Net.Http.HttpClient(httpClientHandler);

            //client.BaseAddress = new Uri("http://localhost/");

            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //var dto = new PODTO()
            //{
            //    DriverName = "123",
            //    FileName = "231",
            //    OrderNo = "order",
            //    CreatedOn = DateTime.Now,
            //    Createdby = "zhangzs",
            //    SafeLevel = SafeLevel.Classified,
            //    PrintBy = new Guid("AE065557-B9E0-4D93-9DE5-54C6D85400C1")
            //};
            //HttpResponseMessage resp =
            //    client.PostAsJsonAsync(
            //        "api/POAPI/CreatPO", dto).Result;

            //if (resp.IsSuccessStatusCode)
            //{
            //    var result = resp.Content.ReadAsStringAsync().Result;
            //}

            //RestClientInvoke1();
            ReadCookie();
            //GetCooKie("http://localhost:8006/");
            //RestClientInvoke();
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
