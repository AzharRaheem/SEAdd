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
        [UniqueQota(ErrorMessage = "Already exist.")]
        public string name { get; set; }
        [Required]
        [Display(Name = "Total Seats")]
        public int numberOfSeats { get; set; }
    }
}