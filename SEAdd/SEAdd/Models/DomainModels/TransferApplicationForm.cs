using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class TransferApplicationForm
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int programId { get; set; }
        [Required]
        public int oldDepartmentId { get; set; }
        [Required]
        public int newDepartmentId { get; set; }
        [Required]
        public int applicantId { get; set; }
    }
}