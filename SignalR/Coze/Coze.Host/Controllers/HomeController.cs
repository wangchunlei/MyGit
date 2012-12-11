using System.Web.Mvc;

namespace Coze.Host.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
