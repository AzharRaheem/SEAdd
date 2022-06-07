using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class Provience
    {
        [Key]
        public int id { get; set; }
        [Required][Display(Name = "Provience Name")]
        [StringLength(100)]
        [RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$", ErrorMessage = "Please enter valid provience name.")]
        public string name { get; set; }

        public virtual ICollection<Applicant> Applicants { get; set; }
    }
}