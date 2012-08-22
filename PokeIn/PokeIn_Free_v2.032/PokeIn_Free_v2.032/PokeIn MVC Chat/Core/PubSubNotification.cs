using System;
using System.Collections.Generic;
using System.Text;
using PokeIn;
using PokeIn.Comet;

namespace PokeInMVC_Chat.Core
{
    public class PubSubNotification:IDisposable
    {
        private const string Userlistfmt = "<option value=\'{0}\'>{1}</option>";
        public static Dictionary<string, string> Users = new Dictionary<string, string>();
        public static Dictionary<string, string> Names = new Dictionary<string, string>();
        private readonly string _clientID;
        private string _username;

        public PubSubNotification(string clientId)
        {
            _clientID = clientId;
            _username = "";
        }

        public void Dispose()
        {
            lock (Names)
            {
                Users.Remove(_clientID);
                Names.Remove(_username);
            }
            RenderMemberList();
            CometWorker.SendToAll( JSON.Method("AppendToChat", "<strong>" + _username + " has left the building!</strong>") );
        }

        public void SetName(string userName)
        {
            if (_username != "")
            {
                CometWorker.SendToClient(_clientID, "alert('You already have a username!');");
                return;
            }
            lock (Names)
            {
                if (Names.ContainsKey(userName))
                {
                    CometWorker.SendToClient(_clientID, "alert('Another user is using the name you choose! Please try another one.');");
                    return;
                }
                else
                {
                    Names.Add(userName, _clientID);
                    Users.Add(_clientID, userName);
                    _username = userName;
                }
            }

            CometWorker.SendToClient(_clientID, JSON.Method("UsernameSet", PokeIn.JSON.Tidy(userName)));
            CometWorker.SendToAll(JSON.Method("AppendToChat", "<strong>" + PokeIn.JSON.Tidy(userName) + " has enterred the building!</strong>"));
            RenderMemberList();
        }

        public void Send(string message)
        {
            string json = PokeIn.JSON.Method("ChatMessageFrom", _username, message);
            CometWorker.SendToAll(json);
        }

        public void SendPrivateMessage(string destinationUser, string message)
        {  
            string privateMessage = "<strong>" + GetUserName(_clientID) + "</strong>::" + message;
            string json = PokeIn.JSON.Method("HandlePrivateMessage", privateMessage);
            CometWorker.SendToClient(destinationUser, json);
        }

        public void RenderMemberList()
        {
            CometWorker.SendToAll("GenerateMemberList('" + PokeIn.JSON.Tidy(UserList()) + "');");
        }

        private string GetUserName(string userID)
        {
            if (Users.ContainsKey(userID))
            {
                return Users[userID];
            }
            return "";
        }

        private string UserList()
        {
            var sb = new StringBuilder();

            var nicks = new string[Users.Count];
            Users.Values.CopyTo(nicks, 0);

            foreach (string user in nicks)
            {
                sb.Append(string.Format(Userlistfmt, Names[user], user));
            }

            return sb.ToString();
        }
    }
}