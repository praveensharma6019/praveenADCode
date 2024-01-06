namespace Sitecore.Feature.Accounts.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;

    [Serializable]
    public class ENachRegistrationModel
    {
        public static string InvalidAccountNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Account", "Entered Account no is invalid. Please enter a valid 9 digit Account No.");

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ENachRegistrationModel))]
        [RegularExpression(@"^[0-9]{8,12}$", ErrorMessageResourceName = nameof(InvalidAccountNumber), ErrorMessageResourceType = typeof(ENachRegistrationModel))]
        public string AccountNo { get; set; }

        public string Name { get; set; }

        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a valid Mobile Number");
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(ENachRegistrationModel))]
        public string MobileNo { get; set; }

        public string EmailId { get; set; }

        public string OTPNumber { get; set; }
        //[Required(ErrorMessage = "Please validate captcha to continue")]
        public string Captcha { get; set; }

        public bool IsvalidatAccount { get; set; }

        public bool IsOTPSent { get; set; }
        public bool IsOTPValid { get; set; }

        public bool isPPISet { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
    }

    public class ENachRedirectionModel
    {
        public string AppID { get; set; }
        public string EntityMerchantKey { get; set; }
        public string ToDebit { get; set; }
        public string Amount { get; set; }
        public string Frequency { get; set; }
        public string DebitType { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailID { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string NameInBankRecords1 { get; set; }
        public string NameInBankRecords2 { get; set; }
        public string NameInBankRecords3 { get; set; }
    }
}