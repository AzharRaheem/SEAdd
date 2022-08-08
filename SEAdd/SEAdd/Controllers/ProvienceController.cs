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
    public class ProvienceController : Controller
    {
        ApplicationDbContext db;
        public ProvienceController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Provience
        public ActionResult Index()
        {
            var model = db.Proviences.ToList();
            return View(model);
        }
        public ActionResult AddNewProvience()
        {
            Provience model = new Provience();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddNewProvience(Provience model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var alreadyExist = db.Proviences.Where(p => p.name.ToLower() == model.name.ToLower()).FirstOrDefault();
            if (alreadyExist != null)
            {
                ViewBag.ErrorMsg = "Provience already exist.";
                return View(model);
            }
            db.Proviences.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult EditProvience(int id)
        {
            var model = db.Proviences.Where(p => p.id == id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult EditProvience(Provience model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteProvience(int id)
        {
            var model = db.Proviences.Where(c => c.id == id).FirstOrDefault();
            if (model != null)
            {
                db.Proviences.Remove(model);
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