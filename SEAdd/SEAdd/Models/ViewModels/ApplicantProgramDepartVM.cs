using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class ApplicantProgramDepartVM
    {
        public List<Applicant> DepartmentalApplicants { get; set; }
        public ProgramSelection ProgramSelection { get; set; }
        public string adminDepartment { get; set; }
    }
}