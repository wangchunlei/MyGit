using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using SignalR;

namespace HubService
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest() || IsNotifierRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }

        protected void Application_AcquireRequestState()
        {
            if (IsWebApiRequest() || IsNotifierRequest())
            {
                var session = HttpContext.Current.Session;

                if (session == null)
                {
                    throw new Exception("Session 已经超时");
                }
            }
        }

        private static bool IsWebApiRequest()
        {
            const string webApiPrefix = "api";
            string webApiExecutionPath = String.Format("~/{0}", webApiPrefix);

            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(webApiExecutionPath);
        }
        private static bool IsNotifierRequest()
        {
            const string notifierPrefix = "signalr";
            string notifierExecutionPath = String.Format("~/{0}", notifierPrefix);

            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(notifierExecutionPath);
        }

       
    }
}