namespace Sitecore.Feature.Accounts.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;

    [Serializable]
    public class ChangeOfNameRegistrationModel
    {
        public static string InvalidAccountNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Account", "Entered Account no is invalid. Please enter a valid 9 digit Account No.");

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ChangeOfNameRegistrationModel))]
        [RegularExpression(@"^[0-9]{8,12}$", ErrorMessageResourceName = nameof(InvalidAccountNumber), ErrorMessageResourceType = typeof(ChangeOfNameRegistrationModel))]
        public string AccountNo { get; set; }

        public string Captcha { get; set; }

        public string RegistrationNo { get; set; }
        public string Name { get; set; }

        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a valid Mobile Number");
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(ChangeOfNameRegistrationModel))]
        public string MobileNo { get; set; }

        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(ChangeOfNameRegistrationModel))]
        public string LECMobileNo { get; set; }

        public string LECRegistrationNumber { get; set; }
        public bool IsLEC { get; set; }

        public string EmailId { get; set; }

        public string OTPNumber { get; set; }

        public bool IsvalidatAccount { get; set; }

        public bool IsOTPSent { get; set; }
        public bool IsOTPValid { get; set; }
        public bool IsOTPCallExceeded { get; set; }

        public string AccountNoForCheckApplication { get; set; }
        public string RegistrationNoForCheckApplication { get; set; }
        public string OTPNumberForCheckApplication { get; set; }
        public bool IsApplicationExistsForAccount { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");

    }

    public class CheckApplicationModel
    {
        public string AccountNo { get; set; }
        public string RegistrationNumber { get; set; }
        public bool Issuccess { get; set; }
        public bool IsOTPSent { get; set; }
        public bool IsVerified { get; set; }
        public string Message { get; set; }
        public string Captcha { get; set; }
    }
}