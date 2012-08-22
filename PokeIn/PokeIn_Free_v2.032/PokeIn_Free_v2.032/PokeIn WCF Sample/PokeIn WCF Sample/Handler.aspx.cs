using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PokeIn.Comet;

namespace PokeIn_WCF_Sample
{
    public partial class Handler : System.Web.UI.Page
    {
        static Handler()
        {
            CometWorker.OnClientConnected += new DefineClassObjects(CometWorker_OnClientConnected);
        }

        static void CometWorker_OnClientConnected(ConnectionDetails details, ref Dictionary<string, object> classList)
        {
            classList.Add("WCFSample", new WCFSample(details.ClientId));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CometWorker.Handle();
        }
    }
}
