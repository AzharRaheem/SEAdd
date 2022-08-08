using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class EntryTestResult
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string userId { get; set; }
        [Required]
        public string rollNumber { get; set; }
        [Required]
        public double obtainedScorePercentage { get; set; }
        [Required]
        public double totalScorePercentage { get; set; }
        [Required]
        public string entryTestName { get; set; }
        [Required]
        public string departmentName { get; set; }
        [Required]
        public string year { get; set; }
        [Required]
        public DateTime EntryTestStartTime { get; set; }
        [Required]
        public DateTime EntryTestEndTime { get; set; }
        [Required]
        public int programId { get; set; }
    }
}