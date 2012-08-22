using System;
using System.Collections.Generic;
using System.Text;
using PokeIn.Comet;

namespace PokeInMVC_Sample.Core
{
    //Both AdminUserClass and SimpleUserClass inherits same BaseUserClass
    //to define common functionalities efficiently
    public class SimpleUserClass : BaseUserClass, IDisposable
    {
        internal AdminSentSomeApple listener;

        //PokeIn searches for this function to dispose object
        //You may not remove event assignment inside destructor
        //Because GC may not finalize an object with active listener
        public void Dispose()
        {
            BaseUserClass.AppleSent -= listener;
        }

        protected override void InitializeUser()
        {
            //let user know, how many apple he has
            AdminSent(userDefinition.Username);
        }

        //new simple user logged in and its time to associate his instance with Admin events
        public SimpleUserClass(string clientId)
            : base(clientId)
        {
            listener = new AdminSentSomeApple(AdminSent);
            BaseUserClass.AppleSent += listener;
        } 

        //Event Fired!
        void AdminSent(string userName)
        {
            if (userName == userDefinition.Username)
            { 
                //update our definition
                MessageBroker.PreUserDefinitions.TryGetValue(userName, out userDefinition);

                int countLeft = 0;
                lock (UsersPool.Requests)
                {
                    UsersPool.Requests.TryGetValue(userDefinition.Username, out countLeft);
                }

                CometWorker.SendToClient(ClientId, "AdminSentApple(" + userDefinition.Apples.ToString() + "," + countLeft.ToString() + ");");
            }
        }

        //Request more apple
        public void RequestSomeApple()
        {
            UsersPool.RequestApple(userDefinition.Username);
            AdminSent(userDefinition.Username);
        }
    }
}