using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PokeIn.Comet;

namespace PokeIn_Silverlight.Web
{
    public partial class Default : System.Web.UI.Page
    {
        //runs one time
        static Default()
        {
            CometWorker.OnClientConnected += new DefineClassObjects(Definer.Entry);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}