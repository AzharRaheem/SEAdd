using SEAdd.Models;
using SEAdd.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEAdd.Controllers
{
    public class QuestionController : Controller
    {
        ApplicationDbContext db;
        public QuestionController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Question
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddNewQuestion()
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            QuestionCategoryVM model = new QuestionCategoryVM()
            {
                Question = new Models.DomainModels.TestQuestion(),
                Categories = db.CategoriesPercentage.Where(c => c.department.ToLower() == user.department.ToLower()).ToList(),
                TestName = db.TestNames.Where(t => t.departmentName.ToLower() == user.department.ToLower()).ToList(),
                TestTypes = db.TestTypes.ToList()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult AddNewQuestion(QuestionCategoryVM model)
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            if (!ModelState.IsValid)
            {
                QuestionCategoryVM vm = new QuestionCategoryVM()
                {
                    Question = model.Question ,
                    Categories = db.CategoriesPercentage.Where(c => c.department.ToLower() == user.department.ToLower()).ToList(),
                    TestName = db.TestNames.Where(t => t.departmentName.ToLower() == user.department.ToLower()).ToList(),
                    TestTypes = db.TestTypes.ToList()
                };
                return View(vm);
            }
            var isAlreadyExist = db.TestQuestions.Where(t => t.QuestionText.ToLower() == model.Question.QuestionText.ToLower() && t.departmentName.ToLower() == user.department.ToLower()).FirstOrDefault();
            if(isAlreadyExist != null)
            {
                TempData["Message"] = "Question Already Added.";
                TempData.Keep();
                return RedirectToAction("AddNewQuestion", "Question");
            }
            model.Question.departmentName = user.department;
            db.TestQuestions.Add(model.Question);
            db.SaveChanges();
            TempData["SuccessMsg"] = "Question Added Successfully.";
            TempData.Keep();
            return RedirectToAction("AddNewQuestion", "Question");
        }
        public ActionResult ViewAllQuestions()
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            var model = db.TestQuestions.Where(q => q.departmentName.ToLower() == user.department.ToLower()).ToList();
            return View(model);
        }
        public ActionResult DeleteQuestion(int id)
        {
            var question = db.TestQuestions.Where(q => q.id == id).FirstOrDefault();
            db.TestQuestions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("ViewAllQuestions", "Question");
        }
    }
}