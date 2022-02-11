using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class ProgramSelectionVM
    {
        public ProgramSelection programSelection { get; set; }
        public List<Program> Programs { get; set; }
        public List<Department> Departments { get; set; }
        public List<ProgramSelection> AppliedPrograms { get; set; }
        public int AppliedProgramsCount { get; set; }
    }
}