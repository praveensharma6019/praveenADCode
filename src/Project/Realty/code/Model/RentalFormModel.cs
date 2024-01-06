
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Realty.Website.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Realty.Website.Model
{
    [Serializable]
    public class RentalFormModel
    {
        public RentalFormModel()
        {
            RentalFormHelper helper = new RentalFormHelper();
            allApartmentTypeList = helper.GetallApartmentTypeList();
            allApartmentSizeList = helper.GetallApartmentSizeList();
        }
        [Display(Name = nameof(NameCaption), ResourceType = typeof(RentalFormModel))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(RentalFormModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RentalFormModel))]
        public string Name { get; set; }

        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(RentalFormModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RentalFormModel))]
        public string Occupation { get; set; }

        [Display(Name = nameof(MobileCaption), ResourceType = typeof(RentalFormModel))]
        [RegularExpression(@"^(?:(?:\+|0{0,2})91(\s*[\ -]\s*)?|[0]?)?[789]\d{9}|(\d[ -]?){10}\d$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(RentalFormModel))]
        public string ContactNo { get; set; }

        [Display(Name = nameof(EmailCaption), ResourceType = typeof(RentalFormModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RentalFormModel))]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(RentalFormModel))]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(RentalFormModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RentalFormModel))]
        public string Location { get; set; }

       

        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(RentalFormModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RentalFormModel))]
        public string Budget { get; set; }

        [Display(Name = nameof(BrokerNameCaption), ResourceType = typeof(RentalFormModel))]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9. -]*$", ErrorMessageResourceName = nameof(EnterValidValue), ErrorMessageResourceType = typeof(RentalFormModel))]
        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RentalFormModel))]
        public string BrokerName { get; set; }

        [Display(Name = nameof(InvalidMobile), ResourceType = typeof(RentalFormModel))]
        [RegularExpression(@"^(?:(?:\+|0{0,2})91(\s*[\ -]\s*)?|[0]?)?[789]\d{9}|(\d[ -]?){10}\d$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(RentalFormModel))]
        public string BrokerContactNo { get; set; }
        public DateTime SubmittedOn { get; set; }
        public string PageInfo { get; set; }
        public string FormName { get; set; }
        public string ResponseURL { get; set; }
        public string reResponse { get; set; }
        public string ReturnViewMessage { get; set; }
        
        public string ApartmentTypeList
        {
            get;
            set;
        }
      
        public string ApartmentSizeList
        {
            get;
            set;
        }
        public string Remarks
        {
            get;
            set;
        }
        public string ReferenceFrom
        {
            get;
            set;
        }
        public IEnumerable<SelectListItem> allApartmentTypeList { get; set; }
        public IEnumerable<SelectListItem> allApartmentSizeList { get; set; }

        public string[] selectedApartmentType { get; set; }
        public string[] selectedApartmentSize { get; set; }
      
        public static string NameCaption => DictionaryPhraseRepository.Current.Get("/Controller/RentalForm/Name", "Name");
        public static string EmailCaption => DictionaryPhraseRepository.Current.Get("/Controller/RentalForm/Email", "Email");
        public static string MobileCaption => DictionaryPhraseRepository.Current.Get("/Controller/RentalForm/ContactNumber", "Contact Number");
        public static string BrokerNameCaption => DictionaryPhraseRepository.Current.Get("/Controller/RentalForm/BrokerName", "Broker Name");
        public static string ApartmentTypeCaption => DictionaryPhraseRepository.Current.Get("/Controller/RentalForm/ApartmentType", "Apartment Type");
        public static string ApartmentSizeCaption => DictionaryPhraseRepository.Current.Get("/Controller/RentalForm/ApartmentSize", "Apartment Size");
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Controller/RentalForm/Invalid Contact", "Please enter a valid Contact Number");
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Controller/RentalForm/Invalid Email Address", "Please enter a valid email address");
        public static string EnterValidValue => DictionaryPhraseRepository.Current.Get("/Controller/RentalForm/InvalidValue", "Invalid value for {0}, Only alphanumeric, space and . - charachters allowed");
        public static string Required => DictionaryPhraseRepository.Current.Get("/Controller/RentalForm/Required", "Please enter a value for {0}");
        public static string DropdownRequired => DictionaryPhraseRepository.Current.Get("/Controller/RentalForm/Required", "Please select a value for {0}");
        
    }
    
}