using System.ComponentModel.DataAnnotations;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Web.Mvc;
using System.Collections.Generic;
namespace Sitecore.Feature.Accounts.Models
{
    public class CustomerFeedbackAdaniGasModel
    {
        [Display(Name = nameof(CustomerIDCaption), ResourceType = typeof(CustomerFeedbackAdaniGasModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(CustomerFeedbackAdaniGasModel))]
        [RegularExpression(@"^[0-9]{8,10}$", ErrorMessageResourceName = nameof(InvalidCustomerIDNumber), ErrorMessageResourceType = typeof(CustomerFeedbackAdaniGasModel))]
        public string CustomerID { get; set; }

        [Display(Name = nameof(CustomerNameCaption), ResourceType = typeof(CustomerFeedbackAdaniGasModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(CustomerFeedbackAdaniGasModel))]
        public string CustomerName { get; set; }

        [Display(Name = nameof(CustomerNameCaption), ResourceType = typeof(CustomerFeedbackAdaniGasModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(CustomerFeedbackAdaniGasModel))]
        public string NewCustomerName { get; set; }

        [Display(Name = nameof(AddressCaption), ResourceType = typeof(CustomerFeedbackAdaniGasModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(CustomerFeedbackAdaniGasModel))]
        public string Address { get; set; }

        [Display(Name = nameof(CityCaption), ResourceType = typeof(CustomerFeedbackAdaniGasModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(CustomerFeedbackAdaniGasModel))]
        public string City { get; set; }

        [Display(Name = nameof(PincodeCaption), ResourceType = typeof(CustomerFeedbackAdaniGasModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(CustomerFeedbackAdaniGasModel))]
        public string Pincode { get; set; }

        [Display(Name = nameof(ContactNumberCaption), ResourceType = typeof(CustomerFeedbackAdaniGasModel))]
        [RegularExpression(@"^(?:(?:\+|0{0,2})91(\s*[\ -]\s*)?|[0]?)?[789]\d{9}|(\d[ -]?){10}\d$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(CustomerFeedbackAdaniGasModel))]
        public string ContactNumber { get; set; }
        public string Response_CQR { get; set; }
        public string Rep_Performance { get; set; }
        public string Del_Performance { get; set; }
        public string Pricing { get; set; }
        public string Handl_Cust_Comp { get; set; }
        public string Overall_Performance { get; set; }
        public string Comments { get; set; }
        public string ReturnViewMessage { get; set; }
        public string Captcha { get; set; }

        public static string CustomerIDCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Feedback/Customer ID", "Customer ID.");
        public static string ComplaintNumberCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Feedback/Complaint Number", "Complaint Number");
        public static string CustomerNameCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Feedback/CustomerName", "Customer Name");
        public static string AddressCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Feedback/Address", "Address");
        public static string CityCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Feeedback/City", "City");
        public static string PincodeCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Feeedback/Pincode", "Pincode");
        public static string ContactNumberCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Feeedback/ContactNumber", "Contact Number");

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Feedback/Required", "Please enter a value for {0}");
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Feedback/Invalid Contact", "Please enter a valid Contact Number");
        public static string InvalidCustomerIDNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Account", "Please enter a valid Account Number");
        public static string InvalidComplaintNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Complaint Number", "Please enter a valid Complaint Number");


    }

    public class CustomerFeedbackMainAdaniGasModel
    {
        [Display(Name = nameof(CustomerIDCaption), ResourceType = typeof(CustomerFeedbackAdaniGasModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(CustomerFeedbackAdaniGasModel))]
        [RegularExpression(@"^[0-9]{8,10}$", ErrorMessageResourceName = nameof(InvalidCustomerIDNumber), ErrorMessageResourceType = typeof(CustomerFeedbackAdaniGasModel))]
        public string CustomerID { get; set; }

        [Display(Name = nameof(ComplaintNumberCaption), ResourceType = typeof(CustomerFeedbackAdaniGasModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(CustomerFeedbackAdaniGasModel))]
        //[RegularExpression(@"^R[0-9]{8,10}$", ErrorMessageResourceName = nameof(InvalidComplaintNumber), ErrorMessageResourceType = typeof(CustomerFeedbackAdaniGasModel))]
        public string ComplaintNumber { get; set; }

        public string Response_CQR { get; set; }
        public string Rep_Performance { get; set; }
        public string Del_Performance { get; set; }
        public string Pricing { get; set; }
        public string Handl_Cust_Comp { get; set; }
        public string Overall_Performance { get; set; }
        //public string Comments { get; set; }
        public string ReturnViewMessage { get; set; }
        public string Captcha { get; set; }

        public static string CustomerIDCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Feedback/Customer ID", "Customer ID.");
        public static string ComplaintNumberCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Feedback/Complaint Number", "Complaint Number");
        public static string CustomerNameCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Feedback/CustomerName", "Customer Name");
        public static string AddressCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Feedback/Address", "Address");
        public static string CityCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Feeedback/City", "City");
        public static string PincodeCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Feeedback/Pincode", "Pincode");
        public static string ContactNumberCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Feeedback/ContactNumber", "Contact Number");

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Feedback/Required", "Please enter a value for {0}");
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Feedback/Invalid Contact", "Please enter a valid Contact Number");
        public static string InvalidCustomerIDNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Account", "Please enter a valid Account Number");
        public static string InvalidComplaintNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Complaint Number", "Please enter a valid Complaint Number");


    }
}