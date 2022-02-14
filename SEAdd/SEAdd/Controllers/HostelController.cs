using SEAdd.Models;
using SEAdd.Models.DomainModels;
using SEAdd.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEAdd.Controllers
{
    public class HostelController : Controller
    {
        ApplicationDbContext db;
        public HostelController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Hostel
        public ActionResult Index()
        {
            var model = db.Hostels.ToList();
            return View(model);
        }
        public ActionResult AddNewHostel()
        {
            HostelCampusVM model = new HostelCampusVM()
            {
                hostel = new Hostel() , 
                Campuses = db.Campuses.ToList()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult AddNewHostel(HostelCampusVM model)
        {
            if (!ModelState.IsValid)
            {
                HostelCampusVM vm = new HostelCampusVM()
                {
                    hostel = model.hostel,
                    Campuses = db.Campuses.ToList()
                };
                return View(vm);
            }
            var alreadyExist = db.Hostels.Where(h => h.Name == model.hostel.Name).FirstOrDefault();
            if (alreadyExist != null)
            {
                ViewBag.ErrorMsg = "Sorry! Hostel already exist.";
            }
            else
            {
                db.Hostels.Add(model.hostel);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult EditHostel(int id)
        {
            HostelCampusVM model = new HostelCampusVM()
            {
                hostel = db.Hostels.Where(h => h.id == id).FirstOrDefault() ,
                Campuses = db.Campuses.ToList()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult EditHostel(HostelCampusVM model)
        {
            if (!ModelState.IsValid)
            {
                HostelCampusVM vm = new HostelCampusVM()
                {
                    hostel = model.hostel,
                    Campuses = db.Campuses.ToList()
                };
                return View(vm);
            }
            db.Entry(model.hostel).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteHostel(int id)
        {
            var model = db.Hostels.Where(h => h.id == id).FirstOrDefault();
            db.Hostels.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }
    }
}