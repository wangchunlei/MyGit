using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domas.Web.Tools.PortableAreas;

namespace LoginPartialArea.Login.Messages
{
    public class RegistrationMessage:IEventMessage
    {
        private string _message;
        public RegistrationMessage(string message)
        {
            _message = message;
        }

        public override string ToString()
        {
            return _message;
        }
    }
}
