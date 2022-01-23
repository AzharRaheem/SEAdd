using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class UserDashboardVM
    {
        public string  applicationStatus { get; set; }
        public DateTime admissionLastDate { get; set; }
        public List<Notification> Notifications { get; set; }
    }
}