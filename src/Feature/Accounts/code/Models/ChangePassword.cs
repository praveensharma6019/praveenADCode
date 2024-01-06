namespace Sitecore.Feature.Accounts.Models
{
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;

    public class ChangePassword : ProfileBasicInfo
    {
        [Display(Name = nameof(OldPasswordCaption), ResourceType = typeof(ChangePassword))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ChangePassword))]
        [PasswordMinLength(ErrorMessageResourceName = nameof(MinimumPasswordLength), ErrorMessageResourceType = typeof(ChangePassword))]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = nameof(PasswordCaption), ResourceType = typeof(ChangePassword))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ChangePassword))]
        [PasswordMinLength(ErrorMessageResourceName = nameof(MinimumPasswordLength), ErrorMessageResourceType = typeof(ChangePassword))]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{12,20}$", ErrorMessageResourceName = nameof(PasswordRegexFailed), ErrorMessageResourceType = typeof(ChangePassword))]
        public string Password { get; set; }

        [Display(Name = nameof(ConfirmPasswordCaption), ResourceType = typeof(ChangePassword))]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessageResourceName = nameof(ConfirmPasswordMismatch), ErrorMessageResourceType = typeof(ChangePassword))]
        public string ConfirmPassword { get; set; }
        public string Captcha { get; set; }
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/Required", "Please enter a value for {0}");
        public static string PasswordCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/PasswordCaption", "New Password");
        public static string OldPasswordCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/OldPasswordCaption", "Change Password");
        public static string ConfirmPasswordCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/ConfirmPasswordCaption", "Retype Password");
        public static string ConfirmPasswordMismatch => DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/Confirm Password Mismatch", "Your password confirmation does not match. Please enter a new password.");
        public static string MinimumPasswordLength => DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/Minimum Password Length", "Please enter a password with at lease {1} characters");
        public static string PasswordRegexFailed => DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/PasswordRegexFailed", "Your password must be at least 12 char long and must contains 1 special character,  1 lower case, 1 upper case and 1 number");
    }

    public class ChangePasswordAdaniGas : ChangePassword
    {
        public string CustomerID { get; set; }
    }
    public class ForgotPasswordAdaniGas
    {
        [Required]
        public string CustomerID { get; set; }
        public string Mobile_No { get; set; }
        public string OTP_Validity_Minutes { get; set; }
        public string Msg_Flag { get; set; }
        public string Message { get; set; }
        public string Captcha { get; set; }

    }
    public class ForgotPasswordValidateOTP
    {
        public string Valid_OTP { get; set; }

        [Display(Name = nameof(ChangePassword.PasswordCaption), ResourceType = typeof(ChangePassword))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ChangePassword))]
        [PasswordMinLength(ErrorMessageResourceName = nameof(ChangePassword.MinimumPasswordLength), ErrorMessageResourceType = typeof(ChangePassword))]
       // [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessageResourceName = nameof(ChangePassword.PasswordValidation), ErrorMessageResourceType = typeof(RegistrationInfo))]
        [DataType(DataType.Password)]
        public string New_Password { get; set; }

        [Display(Name = nameof(ChangePassword.ConfirmPasswordCaption), ResourceType = typeof(ChangePassword))]
        [DataType(DataType.Password)]
        [Compare(nameof(New_Password), ErrorMessageResourceName = nameof(ChangePassword.ConfirmPasswordMismatch), ErrorMessageResourceType = typeof(ChangePassword))]
        public string Repeat_Password { get; set; }

        public string Msg_Flag { get; set; }
        public string Message { get; set; }
    }
}