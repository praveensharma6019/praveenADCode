namespace Sitecore.Feature.Accounts.Models
{
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;

    public class NonRegisteredAccount 
    {
        [Display(Name = nameof(AccountNoCaption), ResourceType = typeof(NonRegisteredAccount))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(NonRegisteredAccount))]
        public string Accountnumber { get; set; }

        [Display(Name = nameof(MeterNoCaption), ResourceType = typeof(NonRegisteredAccount))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(NonRegisteredAccount))]
        public string MeterNumber { get; set; }

        public string MasterAccountNumber { get; set; }

        //[Required(ErrorMessageResourceName = nameof(CaptchaRequired), ErrorMessageResourceType = typeof(NonRegisteredAccount))]
        public string Captcha { get; set; }

        public static string CaptchaRequired => DictionaryPhraseRepository.Current.Get("/Accounts/Register/CaptchaRequired", "Please validate captcha to continue");
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
        public static string AccountNoCaption => DictionaryPhraseRepository.Current.Get("/Accounts/NonRegister/Account Number", "Account Number");
        public static string MeterNoCaption => DictionaryPhraseRepository.Current.Get("/Accounts/NonRegister/Meter Number", "Meter Number");
    }
}