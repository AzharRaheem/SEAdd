using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class ApplicantProgramVM
    {
        //Personal Details
        public int Id { get; set; }
        public string userId { get; set; } //This is logged user id....
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string CNIC { get; set; }
        public string PersonalContact { get; set; }
        public string GuardianContact { get; set; }
        public string PermanentAddress { get; set; }
        public string PostalAddress { get; set; }
        public string StateProvince { get; set; }
        public string City { get; set; }
        public string Domicile { get; set; }
        public string profileImgUrl { get; set; }
        //Previous Qualification Details
        //Metric Details
        public double MetricTotalMarks { get; set; }
        public double MetricObtainedMarks { get; set; }
        public string MetricProgram { get; set; }
        public string MetricBoard { get; set; }
        public string MetricGrade { get; set; }
        public string MetricDivision { get; set; }
        public double MetricPercentage { get; set; }
        public string MetricYearOfPassing { get; set; }
        public string MetricRollNumber { get; set; }
        public string MetricMarksSheetUrl { get; set; }
        //FSC Details
        public double FScTotalMarks { get; set; }
        public double FScObtainedMarks { get; set; }
        public string FScProgram { get; set; }
        public string FScBoard { get; set; }
        public string FScGrade { get; set; }
        public string FScDivision { get; set; }
        public double FScPercentage { get; set; }
        public string FScYearOfPassing { get; set; }
        public string FScRollNumber { get; set; }
        public string FScMarksSheetUrl { get; set; }
        //Program Selection Details
        public string Campus { get; set; }
        public string Department { get; set; }
        public string Program { get; set; }
        public string Quota { get; set; }
    }
}