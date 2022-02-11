using Microsoft.AspNet.Identity.EntityFramework;
using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEAdd.Models.ViewModels
{
    public class UserRoleVM
    {
        public NewUserViewModel newUser { get; set; }
        public List<string> Gender { get; set; }
        public List<IdentityRole> roles { get; set; }
        public List<Department> departments { get; set; }
    }
}