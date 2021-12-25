using SEAdd.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.CustomValidations
{
    public class UniqueQota : ValidationAttribute
    {
        ApplicationDbContext db;
        public UniqueQota()
        {
            db = new ApplicationDbContext();
        }
        public override bool IsValid(object value)
        {
            var qota = db.Qotas.Where(q => q.name == value).FirstOrDefault();
            return qota == null;
        }
    }
}