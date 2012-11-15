using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using RestSharp;

namespace RestTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("http://192.168.100.254");
            var request = new RestRequest("", Method.POST);

            var username = args[0];
            var password = args[1];
            request.AddParameter("username", username);
            request.AddParameter("password", password);
            request.AddParameter("password_enc", "bHgzMTQxMTQ%3D&newpassword_enc=");
            request.AddParameter("login", "1");
            request.AddParameter("login_type", "login");
            request.AddParameter("uri", "aHR0cDovLzE5Mi4xNjguNjAuNjUv");
            request.AddParameter("password_type", "normal");

            var response = client.Execute(request);

            var content = response.Content;
            Console.WriteLine(content);
            Console.WriteLine("完成");
            Console.ReadKey(false);
        }
    }
}
