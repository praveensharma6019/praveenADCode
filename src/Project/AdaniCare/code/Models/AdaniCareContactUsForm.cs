using Sitecore.Foundation.Dictionary.Repositories;

using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Sitecore.AdaniCare.Website.Models
{

    public class AdaniCareContactUsForm
    {
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessageResourceType = typeof(AdaniCareContactUsForm))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdaniCareContactUsForm))]
        public string EmailID
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
                return DictionaryPhraseRepository.Current.Get("/AdaniCare/Contact-Form/Invalid Email Address", "Please enter a valid email address");
            }
        }

        public static string InvalidMobile
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("AdaniCare/Contact-Form/Invalid Mobile", "Please enter a valid Mobile Number");
            }
        }

        public static string InvalidName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("AdaniCare/Contact-Form/Invalid Name", "Please enter a valid Name");
            }
        }

        public string Query
        {
            get;
            set;
        }


        [RegularExpression("^[0-9]{10,10}$", ErrorMessageResourceName = "InvalidMobile", ErrorMessageResourceType = typeof(AdaniCareContactUsForm))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdaniCareContactUsForm))]
        public string MobileNo
        {
            get;
            set;
        }

        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = "InvalidName", ErrorMessageResourceType = typeof(AdaniCareContactUsForm))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdaniCareContactUsForm))]
        public string Name
        {
            get;
            set;
        }

        public string PageInfo
        {
            get;
            set;
        }

        public string reResponse
        {
            get;
            set;
        }

        public static string Required
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("AdaniCare/Contact-Form/Required", "Please enter value for {0}");
            }
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

       
    }
}