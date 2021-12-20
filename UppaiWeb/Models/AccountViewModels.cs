using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UppaiWeb.Helpers;

namespace UppaiWeb.Models
{
    #region Register
    public class UserRegister
    {
        public string Id { get; set; }
        //[Required(ErrorMessage = "Enter User Email ID")]
        [EmailAddress(ErrorMessage = "Enter Valid Email ID")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter Contact No.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Enter User Profile Name")]
        public string ProfileName { get; set; }
        [Required(ErrorMessage = "Select User Role")]
        public string RoleName { get; set; }
        public string UserName { get; set; }

    }
    public class UserRegisterModel
    {
        [Required(ErrorMessage = "Enter User Profile Name")]
        public string ProfileName { get; set; }
        [Required(ErrorMessage = "Enter User Name")]
        public string UserName { get; set; }
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Please enter correct email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter Phone Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string PhoneNo { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string RoleName { get; set; }

    }

    public class RegisterModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Enter User Email ID")]
        [EmailAddress(ErrorMessage = "Enter Valid Email ID")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter Contact No.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Enter User Profile Name")]
        public string ProfileName { get; set; }
        [Required(ErrorMessage = "Select User Role")]
        public string RoleName { get; set; }
        public string UserName { get; set; }

    }
    public class UserRegisterVM
    {
        public RegisterModel RegisterModel { get; set; }
        public IEnumerable<RoleDD> RoleDDList { get; set; }
    }
    public class RoleDD
    {
        public string RoleName { get; set; }
    }

    public class UserListVM
    {
        public IEnumerable<IdentityListView> UserList { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    public class UserListAPIVM
    {
        public IEnumerable<IdentityListView> UserList { get; set; }
        public int TotalRecords { get; set; }
    }
    public class IdentityListView
    {
        public string Id { get; set; }
        public string ProfileName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }

    }
    public class ActivateModel
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string UserID { get; set; }
    }
    public class SetPasswordManageModel
    {
        public UserProfileModel UserProfile { get; set; }
        public SetPasswordModel SetPassword { get; set; }
        public string Code { get; set; }
    }
    public class UserProfileModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserImage { get; set; }
        public string ProfileName { get; set; }
    }
    public class SetPasswordModel
    {
        public string UserID { get; set; }
        public string Code { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    #endregion

    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
    public class LoginModel
    {
        [Required(ErrorMessage = "Enter User Name.")]
        [Display(Name = "Email/Mobile No.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Enter Password")]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
    public class LoginErrorModel
    {
        public string error { get; set; }
        public string error_description { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class TokenModel
    {
        public string access_token { get; set; }
        public string userId { get; set; }
        public string profileName { get; set; }
        public string expires_in { get; set; }
        public string role { get; set; }
        public string userImage { get; set; }
        public string email { get; set; }
        public string userName { get; set; }
    }
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Enter Registered Email")]
        [EmailAddress(ErrorMessage = "Enter Valid Email Address")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Enter Registered Email")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter New Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        //[RegularExpression(@"^(?=.*[A-Z].*[A-Z])(?=.*[!@#$&*])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8}$")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //[RegularExpression(@"^(?=.*[A-Z].*[A-Z])(?=.*[!@#$&*])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8}$")]
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Enter Your Current Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Enter New Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class ChangePasswordModel
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
