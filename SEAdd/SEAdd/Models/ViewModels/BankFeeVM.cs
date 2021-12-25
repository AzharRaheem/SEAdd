using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class BankFeeVM
    {
        public Fee FeeDetails { get; set; }
        public List<Bank> banks { get; set; }
    }
}