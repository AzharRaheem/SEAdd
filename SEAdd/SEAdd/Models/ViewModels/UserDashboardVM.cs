using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class UserDashboardVM
    {
        public Applicant applicant { get; set; }
        public int ApplicantId { get; set; }
        public List<ProgramSelection> programSelections { get; set; }
        public AdmissionDate admissionDate { get; set; }
        public List<Notification> Notifications { get; set; }
    }
}