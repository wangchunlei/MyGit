using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OAuthProviderMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.AcceptTypes.Contains("application/xrds+xml"))
            {
                ViewData["OPIdentifier"] = true;
                return View("Xrds");
            }

            ViewData["Message"] = "Welcome to ASP.NET MVC!";
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Xrds()
        {
            ViewData["OPIdentifier"] = true;
            return View();
        }
    }
}
