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
    public class Applicant
    {
        //Personal Details
        [Key]
        public int Id { get; set; }
        public string userId { get; set; } //This is logged user id....
        [Display(Name ="First Name")][Required][StringLength(50)][RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$" , ErrorMessage ="Please enter valid name like \'John\'")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")][Required][StringLength(50)][RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$" , ErrorMessage ="Please enter valid name like \'John\'")]
        public string LastName { get; set; }
        [Display(Name = "Father's Name")][Required][StringLength(70)][RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$" , ErrorMessage = "Please enter valid name like \'John , John Smith\'")]
        public string FatherName { get; set; }
        [Display(Name = "Email")][EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Birth Date")][Required]
        [CheckValidDOB(ErrorMessage ="You must be atleast 18 years old.")]
        public DateTime BirthDate { get; set; }
        [Display(Name = "Gender")][Required]
        public string Gender { get; set; }
        [Display(Name ="CNIC")][Required]
        [RegularExpression("^[0-9]{5}-[0-9]{7}-[0-9]$", ErrorMessage = "CNIC must follow the XXXXX-XXXXXXX-X format!")]
        public string CNIC { get; set; }
        [Display(Name ="Personal Contact")][Required]
        [RegularExpression("^((\\+92)?(0092)?(92)?(0)?)(3)([0-9]{9})$", ErrorMessage = "Enter Correct Mobile Number (03000000000)")]
        public string PersonalContact { get; set; }
        [Display(Name = "Guardian Contact")][Required]
        [RegularExpression("^((\\+92)?(0092)?(92)?(0)?)(3)([0-9]{9})$", ErrorMessage = "Enter Correct Mobile Number (03000000000)")]
        public string GuardianContact { get; set; }
        [Display(Name = "Permanent Address")][Required][RegularExpression("^[#.0-9a-zA-Z\\s,-]+$" , ErrorMessage ="Please enter a valid address.")]
        public string PermanentAddress { get; set; }
        [Display(Name = "Postal Address")][Required][RegularExpression("^[#.0-9a-zA-Z\\s,-]+$" , ErrorMessage ="Please enter a valid address.")]
        public string PostalAddress { get; set; }
        [Display(Name = "State / Province")][Required]
        public string StateProvince { get; set; }
        [Display(Name = "City")][Required]
        public string City { get; set; }
        [Display(Name = "Domicile")][Required]
        public string Domicile { get; set; }
        public string profileImgUrl { get; set; }
        //Previous Qualification Details
               //Metric Details
        [Display(Name ="Total Marks")][Required]
        [Range(1100, 1100, ErrorMessage = "Total marks must be 1100.")]
        public double MetricTotalMarks { get; set; }
        [Display(Name ="Obtained Marks")][Required]
        public double MetricObtainedMarks { get; set; }
        [Display(Name = "Program")][Required]
        public string MetricProgram { get; set; }
        [Display(Name = "Board")][Required]
        public string MetricBoard { get; set; }
        [Display(Name = "Grade")][Required]
        public string MetricGrade { get; set; }
        [Display(Name = "Division")][Required]
        public string MetricDivision { get; set; }
        public double MetricPercentage { get; set; }
        [Display(Name = "Year of Passing")][Required][Range(1999,2022 , ErrorMessage ="Please enter a valid year.")]
        public string MetricYearOfPassing { get; set; }
        [Display(Name = "Roll Number")][Required][Range(1, 10000000, ErrorMessage = "Please enter a valid Roll Number.")]
        public string MetricRollNumber { get; set; }
        public string MetricMarksSheetUrl { get; set; }
        //FSC Details
        [Display(Name = "Total Marks")][Required][GetTotalMarks]
        public double FScTotalMarks { get; set; }
        [Display(Name = "Obtained Marks")][Required]
        [CheckValidObtainedMarks(ErrorMessage ="Obtained marks must be less than total marks.")]
        public double FScObtainedMarks { get; set; }
        [Display(Name = "Program")][Required]
        public string FScProgram { get; set; }
        [Display(Name = "Board")][Required]
        public string FScBoard { get; set; }
        [Display(Name = "Grade")][Required]
        public string FScGrade { get; set; }
        [Display(Name = "Division")][Required]
        public string FScDivision { get; set; }
        public double FScPercentage { get; set; }
        [Display(Name = "Year of Passing")][Required][Range(1999 , 2022 , ErrorMessage ="Please enter a valid year.")]
        public string FScYearOfPassing { get; set; }
        [Display(Name = "Roll Number")][Required][Range(1, 10000000, ErrorMessage = "Please enter a valid Roll Number.")]
        public string FScRollNumber { get; set; }
        public string FScMarksSheetUrl { get; set; }
        //Program Selection Details
        public virtual Campus Campus { get; set; }
        [Display(Name ="Campus")][Required]
        [ForeignKey("Campus")]
        public  int CampusId { get; set; }
        public virtual Department Department { get; set; }
        [Display(Name = "Department")][Required]
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Program Program { get; set; }
        [Display(Name = "Program")][Required]
        [ForeignKey("Program")]
        public int ProgramId { get; set; }
        public virtual Qota Qota { get; set; }
        [Display(Name = "Quota")][Required]
        [ForeignKey("Qota")]
        public int QotaId { get; set; }
        [DefaultValue(false)]
        public bool isApproved { get; set; }
    }
}