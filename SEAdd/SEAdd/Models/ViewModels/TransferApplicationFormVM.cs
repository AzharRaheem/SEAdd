using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class TransferApplicationFormVM
    {
        [Required]
        public int oldProgramId { get; set; }
        [Required]
        public int oldDepartmentId { get; set; }
        [Required]
        public int departmentId { get; set; }
        [Required]
        public int applicantId { get; set; }
        public List<Department> Departments { get; set; }
    }
}