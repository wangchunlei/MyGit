/*Copyright © PokeIn Library 2011*/
using System;
using System.Collections.Generic;
using PokeIn.Comet;

namespace Web_Controls_under_Pokein
{
    public partial class Handler : System.Web.UI.Page
    { 
        static Handler()
        {
            CometWorker.OnClientConnected += new DefineClassObjects(CometWorker_OnClientConnected);
        } 

        static void CometWorker_OnClientConnected(ConnectionDetails details, ref Dictionary<string, object> classList)
        { 
            classList.Add("Test", new Test(details.ClientId)); 
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CometWorker.Handle();
        }
    }


}
