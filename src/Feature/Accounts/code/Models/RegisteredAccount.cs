namespace Sitecore.Feature.Accounts.Models
{
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;

    public class RegisteredAccount : ProfileBasicInfo
    {
        [Display(Name = nameof(ExistingAcNoCaption), ResourceType = typeof(RegisteredAccount))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegisteredAccount))]
        public string ExistingAcNo { get; set; }

        [Display(Name = nameof(ExistingUserIdCaption), ResourceType = typeof(RegisteredAccount))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegisteredAccount))]
        public string ExistingUserId { get; set; }

        [Display(Name = nameof(PasswordCaption), ResourceType = typeof(RegisteredAccount))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegisteredAccount))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Required(ErrorMessageResourceName = nameof(CaptchaRequired), ErrorMessageResourceType = typeof(RegisteredAccount))]
        public string Captcha { get; set; }
        public static string CaptchaRequired => DictionaryPhraseRepository.Current.Get("/Accounts/Register/CaptchaRequired", "Please validate captcha to continue");
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
        public static string PasswordCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/ExistingAccountPassword", "Existing Account Password");
        public static string ExistingUserIdCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/ExistingUserId", "Existing Account User ID");
        public static string ExistingAcNoCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/ExistingAcNo", "Existing Account Number");
    }
}