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
    public class ChangeOfNameLECUserRegistrationModel
    {
        public bool IsValidatedByRegistrationNumber { get; set; }
        public bool IsValidatedByMobileNumber { get; set; }
        public bool IsValidatedByEmailId { get; set; }

        public string InputByUser { get; set; }
        public bool ValidateRegistrationNumber { get; set; }
        public bool ValidateOTP { get; set; }

        public string OTPNumber { get; set; }
        public string LECUserName { get; set; }

        public string LECValidity { get; set; }
        public string UserNameType { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ChangeOfNameLECUserRegistrationModel))]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessageResourceName = nameof(InvalidLECRegistrationNumber), ErrorMessageResourceType = typeof(ChangeOfNameLECUserRegistrationModel))]
        public string LECRegistrationNumber { get; set; }

        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ChangeOfNameLECUserRegistrationModel))]
        //[RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(ChangeOfNameLECUserRegistrationModel))]
        public string LECMobileNumber { get; set; }

        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ChangeOfNameLECUserRegistrationModel))]
        //[RegularExpression("^[a-zA-Z0-9]*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(ChangeOfNameLECUserRegistrationModel))]
        public string LECName { get; set; }

        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ChangeOfNameLECUserRegistrationModel))]
        //[EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(ChangeOfNameLECUserRegistrationModel))]
        //[DataType(DataType.EmailAddress)]
        public string LECEmailId { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ChangeOfNameLECUserRegistrationModel))]
        [PasswordMinLength(ErrorMessageResourceName = nameof(MinimumPasswordLength), ErrorMessageResourceType = typeof(ChangeOfNameLECUserRegistrationModel))]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessageResourceName = nameof(PasswordValidation), ErrorMessageResourceType = typeof(ChangeOfNameLECUserRegistrationModel))]
        [DataType(DataType.Password)]
        public string LECPassword { get; set; }

        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare(nameof(LECPassword), ErrorMessageResourceName = nameof(ConfirmPasswordMismatch), ErrorMessageResourceType = typeof(ChangeOfNameLECUserRegistrationModel))]
        public string LECConfirmPassword { get; set; }

        public string Captcha { get; set; }

        public static string ConfirmPasswordMismatch => DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Confirm Password Mismatch", "Your password confirmation does not match. Please enter a new password.");
        public static string PasswordValidation => DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Invalid Password", "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character.");
        public static string MinimumPasswordLength => DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Minimum Password Length", "Please enter a password with at lease {1} characters");
        public static string Required => DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Required", "Please enter a value");
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Invalid Email Address", "Please enter a valid email address");
        public static string InvalidLECRegistrationNumber => DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Invalid Registration Number", "Please enter a valid Registration Number");
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Invalid Mobile", "Please enter a valid Mobile Number");
        public static string InvalidName => DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Invalid Name", "Please enter a valid Name");
    }

    [Serializable]
    public class ChangeOfNameLECRegistrationDetails {
        public string InputByUser { get; set; }
        public string RegistrationNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string ValidityInfo { get; set; }
        public bool IsValidatedByRegistrationNumber { get; set; }
        public bool IsValidatedByMobileNumber { get; set; }
        public bool IsValidatedByEmail { get; set; }
    }
}