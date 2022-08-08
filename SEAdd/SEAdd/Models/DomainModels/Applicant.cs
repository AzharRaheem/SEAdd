using SEAdd.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SEAdd.Models.DomainModels
{
    public class Applicant
    {
        [Key]
        public int id { get; set; }
        public string userId { get; set; }
        [Display(Name = "Full Name")]
        [Required]
        [StringLength(50)]
        [RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$", ErrorMessage = "Please enter valid name like \'John , John Smith\'")]
        public string FullName { get; set; }
        [Display(Name = "Father's Name")]
        [Required]
        [StringLength(70)]
        [RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$", ErrorMessage = "Please enter valid name like \'John , John Smith\'")]
        public string FatherName { get; set; }
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Birth Date")]
        [Required]
        [CheckValidDOB(ErrorMessage = "You must be atleast 15 years old.")]
        public DateTime BirthDate { get; set; }
        [Display(Name = "Gender")]
        [Required]
        public string Gender { get; set; }
        [Display(Name = "CNIC")]
        [Required]
        [RegularExpression("^[0-9]{5}-[0-9]{7}-[0-9]$", ErrorMessage = "CNIC must follow the XXXXX-XXXXXXX-X format!")]
        public string CNIC { get; set; }
        [Display(Name = "Personal Mobile Number")]
        [Required]
        [RegularExpression("^((\\+92)?(0092)?(92)?(0)?)(3)([0-9]{9})$", ErrorMessage = "Enter Correct Mobile Number (03000000000)")]
        public string PersonalContact { get; set; }
        [Display(Name = "Guardian Mobile Number")]
        [Required]
        [RegularExpression("^((\\+92)?(0092)?(92)?(0)?)(3)([0-9]{9})$", ErrorMessage = "Enter Correct Mobile Number (03000000000)")]
        public string GuardianContact { get; set; }
        [Display(Name = "Nationality")]
        [Required]
        public string Nationality { get; set; }
        [Display(Name = "Religion")]
        [Required]
        public string Religion { get; set; }
        [Display(Name = "Domicile")]
        [Required]
        public string Domicile { get; set; }
        public virtual Qota Qota { get; set; }
        [Display(Name = "Are you applying on merit or quota/reserved seats?")]
        [Required]
        [ForeignKey("Qota")]
        public int QotaId { get; set; }
        [Display(Name = "Permanent Address")]
        [Required]
        [RegularExpression("^[#.0-9a-zA-Z\\s,-]+$", ErrorMessage = "Please enter a valid address.")]
        public string PermanentAddress { get; set; }
        [Display(Name = "Postal Address")]
        [Required]
        [RegularExpression("^[#.0-9a-zA-Z\\s,-]+$", ErrorMessage = "Please enter a valid address.")]
        public string PostalAddress { get; set; }
        public virtual Provience Provience { get; set; }
        [Display(Name = "Provience")]
        [Required]
        [ForeignKey("Provience")]
        public int ProvienceId { get; set; }
        public string profileImgUrl { get; set; }
        [Display(Name = "District Name")]
        [Required]
        public string District { get; set; }
        [Required]
        public DateTime ApplyDate { get; set; }
        [DefaultValue(false)]
        public bool isHostelRequired { get; set; }
        public string HostelName { get; set; }
        [DefaultValue(false)]
        public bool isTransportRequired { get; set; }
        public string TransportRouteName { get; set; }
        [DefaultValue(false)]
        public bool isRegistrationFinished { get; set; }
        [DefaultValue(false)]
        public bool isApproved { get; set; }
        [DefaultValue(false)]
        public bool isRejected { get; set; }
        public string departmentName { get; set; }
        public string programName { get; set; }

        public virtual ICollection<ProgramSelection> ProgramsSelection { get; set; }
        public virtual ICollection<Academic> Academics { get; set; }
        public virtual ICollection<UserQuestionResult> UsertQuestionsResults { get; set; }


    }
}