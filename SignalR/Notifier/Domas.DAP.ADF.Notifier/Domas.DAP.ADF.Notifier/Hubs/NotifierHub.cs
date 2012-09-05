using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domas.DAP.ADF.Cookie;
using Domas.DAP.ADF.Notifier.Services;
using SignalR.Hubs;

namespace Domas.DAP.ADF.Notifier.Hubs
{
    public class NotifierHub : Hub, IConnected, IDisconnect
    {
        private readonly INotifierRepository _repository;
        private static readonly Version _version = typeof(NotifierHub).Assembly.GetName().Version;
        private static readonly string _versionString = _version.ToString();

        private string UserAgent
        {
            get
            {
                if (Context.Headers != null)
                {
                    return Context.Headers["User-Agent"];
                }
                return null;
            }
        }

        private bool OutOfSync
        {
            get
            {
                string version = Caller.version;
                return string.IsNullOrEmpty(version) || new Version(version) != _version;
            }
        }
        public bool Join()
        {
            SetVersion();
            UserInfoDTO userInfo = GetUserInfoFromCookie();

            
            return true;
        }

        private void SetVersion()
        {
            Caller.version = _versionString;
        }

        private string GetCurrentUserFromContext()
        {
            return Domas.DAP.ADF.Context.ContextFactory.GetCurrentContext().LoginID;
        }

        private UserInfoDTO GetUserInfoFromCookie()
        {
            var userCookie = Context.RequestCookies["UserInfo"];
            if (userCookie != null)
            {
                var userInfo = CookieManger.DecryCookie(userCookie.Value);
                return userInfo;
            }
            return null;
        }
        public Task Connect()
        {
            throw new NotImplementedException();
        }

        public Task Reconnect(IEnumerable<string> groups)
        {
            throw new NotImplementedException();
        }

        public Task Disconnect()
        {
            throw new NotImplementedException();
        }
    }
}
