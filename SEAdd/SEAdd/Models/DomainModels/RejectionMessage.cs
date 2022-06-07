using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class RejectionMessage
    {
        [Key]
        public int id { get; set; }
        public int applicantId { get; set; }
        public string title { get; set; }
        [Required(ErrorMessage ="Please enter rejection message.")]
        public string message { get; set; }
        public int programId { get; set; }
        public int departmentId { get; set; }
    }
}