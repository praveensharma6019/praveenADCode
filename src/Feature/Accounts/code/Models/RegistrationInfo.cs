namespace Sitecore.Feature.Accounts.Models
{
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;
    using System.Web.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    public class RegistrationInfo
    {
        [Display(Name = nameof(EmailCaption), ResourceType = typeof(RegistrationInfo))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegistrationInfo))]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(RegistrationInfo))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = nameof(PasswordCaption), ResourceType = typeof(RegistrationInfo))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegistrationInfo))]
        [PasswordMinLength(ErrorMessageResourceName = nameof(MinimumPasswordLength), ErrorMessageResourceType = typeof(RegistrationInfo))]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{12,20}$", ErrorMessageResourceName = nameof(PasswordRegexFailed), ErrorMessageResourceType = typeof(RegistrationInfo))]

        public string Password { get; set; }

        [Display(Name = nameof(ConfirmPasswordCaption), ResourceType = typeof(RegistrationInfo))]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare(nameof(Password), ErrorMessageResourceName = nameof(ConfirmPasswordMismatch), ErrorMessageResourceType = typeof(RegistrationInfo))]
        public string ConfirmPassword { get; set; }

        [Display(Name = nameof(LoginNameCaption), ResourceType = typeof(RegistrationInfo))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegistrationInfo))]
        [MinLength(6, ErrorMessageResourceName = nameof(MinimumLoginNameLength), ErrorMessageResourceType = typeof(RegistrationInfo))]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessageResourceName = nameof(InvalidLoginName), ErrorMessageResourceType = typeof(RegistrationInfo))]
        public string LoginName { get; set; }

        [Display(Name = nameof(SecretQuestionCaption), ResourceType = typeof(RegistrationInfo))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegistrationInfo))]
        public string SecretQuestion { get; set; }

        
        public List<SelectListItem> QuestionList
        {
            get;set;
        }
        [Display(Name = nameof(AnswerCaption), ResourceType = typeof(RegistrationInfo))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegistrationInfo))]
        public string Answer { get; set; }

        [Display(Name = nameof(FirstNameCaption), ResourceType = typeof(RegistrationInfo))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegistrationInfo))]
        public string FirstName { get; set; }

        [Display(Name = nameof(LastNameCaption), ResourceType = typeof(RegistrationInfo))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegistrationInfo))]
        public string LastName { get; set; }

        [Display(Name = nameof(GenderCaption), ResourceType = typeof(RegistrationInfo))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegistrationInfo))]
        public string Gender { get; set; }

        [Display(Name = nameof(LandlineNumberCaption), ResourceType = typeof(RegistrationInfo))]
        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegistrationInfo))]
        [RegularExpression(@"\d{5}([- ]*)\d{6}", ErrorMessageResourceName = nameof(InvalidLandline), ErrorMessageResourceType = typeof(RegistrationInfo))]
        public string LandlineNumber { get; set; }

        [Display(Name = nameof(MobileNumberCaption), ResourceType = typeof(RegistrationInfo))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegistrationInfo))]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(RegistrationInfo))]
        public string MobileNumber { get; set; }

        [Display(Name = nameof(DateofBirthCaption), ResourceType = typeof(RegistrationInfo))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegistrationInfo))]
        public string DateofBirth { get; set; }

        [Display(Name = nameof(AccountNoCaption), ResourceType = typeof(RegistrationInfo))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegistrationInfo))]
        [RegularExpression(@"^[0-9]{8,10}$", ErrorMessageResourceName = nameof(InvalidAccountNumber), ErrorMessageResourceType = typeof(RegistrationInfo))]
        public string AccountNo { get; set; }

        [Display(Name = nameof(MeterNoCaption), ResourceType = typeof(RegistrationInfo))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegistrationInfo))]
        public string MeterNo { get; set; }

        [Display(Name = nameof(TextVerificationCaption), ResourceType = typeof(RegistrationInfo))]
        public string TextVerification { get; set; }

        [Display(Name = nameof(EBillCaption), ResourceType = typeof(RegistrationInfo))]
        public bool EBill { get; set; }

        [Display(Name = nameof(RecieveNotificationCaption), ResourceType = typeof(RegistrationInfo))]
        public bool RecieveNotification { get; set; }

        public string Captcha { get; set; }

        public static string ConfirmPasswordCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Confirm Password", "Re-type Password");
        public static string EmailCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Email", "E-mail id");
        public static string PasswordCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Password", "Password");
        public static string ConfirmPasswordMismatch => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Confirm Password Mismatch", "Your password confirmation does not match. Please enter a new password.");
        public static string MinimumPasswordLength => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Minimum Password Length", "Please enter a password with at lease {1} characters");
        public static string PasswordRegexFailed => DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/PasswordRegexFailed", "Your password must be at least 12 char long and must contains 1 special character,  1 lower case, 1 upper case and 1 number");

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Email Address", "Please enter a valid email address");
        public static string MinimumLoginNameLength => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Minimum Login Length", "Please enter a Login Name with at lease 6 characters");
        public static string InvalidLoginName => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Login Name", "Please enter a Valid Login Name");

        public static string LoginNameCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/LoginName", "Choose Your Login Name");
        public static string SecretQuestionCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/SecretQuestion", "Secret Question");
        public static string AnswerCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Answer", "Answer");
        public static string FirstNameCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/FirstName", "First Name");
        public static string LastNameCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/LastName", "Last Name");
        public static string GenderCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Gender", "Gender");
        public static string LandlineNumberCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/LandlineNumber", "Landline Number");
        public static string MobileNumberCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/MobileNumber", "Mobile No.");
        public static string DateofBirthCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/DateofBirth", "Date of Birth");
        public static string AccountNoCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/AccountNo", "Account No.");
        public static string MeterNoCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/MeterNo", "Meter No.");
        public static string TextVerificationCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/TextVerification", "Text Verification");
        public static string EBillCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/EBill", "E-Bill (I want to received my electricity bill by email on above mentioned email id.)");
        public static string RecieveNotificationCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/RecieveNotification", "I want to receive information updates about Adani Electricity's new services and initiatives.");

        public static string InvalidLandline => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Landline Number", "Please enter a valid Landline Number");
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a valid Mobile Number");

        public static string InvalidAccountNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Account", "Please enter a valid Account Number");
    }

    public class RegistrationInfoAdaniGas
    {
        public RegistrationInfoAdaniGas()
        {
            PartnerTypeList = new List<SelectListItem>();
        }

        [Display(Name = nameof(CustomerIDCaption), ResourceType = typeof(RegistrationInfoAdaniGas))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegistrationInfoAdaniGas))]
        [RegularExpression(@"^[0-9]{8,10}$", ErrorMessageResourceName = nameof(InvalidCustomerIDNumber), ErrorMessageResourceType = typeof(RegistrationInfoAdaniGas))]
        public string CustomerID { get; set; }

        [Display(Name = nameof(EmailCaption), ResourceType = typeof(RegistrationInfoAdaniGas))]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(RegistrationInfoAdaniGas))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = nameof(PasswordCaption), ResourceType = typeof(RegistrationInfoAdaniGas))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegistrationInfoAdaniGas))]
        [PasswordMinLength(ErrorMessageResourceName = nameof(MinimumPasswordLength), ErrorMessageResourceType = typeof(RegistrationInfoAdaniGas))]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessageResourceName = nameof(PasswordValidation), ErrorMessageResourceType = typeof(RegistrationInfoAdaniGas))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = nameof(ConfirmPasswordCaption), ResourceType = typeof(RegistrationInfoAdaniGas))]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare(nameof(Password), ErrorMessageResourceName = nameof(ConfirmPasswordMismatch), ErrorMessageResourceType = typeof(RegistrationInfoAdaniGas))]
        public string ConfirmPassword { get; set; }

        //[Display(Name = nameof(OrganizationNameCaption), ResourceType = typeof(RegistrationInfoAdaniGas))]
        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegistrationInfoAdaniGas))]
        //public string OrganizationName { get; set; }

        [Display(Name = nameof(FirstNameCaption), ResourceType = typeof(RegistrationInfoAdaniGas))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegistrationInfoAdaniGas))]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "First Name max length is 50 chanracters only.")]
        public string FirstName { get; set; }

        [Display(Name = nameof(LastNameCaption), ResourceType = typeof(RegistrationInfoAdaniGas))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegistrationInfoAdaniGas))]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Last Name max length is 50 chanracters only.")]
        public string LastName { get; set; }

        [Display(Name = nameof(MobileNumberCaption), ResourceType = typeof(RegistrationInfoAdaniGas))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegistrationInfoAdaniGas))]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(RegistrationInfoAdaniGas))]
        public string MobileNumber { get; set; }

        public string Captcha { get; set; }
        public string ReturnViewMessage { get; set; }

        public List<SelectListItem> PartnerTypeList { get; set; }
        public string PartnerType { get; set; }

        [Display(Name = nameof(OrganizationNameCaption), ResourceType = typeof(RegistrationInfoAdaniGas))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegistrationInfoAdaniGas))]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Organization Name max length is 50 chanracters only.")]
        public string OrganizationName { get; set; }

        public static string ConfirmPasswordCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Confirm Password", "Re-type Password");
        public static string EmailCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Email", "E-mail id");
        public static string PasswordCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Password", "Password");
        public static string ConfirmPasswordMismatch => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Confirm Password Mismatch", "Your password confirmation does not match. Please enter a new password.");
        public static string MinimumPasswordLength => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Minimum Password Length", "Please enter a password with at lease {1} characters");
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Email Address", "Please enter a valid email address");
        public static string OrganizationNameCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/OrganizationName", "Organization Name");
        public static string MobileNumberCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/MobileNumber", "Mobile No.");
        public static string CustomerIDCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Customer ID", "Customer ID.");
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a valid Mobile Number");
        public static string InvalidCustomerIDNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Account", "Please enter a valid Account Number");
        public static string PasswordValidation => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Password", "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character.");
        public static string FirstNameCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/FirstName", "First Name");        
        public static string LastNameCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/LastName", "Last Name");

    }
}