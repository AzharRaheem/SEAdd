using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEAdd.Models.DomainModels
{
    public class UserQuestionResult
    {
        [Key]
        public int id { get; set; }
        public virtual Applicant applicant { get; set; }
        [ForeignKey("applicant")]
        public int applicantId { get; set; }
        public string rollNumber { get; set; }
        public string departmentName { get; set; }
        public virtual TestQuestion Question { get; set; }
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        [AllowHtml]
        public string userAns { get; set; }
        [AllowHtml]
        public string correctAns { get; set; }
        public int TotalMarks { get; set; }
        public int ObtainedMarks { get; set; }
    }
}