using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class ApplicantPreviousQualificationVM
    {
        public Academic academic { get; set; }
        public List<Country> Countries { get; set; }
        public List<AcademicDegree> AcademicDegrees { get; set; }
        public int academicCount { get; set; }
        public List<Academic> ExistingAcademics { get; set; }
        public int ProgramSelectionCount { get; set; }
    }
}