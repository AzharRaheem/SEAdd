using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class AdmissionDate
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(500)][RegularExpression("^[A-Za-z0-9]+((\\s)?([A-Za-z0-9])+)*$", ErrorMessage ="Title only contains letters and numbers.")]
        public string Title { get; set; }
        public DateTime NotificationDate { get; set; }
        [Required][Display(Name ="Start Date")]
        public DateTime StartDate { get; set; }
        [Required][Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        public string NotificationFileUrl { get; set; }
    }
}