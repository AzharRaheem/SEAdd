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
            var allApplicants = db.Applicants.Where(a => a.ApplyDate.Year == vm.year && a.isRegistrationFinished == true).ToList();
            var meritCriteria = db.MeritCriterias.Where(m => m.departmentName.ToLower() == user.department.ToLower()).FirstOrDefault();
            List<MeritList> model = new List<MeritList>();
            if(meritCriteria != null)
            {
                if(allApplicants.Count() != 0)
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
                                double matricPercentage = 0;
                                double intermediatePercentage = 0;
                                double entryTestPercentage = 0;
                                foreach (var item in applicant.Academics)
                                {
                                    if (item.AcademicDegree.ToLower() == "ssc")
                                    {
                                        matricPercentage = (item.ObtainedMarks / item.TotalMarks) * meritCriteria.BsMetricPercentage;
                                    }
                                    if (item.AcademicDegree.ToLower() == "hssc")
                                    {
                                        intermediatePercentage = (item.ObtainedMarks / item.TotalMarks) * meritCriteria.BsIntermediatePercentage;
                                    }
                                }
                                if (meritCriteria.BsEntryTestPercentage != 0)
                                {
                                    //Get Entry Test Percentage...
                                    TempData["ErrorMessage"] = "Please conduct entry test to generate merit list.";
                                    TempData.Keep();
                                    return RedirectToAction("GenerateMeritList", "MeritLists");
                                }
                                else
                                {
                                    entryTestPercentage = meritCriteria.BsEntryTestPercentage;
                                    double totalScore = matricPercentage + intermediatePercentage + entryTestPercentage;
                                    applicantDetails.scores = Convert.ToSingle(totalScore);
                                    model.Add(applicantDetails);
                                }
                            }
                        }
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "No applicant exist under this program.";
                    TempData.Keep();
                    return RedirectToAction("GenerateMeritList", "MeritLists");
                }
                //Create and Download Report...
                if(model.Count() != 0)
                {
                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports/"), "MeritListWithoutETest.rpt"));
                    rd.SetDataSource(model);
                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", user.department + "-" + "MeritList.pdf");
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
}