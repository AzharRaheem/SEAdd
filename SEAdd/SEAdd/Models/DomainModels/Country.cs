using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class Country
    {
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(100)]
        [RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$", ErrorMessage = "Please enter valid country name.")]
        public string name { get; set; }

        public virtual ICollection<Academic> Academics { get; set; }
    }
}