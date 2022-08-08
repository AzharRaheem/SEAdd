using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using SEAdd.Models;
using SEAdd.Models.DomainModels;
using SEAdd.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEAdd.Controllers
{
    [HandleError]
    public class MeritListsController : Controller
    {
        ApplicationDbContext db;
        public MeritListsController()
        {
            db = new ApplicationDbContext();
        }
        public ActionResult GenerateMeritList()
        {
            MeritListVM model = new MeritListVM()
            {
                Programs = new List<Program>()
            };
            model.Programs = db.Programs.ToList();
            return View(model);
        }
        // GET: MeritLists
        [HttpPost]
        public ActionResult GenerateMeritList(MeritListVM vm)
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            var allApplicants = db.Applicants.Where(a => a.ApplyDate.Year.ToString() == vm.year && a.isRegistrationFinished == true).ToList();
            var meritCriteria = db.MeritCriterias.Where(m => m.departmentName.ToLower() == user.department.ToLower()).FirstOrDefault();
            var meritList = db.MeritsLists.Where(m => m.departmentName.ToLower() == user.department.ToLower() && m.meritListYear == DateTime.Today.Year.ToString()).ToList();
            if(meritList.Count() != 0)
            {
                Session["MeritList"] = meritList;
                return RedirectToAction("ViewMeritList");
            }
            else
            {
                List<MeritList> model = new List<MeritList>();
                if (meritCriteria != null)
                {
                    if (allApplicants.Count() != 0)
                    {
                        foreach (var applicant in allApplicants)
                        {
                            foreach (var programSelection in applicant.ProgramsSelection)
                            {
                                if (programSelection.Department.name.ToLower() == user.department.ToLower() && programSelection.Program.ProgramName.ToLower() == vm.program.ToLower() && programSelection.isApproved == true)
                                {
                                    MeritList applicantDetails = new MeritList();
                                    applicantDetails.formNo = programSelection.id;
                                    applicantDetails.fullName = applicant.FullName;
                                    applicantDetails.fatherName = applicant.FatherName;
                                    applicantDetails.cnic = applicant.CNIC;
                                    applicantDetails.QuotaName = applicant.Qota.name;
                                    applicantDetails.nominationFrom = applicant.Qota.NominationFrom;
                                    applicantDetails.programName = programSelection.Program.ProgramName;
                                    applicantDetails.departmentName = user.department.ToUpper();
                                    applicantDetails.meritListYear = DateTime.Today.Year.ToString();
                                    applicantDetails.email = applicant.Email;
                                    applicantDetails.domicile = applicant.Domicile;
                                    applicantDetails.isVisible = true;
                                    if (applicant.Gender == "Male")
                                        applicantDetails.gender = "M";
                                    else
                                        applicantDetails.gender = "F";
                                    double matricPercentage = 0;
                                    double intermediatePercentage = 0;
                                    double entryTestPercentage = 0;
                                    foreach (var item in applicant.Academics)
                                    {
                                        if (item.AcademicDegree.ToLower() == "ssc")
                                        {
                                            matricPercentage = (item.ObtainedMarks / item.TotalMarks) * meritCriteria.BsMetricPercentage;
                                            applicantDetails.metricTotalMarks = item.TotalMarks;
                                            applicantDetails.metricObtainedMarks = item.ObtainedMarks;
                                        }
                                        if (item.AcademicDegree.ToLower() == "hssc")
                                        {
                                            intermediatePercentage = (item.ObtainedMarks / item.TotalMarks) * meritCriteria.BsIntermediatePercentage;
                                            applicantDetails.intermediateTotalMarks = item.TotalMarks;
                                            applicantDetails.intermediateObtainedMarks = item.ObtainedMarks;
                                            applicantDetails.annualPassingYear = item.YearOfPassing;
                                        }
                                    }
                                    if (meritCriteria.BsEntryTestPercentage != 0)
                                    {
                                        //Get Entry Test Percentage...
                                        var entryTestResult = db.EntryTestResults.Where(e => e.departmentName.ToLower() == user.department.ToLower() && e.year == vm.year.ToString() && e.programId == programSelection.id).FirstOrDefault();
                                        if (entryTestResult != null)
                                        {
                                            //Do Merit List Percentage....
                                            entryTestPercentage = meritCriteria.BsEntryTestPercentage;
                                            double totalScore = matricPercentage + intermediatePercentage + entryTestResult.obtainedScorePercentage;
                                            applicantDetails.scores = Convert.ToSingle(totalScore);
                                            applicantDetails.rollNo = entryTestResult.rollNumber;
                                            model.Add(applicantDetails);
                                            db.MeritsLists.Add(applicantDetails);
                                            db.SaveChanges();
                                            Session["MeritList"] = model;
                                        }
                                        else
                                        {
                                            TempData["ErrorMessage"] = "Please conduct entry test to generate merit list.";
                                            TempData.Keep();
                                            return RedirectToAction("GenerateMeritList", "MeritLists");
                                        }
                                    }
                                    else
                                    {
                                        entryTestPercentage = meritCriteria.BsEntryTestPercentage;
                                        double totalScore = matricPercentage + intermediatePercentage + entryTestPercentage;
                                        applicantDetails.scores = Convert.ToSingle(totalScore);
                                        model.Add(applicantDetails);
                                        db.MeritsLists.Add(applicantDetails);
                                        db.SaveChanges();
                                        Session["MeritList"] = model;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Merit list has not been created for department of " + user.department+" "+vm.program + "-" + vm.year+".";
                        TempData.Keep();
                        return RedirectToAction("GenerateMeritList", "MeritLists");
                    }
                    //Create and Download Report...
                    if (model.Count() != 0 && model[0].rollNo == null)
                    {
                        //Load the WithoutEntryTestMerit Report List...
                        //ReportDocument rd = new ReportDocument();
                        //rd.Load(Path.Combine(Server.MapPath("~/Reports/"), "MeritListWithoutETest.rpt"));
                        //rd.SetDataSource(model);
                        //Response.Buffer = false;
                        //Response.ClearContent();
                        //Response.ClearHeaders();
                        //Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                        //stream.Seek(0, SeekOrigin.Begin);
                        //return File(stream, "application/pdf", user.department + "-" + "MeritList.pdf");
                        return RedirectToAction("ViewMeritList");
                    }
                    else if (model.Count() != 0 && model[0].rollNo != null)
                    {
                        //Load the EntryTestMerit Report List...
                        //ReportDocument rd = new ReportDocument();
                        //rd.Load(Path.Combine(Server.MapPath("~/Reports/"), "MeritListWithEntryTest.rpt"));
                        //rd.SetDataSource(model);
                        //Response.Buffer = false;
                        //Response.ClearContent();
                        //Response.ClearHeaders();
                        //Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                        //stream.Seek(0, SeekOrigin.Begin);
                        //return File(stream, "application/pdf", user.department + "-" + "MeritList.pdf");
                        return RedirectToAction("ViewMeritList");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "No applicant exist under this program.";
                        TempData.Keep();
                        return RedirectToAction("GenerateMeritList", "MeritLists");
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Please set merit criteria to generate merit list.";
                    TempData.Keep();
                    return RedirectToAction("GenerateMeritList", "MeritLists");
                }
            }
        }
        public ActionResult ViewMeritList()
        {
            List<MeritList> model = Session["MeritList"] as List<MeritList>;
            var sortApplicant = model.Where(a => a.isVisible==true).OrderByDescending(m => m.scores).ToList();
            var quotas = db.Qotas.ToList();
            List<MeritList> vm = new List<MeritList>();
            foreach (var item in quotas)
            {
                foreach (var applicant in sortApplicant)
                {
                    if(applicant.QuotaName.ToLower() == item.name.ToLower())
                    {
                        var applicantCount = vm.Where(a => a.QuotaName.ToLower() == item.name.ToLower()).Count();
                        if(applicantCount != item.numberOfSeats)
                        {
                            vm.Add(applicant);
                        }
                    }
                }
            }
            return View(vm);
        }
        public ActionResult GenerateUserMeritList()
        {
            MeritListUserVM model = new MeritListUserVM()
            {
                Departments = db.Departments.ToList() , 
                Programs = db.Programs.ToList() ,
                meritList = null
            };
            return View(model);
        }
        public ActionResult ViewUserMeritList(MeritListUserVM model)
        {
            if(!ModelState.IsValid)
            {
                MeritListUserVM vm = new MeritListUserVM()
                {
                    Departments = db.Departments.ToList(),
                    Programs = db.Programs.ToList() , 
                    meritList = null
                };
                TempData["ErrorMsg"] = "Please fill all the field.All fields are required.";
                TempData.Keep();
                return RedirectToAction("GenerateUserMeritList");
            }
            MeritListUserVM meritListUserVM = new MeritListUserVM()
            {
                Departments = db.Departments.ToList(),
                Programs = db.Programs.ToList()
            };
            var applicantwithHighScore = db.MeritsLists.Where(m => m.departmentName.ToLower() == model.departmentName.ToLower() && m.programName.ToLower() == model.programName.ToLower() && m.meritListYear == model.year).ToList();
            var sortApplicant = applicantwithHighScore.Where(a => a.isVisible == true).OrderByDescending(m => m.scores).ToList();
            var quotas = db.Qotas.ToList();
            List<MeritList> meritLst = new List<MeritList>();
            foreach (var item in quotas)
            {
                foreach (var applicant in sortApplicant)
                {
                    if (applicant.QuotaName.ToLower() == item.name.ToLower())
                    {
                        var applicantCount = meritLst.Where(a => a.QuotaName.ToLower() == item.name.ToLower()).Count();
                        if (applicantCount != item.numberOfSeats)
                        {
                            meritLst.Add(applicant);
                        }
                    }
                }
            }
            meritListUserVM.meritList = meritLst;
            if(meritListUserVM.meritList.Count() == 0)
            {
                TempData["ErrorMsg"] = "Merit list has not been created for department of "+model.departmentName+" "+model.programName+"-"+model.year+".";
                TempData.Keep();
                return RedirectToAction("GenerateUserMeritList");
            }
            return View(meritListUserVM);
        }
        public ActionResult ViewAllMeritApplicants()
        {
            var model = new MeritListVM()
            {
                Programs = new List<Program>()
            };
            model.Programs = db.Programs.ToList();
            return View(model);
        }
        public ActionResult ViewAllMeritsApplicants(MeritListVM obj)
        {
            if(!ModelState.IsValid)
            {
                TempData["ErrorMsg"] = "Please fill all the field.All fields are required.";
                TempData.Keep();
                return RedirectToAction("ViewAllMeritApplicants");
            }
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            List<MeritList> model = db.MeritsLists.Where(a => a.programName.ToLower() == obj.program.ToLower() && a.meritListYear.ToLower() == obj.year && a.departmentName.ToLower() == user.department.ToLower()).ToList();
            if (model.Count() == 0)
            {
                TempData["ErrorMsg"] = "Merit list has not been created.";
                TempData.Keep();
                return RedirectToAction("ViewAllMeritApplicants");
            }
            var sortApplicant = model.OrderByDescending(m => m.scores).ToList();
            return View(sortApplicant);
        }
        public ActionResult VisibilityOn(int id)
        {
            var model = db.MeritsLists.Where(a => a.id == id).FirstOrDefault();
            model.isVisible = true;
            db.SaveChanges();
            List<MeritList> vm = db.MeritsLists.Where(a => a.programName.ToLower() == model.programName.ToLower() && a.meritListYear.ToLower() == model.meritListYear && a.departmentName.ToLower() == model.departmentName.ToLower()).ToList();
            var sortApplicant = vm.OrderByDescending(m => m.scores).ToList();
            Session["AllMeritApplicants"] = sortApplicant;
            return RedirectToAction("ViewAllApplicantsMerit");
        }
        public ActionResult VisibilityOff(int id)
        {
            var model = db.MeritsLists.Where(a => a.id == id).FirstOrDefault();
            model.isVisible = false;
            db.SaveChanges();
            List<MeritList> vm = db.MeritsLists.Where(a => a.programName.ToLower() == model.programName.ToLower() && a.meritListYear.ToLower() == model.meritListYear && a.departmentName.ToLower() == model.departmentName.ToLower()).ToList();
            var sortApplicant = vm.OrderByDescending(m => m.scores).ToList();
            Session["AllMeritApplicants"] = sortApplicant;
            return RedirectToAction("ViewAllApplicantsMerit");
        }
        public ActionResult ViewAllApplicantsMerit()
        {
            var model = Session["AllMeritApplicants"] as List<MeritList>;
            return View(model);
        }
    }
}