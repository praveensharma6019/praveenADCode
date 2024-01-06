namespace Sitecore.Feature.Accounts.Models
{
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;

    public class ResetPasswordInfo
    {

        [Display(Name = nameof(PasswordCaption), ResourceType = typeof(ResetPasswordInfo))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ResetPasswordInfo))]
        [PasswordMinLength(ErrorMessageResourceName = nameof(MinimumPasswordLength), ErrorMessageResourceType = typeof(ResetPasswordInfo))]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{12,20}$", ErrorMessageResourceName = nameof(PasswordRegexFailed), ErrorMessageResourceType = typeof(ResetPasswordInfo))]

        public string Password { get; set; }

        [Display(Name = nameof(ConfirmPasswordCaption), ResourceType = typeof(ResetPasswordInfo))]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessageResourceName = nameof(ConfirmPasswordMismatch), ErrorMessageResourceType = typeof(ResetPasswordInfo))]
        public string ConfirmPassword { get; set; }
        public string Captcha { get; set; }
        public string UserID { get; set; }
        public string Token { get; set; }

        public bool IsValidRequest { get; set; }

        public static string PasswordValidation => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Password", "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character.");
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Reset Password/Required", "Please enter a value for {0}");
        public static string ConfirmPasswordCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Reset Password/Confirm Password", "Re-type Password");
        public static string PasswordCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Reset Password/Password", "Password");
        public static string ConfirmPasswordMismatch => DictionaryPhraseRepository.Current.Get("/Accounts/Reset Password/Confirm Password Mismatch", "Your password confirmation does not match. Please enter a new password.");
        public static string MinimumPasswordLength => DictionaryPhraseRepository.Current.Get("/Accounts/Reset Password/Minimum Password Length", "Please enter a password with at lease {1} characters");
        public static string PasswordRegexFailed => DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/PasswordRegexFailed", "Your password must be at least 12 char long and must contains 1 special character,  1 lower case, 1 upper case and 1 number");

    }
}