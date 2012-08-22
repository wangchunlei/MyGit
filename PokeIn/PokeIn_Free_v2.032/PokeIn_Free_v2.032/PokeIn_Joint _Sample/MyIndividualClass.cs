using System; 
using PokeIn;
using PokeIn.Comet;

namespace PokeIn_Joint_Sample
{
    public class MyIndividualClass : IDisposable
    {
        private string _clientId;
        private string _jointId;

        public MyIndividualClass(string clientId, string jointId)
        {
            _clientId = clientId;
            _jointId = jointId;
            
        }

        public void Dispose()
        {
            //Do something
        }

        private int number = 0;
        public void Test()
        {
            //the below number will be increased for only the owner of this instance
            number++;
            string message = JSON.Method("NonSharedNumber", _clientId, number);

            //Send the above message directly to the client
            CometWorker.SendToJoint(_jointId, message);
        }
    }
}

