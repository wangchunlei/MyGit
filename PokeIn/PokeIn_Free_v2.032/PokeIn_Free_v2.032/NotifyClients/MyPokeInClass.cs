using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotifyClients
{
    /// <summary>
    /// Because this sample project demonstrates only Server to Client calls,
    /// we don't implement any method inside the below class (only necessary parts PokeIn needs)
    /// </summary>
    public class MyPokeInClass:IDisposable
    {
        //empty constructor
        public MyPokeInClass()
        {
        }

        internal string clientId;
        public MyPokeInClass(string _clientId)
        {
            clientId = _clientId;
        }

        public void Dispose()
        {
            //PokeIn calls this method when the client is disconnected.
        }
    }
}
