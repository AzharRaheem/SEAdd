using SEAdd.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class Department
    {
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(100)][RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$" , ErrorMessage ="Please enter valid department name.")]
        [Display(Name = "Department Name")]
        public string name { get; set; }
        public virtual Campus Campus { get; set; }
        [Display(Name = "Campus")]
        [Required]
        [ForeignKey("Campus")]
        public int CampusId { get; set; }
        [Required]
        [DefaultValue(false)]
        public bool BS { get; set; }
        [DefaultValue(false)]
        public bool MS { get; set; }
        [DefaultValue(false)]
        public bool PHD { get; set; }
        [Required]
        [DefaultValue(false)]
        public bool EntryTestRequired { get; set; }
        public string DeptLogUrl { get; set; }

        public virtual ICollection<ProgramSelection> ProgramsSelection { get; set; }
    }
}