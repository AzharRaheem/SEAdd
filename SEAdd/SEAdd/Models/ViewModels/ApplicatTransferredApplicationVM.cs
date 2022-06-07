using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class ApplicatTransferredApplicationVM
    {
        public Applicant applicant { get; set; }
        public ProgramSelection programDetails { get; set; }
        public Department oldDepartment { get; set; }
        public Department newDepartment { get; set; }
    }
}