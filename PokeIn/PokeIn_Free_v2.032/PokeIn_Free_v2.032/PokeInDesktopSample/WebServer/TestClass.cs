using System;
using System.Threading;
using PokeIn;
using PokeIn.Comet;
using SharedObjects;

namespace WebServer
{
    public class ServerInstance : IDisposable
    {
        public bool IsDesktop = false;
        public string ClientId = "";
        public ServerInstance(string clientId, bool isDesktop)
        {
            IsDesktop = isDesktop;
            ClientId = clientId;

            string message = "";

            //if you call a method from DesktopClient you must serialize it by EXTML class
            if (IsDesktop)
                message = EXTML.Method("ParameterTest", new TestClass());//calls a method from DesktopTest class defined on Desktop application
            else
                message = JSON.Method("ParameterTest", new TestClass());

            CometWorker.SendToClient(ClientId, message);
        }

        //static ServerInstance()
        //{
        //    ////Define image resource
        //    //ResourceManager.AddResource(CometWorker.GetApplicationPath() + "pokein_logo.gif" //resource location
        //    //    , "Logo" //public name of the resource
        //    //    , ResourceType.Image //Image type
        //    //    , string.Empty //This resource is application wide
        //    //    );

        //    //string message = "PokeIn Library";
        //    //byte[] bt = CometSettings.SerializationEncoding.GetBytes(message);
        //    ////Define text resource
        //    //ResourceManager.AddResource(ref bt
        //    //    , "Message" //public name of the resource
        //    //    , ResourceType.Text //Text type
        //    //    , "txt" //file type
        //    //    , string.Empty //This resource is application wide
        //    //    );

        //    new Thread(delegate()
        //    {
        //        while (!CometWorker.IsApplicationRecycling)
        //        {
        //            if (CometWorker.Groups.GroupHasMembers("ServerTime-Desktop"))
        //            {
        //                string ext = EXTML.Method("ServerTimeUpdated", DateTime.Now);
        //                CometWorker.Groups.Send("ServerTime-Desktop", ext);
        //            }

        //            if (CometWorker.Groups.GroupHasMembers("ServerTime-Web"))
        //            {
        //                string json = JSON.Method("ServerTimeUpdated", DateTime.Now);
        //                CometWorker.Groups.Send("ServerTime-Web", json);
        //            }

        //            Thread.Sleep(800);
        //        }
        //    }).Start();
        //}

        public void ClassTest(TestClass tc, DateTime dt)
        {
            string message = "";
            if (IsDesktop)
            {
                message = EXTML.Method("TestClassReceived", tc.number, tc.items.Count, tc.text, dt.ToShortDateString());
            }
            else
            {
                message = JSON.Method("TestClassReceived", tc.number, tc.items.Count, tc.text, dt.ToShortDateString());
            }
            CometWorker.SendToClient(ClientId, message);
        }

        public void Subscribe()
        {
            string message = "";
            if (IsDesktop)
            {
                CometWorker.Groups.PinClientID(ClientId, "ServerTime-Desktop");
                message = EXTML.Method("Subscribed");
            }
            else
            {
                CometWorker.Groups.PinClientID(ClientId, "ServerTime-Web");
                message = JSON.Method("Subscribed");
            }

            CometWorker.SendToClient(ClientId, message);
        }

        public void UnSubscribe()
        {
            CometWorker.Groups.UnpinClient(ClientId);
            string message = "";
            if (IsDesktop)
            {
                message = EXTML.Method("UnSubscribed");
            }
            else
            {
                message = JSON.Method("UnSubscribed");
            }

            CometWorker.SendToClient(ClientId, message);
        }

        public void Dispose()
        {
            //There is no need to unsubscribe from group here. PokeIn automatically handles it
        }
    }
}