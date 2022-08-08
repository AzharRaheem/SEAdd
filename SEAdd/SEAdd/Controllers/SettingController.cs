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
    public class SettingController : Controller
    {
        ApplicationDbContext db;
        public SettingController()
        {
            db = new ApplicationDbContext(); 
        }
        // GET: Setting
        public ActionResult Index()
        {
            Setting model = db.Settings.FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult EditApplicantApplySetting(Setting model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            TempData["SuccessMsg"] = "Setting saved successfully.";
            TempData.Keep();
            return RedirectToAction("Index");
        }
    }
}