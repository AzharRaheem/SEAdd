using SEAdd.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.CustomValidations
{
    public class UniqueBoarrd : ValidationAttribute
    {
        ApplicationDbContext db;
        public UniqueBoarrd()
        {
            db = new ApplicationDbContext();
        }
        public override bool IsValid(object value)
        {
            var board = db.Boards.Where(b => b.name == value).FirstOrDefault();
            return board == null;
        }
    }
}