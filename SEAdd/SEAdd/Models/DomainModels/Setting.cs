using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class Setting
    {
        [Key]
        public int id { get; set; }
        [Required][Display(Name ="One Applicant can apply for departments?")]
        public int applicantDeptApply { get; set; }
    }
}