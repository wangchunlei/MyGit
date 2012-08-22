using System.Collections.Generic;
using System.Text;
using PokeIn.Comet;
using PokeIn;

namespace PokeInMVC_Sample.Core
{
    public class UsersPool
    {

        //User Request Pool - userName - requestCount pair
        public static Dictionary<string, int> Requests = new Dictionary<string, int>();
        public static void RequestApple(string userName)
        {
            lock (Requests)
            {
                if (Requests.ContainsKey(userName))
                    Requests[userName]++;
                else
                    Requests.Add(userName, 1);

                UpdateAdminScreens(userName, Requests[userName]);
            }
        }

        //Admin screen might be open on different browsers or machines
        //So we need a client list
        public static List<string> AdminClientIDs = new List<string>();

        public static void UpdateAdminScreens(string userName, int appleCount)
        {
            lock (AdminClientIDs)
            {
                if (AdminClientIDs.Count > 0)//admin logged in
                {
                    string jm = JSON.Method("UpdateUserRequest", userName, appleCount);

                    //send message to each admin screen
                    CometWorker.SendToClients(AdminClientIDs.ToArray(), jm);
                }
            }
        }
    }
}