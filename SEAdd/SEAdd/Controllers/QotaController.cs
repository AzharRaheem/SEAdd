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
    [Authorize(Roles = "Admin")]
    public class QotaController : Controller
    {
        private readonly ApplicationDbContext db;
        public QotaController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Qota
        public ActionResult Index()
        {
            var qotas = db.Qotas.ToList();
            return View(qotas);
        }
        public ActionResult AddNewQota()
        {
            Qota qota = new Qota();
            return View(qota);
        }
        [HttpPost]
        public ActionResult AddNewQota(Qota model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                db.Qotas.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
        }
        public ActionResult EditQota(int id)
        {
            var qota = db.Qotas.Where(q => q.id == id).FirstOrDefault();
            if(qota != null)
            {
                return View(qota);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult EditQota(Qota model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        public ActionResult DeleteQota(int id)
        {
            var qota = db.Qotas.Where(q => q.id == id).FirstOrDefault();
            if(qota != null)
            {
                db.Qotas.Remove(qota);
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