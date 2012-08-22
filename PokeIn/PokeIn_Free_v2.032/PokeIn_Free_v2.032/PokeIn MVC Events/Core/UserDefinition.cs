using System.Collections.Generic;
using System.Text;
using PokeIn.Comet;

namespace PokeInMVC_Sample.Core
{
    public class UserDefinition
    {
        public string Username, Password;
        public UserRole Role; 
        public string SessionId = "";

        public int Apples = 0;

        public UserDefinition(string userName, string password, UserRole role)
        {
            Username = userName;
            Password = password;
            Role = role;
        }
    }

    public enum UserRole
    {
        None = 0,
        Admin = 1,
        User = 2
    }
}