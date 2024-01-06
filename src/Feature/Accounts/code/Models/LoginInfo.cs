namespace Sitecore.Feature.Accounts.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;

    [Serializable]
    public class LoginInfo
    {
        //[Display(Name = nameof(EmailCaption), ResourceType = typeof(LoginInfo))]
        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(LoginInfo))]
        //[EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(LoginInfo))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = nameof(LoginNameCaption), ResourceType = typeof(LoginInfo))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(LoginInfo))]
        public string LoginName { get; set; }

        [Display(Name = nameof(PasswordCaption), ResourceType = typeof(LoginInfo))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(LoginInfo))]
        [PasswordMinLength(ErrorMessageResourceName = nameof(MinimumPasswordLength), ErrorMessageResourceType = typeof(LoginInfo))]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string CustomerID { get; set; }
        public string Captcha { get; set; }
        public string LoginType { get; set; }
        public string ReturnUrl { get; set; }
        public IEnumerable<FedAuthLoginButton> LoginButtons { get; set; }

        [Display(Name = nameof(RememberMeCaption), ResourceType = typeof(LoginInfo))]
        public bool RememberMe { get; set; }
        public static string LoginNameCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Login/Customer ID", "Customer ID");
        public static string PasswordCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Login/Password", "Password");
        public static string MinimumPasswordLength => DictionaryPhraseRepository.Current.Get("/Accounts/Login/Minimum Password Length", "Please enter a password with at least {1} characters");
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Login/Required", "Please enter a value.");
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/Login/Invalid Email Address", "Please enter a valid email address");
        public static string RememberMeCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Login/Remember me", "Remember me");
    }

    [Serializable]
    public class LoginInfoComplaint : LoginInfo
    {
        public bool IsLoginViaOTP { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(LoginInfoComplaint))]
        [RegularExpression(@"^[0-9]{8,10}$", ErrorMessageResourceName = nameof(InvalidAccountNumber), ErrorMessageResourceType = typeof(LoginInfoComplaint))]
        public string AccountNumber { get; set; }
        public static string InvalidAccountNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Account", "Please enter a valid Account Number");

        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(LoginInfoComplaint))]
        //[RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        public string MobileNumber { get; set; }
        public bool IsRegistered { get; set; }
        public bool IsAdmin { get; set; }
        public string AdminRole { get; set; }
        public string ComplaintLevel { get; set; }
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a valid Mobile Number");
        public static new string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Login/Required", "Please enter a value.");
        public string OTPNumber { get; set; }
        public bool IsAccountValid { get; set; }
        public bool IsOTPSend { get; set; }
        public string Captcha2 { get; set; }
        public bool IsAuthenticated { get; set; }

        public string SessionId { get; set; }
    }

    public class UserLoginInfo
    {
        public string CustomerID { get; set; }
        public string Name { get; set; }
        public string CustomerType { get; set; }
        public string Message { get; set; }
        public bool IsError { get; set; }
        public string MessageFlag { get; set; }
        public string Contract_No { get; set; }
        public string Partner { get; set; }
        public string Inst_No { get; set; }
        public string Meter_Fromdt { get; set; }
        public string Meter_Uptodt { get; set; }
        public string Meter_SerialNumber { get; set; }
        public string DeviceId { get; set; }
        public string RegNo { get; set; }
        public string ReadingUnit { get; set; }

    }
}