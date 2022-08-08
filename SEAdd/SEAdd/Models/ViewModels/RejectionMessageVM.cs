using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class RejectionMessageVM
    {
        public RejectionMessage rejectionMessage { get; set; }
        public string departmentName { get; set; }
        public string programName { get; set; }
    }
}