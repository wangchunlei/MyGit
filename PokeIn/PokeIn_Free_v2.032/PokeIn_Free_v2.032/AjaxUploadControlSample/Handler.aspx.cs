using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AjaxUploadControlSample
{
    public partial class Handler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check UserLogin etc
            PokeIn.Comet.CometWorker.Handle();
        }
    }
} 
