using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class StudentApplicationVM
    {
        public Applicant applicant { get; set; }
        public List<string> GenderList { get; set; }
        public List<Department> Departments { get; set; }
        public List<Program> Programs { get; set; }
        public List<Campus> Campuses { get; set; }
        public List<Qota> Quotas { get; set; }
        public List<Board> Boards { get; set; }
        public List<string> GradesList { get; set; }
        public List<string> DivisionsList { get; set; }
        public List<string> MetricProgramsList { get; set; }
        public List<string> FScProgramsList { get; set; }
    }
}