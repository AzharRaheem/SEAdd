using SEAdd.Models;
using SEAdd.Models.DomainModels;
using SEAdd.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEAdd.Controllers
{
    public class EntryTestController : Controller
    {
        ApplicationDbContext db;
        public EntryTestController()
        {
            db = new ApplicationDbContext();
        }
        public ActionResult CheckRollNumber()
        {
            RollNumberDepartmentVM vm = new RollNumberDepartmentVM()
            {
                departments = db.Departments.ToList()
            };
            return View(vm);
        }
        [HttpPost]
        public ActionResult CheckRollNumber(RollNumberDepartmentVM model)
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            Session["TestQuestions"] = null;
            Session["EntryTestSetting"] = null;
            if (!ModelState.IsValid)
            {
                RollNumberDepartmentVM vm = new RollNumberDepartmentVM()
                {
                    department = model.department ,
                    rollNumber = model.rollNumber ,
                    departments = db.Departments.ToList()
                };
                return View(vm);
            }
            var departmentEntryTest = db.DepartmentSettings.Where(e => e.departmentName.ToLower() == model.department.ToLower()).FirstOrDefault();
            if(departmentEntryTest != null && departmentEntryTest.isEntryTestRequired)
            {
                //Check Roll Number and Time for Entry Test.......
                var rollNumberDetails = db.RollNumbers.Where(r => r.userId == user.Id).FirstOrDefault();
                if(rollNumberDetails != null)
                {
                    if (rollNumberDetails.applicantRollNumber == model.rollNumber)
                    {
                        var entryTestSetting = db.EntryTestSettings.Where(e => e.departmentName.ToLower() == model.department.ToLower()).FirstOrDefault();
                        if (entryTestSetting.TestStartDateTime == DateTime.Today || (entryTestSetting.TestStartDateTime < entryTestSetting.TestEndDateTime && entryTestSetting.TestStartDateTime != entryTestSetting.TestEndDateTime && entryTestSetting.TestEndDateTime > DateTime.Now))
                        {
                            //Test Start...
                            //Get all categories...
                            var categories = db.CategoriesPercentage.Where(c => c.department.ToLower() == model.department.ToLower() && c.QuestionPercentage != 0).ToList();
                            //Now get question according to the question percentage..
                            List<TestQuestion> TestQuestions = new List<TestQuestion>();
                            foreach (var category in categories)
                            {
                                var T_QuestionInCategory = entryTestSetting.TotalQuestions * category.QuestionPercentage / 100;   //Get the Total Count of Questions in particular category...
                                var questions = db.TestQuestions.Where(q => q.categoryId == category.id).ToList();  //Get all Question according to the category...
                                int MaxIndex = 0;
                                for (int i = 0; i < T_QuestionInCategory; i++)
                                {
                                    //Get Question from Random Index and then after adding question in the list remove it from the questions's List...
                                    Random random = new Random();
                                    int index = random.Next(0, MaxIndex);
                                    var Question = questions[index];
                                    questions.RemoveAt(index);
                                    MaxIndex = T_QuestionInCategory - 1;
                                    TestQuestions.Add(Question);
                                }
                            }
                            Session["RollNumber"] = model.rollNumber;
                            Session["TestQuestions"] = TestQuestions;
                            Session["EntryTestSetting"] = entryTestSetting;
                            Session["StartTestNow"] = true;
                            return RedirectToAction("StartTest", "EntryTest");
                        }
                        else if (entryTestSetting.TestEndDateTime == DateTime.Now || entryTestSetting.TestEndDateTime < DateTime.Now)
                        {
                            //Test is End...
                            return RedirectToAction("TestEnd", "EntryTest");
                        }
                        else
                        {
                            //Test Not Started Yet...
                            return RedirectToAction("TestNotStarted", "EntryTest");
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Your RollNumber is not correct.Please enter the correct RollNumber.";
                        TempData.Keep();
                        return RedirectToAction("CheckRollNumber", "EntryTest");
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Your RollNumber is not correct.Please enter the correct RollNumber.";
                    TempData.Keep();
                    return RedirectToAction("CheckRollNumber", "EntryTest");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "This department doesn't required entry test for admission.";
                TempData.Keep();
                return RedirectToAction("CheckRollNumber", "EntryTest");
            }            
        }
        public ActionResult StartTest() //Display the Test View to user...
        {
            var Questions = Session["TestQuestions"] as List<TestQuestion>;
            var ETestSetting = Session["EntryTestSetting"] as EntryTestSetting;
            Session["TotalQuestions"] = ETestSetting.TotalQuestions;
            var startTestNow = Convert.ToBoolean(Session["StartTestNow"]);
            if(startTestNow)
            {
                Session["StartTestNow"] = false;
                Random random = new Random();
                int index = random.Next(0, Questions.Count());
                QuestionEntryTestVM vm = new QuestionEntryTestVM()
                {
                    entryTestSetting = ETestSetting,
                    Question = Questions[index]
                };
                Session["CurrentQuestionCount"] = 1;
                Questions.Remove(vm.Question);
                return View(vm);
            }
            Random rndm = new Random();
            int indx = rndm.Next(0, Questions.Count());
            if(Questions.Count() != 0)
            {
                var Question = Questions[indx];
                Session["CurrentQuestionCount"] = Convert.ToInt32(Session["CurrentQuestionCount"]) + 1;
                Questions.Remove(Question);
                return PartialView("_EntryTestQuestion", Question);
            }
            else
            {
                return RedirectToAction("TestEnd", "EntryTest");
            }
        }
        [HttpPost]
        public ActionResult StartTest(TestQuestion Question , string department , string userAns , string correctAns , string rollNumber) //Post the Question with User's selected answer...
        {
            return RedirectToAction("StartTest", "EntryTest");
        }
        public ActionResult TestEnd()
        {
            return View();
        }
        public ActionResult TestNotStarted()
        {
            return View();
        }
    }
}