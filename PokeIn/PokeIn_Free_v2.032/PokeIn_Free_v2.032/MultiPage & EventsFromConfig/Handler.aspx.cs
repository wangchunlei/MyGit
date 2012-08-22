using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EventsFromConfig
{
    using PokeIn.Comet;

    public partial class Handler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //UploadControl Handler
            CometWorker.Handle();
        }
    }
}