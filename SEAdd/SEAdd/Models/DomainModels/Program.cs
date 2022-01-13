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
        [RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$" , ErrorMessage ="Please enter valid program name.")]
        [Display(Name ="Program Name")]
        public string ProgramName { get; set; }
        [Required][Display(Name ="Duration(in years)")]
        public int duration { get; set; }




        public virtual ICollection<Applicant> Applicants { get; set; }
    }
}