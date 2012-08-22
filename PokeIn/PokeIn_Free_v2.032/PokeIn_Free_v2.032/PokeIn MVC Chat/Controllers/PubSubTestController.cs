using System.Web.Mvc;
using PokeIn.Comet;
using PokeInMVC_Chat.Core;

namespace PokeInMVC_Chat.Controllers
{
    public class PubSubTestController : Controller
    {
        static PubSubTestController()
        {
            CometWorker.OnClientConnected +=new DefineClassObjects(MessageBroker.OnClientConnected); 
        } 

        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult Handler()
        {

            return View();
        }


    }
}
