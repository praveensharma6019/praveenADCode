namespace Sitecore.Feature.Accounts.Models
{
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Foundation.Dictionary.Repositories;

    public class ForgotPasswordInfo
    {
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ForgotPasswordInfo))]
        [Display(Name = nameof(AccountNoCaption), ResourceType = typeof(ForgotPasswordInfo))]
        public string AccountNo { get; set; }

        [Required(ErrorMessageResourceName = nameof(UserNameEmailAddress), ErrorMessageResourceType = typeof(ForgotPasswordInfo))]
        public string LoginName { get; set; }

        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(RegistrationInfo))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Captcha { get; set; }
        public static string AccountNoCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Forgot Password/Email", "Account Number");
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Forgot Password/Required", "Please enter a value for {0}");
        public static string UserNameEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/Forgot Password/UserNameEmailAddress", "Please enter a value for User Name / Email");
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/Forgot Password/Invalid Email Address", "Please enter a valid email address");
    }
}