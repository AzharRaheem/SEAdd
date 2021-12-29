using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.CustomValidations
{
    public class GetTotalMarks : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            Marks.totalMarks = Convert.ToDouble(value);
            return true;
        }
    }
}