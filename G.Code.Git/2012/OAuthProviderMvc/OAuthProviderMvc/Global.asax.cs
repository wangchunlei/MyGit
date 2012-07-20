using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OAuthProviderMvc
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

        private static object behaviorInitializationSyncObject = new object();

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            InitializeBehaviors();
        }

        private static void InitializeBehaviors()
        {
            if (DotNetOpenAuth.OpenId.Provider.Behaviors.PpidGeneration.PpidIdentifierProvider == null)
            {
                lock (behaviorInitializationSyncObject)
                {
                    if (DotNetOpenAuth.OpenId.Provider.Behaviors.PpidGeneration.PpidIdentifierProvider == null)
                    {
                        DotNetOpenAuth.OpenId.Provider.Behaviors.PpidGeneration.PpidIdentifierProvider = new Code.AnonymousIdentifierProvider();
                        DotNetOpenAuth.OpenId.Provider.Behaviors.GsaIcamProfile.PpidIdentifierProvider = new Code.AnonymousIdentifierProvider();
                    }
                }
            }
        }
    }
}