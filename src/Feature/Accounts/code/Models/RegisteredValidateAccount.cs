namespace Sitecore.Feature.Accounts.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;

    [Serializable]
    public class RegisteredValidateAccount 
    {
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegisteredValidateAccount))]
        public string AccountNo { get; set; }

        
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegisteredValidateAccount))]
        public string MeterNo { get; set; }

        public string MobileNo { get; set; }

        public string ExistingMobileNo { get; set; }

        public string OTPNumber { get; set; }

        public bool isvalidatAccount { get; set; }

        public bool isOTPSent { get; set; }

        public string Captcha { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
       
    }
}