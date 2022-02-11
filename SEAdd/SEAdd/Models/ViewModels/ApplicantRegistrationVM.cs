using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class ApplicantRegistrationVM
    {
        public Applicant applicant { get; set; }
        public List<Provience> Proviences { get; set; }
        public List<Country> Countries { get; set; }
        public List<Qota> Quotas { get; set; }
        public List<string> Religions { get; set; }
        public List<string> Genders { get; set; }
        public int academicsCount { get; set; }
        public int programSelectionCount { get; set; }
    }
}