using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class HostelServiceVM
    {
        public Hostel hostel { get; set; }
        public List<Hostel> Hostels { get; set; }
        public List<string> YesNoList { get; set; }
        [Required(ErrorMessage ="Please select an option (Yes or No)")]
        public string userSelectionYesNo { get; set; }
        public string userHostelSelection { get; set; }
        public List<string> HostelCampusNames { get; set; }
    }
}