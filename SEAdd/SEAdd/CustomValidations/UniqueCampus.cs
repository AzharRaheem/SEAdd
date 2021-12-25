using SEAdd.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.CustomValidations
{
    public class UniqueCampus : ValidationAttribute
    {
        ApplicationDbContext db;
        public UniqueCampus()
        {
            db = new ApplicationDbContext();
        }
        public override bool IsValid(object value)
        {
            var campus = db.Campuses.Where(b => b.name == value).FirstOrDefault();
            return campus == null;
        }
    }
}