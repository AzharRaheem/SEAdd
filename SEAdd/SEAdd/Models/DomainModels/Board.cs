using SEAdd.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class Board
    {
        [Key]
        public int id { get; set; }
        [Display(Name ="Board Name")]
        [Required][RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$" , ErrorMessage ="Please enter valid board name.")]
        public string name { get; set; }
    }
}