using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class TransportServiceVM
    {
        public TransportRoute TransportRoute { get; set; }
        public List<TransportRoute> Routes { get; set; }
        public List<string> YesNoList { get; set; }
        [Required(ErrorMessage ="Please select an option (Yes or No).")]
        public string userSelectionYesNo { get; set; }
        public string userRouteSelection { get; set; }
    }
}