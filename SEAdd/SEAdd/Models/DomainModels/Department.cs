﻿using SEAdd.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class Department
    {
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(100)]
        [UniqueDepartment(ErrorMessage = "Department already exist.")]
        [Display(Name = "Department Name")]
        public string name { get; set; }
    }
}