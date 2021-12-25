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
    [Authorize(Roles = "Admin")]
    public class FeeController : Controller
    {
        private readonly ApplicationDbContext db;
        public FeeController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Fee
        public ActionResult Index()
       {
            var model = db.Fees.Where(f=>f.id == 1).FirstOrDefault();
            return View(model);
        }
        public ActionResult ManageFee()
        {
            BankFeeVM model = new BankFeeVM()
            {
                FeeDetails = db.Fees.Where(f => f.id == 1).FirstOrDefault() ,
                banks = db.Banks.ToList()
            };
            return View(model);
        }
        public ActionResult ManageBank()
        {
            var banks = db.Banks.ToList();
            return View(banks);
        }
        public ActionResult AddNewBank()
        {
            var model = new Bank();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddNewBank(Bank model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                db.Banks.Add(model);
                db.SaveChanges();
            }
            return RedirectToAction("ManageBank");
        }
        public ActionResult EditBank(int id)
        {
            var model = db.Banks.Where(b => b.id == id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult EditBank(Bank model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("ManageBank");
        }
        public ActionResult DeleteBank(int id)
        {
            var model = db.Banks.Where(b => b.id == id).FirstOrDefault();
            if(model != null)
            {
                db.Banks.Remove(model);
                db.SaveChanges();
            }
            return RedirectToAction("ManageBank");
        }
        [HttpPost]
        public ActionResult UpdateFeeDetails(BankFeeVM model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                db.Entry(model).State = EntityState.Modified;
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