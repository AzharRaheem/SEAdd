using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class Hostel
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual Campus Campus { get; set; }
        [Display(Name = "Campus")]
        [Required]
        [ForeignKey("Campus")]
        public int CampusId { get; set; }
    }
}