using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class QuestionCategoryVM
    {
        public TestQuestion Question { get; set; }
        public List<CategoryPercentage> Categories { get; set; }
        public List<TestType> TestTypes { get; set; }
        public List<TestName> TestName { get; set; }
    }
}