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
    public class AcademicDegreeController : Controller
    {
        ApplicationDbContext db;
        public AcademicDegreeController()
        {
            db = new ApplicationDbContext();
        }
        // GET: AcademicDegree
        public ActionResult Index()
        {
            var model = db.AcademicDegrees.OrderBy(ad => ad.id).ToList();
            return View(model);
        }
        public ActionResult AddNewDegree()
        {
            AcademicDegree model = new AcademicDegree();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddNewDegree(AcademicDegree model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var alreadyExist = db.AcademicDegrees.Where(ad => ad.name == model.name).FirstOrDefault();
            if (alreadyExist != null)
            {
                ViewBag.ErrorMsg = "Degree already exist.";
                return View(model);
            }
            db.AcademicDegrees.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult EditDegree(int id)
        {
            var model = db.AcademicDegrees.Where(ad => ad.id == id).FirstOrDefault();
            return View(model);
        }
        public ActionResult EditDegree(AcademicDegree model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteDegree(int id)
        {
            var degree = db.AcademicDegrees.Where(ad => ad.id == id).FirstOrDefault();
            if (degree != null)
            {
                db.AcademicDegrees.Remove(degree);
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