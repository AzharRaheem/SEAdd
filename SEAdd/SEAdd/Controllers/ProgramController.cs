using SEAdd.CustomValidations;
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
    [HandleError]
    [Authorize(Roles = "SuperAdmin")]
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
            ProgramTypeVM model = new ProgramTypeVM()
            {
                program = new Program(),
                ProgramType = GetLists.GetProgramTypeList()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult AddNewProgram(ProgramTypeVM model)
        {
            if (!ModelState.IsValid)
            {
                ProgramTypeVM vm = new ProgramTypeVM()
                {
                    program = model.program , 
                    ProgramType = GetLists.GetProgramTypeList()
                };
                return View(vm);
            }
            var alreadyExist = db.Programs.Where(p => p.ProgramName.ToLower() == model.program.ProgramName.ToLower()).FirstOrDefault();
            if (alreadyExist != null)
            {
                ViewBag.ErrorMsg = "Program already exist.";
                return View(model);
            }
            db.Programs.Add(model.program);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult EditProgram(int id)
        {
            ProgramTypeVM model = new ProgramTypeVM()
            {
                program = db.Programs.Where(p => p.id == id).FirstOrDefault(),
                ProgramType = GetLists.GetProgramTypeList()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult EditProgram(ProgramTypeVM model)
        {
            if (!ModelState.IsValid)
            {
                ProgramTypeVM vm = new ProgramTypeVM()
                {
                    program = model.program,
                    ProgramType = GetLists.GetProgramTypeList()
                };
                return View(vm);
            }
            db.Entry(model.program).State = EntityState.Modified;
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