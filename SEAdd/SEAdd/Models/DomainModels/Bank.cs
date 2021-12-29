using SEAdd.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class Bank
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name ="Bank Name")]
        [UniqueBank(ErrorMessage ="Bank name already exist.")]
        [RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$", ErrorMessage = "Please enter valid bank name.")]
        public string name { get; set; }
        [Required]
        [StringLength(14)]
        [Display(Name ="Account No.")]
        public string accountNo { get; set; }

        public ICollection<Fee> Fees { get; set; }
    }
}