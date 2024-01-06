using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Feature.Accounts.Models.AdaniGasCNGRegistration
{
    public class AdaniGasCNG_DealerRegistration
    {


        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(AdaniGasCNG_DealerRegistration))]
        public string VendorCode { get; set; }
        [Display(Name = nameof(MobileCaption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [RegularExpression(@"^(?:(?:\+|0{0,2})91(\s*[\ -]\s*)?|[0]?)?[789]\d{9}|(\d[ -]?){10}\d$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string Mobile { get; set; }
        [Display(Name = nameof(OTPCaption), ResourceType = typeof(AdaniGasCNG_DealerRegistration))]
        [RegularExpression(@"^\d{4}$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(AdaniGasCNG_DealerRegistration))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(AdaniGasCNG_DealerRegistration))]
        public string OTP { get; set; }
        public bool IsOTPSent { get; set; }
        public bool IsOTPValid { get; set; }
        public bool IsSavedintoDB { get; set; }
        public DateTime SubmittedOn { get; set; }
        public string FormURL { get; set; }
        public string ResponseURL { get; set; }
        public string Captcha { get; set; }
        public string PageInfo { get; set; }
        public string FormName { get; set; }
        public static string MobileCaption => DictionaryPhraseRepository.Current.Get("/Accounts/CNG Registration/ContactNumber", "Mobile Number");
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/CNG Registration/Invalid Contact", "Please enter a valid Mobile Number");
        public static string OTPCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "OTP");
        public static string EnterValidValue => DictionaryPhraseRepository.Current.Get("/Accounts/CNG Registration/InvalidValue", "Invalid value for {0}, Only alphanumeric, space and . - charachters allowed");
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/CNG Registration/Required", "Please enter a value for {0}");

    }
       
}