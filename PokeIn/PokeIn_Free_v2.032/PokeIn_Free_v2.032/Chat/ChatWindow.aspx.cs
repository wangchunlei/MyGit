using System;
using System.Collections.Generic;
using PokeIn.Comet;

namespace ChatSample
{
    public partial class _Default : System.Web.UI.Page
    {
        //We don't suggest you to implement/define below listeners inside the static constructor of your
        //main page. Please consider using Global.asax as shown in ServerTimeSample project
        static _Default()
        {
            //Chunked mode allows multiple message using same listener request
            CometSettings.ChunkedMode = true;

            //You must define a listener for below event
            CometWorker.OnClientConnected += new DefineClassObjects(CometWorker_OnClientConnected);
        }

        static void CometWorker_OnClientConnected(ConnectionDetails details, ref Dictionary<string, object> classList)
        {
            classList.Add("Chat", new ChatApp(details.ClientId));
        } 

        protected void Page_Load(object sender, EventArgs e)
        {

        } 
    }
}
