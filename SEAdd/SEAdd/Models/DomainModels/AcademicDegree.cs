using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class AcademicDegree
    {
        [Key]
        public int id { get; set; }
        [Required][Display(Name ="Degree Name")]
        [StringLength(10)]
        public string name { get; set; }
    }
}