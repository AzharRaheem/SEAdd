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
        [StringLength(100)]
        [UniqueCampus(ErrorMessage = "Campus already exist.")]
        [Display(Name = "Campus Name")]
        public string name { get; set; }
    }
}