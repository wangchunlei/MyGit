using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PokeIn.Comet;

namespace SQLSample
{
    public partial class _Default : System.Web.UI.Page
    {
        static _Default()
        {
            CometWorker.OnClientConnected += new DefineClassObjects(CometWorker_OnClientConnected);
        }

        static void CometWorker_OnClientConnected(ConnectionDetails details, ref Dictionary<string, object> classList)
        {
            classList.Add("SQL", new SQLInterface(details.ClientId));
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        } 
    }
}
