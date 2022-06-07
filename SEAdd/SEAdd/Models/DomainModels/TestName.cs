using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class TestName
    {
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage ="Test Name field is required.")]
        [Display(Name ="Test Name")]
        public string testName { get; set; }
        [Display(Name = "Description (Optional)")]
        public string description { get; set; }
        public string departmentName { get; set; }

        public virtual ICollection<TestQuestion> TestQuestions { get; set; }
    }
}