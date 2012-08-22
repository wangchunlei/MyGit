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
