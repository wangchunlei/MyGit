using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SignalR;

namespace Domas.DAP.ADF.Notifier
{
    public class SignalRConnectionFactory : IConnectionIdGenerator
    {
        public string GenerateConnectionId(IRequest request)
        {
            if (request.Cookies["UserInfo"] != null)
            {
                var loginId = Domas.DAP.ADF.Context.ContextFactory.GetCurrentContext().LoginID;
                return loginId;
            }
            return Guid.NewGuid().ToString();
        }
    }
}
