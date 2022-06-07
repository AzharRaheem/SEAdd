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
    public class CountryController : Controller
    {
        ApplicationDbContext db;
        public CountryController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Country
        public ActionResult Index()
        {
            var model = db.Countries.ToList();
            return View(model);
        }
        public ActionResult AddNewCountry()
        {
            Country model = new Country();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddNewCountry(Country model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var alreadyExist = db.Countries.Where(c => c.name.ToLower() == model.name.ToLower()).FirstOrDefault();
            if (alreadyExist != null)
            {
                ViewBag.ErrorMsg = "Country already exist.";
                return View(model);
            }
            db.Countries.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult EditCountry(int id)
        {
            var model = db.Countries.Where(c => c.id == id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult EditCountry(Country model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteCountry(int id)
        {
            var model = db.Countries.Where(c => c.id == id).FirstOrDefault();
            if (model != null)
            {
                db.Countries.Remove(model);
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