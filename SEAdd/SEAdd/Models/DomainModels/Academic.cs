using SEAdd.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class Academic
    {
        [Key]
        public int id { get; set; }
        public virtual Applicant Applicant { get; set; }
        [Required]
        [ForeignKey("Applicant")]
        public int ApplicantId { get; set; }
        [Display(Name = "Certificate / Degree")]
        [Required]
        public string AcademicDegree { get; set; }
        [Display(Name = "Subject / Discipline")]
        [Required]
        public string Discipline{ get; set; }
        [Display(Name = "Year of Passing")]
        [Required]
        [Range(0, 50000, ErrorMessage = "Please enter a valid year.")]
        public string YearOfPassing { get; set; }
        [Display(Name = "Roll Number")]
        [Required]
        [Range(1, 10000000, ErrorMessage = "Please enter a valid Roll Number.")]
        public string RollNumber { get; set; }
        [Display(Name = "University/ Board / Institute")]
        [Required]
        public string boardOrInstitute { get; set; }
        public virtual Country Country { get; set; }
        [Display(Name = "Country")]
        [Required]
        [ForeignKey("Country")]
        public int CountryId { get; set; }
        [Display(Name = "Total Marks")]
        [Required]
        [GetTotalMarks]
        public double TotalMarks { get; set; }
        [Display(Name = "Obtained Marks")]
        [Required][CheckValidObtainedMarks]
        public double ObtainedMarks { get; set; }
        [Display(Name = "Percentage")]
        [Required]
        public double Percentage { get; set; }       
        public string DMcMarksSheetUrl { get; set; }
    }
}