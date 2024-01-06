using Sitecore.Foundation.Dictionary.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Sitecore.Feature.Accounts.Models.ElectricityRevamp
{
    public class ReportElectricityTheftRevamp
    {
        public string Area { get; set; }
        public string Name { get; set; }

        [Display(Name = nameof(MobileNumberCaption), ResourceType = typeof(ReportElectricityTheftRevamp))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ReportElectricityTheftRevamp))]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(ReportElectricityTheftRevamp))]
        public string MobileNumber { get; set; }

        [Display(Name = nameof(EmailCaption), ResourceType = typeof(ReportElectricityTheftRevamp))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ReportElectricityTheftRevamp))]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(ReportElectricityTheftRevamp))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string ReportTheft { get; set; }
        public string Captcha { get; set; }
        public bool isRegistered { get; set; }
        public static string EmailCaption => DictionaryPhraseRepository.Current.Get("/ReportElectricityTheft/Email", "E-mail id");
        public static string Required => DictionaryPhraseRepository.Current.Get("/ReportElectricityTheft/Required", "Please enter a value for {0}");
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/ReportElectricityTheft/Invalid Email Address", "Please enter a valid email address");
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a Valid Mobile Number");
        public static string MobileNumberCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/Mobile No", "Mobile No.");
    }
}