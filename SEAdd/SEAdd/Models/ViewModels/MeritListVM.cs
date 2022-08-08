using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class MeritListVM
    {
        [Required][Display(Name ="Program")]
        public string program { get; set; }
        [Required][Display(Name = "Year")]
        public string year { get; set; }
        public List<Program> Programs { get; set; }
    }
}