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
    public class TestController : Controller
    {
        ApplicationDbContext db;
        public TestController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Country
        public ActionResult Index()
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            var model = db.TestNames.Where(t => t.departmentName.ToLower() == user.department.ToLower()).ToList();
            return View(model);
        }
        public ActionResult AddNewTest()
        {
            TestName model = new TestName();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddNewTest(TestName model)
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.departmentName = user.department;
            var alreadyExist = db.TestNames.Where(c => c.testName.ToLower() == model.testName.ToLower() && c.departmentName.ToLower() == user.department.ToLower()).FirstOrDefault();
            if (alreadyExist != null)
            {
                ViewBag.ErrorMsg = "Test Name already exist.";
                return View(model);
            }
            db.TestNames.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult EditTest(int id)
        {
            var model = db.TestNames.Where(c => c.id == id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult EditTest(TestName model)
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteTest(int id)
        {
            var model = db.TestNames.Where(c => c.id == id).FirstOrDefault();
            if (model != null)
            {
                db.TestNames.Remove(model);
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