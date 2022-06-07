using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class ApplicantDepartmentReviewVM
    {
        public Applicant applicant { get; set; }
        public ProgramSelection programSelection { get; set; }
        public string adminDepartment { get; set; }
        public RejectionMessage RejectionMsg { get; set; }
        public TransferApplicationFormVM TransferApplication { get; set; }

    }
}