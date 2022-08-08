using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEAdd.Models.DomainModels
{
    public class TestQuestion
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name ="Question")]
        [AllowHtml]
        public string QuestionText { get; set; }
        [Required]
        [AllowHtml]
        [Display(Name = "Option A")]
        public string OptionA { get; set; }
        [Required]
        [AllowHtml]
        [Display(Name = "Option B")]
        public string OptionB { get; set; }
        [Required]
        [AllowHtml]
        [Display(Name = "Option C")]
        public string OptionC { get; set; }
        [Required]
        [AllowHtml]
        [Display(Name = "Option D")]
        public string OptionD { get; set; }
        [Required]
        [AllowHtml]
        [Display(Name = "Correct Answer")]
        public string CorrectAns { get; set; }
        [Display(Name = "Hint!")]
        public string Hint { get; set; }
        [Required]
        [Display(Name = "Points/Marks")]
        public int marks { get; set; }
        public string departmentName { get; set; }

        public virtual CategoryPercentage Category { get; set; }
        [Required]
        [Display(Name = "Category")]
        [ForeignKey("Category")]
        public int categoryId { get; set; }
        public virtual TestName TestName { get; set; }
        [Required]
        [Display(Name = "Test Name")]
        [ForeignKey("TestName")]
        public int TestId { get; set; }
        public virtual TestType TestType { get; set; }
        [Required]
        [Display(Name = "Question Type")]
        [ForeignKey("TestType")]
        public int testTypeId { get; set; }



        public virtual ICollection<UserQuestionResult> UsertQuestionsResults { get; set; }
    }
}