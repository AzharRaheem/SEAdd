using CrystalDecisions.ReportAppServer.DataDefModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class DepartmentSetting
    {
        [Key]
        public int id { get; set; }
        [DefaultValue(false)]
        [Required][Display(Name ="Entry Test Required?")]
        public bool isEntryTestRequired { get; set; }
        public string departmentName { get; set; }
    }
}