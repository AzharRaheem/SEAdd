using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class ProgramSelection
    {
        [Key]
        public int id { get; set; }
        public virtual Applicant Applicant { get; set; }
        [Required]
        [ForeignKey("Applicant")]
        public int ApplicantId { get; set; }
        public virtual Department Department { get; set; }
        [Display(Name = "Department")]
        [Required]
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Program Program { get; set; }
        [Display(Name = "Program")]
        [Required(ErrorMessage ="Please choose a program to apply for admission")]
        [ForeignKey("Program")]
        public int ProgramId { get; set; }
    }
}