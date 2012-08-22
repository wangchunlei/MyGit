using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PokeIn.Comet;

namespace PokeInMSDOS
{
    public partial class _Default : System.Web.UI.Page
    {
        static _Default()
        {
            CometWorker.OnClientConnected += new DefineClassObjects(CometWorker_OnClientConnected);
        }

        static void CometWorker_OnClientConnected(ConnectionDetails details, ref Dictionary<string, object> classList)
        {
            classList.Add("Console", new MSDOS(details.ClientId));
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
