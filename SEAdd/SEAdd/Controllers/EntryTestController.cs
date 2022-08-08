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
    [HandleError]
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
            Session["TotalMarks"] = ETestSetting.TotalMarks;
            Session["MarksOfSingleQuestion"] = ETestSetting.TotalMarks / ETestSetting.TotalQuestions;
            var startTestNow = Convert.ToBoolean(Session["StartTestNow"]);
            if (startTestNow)
            {
                var roll = Session["RollNumber"].ToString();
                var rollNumber = db.RollNumbers.Where(r => r.applicantRollNumber == roll).FirstOrDefault();
                var programSelection = db.ProgramSelections.Where(p => p.ApplicantId == rollNumber.applicantId && p.Department.name == rollNumber.departmentName).FirstOrDefault();
                var entryTestResult = db.EntryTestResults.Where(e => e.rollNumber == rollNumber.applicantRollNumber && e.departmentName == rollNumber.departmentName).FirstOrDefault();
                if(programSelection.isRejected == true)
                {
                    TempData["ErrorMessage"] = "Your application for admission has been rejected.You can't participate in the test.";
                    TempData.Keep();
                    return RedirectToAction("CheckRollNumber", "EntryTest");
                }
                else if (entryTestResult != null)
                {
                    TempData["ErrorMessage"] = "Your test already submitted.You can't participate in the test again.";
                    TempData.Keep();
                    return RedirectToAction("CheckRollNumber", "EntryTest");
                }
                else
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
                    Session["UserQuestionAttemptedList"] = null;
                    return View(vm);
                }
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
                EntryTestResult entryTestResult = new EntryTestResult();
                double totalMarks = Convert.ToDouble(Session["TotalMarks"]);
                double obtainedMarks = 0;
                foreach (var item in Session["UserQuestionAttemptedList"] as List<UserQuestionResult>)
                {
                    if (item.userAns == item.correctAns)
                    {
                        obtainedMarks = obtainedMarks + item.ObtainedMarks;
                    }
                    db.UserQuestionResults.Add(item);
                    db.SaveChanges();
                }
                var meritCriteria = db.MeritCriterias.Where(m => m.departmentName.ToLower() == ETestSetting.departmentName.ToLower()).FirstOrDefault();
                entryTestResult.totalScorePercentage = meritCriteria.BsEntryTestPercentage;
                entryTestResult.obtainedScorePercentage = obtainedMarks / totalMarks * meritCriteria.BsEntryTestPercentage;
                entryTestResult.rollNumber = Session["RollNumber"].ToString();
                entryTestResult.userId = Session["UserId"].ToString();
                entryTestResult.entryTestName = ETestSetting.testName;
                entryTestResult.departmentName = ETestSetting.departmentName;
                entryTestResult.EntryTestStartTime = ETestSetting.TestStartDateTime;
                entryTestResult.EntryTestEndTime = ETestSetting.TestEndDateTime;
                entryTestResult.year = DateTime.Today.Date.Year.ToString();
                var rollNumber = Session["RollNumber"].ToString();
                var rollModel = db.RollNumbers.Where(r => r.applicantRollNumber.ToLower() == rollNumber.ToLower()).FirstOrDefault();
                entryTestResult.programId = rollModel.programId;
                //Save the Entry Test Result...
                db.EntryTestResults.Add(entryTestResult);
                db.SaveChanges();
                return PartialView("_EntryTestSubmitted");
            }
        }
        [HttpPost]
        public ActionResult StartTest(TestQuestion Question , string department, string userAnswer , string correctAns , string rollNumber , int totalMarksOfThisQuestion) //Post the Question with User's selected answer...
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            var applicant = db.Applicants.Where(a => a.userId == user.Id).FirstOrDefault();
            UserQuestionResult model = new UserQuestionResult()
            {
                applicantId = applicant.id , 
                departmentName = department , 
                QuestionId = Question.id , 
                TotalMarks = totalMarksOfThisQuestion, 
                rollNumber = rollNumber , 
                userAns = userAnswer ,
                correctAns = correctAns
            };
            model.ObtainedMarks = (userAnswer == correctAns) ? totalMarksOfThisQuestion : 0;
            if(Session["UserQuestionAttemptedList"] != null)
            {
                List<UserQuestionResult> userAttemptedQuestionsList = Session["UserQuestionAttemptedList"] as List<UserQuestionResult>;
                userAttemptedQuestionsList.Add(model);
                Session["UserQuestionAttemptedList"] = userAttemptedQuestionsList;
            }
            else
            {
                List<UserQuestionResult> userAttemptList = new List<UserQuestionResult>();
                userAttemptList.Add(model);
                Session["UserQuestionAttemptedList"] = userAttemptList;
            }
            return RedirectToAction("StartTest", "EntryTest");
        }
        public ActionResult ViewEntryTestResult()
        {
            var rollNumber = Session["RollNumber"].ToString();
            var loggedUserId = Session["UserId"].ToString();
            var applicantDetails = db.Applicants.Where(a => a.userId == loggedUserId).FirstOrDefault();
            var entryTestResultDetails = db.EntryTestResults.Where(e => e.rollNumber == rollNumber).FirstOrDefault();
            EntryTestResultVM model = new EntryTestResultVM()
            {
                applicant = applicantDetails,
                entryTestResult = entryTestResultDetails
            };
            return View(model);
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