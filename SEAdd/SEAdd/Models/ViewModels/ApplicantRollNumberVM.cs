using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class ApplicantRollNumberVM
    {
        public Applicant applicant { get; set; }
        public ProgramSelection programDetail { get; set; }
        public RollNumber applicantRollNumber { get; set; }
    }
}