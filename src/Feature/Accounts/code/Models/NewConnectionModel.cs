using System.ComponentModel.DataAnnotations;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Sitecore.Feature.Accounts.Models
{
    public class NewConnectionModel
    {
        public NewConnectionModel()
        {
            CityList = new List<SelectListItem>();
            AreaList= new List<SelectListItem>();
            ApartmentComplexList = new List<SocietyList>();
            ReferenceSourceList = new List<SelectListItem>();
            HouseTypeList = new List<SelectListItem>();
            PartnerTypeList = new List<SelectListItem>();
            TypeOfCustomerList = new List<SelectListItem>();
            ApplicationList = new List<SelectListItem>();
            CurrentFuelUseList = new List<SelectListItem>();
            TypeOfIndustryList = new List<SelectListItem>();
        }
        [Display(Name = nameof(NameCaption), ResourceType = typeof(NewConnectionModel))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(NewConnectionModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(NewConnectionModel))]
        public string FullName { get; set; }

        [Display(Name = nameof(MobileCaption), ResourceType = typeof(NewConnectionModel))]
        [RegularExpression(@"^(?:(?:\+|0{0,2})91(\s*[\ -]\s*)?|[0]?)?[789]\d{9}|(\d[ -]?){10}\d$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(NewConnectionModel))]
        public string Mobile { get; set; }

        [Display(Name = nameof(PincodeCaption), ResourceType = typeof(NewConnectionModel))]
        [RegularExpression(@"^\d{6}(-\d{4})?$", ErrorMessageResourceName = nameof(InvalidPincode), ErrorMessageResourceType = typeof(NewConnectionModel))]
        public string Pincode { get; set; }

        [Display(Name = nameof(CityCaption), ResourceType = typeof(NewConnectionModel))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(NewConnectionModel))]
        [Required(ErrorMessageResourceName = nameof(ListRequired), ErrorMessageResourceType = typeof(NewConnectionModel))]
        public string City { get; set; }
        public List<SelectListItem> CityList { get; set; }

        [Display(Name = nameof(AreaCaption), ResourceType = typeof(NewConnectionModel))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(NewConnectionModel))]
        [Required(ErrorMessageResourceName = nameof(ListRequired), ErrorMessageResourceType = typeof(NewConnectionModel))]
        public string Area { get; set; }
        public List<SelectListItem> AreaList { get; set; }

        [Display(Name = nameof(HouseTypeCaption), ResourceType = typeof(NewConnectionModel))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(NewConnectionModel))]
        //[Required(ErrorMessageResourceName = nameof(ListRequired), ErrorMessageResourceType = typeof(NewConnectionModel))]
        public string HouseType { get; set; }
        public List<SelectListItem> HouseTypeList { get; set; }

        [Display(Name = nameof(TypeOfCustomerCaption), ResourceType = typeof(NewConnectionModel))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(NewConnectionModel))]        
        public string TypeOfCustomer { get; set; }
        [Display(Name = nameof(TypeOfCustomerCaption), ResourceType = typeof(NewConnectionModel))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(NewConnectionModel))]
        public string OtherTypeOfCustomer { get; set; }
        public List<SelectListItem> TypeOfCustomerList { get; set; }

        [Display(Name = nameof(ApplicationCaption), ResourceType = typeof(NewConnectionModel))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(NewConnectionModel))]
        //[Required(ErrorMessageResourceName = nameof(ListRequired), ErrorMessageResourceType = typeof(NewConnectionModel))]
        public string Application { get; set; }
        [Display(Name = nameof(ApplicationCaption), ResourceType = typeof(NewConnectionModel))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(NewConnectionModel))]
        //[Required(ErrorMessageResourceName = nameof(ListRequired), ErrorMessageResourceType = typeof(NewConnectionModel))]
        public string OtherApplication { get; set; }
        public List<SelectListItem> ApplicationList { get; set; }

        [Display(Name = nameof(CurrentFuelUseCaption), ResourceType = typeof(NewConnectionModel))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(NewConnectionModel))]
        //[Required(ErrorMessageResourceName = nameof(ListRequired), ErrorMessageResourceType = typeof(NewConnectionModel))]
        public string CurrentFuelUse { get; set; }
        [Display(Name = nameof(CurrentFuelUseCaption), ResourceType = typeof(NewConnectionModel))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(NewConnectionModel))]
        //[Required(ErrorMessageResourceName = nameof(ListRequired), ErrorMessageResourceType = typeof(NewConnectionModel))]
        public string OtherCurrentFuelUse { get; set; }
        public List<SelectListItem> CurrentFuelUseList { get; set; }

        [Display(Name = nameof(TypeOfIndustryCaption), ResourceType = typeof(NewConnectionModel))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(NewConnectionModel))]
        //[Required(ErrorMessageResourceName = nameof(ListRequired), ErrorMessageResourceType = typeof(NewConnectionModel))]
        public string TypeOfIndustry { get; set; }
        [Display(Name = nameof(TypeOfIndustryCaption), ResourceType = typeof(NewConnectionModel))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(NewConnectionModel))]
        //[Required(ErrorMessageResourceName = nameof(ListRequired), ErrorMessageResourceType = typeof(NewConnectionModel))]
        public string OtherTypeOfIndustry { get; set; }
        public List<SelectListItem> TypeOfIndustryList { get; set; }

        [Display(Name = nameof(ApartmentComplexCaption), ResourceType = typeof(NewConnectionModel))]
        public string ApartmentComplex { get; set; }
        [Display(Name = nameof(ApartmentComplexCaption), ResourceType = typeof(NewConnectionModel))]
        public string OtherApartmentComplex { get; set; }
        public List<SocietyList> ApartmentComplexList { get; set; }

        [Display(Name = nameof(Partner_TypeCaption), ResourceType = typeof(NewConnectionEnquiryModel))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(NewConnectionModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(NewConnectionEnquiryModel))]
        public string Partner_Type { get; set; }


        public string MonthlyConsumption { get; set; }
        public string EnquiryNo { get; set; }
        public string FormURL { get; set; }
        public string CampaignID { get; set; }
        public string LeadSource { get; set; }
        public string ResponseURL { get; set; }
        public string Captcha { get; set; }

        [Display(Name = nameof(HouseNoCaption), ResourceType = typeof(NewConnectionModel))]
        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(NewConnectionModel))]
        public string HouseNo { get; set; }

        [Display(Name = nameof(Address1Caption), ResourceType = typeof(NewConnectionModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(NewConnectionModel))]
        public string AddressLine1 { get; set; }

        [Display(Name = nameof(Address2Caption), ResourceType = typeof(NewConnectionModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(NewConnectionModel))]
        public string AddressLine2 { get; set; }

        [Display(Name = nameof(ReferenceSourceCaption), ResourceType = typeof(NewConnectionModel))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(NewConnectionModel))]
        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(NewConnectionModel))]
        public string ReferenceSource { get; set; }
        public List<SelectListItem> ReferenceSourceList { get; set; }

        //[Display(Name = nameof(EmailCaption), ResourceType = typeof(NewConnectionModel))]
        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(NewConnectionModel))]
        //[EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(NewConnectionModel))]
        //[DataType(DataType.EmailAddress)]
        //public string Email { get; set; }

        

        public string ReturnViewMessage { get; set; }
        
        public List<SelectListItem> PartnerTypeList { get; set; }


        public static string Partner_TypeCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Connection Type");
        public static string TypeOfIndustryCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Type Of Industry");
        public static string CurrentFuelUseCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Current Fuel Using");
        public static string NameCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Name");
        public static string TypeOfCustomerCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Type Of Customer");
        public static string ApplicationCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Application");
        public static string ApartmentComplexCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Apartment Complex/Society");
        public static string CityCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "City");
        public static string AreaCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Area");
        public static string HouseTypeCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Type of House");
        public static string HouseNoCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "House No/Building No");
        public static string Address1Caption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Address Line 1");
        public static string Address2Caption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Address Line 2");
        public static string ReferenceSourceCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Reference Source");
        public static string EmailCaption => DictionaryPhraseRepository.Current.Get("/Accounts/NewConnection/Email", "Email");
        public static string MobileCaption => DictionaryPhraseRepository.Current.Get("/Accounts/NewConnection/ContactNumber", "Mobile Number");
        public static string PincodeCaption => DictionaryPhraseRepository.Current.Get("/Accounts/NewConnection/Pincode", "Pincode");
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/NewConnection/Required", "Please enter a value for {0}");
        public static string EnterValidValue => DictionaryPhraseRepository.Current.Get("/Accounts/NewConnection/InvalidValue", "Invalid value for {0}, Only alphanumeric, space and . - charachters allowed");
        public static string ListRequired => DictionaryPhraseRepository.Current.Get("/Accounts/NewConnection/Required", "Please select a value for {0}");
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/NewConnection/Invalid Contact", "Please enter a valid Mobile Number");
        public static string InvalidPincode => DictionaryPhraseRepository.Current.Get("/Accounts/NewConnection/Invalid Pincode", "Please enter a valid Pincode");
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/NewConnection/Invalid Email Address", "Please enter a valid email address");
        public static string InvalidName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/Accounts/NewConnection/Invalid Name", "Please enter a valid Name");
            }
        }
    }

    public class SocietyList
    {
        public string SocietyCode { get; set; }
        public string SocietyName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string Msg_Flag { get; set; }
        public string Message { get; set; }
    }
    
    public class HouseList
    {
        public string HouseNumber { get; set; }
        public string SocietyCode { get; set; }
        public string BPNumber { get; set; }
        public string ConsumerNumber { get; set; }
    }

}