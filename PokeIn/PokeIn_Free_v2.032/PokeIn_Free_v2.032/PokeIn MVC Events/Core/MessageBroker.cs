using System.Collections.Generic;

namespace PokeInMVC_Sample.Core
{
    public class MessageBroker
    {
        //to keep simple this sample project, we defined users in a static dictionary (username - UserDefinition class pair)
        public static Dictionary<string, UserDefinition> PreUserDefinitions = new Dictionary<string, UserDefinition>();

        //Session-Username pair
        public static Dictionary<string,string> SessionUserPair = new Dictionary<string, string>();
        static MessageBroker()
        {
            //To try this sample project properly
            //login with Admin and another users on different browsers.

            PreUserDefinitions.Add("Oguz", new UserDefinition("Oguz", "1234", UserRole.User));
            PreUserDefinitions.Add("Madeline", new UserDefinition("Madeline", "1234", UserRole.User));
            PreUserDefinitions.Add("Frank", new UserDefinition("Frank", "1234", UserRole.User));
            PreUserDefinitions.Add("Eran", new UserDefinition("Eran", "1234", UserRole.User));

            PreUserDefinitions.Add("Admin", new UserDefinition("Admin", "1234", UserRole.Admin));
        }

        //Simple User Object Definer for PokeIn
        public static void SimpleDefiner(string clientId, ref Dictionary<string, object> list)
        {
            list.Add("User", new SimpleUserClass(clientId));
        }

        //Admin User Object Definer for PokeIn
        public static void AdminDefiner(string clientId, ref Dictionary<string, object> list)
        {
            list.Add("Admin", new AdminUserClass(clientId));
        }
        
        //Check out static pre defined user dictionary to compare username - password pair.
        public static bool LogUserIn(string userName, string password, out UserRole role)
        {
            //prevent multi write on same object
            lock (PreUserDefinitions)
            {
                UserDefinition definiton = null;
                PreUserDefinitions.TryGetValue(userName, out definiton);
                if (definiton != null)
                {
                    if (definiton.Password == password)
                    {
                        string sessionId = System.Web.HttpContext.Current.Session.SessionID; 

                        if (SessionUserPair.ContainsKey(sessionId))
                        {
                            if (SessionUserPair[sessionId] != userName)
                            {
                                //user might be logged in with different account on the tabs of same browser
                                //dont forget that we assigned PokeIn.Comet.CometSettings.MultiWindowsForSameSession = true; under SampleController.cs
                                //so, it's possible to a session may contains many client ids inside
                                //if the username is same for clientids its not a problem but if not
                                string [] clientIds = null;
                                try
                                {
                                    //get full client id list under the same session
                                    List<string> clientIDS = PokeIn.Comet.CometWorker.GetClientIdsBySessionId(sessionId);

                                    //convert list to array because we need consistent list. the foreach loop below may change the state of List
                                    clientIds = clientIDS.ToArray();
                                }
                                catch { }

                                if (clientIds != null)
                                {
                                    foreach (string clientId in clientIds)
                                    {
                                        BaseUserClass baseUser;
                                        PokeIn.Comet.CometWorker.GetClientObject(clientId, "User", out baseUser);
                                        
                                        if(baseUser==null)
                                            PokeIn.Comet.CometWorker.GetClientObject(clientId, "Admin", out baseUser);

                                        if (baseUser != null)
                                        {
                                            baseUser.Kicked = true;
                                        }
                                        PokeIn.Comet.CometWorker.RemoveClient(clientId);
                                    }
                                }
                            }
                            SessionUserPair.Remove(sessionId);
                        } 

                        SessionUserPair.Add(sessionId, userName); 
                        PreUserDefinitions[userName].SessionId = sessionId; 

                        role = PreUserDefinitions[userName].Role;
                        return true;
                    }
                }
            }
            role = UserRole.None;
            return false;
        }
    }
}