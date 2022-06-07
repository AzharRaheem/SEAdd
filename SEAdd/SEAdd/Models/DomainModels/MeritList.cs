using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class MeritList
    {
        [Key]
        public int id { get; set; }
        public int formNo { get; set; }
        public string  rollNo { get; set; }
        public string programName { get; set; }
        public string departmentName { get; set; }
        public string fullName { get; set; }
        public string fatherName { get; set; }
        public string cnic { get; set; }
        public float scores { get; set; }
        public string nominationFrom { get; set; }
        public string QuotaName { get; set; }
    }
}