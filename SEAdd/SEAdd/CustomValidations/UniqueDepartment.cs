using SEAdd.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.CustomValidations
{
    public class UniqueDepartment : ValidationAttribute
    {
        ApplicationDbContext db;
        public UniqueDepartment()
        {
            db = new ApplicationDbContext();
        }
        public override bool IsValid(object value)
        {
            var department = db.Departments.Where(d => d.name == value).FirstOrDefault();
            return department == null;
        }
    }
}