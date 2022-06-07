using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class QuestionEntryTestVM
    {
        public TestQuestion Question { get; set; }
        public EntryTestSetting entryTestSetting { get; set; }
    }
}