using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.AdaniHousing.Website.Models
{
    public class AdaniHousingContactUsModal
    {
        public AdaniHousingContactUsModal()
        {
        }
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = "InvalidName", ErrorMessageResourceType = typeof(AdaniHousingContactUsModal))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdaniHousingContactUsModal))]
        public string Name
        {
            get;
            set;
        }
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessageResourceType = typeof(AdaniHousingContactUsModal))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdaniHousingContactUsModal))]
        public string EmailID
        {
            get;
            set;
        }
        [RegularExpression("^[0-9]{10,10}$", ErrorMessageResourceName = "InvalidMobile", ErrorMessageResourceType = typeof(AdaniHousingContactUsModal))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdaniHousingContactUsModal))]
        public string MobileNo
        {
            get;
            set;
        }
        public string Subject
        {
            get;
            set;
        }
        public List<SelectListItem> SubjectList { get; set; }
        public string Message
        {
            get;
            set;
        }
        public string TicketID
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
        public bool IsSubmittedToFreshdesk
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
                return DictionaryPhraseRepository.Current.Get("/AdaniHousing/ContactUs/Invalid Email Address", "Please enter a valid email address");
            }
        }
        public static string InvalidMobile
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniHousing/ContactUs/Invalid Mobile", "Please enter a valid Mobile Number");
            }
        }
        public static string InvalidName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniHousing/ContactUs/Invalid Name", "Please enter a valid Name");
            }
        }
        public static string Required
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniHousing/ContactUs/Required", "Please enter value for {0}");
            }
        }
        public string reResponse
        {
            get;
            set;
        }
    }
}