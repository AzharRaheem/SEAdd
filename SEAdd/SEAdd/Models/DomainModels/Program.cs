using SEAdd.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class Program
    {
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(50)]
        [UniqueProgram(ErrorMessage ="Program already exist.")]
        [RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$" , ErrorMessage ="Please enter valid program name.")]
        [Display(Name ="Program Name")]
        public string ProgramName { get; set; }




        public ICollection<Applicant> Applicants { get; set; }
    }
}