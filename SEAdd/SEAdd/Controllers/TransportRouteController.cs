using SEAdd.Models;
using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEAdd.Controllers
{
    [HandleError]
    public class TransportRouteController : Controller
    {
        ApplicationDbContext db;
        public TransportRouteController()
        {
            db = new ApplicationDbContext();
        }
        // GET: TransportRoute
        public ActionResult Index()
        {
            var model = db.TransportRoutes.ToList();
            return View(model);
        }
        public ActionResult AddNewRoute()
        {
            TransportRoute model = new TransportRoute();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddNewRoute(TransportRoute model)
        {
            if (!ModelState.IsValid)
            {
                TransportRoute vm = model;
                
                return View(vm);
            }
            var alreadyExist = db.TransportRoutes.Where(r=>r.route.ToLower() == model.route.ToLower()).FirstOrDefault();
            if (alreadyExist != null)
            {
                ViewBag.ErrorMsg = "Sorry! This Route already exist.";
            }
            else
            {
                db.TransportRoutes.Add(model);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult EditRoute(int id)
        {
            TransportRoute model = db.TransportRoutes.Where(t => t.id == id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult EditRoute(TransportRoute model)
        {
            if (!ModelState.IsValid)
            {
                TransportRoute vm = model;

                return View(vm);
            }
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteRoute(int id)
        {
            var model = db.TransportRoutes.Where(r => r.id == id).FirstOrDefault();
            db.TransportRoutes.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }
    }
}