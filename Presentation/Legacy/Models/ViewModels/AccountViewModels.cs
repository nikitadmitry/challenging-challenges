using System.ComponentModel.DataAnnotations;
using Shared.Framework.Resources;

namespace Presentation.Legacy.Models.ViewModels
{
    public class ForgotViewModel
    {
        [Required]
        [EmailAddress(ErrorMessageResourceType = typeof(Localization), ErrorMessageResourceName = "InvalidEmailAddress")]
        [Display(ResourceType = typeof (Localization), Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [RegularExpression("^([A-Z][a-z]+ [A-Z][a-z]+)$", ErrorMessageResourceType = typeof(Localization),
            ErrorMessageResourceName = "NameValidationMessage")]
        [Display(ResourceType = typeof(Localization), Name = "FullName")]
        [StringLength(50, MinimumLength = 5)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Localization), Name = "Password")]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Display(ResourceType = typeof (Localization), Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [RegularExpression("^([A-Z][a-z]+ [A-Z][a-z]+)$", ErrorMessageResourceType = typeof (Localization),
            ErrorMessageResourceName = "NameValidationMessage")]
        [Display(ResourceType = typeof (Localization), Name = "FullName")]
        [StringLength(50, MinimumLength = 5)]
        public string UserName { get; set; }

        [Required]
        //[EmailAddress(ErrorMessageResourceType = typeof(Localization), ErrorMessageResourceName = "InvalidEmailAddress")]
        [Display(ResourceType = typeof (Localization), Name = "Email")]
        [StringLength(180, MinimumLength = 6)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof (Localization),
            ErrorMessageResourceName = "LengthMessage", MinimumLength = 6)]
        [RegularExpression("^((?=.*\\d)(?=.*[A-Za-z]).{6,})$", ErrorMessageResourceType = typeof(Localization), ErrorMessageResourceName = "PasswordValidationMessage")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Localization), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Localization), Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessageResourceType = typeof (Localization),
            ErrorMessageResourceName = "PasswordsDoNotMatch")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress(ErrorMessageResourceType = typeof(Localization), ErrorMessageResourceName = "InvalidEmailAddress")]
        [Display(ResourceType = typeof (Localization), Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password, ErrorMessageResourceType = typeof(Localization), ErrorMessageResourceName = "PasswordValidationMessage")]
        [StringLength(100, MinimumLength = 6)]
        [Display(ResourceType = typeof (Localization), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Localization), Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessageResourceType = typeof (Localization),
            ErrorMessageResourceName = "PasswordsDoNotMatch")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(ResourceType = typeof (Localization), Name = "Email")]
        public string Email { get; set; }
    }
}