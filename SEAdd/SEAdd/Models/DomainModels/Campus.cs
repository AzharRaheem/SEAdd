using SEAdd.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class Campus
    {
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(100)][RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$" , ErrorMessage ="Please enter valid campus name.")]
        [UniqueCampus(ErrorMessage = "Campus already exist.")]
        [Display(Name = "Campus Name")]
        public string name { get; set; }


        public virtual ICollection<Applicant> Applicants { get; set; }
    }
}