using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class EntryTestResultVM
    {
        public EntryTestResult entryTestResult { get; set; }
        public Applicant applicant { get; set; }
    }
}