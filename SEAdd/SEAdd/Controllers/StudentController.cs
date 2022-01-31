using CrystalDecisions.CrystalReports.Engine;
using SEAdd.CustomValidations;
using SEAdd.Models;
using SEAdd.Models.DomainModels;
using SEAdd.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace SEAdd.Controllers
{
    [HandleError]
    [Authorize(Roles = "User , Admin")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext db;
        public StudentController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewAllApplicants()
        {
            List<Applicant> applicants = db.Applicants.OrderByDescending(a => a.isApproved).ToList();
            return View(applicants);
        }
        public ActionResult GetApplicantDetail(int id)
        {
            var applicant = db.Applicants.Where(a => a.Id == id).FirstOrDefault();
            return View(applicant);
        }
        public ActionResult DeleteApplicant(int id)
        {
            var applicant = db.Applicants.Where(a => a.Id == id).FirstOrDefault();
            if(applicant != null)
            {
                db.Applicants.Remove(applicant);
                db.SaveChanges();
            }
            return RedirectToAction("CurrentYearApplicants");
        }
        public ActionResult ApproveApplicant(int id)
        {
            var applicant = db.Applicants.Where(a => a.Id == id).FirstOrDefault();
            if (applicant != null)
            {
                applicant.isApproved = true;
                applicant.isRejected = false;
                db.Entry(applicant).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var rejectionReason = db.RejectionReasons.Where(r => r.applicantId == applicant.Id).FirstOrDefault();
                if (rejectionReason != null)
                {
                    db.RejectionReasons.Remove(rejectionReason);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("CurrentYearApplicants");
        }
        public ActionResult EligibilityCriteria()
        {
            return View();
        }
        public ActionResult StudentRegistration()
        {
            string userId = Convert.ToString(Session["UserId"]);
            var LoggedUser = Session["User"] as ApplicationUser;
            StudentApplicationVM vm = new StudentApplicationVM()
            {
                applicant = new Applicant() { FirstName = LoggedUser.FirstName, LastName = LoggedUser.LastName, Email = LoggedUser.Email, CNIC = LoggedUser.Cnic },
                Boards = db.Boards.ToList(),
                Campuses = db.Campuses.ToList(),
                Programs = db.Programs.ToList(),
                Departments = db.Departments.ToList(),
                Quotas = db.Qotas.ToList(),
                GenderList = GetLists.GetGenderList(),
                GradesList = GetLists.GetGradesList(),
                DivisionsList = GetLists.GetDivisionsList(),
                MetricProgramsList = GetLists.GetMetricProgramsList(),
                FScProgramsList = GetLists.GetFScProgramsList()
            };
            return View(vm);
        }
        [HttpPost]
        public ActionResult StudentRegistration(StudentApplicationVM vm , HttpPostedFileBase profileImg , HttpPostedFileBase metricMarksSheet , HttpPostedFileBase fscMarksSheetDiploma)
        {
            vm.applicant.MetricPercentage = (vm.applicant.MetricObtainedMarks / vm.applicant.MetricTotalMarks) * 100;
            vm.applicant.FScPercentage = (vm.applicant.FScObtainedMarks / vm.applicant.FScTotalMarks) * 100;
            if (!ModelState.IsValid)
            {
                StudentApplicationVM applicant = new StudentApplicationVM()
                {
                    applicant = vm.applicant,
                    Boards = db.Boards.ToList(),
                    Campuses = db.Campuses.ToList(),
                    Programs = db.Programs.ToList(),
                    Departments = db.Departments.ToList(),
                    Quotas = db.Qotas.ToList(),
                    GenderList = GetLists.GetGenderList(),
                    GradesList = GetLists.GetGradesList(),
                    DivisionsList = GetLists.GetDivisionsList(),
                    MetricProgramsList = GetLists.GetMetricProgramsList(),
                    FScProgramsList = GetLists.GetFScProgramsList()
                };
                return View(applicant);
            }
            vm.applicant.userId = Session["UserId"].ToString();
            vm.applicant.profileImgUrl = GetFileUrl(profileImg);
            vm.applicant.MetricMarksSheetUrl = GetFileUrl(metricMarksSheet);
            vm.applicant.FScMarksSheetUrl = GetFileUrl(fscMarksSheetDiploma);
            vm.applicant.applyDate = DateTime.Today.Date;
            db.Applicants.Add(vm.applicant);
            db.SaveChanges();
            Session["UserAlreadyExist"] = true;
            return RedirectToAction("UserDashboard" , "Dashboard");
        }
        public ActionResult PrintChallan()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/"), "Challan.rpt"));
            var feeDetails = db.Fees.OrderByDescending(f => f.id).FirstOrDefault();
            var model = db.Fees.Where(f => f.id == feeDetails.id).ToList();
            rd.SetDataSource(model);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "Challan_SoftwareEngineering.pdf");
        }
        public ActionResult PrintApplicantForm(int id)
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/"), "ApplicantsDetail.rpt"));
            var model = db.Applicants.Where(a => a.Id == id).ToList();
            var image = model[0].profileImgUrl;
            var cutPathNewImage = image.Remove(0, 1);
            model[0].profileImgUrl = cutPathNewImage;
            rd.SetDataSource(model);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", model[0].Id.ToString()+"-ApplicantionForm.pdf");
        }
        public ActionResult PrintApprovedApplicant()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/"), "ApprovedApplicant.rpt"));
            var model = db.Applicants.Where(a => a.isApproved == true && a.applyDate.Year == DateTime.Today.Date.Year).ToList();
            rd.SetDataSource(model);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "ApprovedApplicants.pdf");
        }
        public ActionResult RejectApplicant(int id , string rejectionMsg)
        {
            var applicant = db.Applicants.Where(a => a.Id == id).FirstOrDefault();
            if (applicant != null)
            {
                try
                {
                    #region SendEmailToUser
                    SendEmail sendEmail = new SendEmail();
                    sendEmail.SendMailMessage(applicant.Email, "Your Application has been rejected !", rejectionMsg);
                    #endregion
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    applicant.isRejected = true;
                    applicant.isApproved = false;
                    db.Entry(applicant).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    RejectionReason rejectionReason = new RejectionReason();
                    rejectionReason.applicantId = id;
                    rejectionReason.RejectionMessage = rejectionMsg;
                    db.RejectionReasons.Add(rejectionReason);
                    db.SaveChanges();
                }               
            }
            return RedirectToAction("CurrentYearApplicants");
        }
        [NonAction]
        private string GetFileUrl(HttpPostedFileBase file)
        {
            string filePath = null;
            string folderPath = Server.MapPath("~/Images/");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            if (file != null && file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                var fileExtension = fileName.Split('.')[1];
                Guid UniqueName = Guid.NewGuid();
                var imageName = UniqueName + "." + fileExtension;
                filePath = "~/Images/" + imageName;
                file.SaveAs(Server.MapPath(filePath));
            }
            return filePath;
        }
        #region FilterPageActions
        public ActionResult FilterApplicants()
        {
            FilterApplicantVM vm = new FilterApplicantVM()
            {
                Applicants = db.Applicants.ToList() , 
                departments = db.Departments.ToList() , 
                year = DateTime.Today.Year,
                departmentName = null , 
                otherAttributes = null
            };
            return View(vm);
        }
        [HttpPost]
        public ActionResult FilterApplicants(FilterApplicantVM vm)
        {
            if (!ModelState.IsValid)
            {
                FilterApplicantVM VmModel = new FilterApplicantVM()
                {
                    Applicants = null,
                    departments = db.Departments.ToList(),
                    year = vm.year,
                    departmentName = null,
                    otherAttributes = null
                };
                return View(VmModel);
            }
            FilterApplicantVM model = new FilterApplicantVM()
            {
                Applicants = db.Applicants.Where(a => a.Department.name == vm.departmentName && a.applyDate.Year == vm.year && (a.FirstName.ToLower().Contains(vm.otherAttributes.Trim().ToLower()) || (a.FirstName+" "+a.LastName).ToLower().Contains(vm.otherAttributes.Trim().ToLower()) ||
                a.LastName.ToLower().Contains(vm.otherAttributes.Trim().ToLower()) || a.FatherName.Trim().ToLower().Contains(vm.otherAttributes.Trim().ToLower()) || a.Email.ToLower().Contains(vm.otherAttributes.Trim().ToLower()) || a.Gender.ToLower().Contains(vm.otherAttributes.Trim().ToLower()) ||
                a.StateProvince.ToLower().Contains(vm.otherAttributes.Trim().ToLower()) || a.City.ToLower().Contains(vm.otherAttributes.Trim().ToLower()) || a.Campus.name.ToLower().Contains(vm.otherAttributes.Trim().ToLower()) || a.FScBoard.ToLower().Contains(vm.otherAttributes.Trim().ToLower()) ||
                a.Program.ProgramName.ToLower().Contains(vm.otherAttributes.Trim().ToLower()) || a.Department.name.ToLower().Contains(vm.otherAttributes.Trim().ToLower()) || a.Qota.name.ToLower().Contains(vm.otherAttributes.Trim().ToLower()))).ToList(),
                departments = db.Departments.ToList(),
                year = vm.year,
                departmentName = vm.departmentName
            };
            return PartialView("_ViewFilteredApplicants", model);
        }
        public ActionResult CurrentYearApplicants()
        {
            var model = db.Applicants.Where(a => a.applyDate.Year == DateTime.Now.Year).OrderBy(a => a.isApproved).ToList();
            return View(model);
        }
        public ActionResult GetApprovedApplicants()
        {
            FilterApplicantVM model = new FilterApplicantVM()
            {
                Applicants = db.Applicants.Where(a => a.isApproved == true).ToList() ,
                departments = db.Departments.ToList(),
                year = DateTime.Now.Year,
                departmentName = null
            };
            return PartialView("_ViewFilteredApplicants", model);
        }
        public ActionResult GetUnApprovedApplicants()
        {
            FilterApplicantVM model = new FilterApplicantVM()
            {
                Applicants = db.Applicants.Where(a => a.isApproved == false).ToList(),
                departments = db.Departments.ToList(),
                year = DateTime.Now.Year,
                departmentName = null
            };
            return PartialView("_ViewFilteredApplicants", model);
        }
        public ActionResult GetRejectedApplicants()
        {
            FilterApplicantVM model = new FilterApplicantVM()
            {
                Applicants = db.Applicants.Where(a => a.isRejected == true).ToList(),
                departments = db.Departments.ToList(),
                year = DateTime.Now.Year,
                departmentName = null
            };
            return PartialView("_ViewFilteredApplicants", model);
        }
        public ActionResult GetTodaysApplicants()
        {
            FilterApplicantVM model = new FilterApplicantVM()
            {
                Applicants = db.Applicants.Where(a => a.applyDate == DateTime.Today.Date).ToList(),
                departments = db.Departments.ToList(),
                year = DateTime.Now.Year,
                departmentName = null
            };
            return PartialView("_ViewFilteredApplicants", model);
        }
        public ActionResult GetThisMonthsApplicants()
        {
            FilterApplicantVM model = new FilterApplicantVM()
            {
                Applicants = db.Applicants.Where(a => a.applyDate.Month == DateTime.Today.Month).ToList(),
                departments = db.Departments.ToList(),
                year = DateTime.Now.Year,
                departmentName = null
            };
            return PartialView("_ViewFilteredApplicants", model);
        }
        public ActionResult GetLocalApplicants()
        {
            FilterApplicantVM model = new FilterApplicantVM()
            {
                Applicants = db.Applicants.Where(a => a.City.ToLower() == "muzaffarabad").ToList(),
                departments = db.Departments.ToList(),
                year = DateTime.Now.Year,
                departmentName = null
            };
            return PartialView("_ViewFilteredApplicants", model);
        }
        #endregion
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }
    }
}