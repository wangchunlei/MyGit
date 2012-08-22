using System;
using System.Collections.Generic; 
using PokeIn.Comet;

namespace PokeIn_Joint_Sample
{
    public partial class Handler : System.Web.UI.Page
    {
        static Handler()
        {
            CometWorker.OnFirstJoint += new FirstJointObjects(CometWorker_OnFirstJoint);
            CometWorker.OnClientJoined += new ClientJoinDelegate(CometWorker_OnClientJoined);
        } 

        static void CometWorker_OnClientJoined(ConnectionDetails details, string jointId, ref Dictionary<string, object> classList)
        {
            //The below class will be unique for each member of joint
            //this is optional, not a requirement for joints
            classList.Add("MyIndividualClass", new MyIndividualClass(details.ClientId, jointId)); 
        }

        //The below event will be fired when a specific named client opens a connection for first time
        static void CometWorker_OnFirstJoint(ConnectionDetails details, string jointId, ref Dictionary<string, object> classList)
        {
            //The below class instance will be shared by all the members of joint
            //You should add at least 1 instance to the classList parameter
            classList.Add("MySharedClass", new MySharedClass(jointId));
        } 

        protected void Page_Load(object sender, EventArgs e)
        {
            CometWorker.Handle();
        }
    } 
}
