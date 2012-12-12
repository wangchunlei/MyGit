using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;
namespace Coze.Host
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapHubs();

            routes.MapRoute(
                name: "Default01",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}