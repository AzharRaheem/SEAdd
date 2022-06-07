using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class EntryTestSettingVM
    {
        public EntryTestSetting ETSetting { get; set; }
        public List<TestName> Tests { get; set; }
    }
}