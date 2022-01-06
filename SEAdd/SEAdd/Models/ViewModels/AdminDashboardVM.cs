using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class AdminDashboardVM
    {
        public int boardCount { get; set; }
        public int usersCount { get; set; }
        public int approvedApplicationsCount { get; set; }
        public int applicationsCount { get; set; }
        public int notificationsCount { get; set; }
        public int departmentCount { get; set; }
        public List<Applicant> Applicants { get; set; }
    }
}