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
    [Authorize(Roles = "User")]
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
        public ActionResult EligibilityCriteria()
        {
            return View();
        }
        public ActionResult StudentRegistration()
        {
            StudentApplicationVM vm = new StudentApplicationVM() {
            applicant=new Applicant() ,
            Boards = db.Boards.ToList() , 
            Campuses = db.Campuses.ToList() , 
            Programs = db.Programs.ToList() , 
            Departments = db.Departments.ToList() , 
            Quotas = db.Qotas.ToList() , 
            GenderList = GetLists.GetGenderList() , 
            GradesList = GetLists.GetGradesList() , 
            DivisionsList = GetLists.GetDivisionsList() , 
            MetricProgramsList = GetLists.GetMetricProgramsList() , 
            FScProgramsList = GetLists.GetFScProgramsList()
            };
            return View(vm);
        }
        [HttpPost]
        public ActionResult StudentRegistration(StudentApplicationVM vm , HttpPostedFileBase profileImg , HttpPostedFileBase metricMarksSheet , HttpPostedFileBase fscMarksSheetDiploma)
        {
            if(!ModelState.IsValid)
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

            return null;
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


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }
    }
}