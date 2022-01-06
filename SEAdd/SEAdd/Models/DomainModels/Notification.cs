using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public DateTime NotificationDate { get; set; }
        [Required][StringLength(100)]
        public string NotificationTitle { get; set; }
        public string NotificationFileUrl { get; set; } 
    }
}