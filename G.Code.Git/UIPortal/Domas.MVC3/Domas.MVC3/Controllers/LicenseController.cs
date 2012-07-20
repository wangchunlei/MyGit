using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domas.DAP.ADF.License.License;
using Domas.MVC3.Models;

namespace Domas.MVC3.Controllers
{   
    public class LicenseController : Controller
    {
		private readonly ILicenseRepository licenseRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public LicenseController() : this(new LicenseRepository())
        {
        }

        public LicenseController(ILicenseRepository licenseRepository)
        {
			this.licenseRepository = licenseRepository;
        }

        //
        // GET: /License/

        public ViewResult Index()
        {
            return View(licenseRepository.All);
        }

        //
        // GET: /License/Details/5

        public ViewResult Details(System.Guid id)
        {
            return View(licenseRepository.Find(id));
        }

        //
        // GET: /License/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /License/Create

        [HttpPost]
        public ActionResult Create(License license)
        {
            if (ModelState.IsValid) {
                licenseRepository.InsertOrUpdate(license);
                licenseRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /License/Edit/5
 
        public ActionResult Edit(System.Guid id)
        {
             return View(licenseRepository.Find(id));
        }

        //
        // POST: /License/Edit/5

        [HttpPost]
        public ActionResult Edit(License license)
        {
            if (ModelState.IsValid) {
                licenseRepository.InsertOrUpdate(license);
                licenseRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /License/Delete/5
 
        public ActionResult Delete(System.Guid id)
        {
            return View(licenseRepository.Find(id));
        }

        //
        // POST: /License/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(System.Guid id)
        {
            licenseRepository.Delete(id);
            licenseRepository.Save();

            return RedirectToAction("Index");
        }
    }
}

