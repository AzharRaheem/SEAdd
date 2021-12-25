using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class Fee
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name ="Main Form Fee")]
        public double formFee { get; set; }
        [Required]
        [Display(Name = "Entry Test Fee")]
        public double entryTestFee { get; set; }
        [Required]
        public Bank Bank { get; set; }
        [ForeignKey("Bank")]
        [Display(Name = "Bank")]
        public int bankId { get; set; }
    }
}