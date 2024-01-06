using System;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Sitecore.Defence.Website.Models
{
    public class DefenceContactModel
    {
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = "InvalidName", ErrorMessageResourceType = typeof(DefenceContactModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(DefenceContactModel))]
        public string Name
        {
            get;
            set;
        }
        [RegularExpression("^[0-9]{10,10}$", ErrorMessageResourceName = "InvalidMobile", ErrorMessageResourceType = typeof(DefenceContactModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(DefenceContactModel))]
        public string MobileNo
        {
            get;
            set;
        }
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessageResourceType = typeof(DefenceContactModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(DefenceContactModel))]
        public string EmailID
        {
            get;
            set;
        }

        public static string InvalidEmailAddress
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/Defence/Contact-Form/Invalid Email Address", "Please enter a valid email address");
            }
        }

        public static string InvalidMobile
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("Defence/Contact-Form/Invalid Mobile", "Please enter a valid Mobile Number");
            }
        }
        public static string InvalidName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("Defence/Contact-Form/Invalid Name", "Please enter a valid Name");
            }
        }
        public static string Required
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("Defence/Contact-Form/Required", "Please enter value for {0}");
            }
        }

        public string reResponse
        {
            get;
            set;
        }
        public string Message
        {
            get;
            set;
        }
        public string MessageType
        {
            get;
            set;
        }


        public string PageInfo
        {
            get;
            set;
        }
        public DateTime SubmitOnDate
        {
            get;
            set;
        }



        public string FormName
        {
            get;
            set;
        }

        public HttpPostedFileBase ResumeAttachment
        {
            get;
            set;
        }



    }
}