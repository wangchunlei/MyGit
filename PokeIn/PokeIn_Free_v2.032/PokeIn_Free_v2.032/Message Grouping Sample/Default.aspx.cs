using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PokeIn;
using PokeIn.Comet;

namespace Message_Grouping_Sample
{
    public partial class _Default : System.Web.UI.Page
    {
        static _Default()
        {
            //our random price generator
            new Thread(delegate()
            {
                Random rndPrices = new Random();
                while (true)
                {
                    string message = JSON.Method("UpdatePrices", "Stock1", rndPrices.Next(0, 99));
                    CometWorker.Groups.Send("Stock1", message);
                    Thread.Sleep(1000); 
                    //Generate a new random stock price every 1 second and send it to the channel
                    //Deploy this test application onto IIS to get better results. for example, try for 0.2 second
                }
            }).Start();

            CometWorker.OnClientConnected += new DefineClassObjects(CometWorker_OnClientConnected);

            CometWorker.OnClientCreated += new ClientCreatedDelegate(CometWorker_OnClientCreated);
        }

        static void CometWorker_OnClientConnected(ConnectionDetails details, ref Dictionary<string, object> classList)
        {
            //There is a new connection attempt from the browser side and
            //PokeIn wants you to define a class for this connection request
            classList.Add("StockDemo", new MyStockDemo(details.ClientId)); 
            //Please notice that the connection has not completed in this step yet.
            //If you need the exact moment of "client connection completed" then you should be listening OnClientCreated event
        }

        static void CometWorker_OnClientCreated(string clientId)
        {
            //Client connection is done
            string message = JSON.Method("UpdateString", "Now, you are connected!");
            CometWorker.SendToClient(clientId, message);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }

    public class MyStockDemo:IDisposable
    {
        string _clientId;
        public MyStockDemo(string clientId)
        {
            _clientId = clientId;
        }

        public void Dispose()
        {
            //PokeIn will call this method after client is disconnected
        }

        //it is not important how many client is listening for this channel. Let PokeIn manage the messages
        //please notice that the FREE edition of PokeIn supports up to 10 concurrent connection
        //Get Commercial edition to break the limits
        public void JoinChannel()
        {
            CometWorker.Groups.PinClientID(_clientId, "Stock1");
            CometWorker.SendToClient(_clientId, "Pinned();");
        }

        public void LeaveChannel()
        {
            CometWorker.Groups.UnpinClient(_clientId, "Stock1");
            CometWorker.SendToClient(_clientId, "Unpinned();");
        }
    }
}
