using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MultiEditWithSignalR.Models;

namespace MultiEditWithSignalR.Controllers
{ 
    public class BlogPostController : Controller
    {
        private BlogPostContext db = new BlogPostContext();

        //
        // GET: /BlogPost/

        public ViewResult Index()
        {
            return View(db.BlogPosts.ToList());
        }

        //
        // GET: /BlogPost/Details/5

        public ViewResult Details(int id)
        {
            BlogPost blogpost = db.BlogPosts.Find(id);
            return View(blogpost);
        }

        //
        // GET: /BlogPost/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /BlogPost/Create

        [HttpPost]
        public ActionResult Create(BlogPost blogpost)
        {
            if (ModelState.IsValid)
            {
                db.BlogPosts.Add(blogpost);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(blogpost);
        }
        
        //
        // GET: /BlogPost/Edit/5
 
        public ActionResult Edit(int id)
        {
            BlogPost blogpost = db.BlogPosts.Find(id);
            return View(blogpost);
        }

        //
        // POST: /BlogPost/Edit/5

        [HttpPost]
        public ActionResult Edit(BlogPost blogpost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blogpost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogpost);
        }

        //
        // GET: /BlogPost/Review/5

        public ActionResult Review(int id)
        {
            BlogPost blogpost = db.BlogPosts.Find(id);
            return View(blogpost);
        }

        //
        // POST: /BlogPost/Review/5

        [HttpPost]
        public ActionResult Review(BlogPost blogpost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blogpost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogpost);
        }

        //
        // GET: /BlogPost/Delete/5
 
        public ActionResult Delete(int id)
        {
            BlogPost blogpost = db.BlogPosts.Find(id);
            return View(blogpost);
        }

        //
        // POST: /BlogPost/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            BlogPost blogpost = db.BlogPosts.Find(id);
            db.BlogPosts.Remove(blogpost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}