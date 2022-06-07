using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class CategoryPercentage
    {
        [Key]
        public int id { get; set; }
        [Required][Display(Name ="Category")]
        public string categoryName { get; set; }
        public string department { get; set; }
        [Required]
        [Display(Name ="Percentage (%)")]
        public int QuestionPercentage { get; set; }

        public virtual ICollection<TestQuestion> TestQuestions { get; set; }
    }
}