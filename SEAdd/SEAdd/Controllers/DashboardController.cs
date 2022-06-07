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
    [Authorize]
    public class DashboardController : Controller
    {
        ApplicationDbContext db;
        public DashboardController()
        {
            db = new ApplicationDbContext();
        }
        [Authorize(Roles ="Admin , SuperAdmin")]
        public ActionResult AdminDashboard()
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            AdminDashboardVM model = new AdminDashboardVM()
            {
                applicationsCount = db.Applicants.Where(a => a.isRegistrationFinished == true && a.ApplyDate.Year == DateTime.Today.Year).Count(),
                boardCount = db.Boards.Count(),
                departmentCount = db.Departments.Count(),
                notificationsCount = db.Notifications.Count(),
                usersCount = db.Users.Count(),
                departmentalUserCount = db.Users.Where(u => u.department.ToLower() == user.department.ToLower() || u.department == null).Count() ,
                hostelCount = db.Hostels.Count(),
                campusCount = db.Campuses.Count(),
                Applicants = new List<Models.DomainModels.Applicant>(),
                departmentalApplicants = new ApplicantProgramDepartVM(),
                approvedApplicationsCount = GetApprovedApplicationCount(),
                rejectedApplicantsCount = GetRejectedApplicationCount(),
                Programs = new List<Program>()
            };
            model.Applicants = db.Applicants.Where(a => a.isRegistrationFinished == true && a.ApplyDate.Year == DateTime.Today.Year).ToList();
            model.Programs = db.Programs.ToList();
            model.departmentalApplicants = GetAllDepartmentalApplicants();
            model.departmentalApplicantsCount = model.departmentalApplicants.DepartmentalApplicants.Count();
            return View(model);
        }
        [NonAction]
        private int GetApprovedApplicationCount()
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            ApplicantProgramDepartVM vm = new ApplicantProgramDepartVM()
            {
                DepartmentalApplicants = new List<Applicant>(),
                adminDepartment = user.department,
                ProgramSelection = new Models.DomainModels.ProgramSelection()
            };
            var model = db.Applicants.Where(a => a.isRegistrationFinished == true && a.ApplyDate.Year == DateTime.Today.Year).ToList();
            foreach (var item in model)
            {
                foreach (var program in item.ProgramsSelection)
                {
                    if (program.Department.name.ToLower() == user.department.ToLower())
                    {
                        vm.ProgramSelection = item.ProgramsSelection.Where(p => p.Department.name == user.department && p.isApproved == true).FirstOrDefault();
                        if (vm.ProgramSelection != null)
                        {
                            vm.DepartmentalApplicants.Add(item);
                        }
                    }
                }
            }
            return vm.DepartmentalApplicants.Count();
        }
        [NonAction]
        private int GetRejectedApplicationCount()
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            ApplicantProgramDepartVM vm = new ApplicantProgramDepartVM()
            {
                DepartmentalApplicants = new List<Applicant>(),
                adminDepartment = user.department,
                ProgramSelection = new Models.DomainModels.ProgramSelection()
            };
            var model = db.Applicants.Where(a => a.isRegistrationFinished == true && a.ApplyDate.Year == DateTime.Today.Year).ToList();
            foreach (var item in model)
            {
                foreach (var program in item.ProgramsSelection)
                {
                    if (program.Department.name.ToLower() == user.department.ToLower())
                    {
                        vm.ProgramSelection = item.ProgramsSelection.Where(p => p.Department.name == user.department && p.isRejected == true).FirstOrDefault();
                        if(vm.ProgramSelection != null)
                        {
                            vm.DepartmentalApplicants.Add(item);
                        }
                    }
                }
            }
            return vm.DepartmentalApplicants.Count();
        }
        [NonAction]
        public ApplicantProgramDepartVM GetAllDepartmentalApplicants()
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            ApplicantProgramDepartVM vm = new ApplicantProgramDepartVM()
            {
                DepartmentalApplicants = new List<Applicant>(),
                adminDepartment = user.department,
                ProgramSelection = new Models.DomainModels.ProgramSelection()
            };
            var model = db.Applicants.Where(a => a.isRegistrationFinished == true && a.ApplyDate.Year == DateTime.Today.Year).ToList();
            foreach (var item in model)
            {
                foreach (var program in item.ProgramsSelection)
                {
                    if (program.Department.name.ToLower() == user.department.ToLower())
                    {
                        vm.ProgramSelection = item.ProgramsSelection.Where(p => p.Department.name == user.department).FirstOrDefault();
                        vm.DepartmentalApplicants.Add(item);
                    }
                }
            }
            return vm;
        }
        
        public ActionResult UserDashboard()
        {
            string loggedUserId = Session["UserId"].ToString();
            UserDashboardVM userDashboardVM = new UserDashboardVM()
            {
                 applicant = db.Applicants.Where(a=>a.userId == loggedUserId).FirstOrDefault() , 
                 admissionDate = db.AdmissionDate.OrderByDescending(a=>a.Id).FirstOrDefault() , 
                 Notifications = db.Notifications.ToList() , 
            };
            if(userDashboardVM.applicant != null)
            {
                userDashboardVM.ApplicantId = userDashboardVM.applicant.id;
                userDashboardVM.programSelections = db.ProgramSelections.Where(p => p.ApplicantId == userDashboardVM.ApplicantId).ToList();
                RejectionMessage msg = db.RejectionMessages.Where(r => r.applicantId == userDashboardVM.ApplicantId).OrderByDescending(r => r.id).FirstOrDefault();
                if(msg != null)
                {
                    Session["MessageCount"] = 1;
                    Session["MessageTitle"] = msg.title;
                    Session["MessageBody"] = msg.message;
                }
            }
            else
            {
                userDashboardVM.ApplicantId = 0;
                userDashboardVM.programSelections = null;
            }
            return View(userDashboardVM);
        }
    }
}