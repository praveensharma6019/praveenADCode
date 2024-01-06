namespace Sitecore.Feature.Accounts.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Foundation.Dictionary.Repositories;

    public class EditProfile : ProfileBasicInfo
    {
        [Display(Name = nameof(EmailCaption), ResourceType = typeof(EditProfile))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(EditProfile))]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(EditProfile))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = nameof(LandlineNumberCaption), ResourceType = typeof(EditProfile))]
        [RegularExpression(@"\d{6,9}$", ErrorMessageResourceName = nameof(InvalidLandline), ErrorMessageResourceType = typeof(EditProfile))]
        public string LandlineNumber { get; set; }

        [Display(Name = nameof(MobileNumberCaption), ResourceType = typeof(EditProfile))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(EditProfile))]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(EditProfile))]
        public string MobileNumber { get; set; }

        [Display(Name = nameof(DateofBirthCaption), ResourceType = typeof(EditProfile))]
        public string DateofBirth { get; set; }

        [Display(Name = nameof(EBillCaption), ResourceType = typeof(EditProfile))]
        public bool EBill { get; set; }

        [Display(Name = nameof(SMSUpdateCaption), ResourceType = typeof(EditProfile))]
        public bool SMSUpdate { get; set; }

        [Display(Name = nameof(PaperlessBillingCaption), ResourceType = typeof(EditProfile))]
        public bool PaperlessBilling { get; set; }


        [Display(Name = nameof(FirstNameCaption), ResourceType = typeof(EditProfile))]
        public string FirstName { get; set; }

        [Display(Name = nameof(LastNameCaption), ResourceType = typeof(EditProfile))]
        public string LastName { get; set; }

        [Display(Name = nameof(PhoneNumberCaption), ResourceType = typeof(EditProfile))]
        [RegularExpression(@"^\+?\d*(\(\d+\)-?)?\d+(-?\d+)+$", ErrorMessageResourceName = nameof(PhoneNumberFormat), ErrorMessageResourceType = typeof(EditProfile))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MaxLengthExceeded), ErrorMessageResourceType = typeof(EditProfile))]
        public string PhoneNumber { get; set; }

        [Display(Name = nameof(InterestsCaption), ResourceType = typeof(EditProfile))]
        public string Interest { get; set; }

        [Display(Name = nameof(LastNameCaption), ResourceType = typeof(EditProfile))]
        public string ComplaintNumber { get; set; }
        public string Captcha { get; set; }
        public string UserType { get; set; }
        public string CustomerType { get; set; }
        public bool isOTPSent { get; set; }
        public bool isOTPValidated { get; set; }
        public string OTPNumber { get; set; }

        public IEnumerable<string> InterestTypes { get; set; }
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
        public static string ComplaintNumberCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/ComplaintNumberCaption", "Complaint Number");
        public static string EmailCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/Email", "E-mail id");
        public static string MobileNumberCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/Mobile No", "Mobile No.");
        public static string LandlineNumberCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/Landline No", "Landline Number");
        public static string DateofBirthCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/Date of Birth", "Date of Birth");
        public static string EBillCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/EBill", "E-Bill (I want to received my electricity bill by email on above mentioned email id.)");
        public static string SMSUpdateCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/EBill", "I want to receive information updates about Adani Electricity's new services and initiatives.");
        public static string PaperlessBillingCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/PaperlessBillingCaption", "Paperless Billing");
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Email Address", "Please enter a valid email address");

        public static string FirstNameCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/First Name", "First name");
        public static string LastNameCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/Last Name", "Last name");
        public static string PhoneNumberCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/Phone Number", "Phone number");
        public static string InterestsCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/Interests", "Interests");
        public static string MaxLengthExceeded => DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/Max Length", "{0} length should be less than {1}");
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a Valid Mobile Number");
        public static string InvalidLandline => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid LandLine", "Please enter a valid LandLine Number");
        public static string PhoneNumberFormat => DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/Phone Number Format", "Phone number should contain only +, ( ) and digits");
    }

    public static class UserTypes
    {
        public static string Standard = "Standard";

        public static string Premium = "Premium";


    }

    public static class AdaniGasUserTypes
    {
        //public static string Domestic = "domestic";
        //public static string Commercial = "commercial";
        //public static string Industrial = "industrial";
        public static string PNG = "png";

        public static string GetUserType(string cust_type)
        {
            return AdaniGasUserTypes.PNG;
        }
    }
}