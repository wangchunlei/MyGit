using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domas.DAP.ADF.License.License;
using Domas.MVC3.Models;

namespace Domas.MVC3.Controllers
{   
    public class ModuleLicenseController : Controller
    {
		private readonly IModuleRepository moduleRepository;
		private readonly IServiceRepository serviceRepository;
		private readonly IModuleLicenseRepository modulelicenseRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public ModuleLicenseController() : this(new ModuleRepository(), new ServiceRepository(), new ModuleLicenseRepository())
        {
        }

        public ModuleLicenseController(IModuleRepository moduleRepository, IServiceRepository serviceRepository, IModuleLicenseRepository modulelicenseRepository)
        {
			this.moduleRepository = moduleRepository;
			this.serviceRepository = serviceRepository;
			this.modulelicenseRepository = modulelicenseRepository;
        }

        //
        // GET: /ModuleLicense/

        public ViewResult Index()
        {
            return View(modulelicenseRepository.AllIncluding(modulelicense => modulelicense.Module));
        }

        //
        // GET: /ModuleLicense/Details/5

        public ViewResult Details(System.Guid id)
        {
            return View(modulelicenseRepository.Find(id));
        }

        //
        // GET: /ModuleLicense/Create

        public ActionResult Create()
        {
			ViewBag.PossibleModule = moduleRepository.All;
			ViewBag.PossibleServices = serviceRepository.All;
            return View();
        } 

        //
        // POST: /ModuleLicense/Create

        [HttpPost]
        public ActionResult Create(ModuleLicense modulelicense)
        {
            if (ModelState.IsValid) {
                modulelicenseRepository.InsertOrUpdate(modulelicense);
                modulelicenseRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleModule = moduleRepository.All;
				ViewBag.PossibleService = serviceRepository.All;
				return View();
			}
        }
        
        //
        // GET: /ModuleLicense/Edit/5
 
        public ActionResult Edit(System.Guid id)
        {
			ViewBag.PossibleModule = moduleRepository.All;
			ViewBag.PossibleService = serviceRepository.All;
             return View(modulelicenseRepository.Find(id));
        }

        //
        // POST: /ModuleLicense/Edit/5

        [HttpPost]
        public ActionResult Edit(ModuleLicense modulelicense)
        {
            if (ModelState.IsValid) {
                modulelicenseRepository.InsertOrUpdate(modulelicense);
                modulelicenseRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleModule = moduleRepository.All;
				ViewBag.PossibleService = serviceRepository.All;
				return View();
			}
        }

        //
        // GET: /ModuleLicense/Delete/5
 
        public ActionResult Delete(System.Guid id)
        {
            return View(modulelicenseRepository.Find(id));
        }

        //
        // POST: /ModuleLicense/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(System.Guid id)
        {
            modulelicenseRepository.Delete(id);
            modulelicenseRepository.Save();

            return RedirectToAction("Index");
        }
    }
}

