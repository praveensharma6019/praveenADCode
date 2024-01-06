using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniCapital.Website.Models
{
    public class RequestCallbackModel
    {
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = "InvalidName", ErrorMessageResourceType = typeof(FeedbackFormModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(FeedbackFormModel))]
        public string Name
        {
            get;
            set;
        }
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessageResourceType = typeof(FeedbackFormModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(FeedbackFormModel))]
        public string EmailID
        {
            get;
            set;
        }
        [RegularExpression("^[0-9]{10,10}$", ErrorMessageResourceName = "InvalidMobile", ErrorMessageResourceType = typeof(FeedbackFormModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(FeedbackFormModel))]
        public string MobileNo
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
        public string Message
        {
            get;
            set;
        }
        public Guid Id
        {
            get;
            set;
        }
        public bool SavedinDB
        {
            get;
            set;
        }
        public string TicketID
        {
            get;
            set;
        }
        public bool IsSubmittedToFreshdesk
        {
            get;
            set;
        }
        public static string InvalidEmailAddress
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniCapital/FeedbackForm/Invalid Email Address", "Please enter a valid email address");
            }
        }
        public static string InvalidMobile
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniCapital/FeedbackForm/Invalid Mobile", "Please enter a valid Mobile Number");
            }
        }
        public static string InvalidName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniCapital/FeedbackForm/Invalid Name", "Please enter a valid Name");
            }
        }
        public static string Required
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniCapital/FeedbackForm/Required", "Please enter value for {0}");
            }
        }
        public string reResponse
        {
            get;
            set;
        }
    }
}