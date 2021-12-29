using SEAdd.CustomValidations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SEAdd.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [StringLength(50)][RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$" , ErrorMessage ="Please enter valid name like \'John\'")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)][RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$" , ErrorMessage ="Please enter valid name like \'John\'")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression("^[0-9]{5}-[0-9]{7}-[0-9]$", ErrorMessage = "CNIC must follow the XXXXX-XXXXXXX-X format!")]
        [StringLength(15)]
        [UniqueCnic(ErrorMessage ="User already exist.")]
        public string Cnic { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string profileImgUrl { get; set; }
        [Required]
        [Display(Name ="Gender")]
        public bool Gender { get; set; }
        public string role { get; set; }
    }
    public class UpdateUserProfileViewModel //This Model is used to update the user detail form user area.
    {
        [Required]
        [StringLength(50)]
        [Display(Name ="First Name")][RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$" , ErrorMessage ="Please enter valid name like \'John\'")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")][RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$" , ErrorMessage ="Please enter valid name like \'John\'")]
        public string LastName { get; set; }
        [Display(Name ="Father Name")][RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$" , ErrorMessage = "Please enter valid name like \'John , John Smith'")]
        public string fatherName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name ="Mobile Number")]
        [RegularExpression("^((\\+92)?(0092)?(92)?(0)?)(3)([0-9]{9})$", ErrorMessage ="Enter Correct Mobile Number (0300-0000000")]
        public string PhoneNumber { get; set; }
        public string profileImgUrl { get; set; }
        public string role { get; set; }

    }
    public class NewUserViewModel //This Model is Used to Add new user in the Application .
    {
        [Required]
        [StringLength(50)]
        [Display(Name ="First Name")][RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$" , ErrorMessage ="Please enter valid name like \'John\'")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")][RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$" , ErrorMessage ="Please enter valid name like \'John\'")]
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Father Name")]
        [StringLength(50)][RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$" , ErrorMessage ="Please enter valid name like \'John , John Smith\'")]
        public string fatherName { get; set; }
        [Required]
        [RegularExpression("^[0-9]{5}-[0-9]{7}-[0-9]$", ErrorMessage = "CNIC must follow the XXXXX-XXXXXXX-X format!")]
        [StringLength(15)]
        [Display(Name = "CNIC")]
        [UniqueCnic(ErrorMessage = "User already exist.")]
        public string Cnic { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Mobile Number")]
        [RegularExpression("^((\\+92)?(0092)?(92)?(0)?)(3)([0-9]{9})$", ErrorMessage = "Enter Correct Mobile Number (0300-0000000")]
        public string PhoneNumber { get; set; }
        public string profileImgUrl { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Display(Name ="Gender")]
        public string gender { get; set; }
        [Required]
        [Display(Name ="Role")]
        public string role { get; set; }
    }
    public class UpdateAdminUserViewModel //This Model is used to Update the User Detail from Admin Area
    {
        [Required]
        [StringLength(50)][RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$" , ErrorMessage ="Please enter valid name like \'John\'")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)][RegularExpression("^[A-Za-z]+((\\s)?([A-Za-z])+)*$" , ErrorMessage ="Please enter valid name like \'John\'")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression("^[0-9]{5}-[0-9]{7}-[0-9]$", ErrorMessage = "CNIC must follow the XXXXX-XXXXXXX-X format!")]
        [StringLength(15)]
        public string Cnic { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
       
        public string profileImgUrl { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public bool gender { get; set; }
    }
    public class UserProfileVM //This Model is used to get all user Profile 
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string fatherName { get; set; }
        public string cnic { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
        public string role { get; set; }
        public string gender { get; set; }
        public string profileImgUrl { get; set; }
    }
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
