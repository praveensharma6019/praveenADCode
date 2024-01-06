namespace Sitecore.Feature.Accounts.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;

    [Serializable]
    public class NewConnectionRegistrationModel
    {
        

        public string Captcha { get; set; }

      

        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a valid Mobile Number");
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(NewConnectionRegistrationModel))]
        public string MobileNo { get; set; }

       
        public string OTPNumber { get; set; }

        public bool IsvalidatAccount { get; set; }

        public bool IsOTPSent { get; set; }
        public bool IsOTPValid { get; set; }
      
      
      
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");

    }
}