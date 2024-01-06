using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Foundation.Dictionary.Repositories;

namespace Sitecore.AdaniCapital.Website.Models
{
    public class BecomeAPartnerModel
    {
        public BecomeAPartnerModel()
        {
        }
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = "InvalidName", ErrorMessageResourceType = typeof(BecomeAPartnerModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(BecomeAPartnerModel))]
        public string Name
        {
            get;
            set;
        }
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessageResourceType = typeof(BecomeAPartnerModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(BecomeAPartnerModel))]
        public string EmailID
        {
            get;
            set;
        }
        [RegularExpression("^[0-9]{10,10}$", ErrorMessageResourceName = "InvalidMobile", ErrorMessageResourceType = typeof(BecomeAPartnerModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(BecomeAPartnerModel))]
        public string MobileNo
        {
            get;
            set;
        }
        [RegularExpression("^[1-9]{1}[0-9]{2}\\s{0,1}[0-9]{3}$", ErrorMessageResourceName = "InvalidPincode", ErrorMessageResourceType = typeof(BecomeAPartnerModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(BecomeAPartnerModel))]
        public string Pincode
        {
            get;
            set;
        }
        public string Subject
        {
            get;
            set;
        }
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
                return DictionaryPhraseRepository.Current.Get("/AdaniCapital/BecomeAPartner/Invalid Email Address", "Please enter a valid email address");
            }
        }
        public static string InvalidMobile
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniCapital/BecomeAPartner/Invalid Mobile", "Please enter a valid Mobile Number");
            }
        }
        public static string InvalidPincode
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniHousing/BecomeAPartner/Invalid Pincode", "Please enter a valid Pincode");
            }
        }
        public static string InvalidName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniCapital/BecomeAPartner/Invalid Name", "Please enter a valid Name");
            }
        }
        public static string Required
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniCapital/BecomeAPartner/Required", "Please enter value for {0}");
            }
        }
        public string reResponse
        {
            get;
            set;
        }
    }
}