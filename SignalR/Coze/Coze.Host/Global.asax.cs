using System;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Coze.Host
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var principal = Context.Cache[authTicket.Name] as IPrincipal;
                if (!authTicket.Expired && principal != null)
                {
                    Context.User = principal;
                }
            }
        }   
    }
}