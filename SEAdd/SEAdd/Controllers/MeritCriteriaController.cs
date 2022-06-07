using SEAdd.Models;
using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEAdd.Controllers
{
    public class MeritCriteriaController : Controller
    {
        ApplicationDbContext db;
        public MeritCriteriaController()
        {
            db = new ApplicationDbContext();
        }
        public ActionResult AddUpdateCriteria()
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            MeritCriteria model = db.MeritCriterias.Where(mc => mc.departmentName.ToLower() == user.department.ToLower()).FirstOrDefault();
            if(model != null)
            {
                return View(model);
            }
            else
            {
                MeritCriteria meritCriteria = new MeritCriteria();
                return View(meritCriteria);
            }
        }
        [HttpPost]
        public ActionResult AddUpdateCriteria(MeritCriteria model)
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            if (!ModelState.IsValid)
            {
                MeritCriteria meritCriteria = new MeritCriteria();
                return View(meritCriteria);
            }
            model.departmentName = user.department;
            if (model.id != 0)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                db.MeritCriterias.Add(model);
                db.SaveChanges();
            }
            TempData["Message"] = "Setting Saved Successfully.";
            TempData.Keep();
            return RedirectToAction("AddUpdateCriteria" , "MeritCriteria");
        }
    }
}