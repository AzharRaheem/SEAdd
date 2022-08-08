using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class MeritList
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int formNo { get; set; }
        public string rollNo { get; set; }
        [Required]
        public string programName { get; set; }
        [Required]
        public string departmentName { get; set; }
        [Required]
        public string fullName { get; set; }
        [Required]
        public string fatherName { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string gender { get; set; }
        [Required]
        public string cnic { get; set; }
        [Required]
        public double metricObtainedMarks { get; set; }
        [Required]
        public double intermediateObtainedMarks { get; set; }
        [Required]
        public double metricTotalMarks { get; set; }
        [Required]
        public double intermediateTotalMarks { get; set; }
        [Required]
        public string annualPassingYear { get; set; }
        [Required]
        public string meritListYear { get; set; }
        [Required]
        public float scores { get; set; }
        public string nominationFrom { get; set; }
        [Required]
        public string QuotaName { get; set; }
        [Required]
        public string domicile { get; set; }
        [Required][DefaultValue(true)]
        public bool isVisible { get; set; }
    }
}