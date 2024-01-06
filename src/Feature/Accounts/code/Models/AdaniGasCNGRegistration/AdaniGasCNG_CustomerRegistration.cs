using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Feature.Accounts.Models.AdaniGasCNGRegistration
{
    [Serializable]
    public class AdaniGasCNG_CustomerRegistration
    {        
        public AdaniGasCNG_CustomerRegistration()
        {
            CityList = new List<TextValueItem>();
            AreaList = new List<TextValueItem>();
            StateList = new List<TextValueItem>();
            VehicleCompanyList = new List<TextValueItem>();
            VehicleModelList = new List<TextValueItem>();
            YearList = new List<TextValueItem>();
        }
        [Display(Name = nameof(FNameCaption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string FirstName { get; set; }

        [Display(Name = nameof(LNameCaption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string LastName { get; set; }

        [Display(Name = nameof(MobileCaption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [RegularExpression(@"^(?:(?:\+|0{0,2})91(\s*[\ -]\s*)?|[0]?)?[789]\d{9}|(\d[ -]?){10}\d$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string Mobile { get; set; }

        [Display(Name = nameof(OTPCaption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [RegularExpression(@"^\d{4}$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string OTP { get; set; }
        public bool IsOTPSent { get; set; }
        public bool IsOTPValid { get; set; }
        public bool IsSavedintoDB { get; set; }
        [Display(Name = nameof(YearCaption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [RegularExpression(@"^\d{4}$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string Year { get; set; }
        public List<TextValueItem> YearList { get; set; }

        [Display(Name = nameof(Address1Caption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string CurrentAddressLine1 { get; set; }

        [Display(Name = nameof(Address2Caption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string CurrentAddressLine2 { get; set; }

        [Display(Name = nameof(PincodeCaption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [RegularExpression(@"^\d{6}(-\d{4})?$", ErrorMessageResourceName = nameof(InvalidPincode), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string CurrentPincode { get; set; }

        [Display(Name = nameof(CityCaption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [Required(ErrorMessageResourceName = nameof(ListRequired), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string CurrentCity { get; set; }
        public List<TextValueItem> CityList { get; set; }

        [Display(Name = nameof(AreaCaption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [Required(ErrorMessageResourceName = nameof(ListRequired), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string CurrentArea { get; set; }
        public List<TextValueItem> AreaList { get; set; }

        [Display(Name = nameof(StateCaption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [Required(ErrorMessageResourceName = nameof(ListRequired), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string CurrentState { get; set; }
        public List<TextValueItem> StateList { get; set; }

        [Display(Name = nameof(Address1Caption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string RegisteredAddressLine1 { get; set; }

        [Display(Name = nameof(Address2Caption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string RegisteredAddressLine2 { get; set; }

        [Display(Name = nameof(PincodeCaption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [RegularExpression(@"^\d{6}(-\d{4})?$", ErrorMessageResourceName = nameof(InvalidPincode), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string RegisteredPincode { get; set; }

        [Display(Name = nameof(CityCaption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [Required(ErrorMessageResourceName = nameof(ListRequired), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string RegisteredCity { get; set; }

        [Display(Name = nameof(AreaCaption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [Required(ErrorMessageResourceName = nameof(ListRequired), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string RegisteredArea { get; set; }

        [Display(Name = nameof(StateCaption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string RegisteredState { get; set; }

        [Display(Name = nameof(VehicleCompanyCaption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [Required(ErrorMessageResourceName = nameof(ListRequired), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string VehicleCompany { get; set; }
        public List<TextValueItem> VehicleCompanyList { get; set; }

        [Display(Name = nameof(VehicleTypeCaption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [Required(ErrorMessageResourceName = nameof(ListRequired), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string VehicleType { get; set; }
        public List<TextValueItem> VehicleTypeList { get; set; }

        [Display(Name = nameof(VehicleModelCaption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [Required(ErrorMessageResourceName = nameof(ListRequired), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string VehicleModel { get; set; }
        public List<TextValueItem> VehicleModelList { get; set; }

        [Display(Name = nameof(VehicleNoCaption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [Required(ErrorMessageResourceName = nameof(ListRequired), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string VehicleNo { get; set; }
        [Display(Name = nameof(VehicleStandardCaption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        public string VehicleStandard { get; set; }

        public bool IsBSVI { get; set; }
        public string RegistrationNo { get; set; }
        public DateTime SubmittedOn { get; set; }
        public string FormURL { get; set; }
        public string HouseNo { get; set; }
        public string ResponseURL { get; set; }
        public string Captcha { get; set; }
        public string PageInfo { get; set; }
        public string FormName { get; set; }

        [Display(Name = nameof(EmailCaption), ResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(AdaniGasCNG_CustomerRegistration))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string ReturnViewMessage { get; set; }

        
        public static string VehicleNoCaption => DictionaryPhraseRepository.Current.Get("/Accounts/VehicleNoCaption", "Vehicle Registration No");
        public static string VehicleStandardCaption => DictionaryPhraseRepository.Current.Get("/Accounts/VehicleNoCaption", "Select vehicle standard");
        public static string FNameCaption => DictionaryPhraseRepository.Current.Get("/Accounts/FName", "First Name");
        public static string LNameCaption => DictionaryPhraseRepository.Current.Get("/Accounts/LName", "Last Name");
        public static string VehicleCompanyCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Vehicle Company", "Vehicle Company");
        public static string VehicleTypeCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Vehicle Type", "Vehicle Type");
        public static string VehicleModelCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Vehicle Model");
        public static string CityCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "City");
        public static string AreaCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Area");
        public static string StateCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "State");
        public static string Address1Caption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Address Line 1");
        public static string Address2Caption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Address Line 2");
        public static string ReferenceSourceCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Reference Source");
        public static string EmailCaption => DictionaryPhraseRepository.Current.Get("/Accounts/CNG Registration/Email", "Email");
        public static string MobileCaption => DictionaryPhraseRepository.Current.Get("/Accounts/CNG Registration/ContactNumber", "Mobile Number");
        public static string PincodeCaption => DictionaryPhraseRepository.Current.Get("/Accounts/CNG Registration/Pincode", "Pincode");
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/CNG Registration/Required", "Please enter a value for {0}");
        public static string EnterValidValue => DictionaryPhraseRepository.Current.Get("/Accounts/CNG Registration/InvalidValue", "Invalid value for {0}, Only alphanumeric, space and . - charachters allowed");
        public static string ListRequired => DictionaryPhraseRepository.Current.Get("/Accounts/CNG Registration/Required", "Please select a value for {0}");
        public static string SelectRequired => DictionaryPhraseRepository.Current.Get("/Accounts/CNG Registration/Required", "Selection required.");
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/CNG Registration/Invalid Contact", "Please enter a valid Mobile Number");
        public static string OTPCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "OTP");
        public static string YearCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Year");
        public static string InvalidPincode => DictionaryPhraseRepository.Current.Get("/Accounts/CNG Registration/Invalid Pincode", "Please enter a valid Pincode");
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/CNG Registration/Invalid Email Address", "Please enter a valid email address");
        public static string InvalidName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/Accounts/NewConnection/Invalid Name", "Please enter a valid Name");
            }
        } 
    }
    [Serializable]
    public class TextValueItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}