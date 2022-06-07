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
    public class TestTypeController : Controller
    {
        ApplicationDbContext db;
        public TestTypeController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Country
        public ActionResult Index()
        {
            var model = db.TestTypes.ToList();
            return View(model);
        }
        public ActionResult AddNewType()
        {
            TestType model = new TestType();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddNewType(TestType model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var alreadyExist = db.TestTypes.Where(c => c.name.ToLower() == model.name.ToLower()).FirstOrDefault();
            if (alreadyExist != null)
            {
                ViewBag.ErrorMsg = "This Type already exist.";
                return View(model);
            }
            db.TestTypes.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult EditType(int id)
        {
            var model = db.TestTypes.Where(c => c.id == id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult EditType(TestType model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteType(int id)
        {
            var model = db.TestTypes.Where(c => c.id == id).FirstOrDefault();
            if (model != null)
            {
                db.TestTypes.Remove(model);
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