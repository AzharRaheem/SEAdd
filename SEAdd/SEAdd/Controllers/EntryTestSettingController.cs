using SEAdd.Models;
using SEAdd.Models.DomainModels;
using SEAdd.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEAdd.Controllers
{
    [HandleError]
    public class EntryTestSettingController : Controller
    {
        ApplicationDbContext db;
        public EntryTestSettingController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Setting
        public ActionResult Index()
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            EntryTestSetting model = db.EntryTestSettings.Where(d => d.departmentName.ToLower() == user.department.ToLower()).FirstOrDefault();
            if (model != null)
            {
                EntryTestSettingVM vm = new EntryTestSettingVM()
                {
                    ETSetting = new EntryTestSetting() ,
                    Tests = new List<TestName>()
                };
                vm.ETSetting = model;
                vm.Tests = db.TestNames.Where(t => t.departmentName.ToLower() == user.department.ToLower()).ToList();
                return View(vm);
            }
            EntryTestSettingVM obj = new EntryTestSettingVM()
            {
                ETSetting = new EntryTestSetting(),
                Tests = new List<TestName>()
            };
            obj.Tests = db.TestNames.Where(t => t.departmentName.ToLower() == user.department.ToLower()).ToList();
            return View(obj);
        }
        [HttpPost]
        public ActionResult EditEntryTestSetting(EntryTestSettingVM model)
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            model.ETSetting.departmentName = user.department;
            if (!ModelState.IsValid)
            {
                EntryTestSettingVM vm = new EntryTestSettingVM()
                {
                    ETSetting = new EntryTestSetting(),
                    Tests = new List<TestName>()
                };
                vm.ETSetting = model.ETSetting;
                vm.Tests = db.TestNames.Where(t => t.departmentName.ToLower() == user.department.ToLower()).ToList();
                return View(model);
            }
            if (model.ETSetting.id != 0)
            {
                db.Entry(model.ETSetting).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                db.EntryTestSettings.Add(model.ETSetting);
                db.SaveChanges();
            }
            TempData["SuccessMsg"] = "Setting saved successfully.";
            TempData.Keep();
            return RedirectToAction("Index");
        }
    }
}