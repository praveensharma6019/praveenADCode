namespace Sitecore.Feature.Accounts.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;
    using System.Web;

    [Serializable]
    public class ChangeOfNameLECUserLoginModel
    {
        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ChangeOfNameLECUserLoginModel))]
        public string UserName { get; set; }

        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ChangeOfNameLECUserLoginModel))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string InputLoginWithOTP { get; set; }
        public bool IsLoginViaOTP { get; set; }
        public bool IsLoginwithEmailId { get; set; }
        public bool IsOTPSent { get; set; }

        public string OTPNumber { get; set; }

        public string Captcha { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Login/Required", "Please enter a value");
    }

    [Serializable]
    public class ChangeOfNameLECUserLoginSessionModel
    {
        public string UserName { get; set; }
        public string LECName { get; set; }
        public string RegistrationNumber { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
    }

    [Serializable]
    public class ChangeOfNameLECUserLoginSessionForOTPModel
    {
        public string LECName { get; set; }
        public string  LECRegistrationNumber { get; set; }
        public string LECInput { get; set; }
        public bool IsMobileNumberInput { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
    }
}