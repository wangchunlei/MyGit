using System;
using System.Collections.Generic;
using System.Threading; 
using PokeIn;
using PokeIn.Comet;

namespace Message_Grouping_Sample 
{
    public partial class Handler : System.Web.UI.Page
    {
        static Handler()
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
        }

        static void CometWorker_OnClientConnected(ConnectionDetails details, ref Dictionary<string, object> classList)
        {
            classList.Add("StockDemo", new MyStockDemo(details.ClientId));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CometWorker.Handle();
        }
    }
}
