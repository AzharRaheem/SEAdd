using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class RejectionReason
    {
        [Key]
        public int id { get; set; }
        public string RejectionMessage { get; set; }
        public virtual Applicant Applicant { get; set; }
        [ForeignKey("Applicant")]
        public int applicantId { get; set; } 
    }
}