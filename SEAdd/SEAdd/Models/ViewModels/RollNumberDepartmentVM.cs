using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class RollNumberDepartmentVM
    {
        [Display(Name = "Department")]
        public string department { get; set; }
        [Required][Display(Name ="Roll Number")]
        public string rollNumber { get; set; }

        public List<Department> departments { get; set; }
    }
}