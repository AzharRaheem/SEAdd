using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class HostelCampusVM
    {
        public Hostel hostel { get; set; }
        public List<Campus> Campuses { get; set; }
    }
}