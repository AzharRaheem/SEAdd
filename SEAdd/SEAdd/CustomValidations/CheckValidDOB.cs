using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.CustomValidations
{
    public class CheckValidDOB : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime today = DateTime.Today.Date;
            DateTime dob = (DateTime)value;
            TimeSpan timeSpan = today.Subtract(dob.Date);
            var totalDays = timeSpan.TotalDays;
            var months = totalDays / 30;
            var years = months / 12;
            return (years > 15) ? true : false;
        }
    }
}