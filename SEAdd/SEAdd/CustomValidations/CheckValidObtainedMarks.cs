using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.CustomValidations
{
    public class CheckValidObtainedMarks : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return (Marks.totalMarks < Convert.ToDouble(value)) ? false : true;
        }
    }
}