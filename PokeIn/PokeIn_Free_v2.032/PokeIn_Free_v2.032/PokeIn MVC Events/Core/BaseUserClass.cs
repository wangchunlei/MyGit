using System.Collections.Generic;
using System.Text;
using PokeIn.Comet;

namespace PokeInMVC_Sample.Core
{
    public delegate void AdminSentSomeApple(string userName);

    public class BaseUserClass
    {
        public bool Kicked;
        protected string ClientId;
        protected UserDefinition userDefinition = null;

        //Event Oriented Approach BEGIN
        private static AdminSentSomeApple appleEvent;
        public static event AdminSentSomeApple AppleSent
        {
            add { appleEvent += value; }
            remove { appleEvent -= value; }
        }
        public static void RaiseAppleSent(string userName)
        {
            //let the client know that Admin sent some apple 
            appleEvent.Invoke(userName);
        }
        //Event Oriented Approach END

        public BaseUserClass(string clientId)
        {
            ClientId = clientId;
        }

        //PokeIn removes session association when user's connection closed somehow
        ~BaseUserClass()
        {
            if (userDefinition!=null)//User may close the window before StartListenForEvents call
            {
                //User's another session may logged in on another tab or window
                if (!Kicked)
                {
                    lock (MessageBroker.PreUserDefinitions)
                    {
                        MessageBroker.SessionUserPair.Remove(MessageBroker.PreUserDefinitions[userDefinition.Username].SessionId);
                        MessageBroker.PreUserDefinitions[userDefinition.Username].SessionId = "";
                    }
                }
            } 
        }

        //Client side calls this method to mark his status and start listen for his events
        public void StartListenForEvents()
        {
            string sessionId = CometWorker.GetSessionId(ClientId);

            //to make consistent log-in process lock the user status
            lock (MessageBroker.PreUserDefinitions)
            {
                string userName;
                MessageBroker.SessionUserPair.TryGetValue(sessionId, out userName);

                if (userName == null)//User Session Lost
                {
                    PokeIn.Comet.BrowserHelper.RedirectPage(ClientId, "/Sample/Home");
                    return;
                } 

                MessageBroker.PreUserDefinitions.TryGetValue(userName, out userDefinition);
            }

            string jsonMethod = PokeIn.JSON.Method("SetTitle", "Hello " + userDefinition.Username + "! ");
            CometWorker.SendToClient(ClientId, jsonMethod);


            //Call Role Based User Initializitation
            InitializeUser();
        }

        protected virtual void InitializeUser() { }
    }
}