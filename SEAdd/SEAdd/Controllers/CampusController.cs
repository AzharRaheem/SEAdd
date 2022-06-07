using SEAdd.Models;
using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEAdd.Controllers
{
    [HandleError]
    [Authorize(Roles = "SuperAdmin")]
    public class CampusController : Controller
    {
        private readonly ApplicationDbContext db;
        public CampusController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Campus
        public ActionResult Index()
        {
            var campuses = db.Campuses.ToList();
            return View(campuses);
        }
        public ActionResult AddNewCampus()
        {
            Campus model = new Campus();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddNewCampus(Campus model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var alreadyExist = db.Campuses.Where(c => c.name.ToLower() == model.name.ToLower()).FirstOrDefault();
                if (alreadyExist != null)
                {
                    ViewBag.ErrorMsg = "Campus already exist.";
                    return View(model);
                }
                db.Campuses.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        public ActionResult EditCampus(int id)
        {
            var model = db.Campuses.Where(c => c.id == id).FirstOrDefault();
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult EditCampus(Campus model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        public ActionResult DeleteCampus(int id)
        {
            var model = db.Campuses.Where(c => c.id == id).FirstOrDefault();
            if (model != null)
            {
                db.Campuses.Remove(model);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }
    }
}