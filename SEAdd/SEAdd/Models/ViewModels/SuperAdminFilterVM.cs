using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class SuperAdminFilterVM
    {
        [Display(Name ="Department")]
        [Required(ErrorMessage ="Please select any department.")]
        public int departmentId { get; set; }
        [Display(Name = "Program")]
        [Required(ErrorMessage = "Please select any program.")]
        public int programId { get; set; }
        [Display(Name = "Year")]
        public int year { get; set; }
        [Display(Name = "Other Attribute")]
        public string otherAttributes { get; set; }

        public List<Department> Departments { get; set; }
        public List<Program> Programs { get; set; }
    }
}