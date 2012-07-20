using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace WorkTest
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            routes.MapRoute(
               name: "Default01",
               url: "DynamicPage/{action}",
               defaults: new { controller = "DynamicPage", action = "Create"}
           );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{typeFullName}",
                defaults: new { controller = "Home", action = "Index", typeFullName = UrlParameter.Optional }
            );


        }
    }
}