using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class UserEntryTestResult
    {
        public string rollNumber { get; set; }
        public string departmentName { get; set; }
        public int QuestionId { get; set; }
        public string userAns { get; set; }
        public int TotalMarks { get; set; }
        public int ObtainedMarks { get; set; }
    }
}