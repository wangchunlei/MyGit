using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC3_Razor_CS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC! (PokeIn Sample)";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        //PokeIn: Handler page is defined here
        public ActionResult Handler()
        {
            return View();
        }
    }
}
