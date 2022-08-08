using SEAdd.Models;
using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEAdd.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    [HandleError]
    public class AdmissionController : Controller
    {
        private readonly ApplicationDbContext db;
        public AdmissionController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Admission
        public ActionResult Index()
        {
            var allRecords = db.AdmissionDate.ToList();
            return View(allRecords);
        }
        public ActionResult AddNewDate()
        {
            AdmissionDate model = new AdmissionDate();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddNewDate(AdmissionDate model , HttpPostedFileBase file)
        {
            model.NotificationDate = DateTime.Today.Date;
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            model.NotificationFileUrl = GetFileUrl(file);
            db.AdmissionDate.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteNotificationDate(int id)
        {
            var record = db.AdmissionDate.Where(d => d.Id == id).FirstOrDefault();
            if(record != null)
            {
                db.AdmissionDate.Remove(record);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult EditAdmissionNotification(int id)
        {
            var record = db.AdmissionDate.Where(d => d.Id == id).FirstOrDefault();
            return View(record);
        }
        [HttpPost]
        public ActionResult EditAdmissionNotification(AdmissionDate model , HttpPostedFileBase file)
        {
            model.NotificationDate = DateTime.Today.Date;
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            if (file != null)
            {
                model.NotificationFileUrl = GetFileUrl(file);
            }
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [NonAction]
        private string GetFileUrl(HttpPostedFileBase file)
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