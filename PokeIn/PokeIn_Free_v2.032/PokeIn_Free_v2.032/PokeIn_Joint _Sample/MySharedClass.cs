using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PokeIn;
using PokeIn.Comet;

namespace PokeIn_Joint_Sample
{
        public class MySharedClass:IDisposable
    {
        private string _jointId;
        public MySharedClass(string jointId)
        {
            _jointId = jointId;
        }

        public void Dispose()
        {
            //Do something
        }

        private int number = 0;
        public void Test()
        {
            //the below number will be increased for all the members of this joint instance
            number++;

            UpdateJoint();
        }

        public void UpdateJoint()
        {
            string message = JSON.Method("SharedNumber", number);

            //Send the above message directly to the joint
            CometWorker.SendToAll(message);
        }
    }
}
