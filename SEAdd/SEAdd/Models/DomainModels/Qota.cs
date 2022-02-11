using SEAdd.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class Qota
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name = "Qota Name")]
        //[UniqueQota(ErrorMessage = "Already exist.")]
        [RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$", ErrorMessage = "Please enter valid quota name.")]
        public string name { get; set; }
        [Required]
        [Display(Name = "Total Seats")]
        public int numberOfSeats { get; set; }


        public virtual ICollection<Applicant> Applicants { get; set; }

    }
}