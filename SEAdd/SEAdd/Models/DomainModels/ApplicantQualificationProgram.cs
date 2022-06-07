using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class ApplicantQualificationProgram
    {
        //This Model is used to Generate Report...
        //Applicant's Details...
        public int id { get; set; }
        public string userId { get; set; }
        public string FullName { get; set; }
        public string FatherName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string CNIC { get; set; }
        public string PersonalContact { get; set; }
        public string GuardianContact { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string Domicile { get; set; }
        public string Quota { get; set; }
        public string PermanentAddress { get; set; }
        public string PostalAddress { get; set; }
        public string Provience { get; set; }
        public string profileImgUrl { get; set; }
        public string District { get; set; }
        public DateTime ApplyDate { get; set; }
        //Previous Qualification Details...
        //Matric...
        public string M_AcademicDegree { get; set; }
        public string M_Discipline { get; set; }
        public string M_YearOfPassing { get; set; }
        public string M_RollNumber { get; set; }
        public string M_boardOrInstitute { get; set; }
        public string M_CountryName { get; set; }
        public double M_TotalMarks { get; set; }
        public double M_ObtainedMarks { get; set; }
        public double M_Percentage { get; set; }
        //Intermediate...
        public string I_AcademicDegree { get; set; }
        public string I_Discipline { get; set; }
        public string I_YearOfPassing { get; set; }
        public string I_RollNumber { get; set; }
        public string I_boardOrInstitute { get; set; }
        public string I_CountryName { get; set; }
        public double I_TotalMarks { get; set; }
        public double I_ObtainedMarks { get; set; }
        public double I_Percentage { get; set; }
        //Program's Details...
        public string DepartmentName { get; set; }
        public string ProgramName { get; set; }
        public string ProgramType { get; set; }
        public int duration { get; set; }
        public string campus { get; set; }

    }
}