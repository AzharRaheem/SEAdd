using CrystalDecisions.CrystalReports.Engine;
using SEAdd.CustomValidations;
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
            List<Applicant> applicants = db.Applicants.ToList();
            return View(applicants);
        }
        public ActionResult EligibilityCriteria()
        {
            return View();
        }
        public ActionResult StudentRegistration()
        {
            string userId = Convert.ToString(Session["UserId"]);
            Applicant applicantDetails = db.Applicants.Where(u => u.userId == userId).FirstOrDefault();
            if(applicantDetails != null)
            {
                applicantDetails.BirthDate = applicantDetails.BirthDate.Date;
                StudentApplicationVM vm = new StudentApplicationVM()
                {
                    applicant = applicantDetails,
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
            else
            {
                var LoggedUser = Session["User"] as ApplicationUser;
                StudentApplicationVM vm = new StudentApplicationVM()
                {
                    applicant = new Applicant() { FirstName = LoggedUser.FirstName , LastName = LoggedUser.LastName ,Email = LoggedUser.Email , CNIC = LoggedUser.Cnic },
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
            db.Applicants.Add(vm.applicant);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult PrintChallan()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "Challan.rpt"));
            var model = db.Fees.ToList();
            rd.SetDataSource(model);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "Challan_SoftwareEngineering.pdf");
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }
    }
}