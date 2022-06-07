using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class SuperAdminDeptVM
    {
        public ApplicantDepartmentReviewVM applicantDepartmentVM { get; set; }
        public int applicantId { get; set; }
        public string departmentName { get; set; }
    }
}