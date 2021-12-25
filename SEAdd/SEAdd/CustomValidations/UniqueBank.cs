using SEAdd.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.CustomValidations
{
    public class UniqueBank : ValidationAttribute
    {
        ApplicationDbContext db;
        public UniqueBank()
        {
            db = new ApplicationDbContext();
        }
        public override bool IsValid(object value)
        {
            var bank = db.Banks.Where(b => b.name == value).FirstOrDefault();
            return bank == null;
        }
    }
}