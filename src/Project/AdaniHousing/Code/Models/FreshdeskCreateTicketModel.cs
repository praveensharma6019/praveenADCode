using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.AdaniHousing.Website.Models
{
    public class FreshdeskCreateTicketModel
    {
        public FreshdeskCreateTicketModel()
        {
            SubjectList = new List<SelectListItem>();
            LoanAccountNoList = new List<TextValueListItem>();
        }
        [RegularExpression("^[a-zA-Z0-9. ]*$", ErrorMessageResourceName = "InvalidName", ErrorMessageResourceType = typeof(FreshdeskCreateTicketModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(FreshdeskCreateTicketModel))]
        public string Name
        {
            get;
            set;
        }
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessageResourceType = typeof(FreshdeskCreateTicketModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(FreshdeskCreateTicketModel))]
        public string EmailID
        {
            get;
            set;
        }
        [RegularExpression("^[0-9]{10,10}$", ErrorMessageResourceName = "InvalidMobile", ErrorMessageResourceType = typeof(FreshdeskCreateTicketModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(FreshdeskCreateTicketModel))]
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
        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessageResourceName = "InvalidLAN", ErrorMessageResourceType = typeof(FreshdeskCreateTicketModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(FreshdeskCreateTicketModel))]
        public string LoanAccountNo
        {
            get;
            set;
        }
        public List<TextValueListItem> LoanAccountNoList
        {
            get;
            set;
        }
        [RegularExpression("^[a-zA-Z0-9. ]*$", ErrorMessageResourceName = "InvalidSubject", ErrorMessageResourceType = typeof(FreshdeskCreateTicketModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(FreshdeskCreateTicketModel))]
        public string Subject
        {
            get;
            set;
        }
        public List<SelectListItem> SubjectList
        {
            get;
            set;
        }
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Invalid Branch", ErrorMessageResourceType = typeof(FreshdeskCreateTicketModel))]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(FreshdeskCreateTicketModel))]
        public string BranchDetail
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
                return DictionaryPhraseRepository.Current.Get("/AdaniHousing/AfterLoginSupportForm/Invalid Email Address", "Please enter a valid email address");
            }
        }
        public static string InvalidMobile
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniHousing/AfterLoginSupportForm/Invalid Mobile", "Please enter a valid Mobile Number");
            }
        }
        public static string InvalidName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniHousing/AfterLoginSupportForm/Invalid Name", "Please enter a valid Name");
            }
        }
        public static string InvalidLAN
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniHousing/AfterLoginSupportForm/Invalid LAN", "Please enter a valid LoanAccountNumber");
            }
        }
        public static string InvalidSubject
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniHousing/AfterLoginSupportForm/Invalid Subject", "Please select a Subject");
            }
        }
        public static string Required
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniHousing/AfterLoginSupportForm/Required", "Please enter value for {0}");
            }
        }
        public string reResponse
        {
            get;
            set;
        }
    }
}