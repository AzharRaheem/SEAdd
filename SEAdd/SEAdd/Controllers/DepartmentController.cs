using SEAdd.Models;
using SEAdd.Models.DomainModels;
using SEAdd.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEAdd.Controllers
{
    [HandleError]
    [Authorize(Roles = "SuperAdmin")]
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
            DepartmentCampusVM model = new DepartmentCampusVM()
            {
                department = new Department() , 
                Campuses = db.Campuses.ToList()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult AddNewDepartment(DepartmentCampusVM model , HttpPostedFileBase DeptLogoImg)
        {
            ViewBag.ErrorMsg = null;
            if (!ModelState.IsValid)
            {
                DepartmentCampusVM vm = new DepartmentCampusVM()
                {
                    department = model.department,
                    Campuses = db.Campuses.ToList()
                };
                return View(vm);
            }
            var alreadyExist = db.Departments.Where(d => d.name == model.department.name).FirstOrDefault();
            if (alreadyExist != null)
            {
                ViewBag.ErrorMsg = "Department already exist.";
                DepartmentCampusVM vm = new DepartmentCampusVM()
                {
                    department = model.department,
                    Campuses = db.Campuses.ToList()
                };
                return View(vm);
            }
            model.department.DeptLogUrl = GetImageUrl(DeptLogoImg);
            db.Departments.Add(model.department);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult EditDepartment(int id)
        {
            DepartmentCampusVM model = new DepartmentCampusVM()
            {
                department = db.Departments.Where(d => d.id == id).FirstOrDefault() ,
                Campuses = db.Campuses.ToList()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult EditDepartment(DepartmentCampusVM model , HttpPostedFileBase DeptLogoImg)
        {
            if (!ModelState.IsValid)
            {
                DepartmentCampusVM vm = new DepartmentCampusVM()
                {
                    department = model.department,
                    Campuses = db.Campuses.ToList()
                };
                return View(vm);
            }
            model.department.DeptLogUrl = GetImageUrl(DeptLogoImg);
            db.Entry(model.department).State = EntityState.Modified;
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
        [NonAction]
        private string GetImageUrl(HttpPostedFileBase file)
        {
            string filePath = null;
            string folderPath = Server.MapPath("~/Images/");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            if (file != null && file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                var fileExtension = fileName.Split('.')[1];
                Guid UniqueName = Guid.NewGuid();
                var imageName = UniqueName + "." + fileExtension;
                filePath = "~/Images/" + imageName;
                file.SaveAs(Server.MapPath(filePath));
            }
            return filePath;
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }
    }
}