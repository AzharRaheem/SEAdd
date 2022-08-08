using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class TransportRoute
    {
        [Key]
        public int id { get; set; }
        [Required][Display(Name ="Route")]
        public string route { get; set; }
    }
}