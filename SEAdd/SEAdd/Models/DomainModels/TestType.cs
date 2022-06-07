using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class TestType
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name = "Type Name")]
        [StringLength(100)]
        public string name { get; set; }

        public virtual ICollection<TestQuestion> TestQuestions { get; set; }
    }
}