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
using Jint;

namespace RestTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var serverIp = System.Configuration.ConfigurationManager.AppSettings["ServerIP"];
            var client = new RestClient(string.IsNullOrEmpty(serverIp) ? "http://8.8.8.8/" : "http://" + serverIp);
            var request = new RestRequest("login", Method.POST);

            var username = args[0];
            var password = args[1];
            var password1 = GetCryPwd(password);

            request.AddParameter("username", username);
            request.AddParameter("password", password1);
            request.AddParameter("password1", password);
            request.AddParameter("terminal", "pc");
            request.AddParameter("login", "1");
            request.AddParameter("login_type", "login");
            request.AddParameter("uri", "d3d3LmJhaWR1LmNvbS8=");

            var response = client.Execute(request);

            var content = response.Content;
            Console.WriteLine(content);
            Console.WriteLine("完成");
        }

        static string GetCryPwd(string pwd)
        {
            var engine = new Engine()
                .Execute(@"function mc(a){ret='';var b='0123456789ABCDEF';if(a==' '.charCodeAt()){ret='+'}else if((a<'0'.charCodeAt()&&a!='-'.charCodeAt()&&a!='.'.charCodeAt())||(a<'A'.charCodeAt()&&a>'9'.charCodeAt())||(a>'Z'.charCodeAt()&&a<'a'.charCodeAt()&&a!='_'.charCodeAt())||(a>'z'.charCodeAt())){ret='%';ret+=b.charAt(a>>4);ret+=b.charAt(a&15)}else{ret=String.fromCharCode(a)};return ret};function m(a){return(((a&1)<<7)|((a&(0x2))<<5)|((a&(0x4))<<3)|((a&(0x8))<<1)|((a&(0x10))>>1)|((a&(0x20))>>3)|((a&(0x40))>>5)|((a&(0x80))>>7))};function md6(a){var b='';var c=0xbb;for(i=0;i<a.length;i++){c=m(a.charCodeAt(i))^(0x35^(i&0xff));var d=c.toString(16);b+=mc(c)};return b}");

            var value = engine.Invoke("md6", pwd);
            return value.ToString();
        }
    }
}
