using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniCare.Website.Models
{
    [Serializable]
    public class AdaniCareLoginModel
    {
        public AdaniCareLoginModel()
        {
        }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(AdaniCareLoginModel))]
        [StringLength(15, ErrorMessage = "Input cannot be longer than 15 characters.")]
        public string InputByUser { get; set; }
        public string reResponse { get; set; }
        public bool IsInputMobileNumber { get; set; }
        public bool IsInputValidated { get; set; }
        public bool IsOTPSent { get; set; }
        public string OTPNumber { get; set; }
        public bool IsOTPValid { get; set; }

        public string MaskedMobileNumber { get; set; }

        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a valid Mobile Number");
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(AdaniCareLoginModel))]
        public string MobileNumber { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter reqired input value");
    }

    [Serializable]
    public class AdaniCareConsumerDetails
    {
        public string AccountNumber { get; set; }
        public string MobileNumber { get; set; }
        public string ConsumerName { get; set; }
        public string ConsumerEmail { get; set; }
    }

}