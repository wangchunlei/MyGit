using System.Collections.Generic;
using System.Text;
using PokeIn.Comet;

namespace PokeInMVC_Sample.Core
{
    public class AdminUserClass:BaseUserClass
    { 
        public AdminUserClass(string clientId):base(clientId)
        { 
        }

        ~AdminUserClass()
        {
            //remove this view from active admin views
            lock (UsersPool.AdminClientIDs)
            {
                UsersPool.AdminClientIDs.Remove(ClientId);
            }
        }

        protected override void InitializeUser()
        {
            //Admin screen might be open on different browsers or machines
            UsersPool.AdminClientIDs.Add(ClientId);//Admin logged in 

            //let him know for waiting user requests
            lock (UsersPool.Requests)
            {
                string jsonMethod = PokeIn.JSON.Method("RequestList", UsersPool.Requests);
                CometWorker.SendToClient(ClientId, jsonMethod);
            }
        }

        public void SendApple(string userName, int count)
        {
            lock (UsersPool.Requests)
            {
                //decrease the amount of user request admin approved
                if (UsersPool.Requests.ContainsKey(userName))
                {
                    UsersPool.Requests[userName] -= count;

                    //update all admin views
                    UsersPool.UpdateAdminScreens(userName, UsersPool.Requests[userName]);
                } 
            }
            lock (MessageBroker.PreUserDefinitions)
            {
                if (MessageBroker.PreUserDefinitions.ContainsKey(userName))
                {
                    MessageBroker.PreUserDefinitions[userName].Apples += count;
                }
            }
            BaseUserClass.RaiseAppleSent(userName);
        }
    }
}