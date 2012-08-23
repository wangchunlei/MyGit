using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Domas.DAP.ADF.NotifierClient;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //PresisTest();
            //HubTest();
            var cookieContainer = new CookieContainer();
            var cookie = new Cookie("user", "admin");
            cookie.Domain = "localhost";
            cookieContainer.Add(cookie);
            NotifierClient.StartListen("NotifierServer", "http://localhost", cookieContainer, ReData);

            Console.ReadKey();
        }
        //private static void HubTest()
        //{
        //    var hubConnection = new HubConnection("http://localhost");
        //    //hubConnection.Credentials = new NetworkCredential("admin", "lanxum1234");
        //    //hubConnection.Credentials = new CredentialCache();
        //    hubConnection.CookieContainer = new CookieContainer();
        //    var cookie = new Cookie("user", "admin");
        //    cookie.Domain = "localhost";
        //    hubConnection.CookieContainer.Add(cookie);
        //    var chat = hubConnection.CreateProxy("NotifierServer");

        //    chat.On("addMessage", message => Reciew(message));
        //    chat.On("addData", data => ReData(data));
        //    hubConnection.Start().Wait();

        //    string line = null;
        //    while ((line = Console.ReadLine()) != null)
        //    {
        //        // Send a message to the server
        //        chat.Invoke("Init", line).Wait();
        //    }
        //}
        //private static void Reciew(dynamic data)
        //{
        //    Console.WriteLine(data.ToString());
        //}

        private static void ReData(Domas.DAP.ADF.NotifierDeploy.NotifierDTO message)
        {

        }
        //private static void PresisTest()
        //{
        //    var connection = new Connection("http://localhost:22431/echo");

        //    connection.Received += data => Console.WriteLine(data);

        //    connection.Start().Wait();

        //    string line = null;

        //    while ((line = Console.ReadLine()) != null)
        //    {
        //        connection.Send(line).Wait();
        //    }
        //}
    }
}
