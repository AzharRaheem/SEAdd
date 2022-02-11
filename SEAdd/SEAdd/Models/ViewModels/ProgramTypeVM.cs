using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class ProgramTypeVM
    {
        public Program program { get; set; }
        public List<string> ProgramType { get; set; }
    }
}