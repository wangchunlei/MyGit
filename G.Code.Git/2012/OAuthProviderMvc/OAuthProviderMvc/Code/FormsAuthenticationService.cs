using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace OAuthProviderMvc.Code
{
    public class FormsAuthenticationService : IFormsAuthentication
    {
        public string SignedInUsername
        {
            get { return HttpContext.Current.User.Identity.Name; }
        }

        public void SignIn(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}