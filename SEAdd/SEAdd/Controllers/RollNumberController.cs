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
    public class RollNumberController : Controller
    {
        ApplicationDbContext db;
        public RollNumberController()
        {
            db = new ApplicationDbContext();
        }   
        [Authorize(Roles ="Admin , SuperAdmin")]
        // GET: RollNumber
        public ActionResult Index()
        {
            var applicants = db.Applicants.Where(a => a.ApplyDate.Year == DateTime.Today.Year).ToList();
            foreach (var applicant in applicants)
            {
                foreach (var program in applicant.ProgramsSelection)
                {
                    var rollNumbers = db.RollNumbers.Where(r => r.programId == program.id && r.applicantId == applicant.id).FirstOrDefault();
                    if(rollNumbers == null)
                    {
                        var applicantRollNumber = new RollNumber();
                        applicantRollNumber.applicantId = applicant.id;
                        applicantRollNumber.programId = program.id;
                        applicantRollNumber.departmentName = program.Department.name;
                        applicantRollNumber.userId = applicant.userId;
                        applicantRollNumber.applicantRollNumber = DateTime.Today.Year.ToString() + "-ET-" + program.id;
                        db.RollNumbers.Add(applicantRollNumber);
                        db.SaveChanges();
                    }
                }
            }
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
              //Model For Viewing the Roll Number with the applicant and program details...
            List<ApplicantRollNumberVM> model = new List<ApplicantRollNumberVM>();
            var ApplicantsRollNumbers = db.RollNumbers.Where(a => a.departmentName.ToLower() == user.department.ToLower()).ToList();
            foreach (var applicantRollNumber in ApplicantsRollNumbers)
            {
                ApplicantRollNumberVM rollNumberDetails = new ApplicantRollNumberVM();
                rollNumberDetails.applicant = db.Applicants.Where(a => a.id == applicantRollNumber.applicantId).FirstOrDefault();
                rollNumberDetails.programDetail = db.ProgramSelections.Where(p => p.id == applicantRollNumber.programId).FirstOrDefault();
                rollNumberDetails.applicantRollNumber = applicantRollNumber;
                model.Add(rollNumberDetails);
            }
            return View(model);
        }
        [Authorize(Roles ="User")]
        public ActionResult GetRollNumber()
        {
            List<ApplicantRollNumberVM> model = new List<ApplicantRollNumberVM>();
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            var applicant = db.Applicants.Where(a => a.userId == user.Id && a.isRegistrationFinished == true).FirstOrDefault();
            if(applicant != null && applicant.ProgramsSelection.Count() != 0)
            {
                foreach (var program in applicant.ProgramsSelection)
                {
                    if (program.isRejected == false)
                    {
                        var deptSettings = db.DepartmentSettings.Where(ds => ds.departmentName.ToLower() == program.Department.name.ToLower()).FirstOrDefault();
                        if (deptSettings != null)
                        {
                            if (deptSettings.isEntryTestRequired)
                            {
                                ApplicantRollNumberVM obj = new ApplicantRollNumberVM();
                                obj.applicant = applicant;
                                obj.applicantRollNumber = db.RollNumbers.Where(r => r.applicantId == applicant.id).FirstOrDefault();
                                obj.programDetail = program;
                                model.Add(obj);
                            }
                        }
                    }
                }
            }
            return View(model);
        }
    }
}