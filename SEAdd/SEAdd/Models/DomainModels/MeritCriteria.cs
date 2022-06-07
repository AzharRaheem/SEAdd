using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class MeritCriteria
    {
        [Key]
        public int id { get; set; }
        //For BS-Program
        [Display(Name ="Matric Percentage")]
        public float BsMetricPercentage { get; set; }
        [Display(Name ="Intermediate Percentage")]
        public float BsIntermediatePercentage { get; set; }
        [Display(Name ="Entry Test Percentage")]
        public float BsEntryTestPercentage { get; set; }
        //For MS-Program
        [Display(Name = "Matric Percentage")]
        public float MsMetricPercentage { get; set; }
        [Display(Name = "Intermediate Percentage")]
        public float MsIntermediatePercentage { get; set; }
        [Display(Name = "BS Percentage")]
        public float MsBsPercentage { get; set; }
        [Display(Name = "NTS/Interview Percentage")]
        public float MsNTsORInterviewPercentage { get; set; }
        //For PHD-Program
        [Display(Name = "Matric Percentage")]
        public float PhdMetricPercentage { get; set; }
        [Display(Name = "Intermediate Percentage")]
        public float PhdIntermediatePercentage { get; set; }
        [Display(Name = "BS Percentage")]
        public float PhdBsPercentage { get; set; }
        [Display(Name = "MS/MPhil Percentage")]
        public float PhdMsPercentage { get; set; }
        [Display(Name = "NTS/Interview Percentage")]
        public float PhdNTsORInterviewPercentage { get; set; }
        public string departmentName { get; set; }
    }
}