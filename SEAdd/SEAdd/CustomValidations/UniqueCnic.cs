using SEAdd.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.CustomValidations
{
    public class UniqueCnic : ValidationAttribute
    {
        ApplicationDbContext db;
        public UniqueCnic()
        {
            db = new ApplicationDbContext();
        }
        public override bool IsValid(object value)
        {
            var user = db.Users.Where(b => b.Cnic == value).FirstOrDefault();
            return user == null;
        }
    }
}