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
    public class ProgramController : Controller
    {
        ApplicationDbContext db;
        public ProgramController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Program
        public ActionResult Index()
        {
            var programs = db.Programs.ToList();
            return View(programs);
        }
        public ActionResult AddNewProgram()
        {
            Program program = new Program();
            return View(program);
        }
        [HttpPost]
        public ActionResult AddNewProgram(Program model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            db.Programs.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult EditProgram(int id)
        {
            var model = db.Programs.Where(b => b.id == id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult EditProgram(Program model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteProgram(int id)
        {
            var model = db.Programs.Where(b => b.id == id).FirstOrDefault();
            if (model != null)
            {
                db.Programs.Remove(model);
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