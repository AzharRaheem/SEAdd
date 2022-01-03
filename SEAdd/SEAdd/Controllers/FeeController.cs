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
            var model = db.Fees.ToList();
            var count = db.Fees.Count();
            ViewBag.FeeCount = count;
            return View(model);
        }
        public ActionResult DeleteFeeDetail(int id)
        {
            var model = db.Fees.Where(f => f.id == id).FirstOrDefault();
            db.Fees.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
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
        public ActionResult AddFee()
        {
            BankFeeVM model = new BankFeeVM()
            {
                FeeDetails = new Fee(),
                banks = db.Banks.ToList()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult AddFee(BankFeeVM model)
        {
            if(!ModelState.IsValid)
            {
                BankFeeVM vm = new BankFeeVM()
                {
                    FeeDetails = model.FeeDetails,
                    banks = db.Banks.ToList()
                };
                return View(vm);
            }
            db.Fees.Add(model.FeeDetails);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult UpdateFeeDetails(int id)
        {
            BankFeeVM vm = new BankFeeVM()
            {
                FeeDetails = db.Fees.Where(f => f.id == id).FirstOrDefault(),
                banks = db.Banks.ToList()
            };
            return View(vm);
        }
        [HttpPost]
        public ActionResult UpdateFeeDetails(BankFeeVM model)
        {
            if(!ModelState.IsValid)
            {
                BankFeeVM vm = new BankFeeVM()
                {
                    FeeDetails = model.FeeDetails,
                    banks = db.Banks.ToList()
                };
                return View(model);
            }
            db.Entry(model.FeeDetails).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }
    }
}