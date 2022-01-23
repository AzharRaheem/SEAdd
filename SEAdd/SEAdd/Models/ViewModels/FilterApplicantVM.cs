using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class FilterApplicantVM
    {
        public List<Applicant> Applicants { get; set; }
        public List<Department> departments { get; set; }
        [Display(Name ="Department")]
        [Required]
        public string departmentName { get; set; }
        [Display(Name ="Admission Year")]
        [RegularExpression("^(19|20)\\d{2}$" , ErrorMessage ="Please enter correct year.")]
        [Required]
        public int? year { get; set; }
        [Display(Name ="Other Attributes")]
        [Required]
        public string otherAttributes { get; set; }
    }
}