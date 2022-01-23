using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SEAdd.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using SEAdd.CustomValidations;
using SEAdd.Models.ViewModels;
using System.Collections.Generic;
using SEAdd.Models.DomainModels;

namespace SEAdd.Controllers
{
    [Authorize]
    [HandleError]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        
        private ApplicationDbContext db;

        public AccountController()
        {
            db = new ApplicationDbContext();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //This code is used to check either user confirmed his/her email address that's sent on his/her email address.
            #region CheckEitherUserConfirmedEmail
            var userId = UserManager.FindByEmail(model.Email).Id;
            if (!UserManager.IsEmailConfirmed(userId))
            {
                ModelState.AddModelError("", "Email not confirmed yet.");
                return View(model);
            }
            #endregion
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    ApplicationUser user = await UserManager.FindAsync(model.Email, model.Password);
                    var roles = await UserManager.GetRolesAsync(user.Id);
                    var applicant = db.Applicants.Where(u => u.userId == user.Id).FirstOrDefault();
                    if(applicant != null)
                    {
                        Session["UserAlreadyExist"] = true;
                    }
                    else
                    {
                        Session["UserAlreadyExist"] = false;
                    }
                    Session["UserId"] = user.Id;//Store logged User Id...
                    Session["UserProfileImage"] = user.profileImgUrl; //Store logged User Image...
                    if (roles.Contains("User"))
                    {
                        Session["UserRole"] = "User"; //Store User Role...
                        Session["User"] = user;
                        AdmissionDate admissionDate = db.AdmissionDate.OrderByDescending(d => d.Id).FirstOrDefault();
                        if(admissionDate != null)
                        {
                            if (admissionDate.EndDate.Date >= DateTime.Today.Date)
                            {
                                Session["AdmissionDateExpire"] = false;
                            }
                            else
                            {
                                Session["AdmissionDateExpire"] = true;
                            }
                        }
                        else
                        {
                            Session["AdmissionDateExpire"] = false;
                        }
                        return RedirectToAction("UserDashboard", "Dashboard");
                    }
                    else
                    {
                        Session["UserRole"] = "Admin"; //Store User Role...
                        return RedirectToAction("AdminDashboard", "Dashboard");
                    }
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }
        //Get Logged User Profile -Code...
        [AllowAnonymous]
        public ActionResult UpdateUserProfile(string id)
        {
            ApplicationUser user = UserManager.FindById(id);
            UpdateUserProfileViewModel model = new UpdateUserProfileViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                fatherName = user.fatherName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                profileImgUrl = user.profileImgUrl,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateUserProfile(UpdateUserProfileViewModel model , HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                ApplicationUser user = await UserManager.FindByIdAsync((string)Session["UserId"]);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.fatherName = model.fatherName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                if(file != null && file.ContentLength > 0)
                {
                    string ImageUrl = GetImageUrl(file);
                    user.profileImgUrl = ImageUrl;
                }
                var result = await UserManager.UpdateAsync(user);
                var roles = await UserManager.GetRolesAsync(user.Id);
                if (result.Succeeded)
                {
                    Session["UserProfileImage"] = user.profileImgUrl;//Store User Updated Image...
                    if (roles.Contains("User"))
                    {
                        return RedirectToAction("UserDashboard", "Dashboard");
                    }
                    else
                    {
                        return RedirectToAction("AdminDashboard", "Dashboard");
                    }
                }
                else
                {
                    return View(model);
                }
            }
        }
        [NonAction]
        private string GetImageUrl(HttpPostedFileBase file)
        {
            string filePath = null;
            string folderPath = Server.MapPath("~/Images/");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            if(file != null && file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                var fileExtension = fileName.Split('.')[1];
                Guid UniqueName = Guid.NewGuid();
                var imageName = UniqueName + "." + fileExtension;
                filePath = "~/Images/" + imageName;
                file.SaveAs(Server.MapPath(filePath));
            }
            return filePath;
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName , 
                    Cnic = model.Cnic,
                    profileImgUrl = model.profileImgUrl ,
                    UserName = model.Email,
                    Email = model.Email , 
                    gender = model.Gender
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
                    var roleManager = new RoleManager<IdentityRole>(roleStore);
                    await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = model.role
                    });
                    await UserManager.AddToRoleAsync(user.Id, model.role);
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("EmailConfirmationToLogin", "Account");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        //Get All Users
        public ActionResult AllUsers()
        {
            var applicationUsers = db.Users.ToList();
            List<UserProfileVM> users = new List<UserProfileVM>();
            foreach (var user in applicationUsers)
            {
                UserProfileVM userProfile = new UserProfileVM();
                userProfile.id = user.Id;
                userProfile.firstName = user.FirstName;
                userProfile.lastName = user.LastName;
                userProfile.fatherName = user.fatherName;
                userProfile.cnic = user.Cnic;
                userProfile.email = user.Email;
                userProfile.address = user.address;
                userProfile.phoneNumber = user.PhoneNumber;
                userProfile.profileImgUrl = user.profileImgUrl;
                userProfile.gender = user.gender.ToString();
                var roles = UserManager.GetRoles(user.Id);
                if (roles.Contains("User"))
                {
                    userProfile.role = "User";
                }
                else
                {
                    userProfile.role = "Admin";
                }
                users.Add(userProfile);
            }
            return View(users);
        }
        //Get // Account / New User
        [HttpGet]
        public ActionResult AddNewUser()
        {
            UserRoleVM model = new UserRoleVM()
            {
                newUser = new NewUserViewModel() , 
                Gender = GetLists.GetGenderList() , 
                roles = db.Roles.ToList()
            } ;
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> AddNewUser(UserRoleVM model , HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                bool Gender = true; // For Male 
                if(model.newUser.gender != "Male")
                {
                    Gender = false; //For Female
                }
                var user = new ApplicationUser
                {
                    FirstName = model.newUser.FirstName,
                    LastName = model.newUser.LastName,
                    Cnic = model.newUser.Cnic,
                    profileImgUrl = GetImageUrl(file),
                    UserName = model.newUser.Email,
                    Email = model.newUser.Email , 
                    PhoneNumber = model.newUser.PhoneNumber ,
                    gender = Gender ,
                    address = model.newUser.Address , 
                    fatherName = model.newUser.fatherName
                };
                var result = await UserManager.CreateAsync(user, model.newUser.Password);
                if (result.Succeeded)
                {
                    var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
                    var roleManager = new RoleManager<IdentityRole>(roleStore);
                    await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = model.newUser.role
                    });
                    await UserManager.AddToRoleAsync(user.Id, model.newUser.role);
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("AllUsers", "Account");
                }
                AddErrors(result);
            }
            UserRoleVM userModel = new UserRoleVM()
            {
                newUser = model.newUser ,
                Gender = GetLists.GetGenderList(),
                roles = db.Roles.ToList()
            };

            // If we got this far, something failed, redisplay form
            return View(userModel);
        }
        [HttpGet]
        public ActionResult EditUser(string id)
        {
            var user = UserManager.FindById(id);
            return View(user);
        }
        //Delete a User
        public async Task<ActionResult> DeleteAUser(string id)
        {
            if(id != null)
            {
                var user = await UserManager.FindByIdAsync(id);
                var result = await UserManager.DeleteAsync(user);
                if(result.Succeeded)
                {
                    return RedirectToAction("AllUsers", "Account");
                }
            }
            return View();
        }
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
        //Email Confirmation to Login
        [AllowAnonymous]
        public ActionResult EmailConfirmationToLogin()
        {
            return View();
        }
        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
                if(db != null)
                {
                    db.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}