using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class CategoryPercentageVM
    {
        public CategoryPercentage categoryPercentage { get; set; }
        public List<TestCategory> Categories { get; set; }
    }
}