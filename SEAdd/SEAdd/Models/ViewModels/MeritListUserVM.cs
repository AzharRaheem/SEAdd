using SEAdd.Models.DomainModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SEAdd.Models.ViewModels
{
    public class MeritListUserVM
    {
        [Required][Display(Name ="Department")]
        public string departmentName { get; set; }
        [Required][Display(Name = "Program")]
        public string programName { get; set; }
        [Required][Display(Name ="Year")]
        public string year { get; set; }

        public List<MeritList> meritList { get; set; }
        public List<Department> Departments { get; set; }
        public List<Program> Programs { get; set; }
    }
}