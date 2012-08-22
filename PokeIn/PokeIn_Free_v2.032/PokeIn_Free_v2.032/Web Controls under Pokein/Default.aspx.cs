/*Copyright © PokeIn Library 2011*/

using System;
using PokeIn;
using PokeIn.Comet;

namespace Web_Controls_under_Pokein
{
    public partial class _Default : System.Web.UI.Page
    {  
        protected void Page_Load(object sender, EventArgs e)
        { 
        }
 
        public void Button1_Click(object sender, EventArgs e)
        {
            TextBox1.Text = DateTime.Now.ToLongTimeString();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Test test;
            string clientId = this.HiddenField1.Value;

            if (clientId != "")
            {
                //get the pokein instance
                CometWorker.GetClientObject<Test>(clientId, "Test", out test);

                //call pokein method of this instance
                test.GetTime();
            }
        }
    }

    public class Test : IDisposable
    {
        internal string clientID = "";
        
        public Test(string clid)
        {
            clientID = clid;
        }

        public void Dispose()
        { 
        }

        public void GetTime()
        {
            string str = JSON.Method("UpdateTime", DateTime.Now.ToLongTimeString() + " Poked!");
            CometWorker.SendToClient(clientID, str);
        } 
    }
}
