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
    [Authorize(Roles ="Admin")]
    public class DepartmentController : Controller
    {
        ApplicationDbContext db;
        public DepartmentController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Department
        public ActionResult Index()
        {
            var departments = db.Departments.ToList();
            return View(departments);
        }
        public ActionResult AddNewDepartment()
        {
            Department model = new Department();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddNewDepartment(Department model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var alreadyExist = db.Departments.Where(d => d.name == model.name).FirstOrDefault();
            if (alreadyExist != null)
            {
                ViewBag.ErrorMsg = "Department already exist.";
                return View(model);
            }
            db.Departments.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult EditDepartment(int id)
        {
            var model = db.Departments.Where(d => d.id == id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult EditDepartment(Department model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteDepartment(int id)
        {
            var model = db.Departments.Where(d => d.id == id).FirstOrDefault();
            if (model != null)
            {
                db.Departments.Remove(model);
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