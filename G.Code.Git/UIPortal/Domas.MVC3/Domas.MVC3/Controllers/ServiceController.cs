using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domas.DAP.ADF.License.Service;
using Domas.MVC3.Models;

namespace Domas.MVC3.Controllers
{   
    public class ServiceController : Controller
    {
		private readonly IServiceRepository serviceRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public ServiceController() : this(new ServiceRepository())
        {
        }

        public ServiceController(IServiceRepository serviceRepository)
        {
			this.serviceRepository = serviceRepository;
        }

        //
        // GET: /Service/

        public ViewResult Index()
        {
            return View(serviceRepository.All);
        }

        //
        // GET: /Service/Details/5

        public ViewResult Details(System.Guid id)
        {
            return View(serviceRepository.Find(id));
        }

        //
        // GET: /Service/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Service/Create

        [HttpPost]
        public ActionResult Create(Service service)
        {
            if (ModelState.IsValid) {
                serviceRepository.InsertOrUpdate(service);
                serviceRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /Service/Edit/5
 
        public ActionResult Edit(System.Guid id)
        {
             return View(serviceRepository.Find(id));
        }

        //
        // POST: /Service/Edit/5

        [HttpPost]
        public ActionResult Edit(Service service)
        {
            if (ModelState.IsValid) {
                serviceRepository.InsertOrUpdate(service);
                serviceRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /Service/Delete/5
 
        public ActionResult Delete(System.Guid id)
        {
            return View(serviceRepository.Find(id));
        }

        //
        // POST: /Service/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(System.Guid id)
        {
            serviceRepository.Delete(id);
            serviceRepository.Save();

            return RedirectToAction("Index");
        }
    }
}

