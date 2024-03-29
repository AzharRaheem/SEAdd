﻿using SEAdd.Models.DomainModels;
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
        public int departmentalUserCount { get; set; }
        public int approvedApplicationsCount { get; set; }
        public int applicationsCount { get; set; }
        public int notificationsCount { get; set; }
        public int rejectedApplicantsCount { get; set; }
        public int departmentCount { get; set; }
        public int hostelCount { get; set; }
        public int campusCount { get; set; }
        public int departmentalApplicantsCount { get; set; }
        public List<Applicant> Applicants { get; set; }
        public ApplicantProgramDepartVM departmentalApplicants { get; set; }
        public List<Program> Programs { get; set; }
    }
}