using SEAdd.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.CustomValidations
{
    public class UniqueProgram : ValidationAttribute
    {
        ApplicationDbContext db;
        public UniqueProgram()
        {
            db = new ApplicationDbContext();
        }
        public override bool IsValid(object value)
        {
            var program = db.Programs.Where(q => q.ProgramName == value).FirstOrDefault();
            return program == null;
        }
    }
}