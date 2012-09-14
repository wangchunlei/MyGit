using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HubService.Models;

namespace HubService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            NotifierServer.Send("123");
            return View();
        }
    }
}
