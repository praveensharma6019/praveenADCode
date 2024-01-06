using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models.AdaniGasCNGRegistration
{
    [Serializable]
    public class AdaniGasCNG_DealerLoginModel
    {
        public bool IsLoggedIn { get; set; }
        public string LoginName { get; set; }
        public string OTP { get; set; }
        public string LoginUser { get; set; }
        public string MobileNo { get; set; }
        public string UserIP { get; set; }
        public string AuthToken { get; set; }
        public string userType { get; set; }
        public string Contract_No { get; set; }
        public string Inst_No { get; set; }
        public string DeviceId { get; set; }
        public string RegNo { get; set; }
        public string ReadingUnit { get; set; }
        public string DealerId { get; set; }
    }

    [Serializable]
    public class AdaniGasCNG_DealerLoginInfo
    {
        //[Display(Name = nameof(EmailCaption), ResourceType = typeof(LoginInfo))]
        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(LoginInfo))]
        //[EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(LoginInfo))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = nameof(DealerIdCaption), ResourceType = typeof(AdaniGasCNG_DealerLoginInfo))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(AdaniGasCNG_DealerLoginInfo))]
        public string DealerId { get; set; }

        [Display(Name = nameof(OTPCaption), ResourceType = typeof(AdaniGasCNG_DealerLoginInfo))]
        [RegularExpression(@"^\d{4}$", ErrorMessageResourceName = nameof(InvalidValue), ErrorMessageResourceType = typeof(AdaniGasCNG_DealerLoginInfo))]
        public string OTP { get; set; }

        public string MobileNo { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string PageInfo { get; set; }
        public string ReturnUrl { get; set; }
        public bool IsAccountValid { get; set; }
        public bool IsOTPSend { get; set; }
        public bool IsOTPValid { get; set; }
        public IEnumerable<FedAuthLoginButton> LoginButtons { get; set; }

        [Display(Name = nameof(RememberMeCaption), ResourceType = typeof(LoginInfo))]
        public bool RememberMe { get; set; }
        public static string DealerIdCaption => DictionaryPhraseRepository.Current.Get("/Accounts/CNGDealerLogin/Customer ID", "Dealer ID");
        public static string OTPCaption => DictionaryPhraseRepository.Current.Get("/Accounts/CNGDealerLogin/Password", "OTP");
        public static string MinimumPasswordLength => DictionaryPhraseRepository.Current.Get("/Accounts/CNGDealerLogin/Minimum Password Length", "Please enter a password with at least {1} characters");
        public static string InvalidValue => DictionaryPhraseRepository.Current.Get("/Accounts/CNGDealerLogin/Required", "Invalid {0}.");
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/CNGDealerLogin/Required", "Please enter a value.");
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/CNGDealerLogin/Invalid Email Address", "Please enter a valid email address");
        public static string RememberMeCaption => DictionaryPhraseRepository.Current.Get("/Accounts/CNGDealerLogin/Remember me", "Remember me");
    }
}