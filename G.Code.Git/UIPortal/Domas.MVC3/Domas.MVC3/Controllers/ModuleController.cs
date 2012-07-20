using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domas.DAP.ADF.License.Module;
using Domas.MVC3.Models;

namespace Domas.MVC3.Controllers
{   
    public class ModuleController : Controller
    {
		private readonly IServiceRepository serviceRepository;
		private readonly IModuleRepository moduleRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public ModuleController() : this(new ServiceRepository(), new ModuleRepository())
        {
        }

        public ModuleController(IServiceRepository serviceRepository, IModuleRepository moduleRepository)
        {
			this.serviceRepository = serviceRepository;
			this.moduleRepository = moduleRepository;
        }

        //
        // GET: /Module/

        public ViewResult Index()
        {
            return View(moduleRepository.AllIncluding(module => module.Service));
        }

        //
        // GET: /Module/Details/5

        public ViewResult Details(System.Guid id)
        {
            return View(moduleRepository.Find(id));
        }

        //
        // GET: /Module/Create

        public ActionResult Create()
        {
			ViewBag.PossibleServices = serviceRepository.All;
            return View();
        } 

        //
        // POST: /Module/Create

        [HttpPost]
        public ActionResult Create(Module module)
        {
            if (ModelState.IsValid) {
                moduleRepository.InsertOrUpdate(module);
                moduleRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleServices = serviceRepository.All;
				return View();
			}
        }
        
        //
        // GET: /Module/Edit/5
 
        public ActionResult Edit(System.Guid id)
        {
			ViewBag.PossibleServices = serviceRepository.All;
             return View(moduleRepository.Find(id));
        }

        //
        // POST: /Module/Edit/5

        [HttpPost]
        public ActionResult Edit(Module module)
        {
            if (ModelState.IsValid) {
                moduleRepository.InsertOrUpdate(module);
                moduleRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleService = serviceRepository.All;
				return View();
			}
        }

        //
        // GET: /Module/Delete/5
 
        public ActionResult Delete(System.Guid id)
        {
            return View(moduleRepository.Find(id));
        }

        //
        // POST: /Module/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(System.Guid id)
        {
            moduleRepository.Delete(id);
            moduleRepository.Save();

            return RedirectToAction("Index");
        }
    }
}

