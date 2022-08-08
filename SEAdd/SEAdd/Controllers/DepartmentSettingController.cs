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
    public class DepartmentSettingController : Controller
    {
        ApplicationDbContext db;
        public DepartmentSettingController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Setting
        public ActionResult Index()
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            DepartmentSetting model = db.DepartmentSettings.Where(d => d.departmentName.ToLower() == user.department.ToLower()).FirstOrDefault();
            if(model != null)
            {
                return View(model);
            }
            DepartmentSetting obj = new DepartmentSetting();
            return View(obj);
        }
        [HttpPost]
        public ActionResult EditDepartmentSetting(DepartmentSetting model)
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            model.departmentName = user.department;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if(model.id != 0)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                db.DepartmentSettings.Add(model);
                db.SaveChanges();
            }
            TempData["SuccessMsg"] = "Setting saved successfully.";
            TempData.Keep();
            return RedirectToAction("Index");
        }
    }
}