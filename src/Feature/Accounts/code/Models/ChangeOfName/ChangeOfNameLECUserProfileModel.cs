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
    public class ChangeOfNameLECUserProfileModel
    {
        public string LECRegistrationNumber { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ChangeOfNameLECUserProfileModel))]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(ChangeOfNameLECUserProfileModel))]
        public string LECMobileNumber { get; set; }

        public string LECName { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ChangeOfNameLECUserProfileModel))]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(ChangeOfNameLECUserProfileModel))]
        [DataType(DataType.EmailAddress)]
        public string LECEmailId { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessageResourceName = nameof(InvalidOTP), ErrorMessageResourceType = typeof(ChangeOfNameLECUserProfileModel))]
        public string OTPNumber { get; set; }

        public bool isOTPSent { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Required", "Please enter a value");
        public static string InvalidOTP => DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Invalid Mobile", "Please enter a valid Mobile Number");
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Invalid Mobile", "Please enter a valid Mobile Number");
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Invalid Email Address", "Please enter a valid email address");
    }

    [Serializable]
    public class ChangeOfNameLECChangePasswordModel
    {
        public string LECRegistrationNumber { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ChangeOfNameLECChangePasswordModel))]
        [PasswordMinLength(ErrorMessageResourceName = nameof(MinimumPasswordLength), ErrorMessageResourceType = typeof(ChangeOfNameLECChangePasswordModel))]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessageResourceName = nameof(PasswordValidation), ErrorMessageResourceType = typeof(ChangeOfNameLECChangePasswordModel))]
        [DataType(DataType.Password)]
        public string LECPassword { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ChangeOfNameLECChangePasswordModel))]
        [PasswordMinLength(ErrorMessageResourceName = nameof(MinimumPasswordLength), ErrorMessageResourceType = typeof(ChangeOfNameLECChangePasswordModel))]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessageResourceName = nameof(PasswordValidation), ErrorMessageResourceType = typeof(ChangeOfNameLECChangePasswordModel))]
        [DataType(DataType.Password)]
        public string LECNewPasssword { get; set; }

        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare(nameof(LECNewPasssword), ErrorMessageResourceName = nameof(ConfirmPasswordMismatch), ErrorMessageResourceType = typeof(ChangeOfNameLECChangePasswordModel))]
        public string LECNewConfirmPasssword { get; set; }

        public static string ConfirmPasswordMismatch => DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Confirm Password Mismatch", "Your password confirmation does not match. Please enter a new password.");
        public static string PasswordValidation => DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Invalid Password", "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character.");
        public static string MinimumPasswordLength => DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Minimum Password Length", "Please enter a password with at lease {1} characters");
        public static string Required => DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Required", "Please enter a value");
    }

    [Serializable]
    public class ChangeOfNameLECDeregisterModel
    {
        public string LECRegistrationNumber { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ChangeOfNameLECDeregisterModel))]
        [PasswordMinLength(ErrorMessageResourceName = nameof(MinimumPasswordLength), ErrorMessageResourceType = typeof(ChangeOfNameLECDeregisterModel))]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessageResourceName = nameof(PasswordValidation), ErrorMessageResourceType = typeof(ChangeOfNameLECDeregisterModel))]
        [DataType(DataType.Password)]
        public string LECPassword { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Required", "Please enter a value");
        public static string PasswordValidation => DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Invalid Password", "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character.");
        public static string MinimumPasswordLength => DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Minimum Password Length", "Please enter a password with at lease {1} characters");
    }
}