using System;
using System.ServiceModel.Description;
using System.ServiceModel;

namespace WCF_Server
{
    class Program
    {
        static void Main()
        {
            ServiceHost host = new ServiceHost(typeof(PokeInWCF),  new[] { new Uri("http://localhost:8090/") } );

    
            ServiceMetadataBehavior behavior = new ServiceMetadataBehavior {HttpGetEnabled = true};

            host.Description.Behaviors.Add(behavior);
            host.AddServiceEndpoint(typeof(PokeInWCF), new BasicHttpBinding(), "PokeInWCF");
            host.AddServiceEndpoint(typeof(IMetadataExchange), new BasicHttpBinding(), "ENDX");
            host.Open();
            Console.WriteLine("Press a key to terminate service");

            Console.ReadKey();
        }
    }

    [ServiceContract]
    public class PokeInWCF
    {
        [OperationContract]
        public string TestMethod(string clientId)
        {
            Console.WriteLine("Client : " + clientId + " Calls TestMethod");
            return "Hello "+clientId+"!, Server Time Is " + DateTime.Now.ToLongTimeString();
        }
    }
}
