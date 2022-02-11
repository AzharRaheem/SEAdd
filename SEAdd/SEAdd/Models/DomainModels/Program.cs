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
        [Display(Name ="Program Name")]
        public string ProgramName { get; set; }
        [Required][Display(Name ="Duration(in years)")]
        public int duration { get; set; }
        [Required]
        [Display(Name = "Type")]
        public string ProgramType { get; set; }


        public virtual ICollection<ProgramSelection> ProgramsSelection { get; set; }
    }
}