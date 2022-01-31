using SEAdd.Models;
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
        [Authorize(Roles ="Admin")]
        public ActionResult AdminDashboard()
        {
            AdminDashboardVM vm = new AdminDashboardVM()
            {
                applicationsCount = db.Applicants.Where(a => a.applyDate.Year == DateTime.Now.Year).Count() , 
                usersCount = db.Users.Count() , 
                departmentCount = db.Departments.Count() , 
                approvedApplicationsCount = db.Applicants.Where(a => a.isApproved == true).Count() , 
                Applicants = db.Applicants.Where(a => a.applyDate.Year == DateTime.Now.Year).OrderByDescending(a => a.isApproved).ToList() , 
                notificationsCount = db.Notifications.Count()
            };
            return View(vm);
        }
        [Authorize(Roles ="User")]
        public ActionResult UserDashboard()
        {
            UserDashboardVM vm = new UserDashboardVM();
            vm.admissionLastDate = new DateTime();
            string userId = Session["UserId"].ToString();
            var applicant = db.Applicants.Where(a => a.userId == userId).FirstOrDefault();
            var admissionLastDate = db.AdmissionDate.OrderByDescending(d => d.Id).FirstOrDefault();
            var result =  (admissionLastDate != null)? admissionLastDate.EndDate:DateTime.Today;
            vm.admissionLastDate = result;
            vm.Notifications = db.Notifications.Where(n => n.NotificationDate.Month == DateTime.Today.Month).ToList();
            if (applicant != null)
            {
                if(applicant.isApproved == true)
                {
                    vm.applicationStatus = "Approved";
                }
                else if(applicant.isRejected == true)
                {
                    vm.applicationStatus = "Rejected";
                }
                else
                {
                    vm.applicationStatus = "Pending";
                }
                Session["RejectMsgCount"] = db.RejectionReasons.Where(d => d.applicantId == applicant.Id).Count();
                var rejectionReason = db.RejectionReasons.Where(d => d.applicantId == applicant.Id).OrderByDescending(d=>d.id).FirstOrDefault(); //This is the object of the RejectionReason Model.
                if(rejectionReason != null)
                {
                    Session["RejectMsg"] = rejectionReason.RejectionMessage;
                }
            }
            else
            {
                vm.applicationStatus = "Not Submitted";
            }
            return View(vm);
        }
    }
}