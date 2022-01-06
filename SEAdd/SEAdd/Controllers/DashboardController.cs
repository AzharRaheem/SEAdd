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
                applicationsCount = db.Applicants.Count() , 
                usersCount = db.Users.Count() , 
                departmentCount = db.Departments.Count() , 
                approvedApplicationsCount = db.Applicants.Where(a => a.isApproved == true).Count() , 
                Applicants = db.Applicants.ToList()
            };
            return View(vm);
        }
        [Authorize(Roles ="User")]
        public ActionResult UserDashboard()
        {
            return View();
        }
    }
}