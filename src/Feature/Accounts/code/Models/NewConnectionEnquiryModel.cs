using System.ComponentModel.DataAnnotations;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Sitecore.Feature.Accounts.Models
{
    public class NewConnectionEnquiryModel
    {
        public NewConnectionEnquiryModel()
        {
            CityList = new List<SelectListItem>();
            AreaList= new List<SelectListItem>();
            PlantCityList = new List<SelectListItem>();
            PartnerTypeList = new List<SelectListItem>();
        }
        public string Category { get; set; }
        public string Comptype { get; set; }
        public string Taskcode { get; set; }
        public string Priority { get; set; }
        public string Text { get; set; }
        public string hdnPartnerType { get; set; }
        public string Captcha { get; set; }
        public string Plant { get; set; }
        [Display(Name = nameof(Partner_TypeCaption), ResourceType = typeof(NewConnectionEnquiryModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(NewConnectionEnquiryModel))]
        public string Partner_Type { get; set; }
        [Display(Name = nameof(NameCaption), ResourceType = typeof(NewConnectionEnquiryModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(NewConnectionEnquiryModel))]
        public string Name { get; set; }
        //[Display(Name = nameof(House_NoCaption), ResourceType = typeof(NewConnectionEnquiryModel))]
        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(NewConnectionEnquiryModel))]
        //public string House_No { get; set; }
        [Display(Name = nameof(Street1Caption), ResourceType = typeof(NewConnectionEnquiryModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(NewConnectionEnquiryModel))]
        public string Street1 { get; set; }
        //[Display(Name = nameof(Street2Caption), ResourceType = typeof(NewConnectionEnquiryModel))]
        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(NewConnectionEnquiryModel))]
        public string Street2 { get; set; }
        //[Display(Name = nameof(Street3Caption), ResourceType = typeof(NewConnectionEnquiryModel))]
        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(NewConnectionEnquiryModel))]
        public string Street3 { get; set; }
        //[Display(Name = nameof(Street4Caption), ResourceType = typeof(NewConnectionEnquiryModel))]
        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(NewConnectionEnquiryModel))]
        //public string Street4 { get; set; }
        //[Display(Name = nameof(Street5Caption), ResourceType = typeof(NewConnectionEnquiryModel))]
        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(NewConnectionEnquiryModel))]
        //public string Street5 { get; set; }
        [Display(Name = nameof(PostalCodeCaption), ResourceType = typeof(NewConnectionEnquiryModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(NewConnectionEnquiryModel))]
        public string PostalCode { get; set; }
        [Display(Name = nameof(CityCaption), ResourceType = typeof(NewConnectionEnquiryModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(NewConnectionEnquiryModel))]
        public string City { get; set; }
        [Display(Name = nameof(Reg_Str_GrpCaption), ResourceType = typeof(NewConnectionEnquiryModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(NewConnectionEnquiryModel))]
        public string Reg_Str_Grp { get; set; }
        //[Display(Name = nameof(CountryCaption), ResourceType = typeof(NewConnectionEnquiryModel))]
        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(NewConnectionEnquiryModel))]
        //public string Country { get; set; }

        //[Display(Name = nameof(EmailCaption), ResourceType = typeof(NewConnectionEnquiryModel))]
        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(NewConnectionEnquiryModel))]
        //[EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(NewConnectionEnquiryModel))]
        //[DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = nameof(MobileCaption), ResourceType = typeof(NewConnectionEnquiryModel))]
        [RegularExpression(@"^(?:(?:\+|0{0,2})91(\s*[\ -]\s*)?|[0]?)?[789]\d{9}|(\d[ -]?){10}\d$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(NewConnectionEnquiryModel))]
        public string Mobile { get; set; }

        public string ReturnViewMessage { get; set; }
        public List<SelectListItem> AreaList { get; set; }
        public List<SelectListItem> CityList { get; set; }
        public List<SelectListItem> PlantCityList { get; set; }
        public List<SelectListItem> PartnerTypeList { get; set; }


        //public static string PlantCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Plant");
        public static string Partner_TypeCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Partner Type");
        public static string NameCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Name");
        //public static string House_NoCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "House No.");
        public static string Street1Caption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Address Line 1");
        public static string Street2Caption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Address Line 2");
        public static string Street3Caption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Address Line 3");
        //public static string Street4Caption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Address Line 4");
        //public static string Street5Caption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Address Line 5");
        public static string PostalCodeCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Postal Code");
        public static string CityCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "City");
        public static string Reg_Str_GrpCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Area");
        //public static string CountryCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Country");
        public static string EmailCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/Email", "Email");
        public static string MobileCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Feeedback/ContactNumber", "Mobile Number");
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Feedback/Required", "Please enter a value for {0}");
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Feedback/Invalid Contact", "Please enter a valid Mobile Number");
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Email Address", "Please enter a valid email address");
    }

}