using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class RollNumber
    {
        [Key]
        public int id { get; set; }
        public int applicantId { get; set; }
        public string userId { get; set; }
        public string applicantRollNumber { get; set; }
        public int programId { get; set; }
        public string departmentName { get; set; }
    }
}