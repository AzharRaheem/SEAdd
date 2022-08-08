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
    public class TestCategoryController : Controller
    {
        ApplicationDbContext db;
        public TestCategoryController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Country
        public ActionResult Index()
        {
            var model = db.TestCategories.ToList();
            return View(model);
        }
        public ActionResult AddNewCategory()
        {
            TestCategory model = new TestCategory();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddNewCategory(TestCategory model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var alreadyExist = db.TestCategories.Where(c => c.name.ToLower() == model.name.ToLower()).FirstOrDefault();
            if (alreadyExist != null)
            {
                ViewBag.ErrorMsg = "Category already exist.";
                return View(model);
            }
            db.TestCategories.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult EditCategory(int id)
        {
            var model = db.TestCategories.Where(c => c.id == id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult EditCategory(TestCategory model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteCategory(int id)
        {
            var model = db.TestCategories.Where(c => c.id == id).FirstOrDefault();
            if (model != null)
            {
                db.TestCategories.Remove(model);
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