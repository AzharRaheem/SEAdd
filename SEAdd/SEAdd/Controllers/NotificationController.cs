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
    [HandleError]
    [Authorize]
    public class NotificationController : Controller
    {
        ApplicationDbContext db;
        public NotificationController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Notification
        [Authorize(Roles ="Admin , User , SuperAdmin")]
        public ActionResult Index()
        {
            var Notifications = db.Notifications.ToList();
            return View(Notifications);
        }
        [Authorize(Roles ="Admin , SuperAdmin")]
        public ActionResult AddNotification()
        {
            var model = new Notification();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddNotification(Notification model , HttpPostedFileBase NotificationFile)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            model.NotificationDate = DateTime.Today.Date;
            model.NotificationFileUrl = GetFileUrl(NotificationFile);
            db.Notifications.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin , SuperAdmin")]
        public ActionResult EditNotification(int id)
        {
            var model = db.Notifications.Where(n => n.Id == id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult EditNotification(Notification model , HttpPostedFileBase NotificationFile)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            model.NotificationDate = DateTime.Today.Date;
            model.NotificationFileUrl = GetFileUrl(NotificationFile);
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin , SuperAdmin")]
        public ActionResult DeleteNotification(int id)
        {
            var model = db.Notifications.Where(n => n.Id == id).FirstOrDefault();
            if(model != null)
            {
                db.Notifications.Remove(model);
                db.SaveChanges();
            }
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