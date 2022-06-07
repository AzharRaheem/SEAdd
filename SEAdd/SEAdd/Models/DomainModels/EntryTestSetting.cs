using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class EntryTestSetting
    {
        [Key]
        public int id { get; set; }
        [Required][Display(Name ="Start Date & Time")]
        public DateTime TestStartDateTime  { get; set; }
        [Required][Display(Name = "End Date & Time")]
        public DateTime TestEndDateTime { get; set; }
        [Required][Display(Name = "Total Marks")]
        public int TotalMarks { get; set; }
        [Required][Display(Name = "Total Questions")]
        public int TotalQuestions { get; set; }
        [Required][Display(Name = "Select Test")]
        public string testName { get; set; }
        public string departmentName { get; set; }
    }
}