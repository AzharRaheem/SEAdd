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
    public class CategoryPercentageController : Controller
    {
        ApplicationDbContext db;
        public CategoryPercentageController()
        {
            db = new ApplicationDbContext();
        }
        // GET: CategoryPercentage
        public ActionResult Index()
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            var model = db.CategoriesPercentage.Where(c => c.department.ToLower() == user.department.ToLower()).ToList();
            return View(model);
        }
        public ActionResult AddNewCategoryPercentage()
        {
            CategoryPercentageVM model = new CategoryPercentageVM()
            {
                categoryPercentage = new CategoryPercentage() , 
                Categories = db.TestCategories.ToList()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult AddNewCategoryPercentage(CategoryPercentageVM model)
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            model.categoryPercentage.department = user.department;
            if (!ModelState.IsValid)
            {
                CategoryPercentageVM vm = new CategoryPercentageVM()
                {
                    Categories = db.TestCategories.ToList() , 
                    categoryPercentage = model.categoryPercentage
                };
                return View(vm);
            }
            var alreadyExist = db.CategoriesPercentage.Where(c => c.categoryName == model.categoryPercentage.categoryName && c.department.ToLower() == user.department.ToLower()).FirstOrDefault();
            if (alreadyExist != null)
            {
                ViewBag.ErrorMsg = "Category already exist.";
                model.Categories = db.TestCategories.ToList();
                return View(model);
            }
            var categories = db.CategoriesPercentage.Where(c => c.department.ToLower() == user.department.ToLower()).ToList();
            int percentage = 0;
            foreach (var item in categories)
            {
                percentage += item.QuestionPercentage;
            }
            percentage = percentage + model.categoryPercentage.QuestionPercentage;
            if(percentage <= 100)
            {
                db.CategoriesPercentage.Add(model.categoryPercentage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMsg = "You cannot add new category because total percentage must be less than or equal to 100.";
                model.Categories = db.TestCategories.ToList();
                return View(model);
            }
            
        }
        public ActionResult EditCategoryPercentage(int id)
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            CategoryPercentageVM model = new CategoryPercentageVM()
            {
                categoryPercentage = db.CategoriesPercentage.Where(c => c.id == id && c.department.ToLower() == user.department.ToLower()).FirstOrDefault() ,
                Categories = db.TestCategories.ToList()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult EditCategoryPercentage(CategoryPercentageVM model)
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            if (!ModelState.IsValid)
            {
                CategoryPercentageVM vm = new CategoryPercentageVM()
                {
                    Categories = db.TestCategories.ToList(),
                    categoryPercentage = model.categoryPercentage
                };
                return View(vm);
            }
            var categories = db.CategoriesPercentage.Where(c => c.department.ToLower() == user.department.ToLower() && c.id != model.categoryPercentage.id).ToList();
            int percentage = 0;
            foreach (var item in categories)
            {
                percentage += item.QuestionPercentage;
            }
            percentage = percentage + model.categoryPercentage.QuestionPercentage;
            if (percentage <= 100)
            {
                var category = db.CategoriesPercentage.Where(c => c.id == model.categoryPercentage.id && c.department == model.categoryPercentage.department).FirstOrDefault();
                category.categoryName = model.categoryPercentage.categoryName;
                category.QuestionPercentage = model.categoryPercentage.QuestionPercentage;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMsg = "You cannot update this category because total percentage must be less than or equal to 100.";
                model.Categories = db.TestCategories.ToList();
                return View(model);
            }
            
        }
        public ActionResult DeleteCategoryPercentage(int id)
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            var model = db.CategoriesPercentage.Where(c => c.id == id && c.department.ToLower() == user.department.ToLower()).FirstOrDefault();
            if (model != null)
            {
                db.CategoriesPercentage.Remove(model);
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