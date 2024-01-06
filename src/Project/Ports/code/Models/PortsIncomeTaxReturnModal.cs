using Newtonsoft.Json;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Ports.Website.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Ports.Website.Models
{
    public class PortsIncomeTaxReturnModal
    {
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = "InvalidVendorName", ErrorMessageResourceType = typeof(PortsIncomeTaxReturnModal))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(PortsIncomeTaxReturnModal))]
        public string VendorName { set; get; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(PortsIncomeTaxReturnModal))]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceName = "InvalidVendorNumber", ErrorMessageResourceType = typeof(PortsIncomeTaxReturnModal))]
        public string VendorNumber
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(PortsIncomeTaxReturnModal))]
        [RegularExpression(@"^(\d{12}|\d{16})$", ErrorMessageResourceName = "InvalidAadharNumber", ErrorMessageResourceType = typeof(PortsIncomeTaxReturnModal))]
        public string AadharCardNumber
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(PortsIncomeTaxReturnModal))]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}", ErrorMessageResourceName = "InvalidPAN", ErrorMessageResourceType = typeof(PortsIncomeTaxReturnModal))]
        public string PanNo
        {
            get;
            set;
        }
        
        public string IsPanLinkedWithAadhar { get; set; }
        public List<string> IsPanLinkedWithAadharList { get; set; }
        public HttpPostedFileBase PanWithAadharAttachment
        {
            get;
            set;
        }
        public string   PanAadharLink
        {
            get;
            set;
        }
        public Guid Id
        {
            get;
            set;
        }
        public string reResponse
        {
            get;
            set;
        }
        public string IsFilledItReturnFilling { get; set; }
        public List<string> IsFilledItReturnFillingList { get; set; }
        public HttpPostedFileBase ITReturnFillingAttachment
        {
            get;
            set;
        }
        public string FormType { set; get; }
        public static string InvalidVendorName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("Ports/ITDeclaration/Invalid Vendor Name", "Please enter a valid Name");
            }
        }
        public static string InvalidAadharNumber
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("Ports/ITDeclaration/Invalid Aadhar Number", "Please enter a valid Number");
            }
        }
        public static string InvalidPAN
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("Ports/ITDeclaration/Invalid Pan No", "Please enter a valid PAN No");
            }
        }
        public static string InvalidVendorNumber
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("Ports/ITDeclaration/Invalid Vendor Number", "Please enter a valid Number");
            }
        }
        public static string Required
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("Ports/ITDeclaration/Required", "Please enter value for {0}");
            }
        }
        public DateTime FormSubmitOn { set; get; }
        public PortsIncomeTaxReturnModal()
        {
            PortsIncomeTaxReturnHelper helper = new PortsIncomeTaxReturnHelper();
            //MSMEList = new List<string>();
            IsPanLinkedWithAadharList = helper.GetIsPanLinkedWithAadharList();
            IsFilledItReturnFillingList = helper.GetIsFilledItReturnFillingListt();
        }
       
    }
}