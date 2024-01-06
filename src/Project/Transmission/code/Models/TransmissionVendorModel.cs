using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Transmission.Website.Helper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Sitecore.Transmission.Website.Models
{
    public class TransmissionVendorModel
    {
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessageResourceType = typeof(TransmissionVendorModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(TransmissionVendorModel))]
        public string EmailID
        {
            get;
            set;
        }
        public string MessageType
        {
            get;
            set;
        }
        public string RegistrationNo
        {
            get;
            set;
        }
        public string CurrentStatus
        {
            get;
            set;
        }
        public string FormName
        {
            get;
            set;
        }

        public Guid Id
        {
            get;
            set;
        }

        public static string InvalidEmailAddress
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/Transmission/Contact-Form/Invalid Email Address", "Please enter a valid email address");
            }
        }

        public static string InvalidMobile
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("Transmission/Contact-Form/Invalid Mobile", "Please enter a valid Mobile Number");
            }
        }

        public static string InvalidName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("Transmission/Contact-Form/Invalid Name", "Please enter a valid Name");
            }
        }
        public static string InvalidCompanyName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("Transmission/Contact-Form/Invalid Company Name", "Please enter a valid Company Name");
            }
        }

        public string Message
        {
            get;
            set;
        }

     

        [RegularExpression("^[0-9]{10,10}$", ErrorMessageResourceName = "InvalidMobile", ErrorMessageResourceType = typeof(TransmissionVendorModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(TransmissionVendorModel))]
        public string MobileNo
        {
            get;
            set;
        }

        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = "InvalidName", ErrorMessageResourceType = typeof(TransmissionVendorModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(TransmissionVendorModel))]
        public string Name
        {
            get;
            set;
        }
        //[RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = "InvalidCompanyName", ErrorMessageResourceType = typeof(TransmissionVendorModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(TransmissionVendorModel))]
        public string CompanyName
        {
            get;
            set;
        }

        public string PageInfo
        {
            get;
            set;
        }

        
        public static string Required
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("Transmission/Contact-Form/Required", "Please enter value for {0}");
            }
        }

        public string reResponse
        {
            get;
            set;
        }

        public DateTime SubmitOnDate
        {
            get;
            set;
        }

        public string SubmittedBy
        {
            get;
            set;
        }

        public TransmissionVendorModel()
        {
            TransmissionVendorEnqiryHelper transmissionvendorenqiryhelper = new TransmissionVendorEnqiryHelper();
        }
    }
}