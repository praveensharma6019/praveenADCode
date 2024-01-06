namespace Sitecore.Feature.Accounts.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;

    [Serializable]
    public class PANUpdateModel
    {
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(PANUpdateModel))]
        [RegularExpression(@"^[0-9]{8,10}$", ErrorMessageResourceName = nameof(InvalidAccountNumber), ErrorMessageResourceType = typeof(PANUpdateModel))]
        public string AccountNumber { get; set; }

        public string Captcha { get; set; }
        public string ConsumerName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }

        public string PANNumberDisplay { get; set; }
        public string GSTNumberDisplay { get; set; }

        [RegularExpression(@"[A-Z]{5}\d{4}[A-Z]{1}", ErrorMessageResourceName = nameof(InvalidPANNumber), ErrorMessageResourceType = typeof(PANUpdateModel))]
        public string PANNumber { get; set; }

         [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$", ErrorMessageResourceName = nameof(InvalidGSTNumber), ErrorMessageResourceType = typeof(PANUpdateModel))]
        public string GSTNumber { get; set; }

        public bool IsAccountNumberValidated { get; set; }
        public bool IsPANToBeUpdated { get; set; }
        public bool IsGSTToBeUpdated { get; set; }

        public bool IsOTPSent { get; set; }

        [RegularExpression(@"^[0-9]{4,8}$", ErrorMessageResourceName = nameof(InvalidOTPNumber), ErrorMessageResourceType = typeof(PANUpdateModel))]
        public string OTPNumber { get; set; }
        public bool IsValidatedOTP { get; set; }

        //public HttpPostedFileBase GSTFile { get; set; }
        //public HttpPostedFileBase PANFile { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
        public static string InvalidAccountNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Account", "Please enter a valid Account Number");
        public static string InvalidOTPNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid OTP", "Please enter a valid OTP Number");

        public static string InvalidPANNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid PANNumber", "Please enter a valid PAN Number");
        public static string InvalidGSTNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid GSTNumber", "Please enter a valid GST Number");
    }
}