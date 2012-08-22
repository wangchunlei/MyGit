using System.Collections.Generic;
using System.Web.Mvc;
using PokeIn.Comet;
using PokeInMVC_Sample.Core;

namespace PokeInMVC_Sample.Controllers
{
    public class SampleController : Controller
    {
        static SampleController()
        {
            CometWorker.OnFirstJoint += new FirstJointObjects(CometWorker_OnFirstJoint);
            CometWorker.OnClientJoined += new ClientJoinDelegate(CometWorker_OnClientJoined);
        }

        static void CometWorker_OnClientJoined(ConnectionDetails details, string jointId, ref Dictionary<string, object> classList)
        {
            //in this sample project we used jointId to carry session ids to locate the user
            string sessionId = jointId;

            UserDefinition userDefinition = null;

            lock (MessageBroker.PreUserDefinitions)
            {
                string userName;
                MessageBroker.SessionUserPair.TryGetValue(sessionId, out userName);

                if (userName == null) 
                { 
                    return;
                }

                MessageBroker.PreUserDefinitions.TryGetValue(userName, out userDefinition);
            }

            //independent object instance
            if(userDefinition.Role== UserRole.User)
                classList.Add("User", new SimpleUserClass(details.ClientId));
            else
                classList.Add("Admin", new AdminUserClass(details.ClientId));
        }

        static void CometWorker_OnFirstJoint(ConnectionDetails details, string jointId, ref Dictionary<string, object> classList)
        {
            //Shared object instance for all the joint members
            //In this sample there is no shared instance but we want use Joint feature for other reasons
        } 

        public ActionResult SimpleUser()
        {
            return View();
        }

        public ActionResult AdminUser()
        {
            return View();
        }

        public ActionResult Handler()
        {

            return View();
        }

        public ActionResult Home()
        {
            LoginCheck();
            return View();
        }

        public void LoginCheck()
        {
            string username = Request.Params["Username"];
            string password = Request.Params["Password"];
            if(username==null || password==null)
                return;

            UserRole role;
            if (!MessageBroker.LogUserIn(username, password, out role))
            {
                return;
            }

            //logged in
            //you should check this role on target page for real life usage
            if (role == UserRole.Admin)
                Response.Redirect("/Sample/AdminUser");
            else
                Response.Redirect("/Sample/SimpleUser");
        }

    }
}
