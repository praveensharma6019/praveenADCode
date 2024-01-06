using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.BelvedereClubGurgaon.Website.Models
{
    [Serializable]
    public class UserSession
    {
        public static LoginModel UserSessionContext
        {
            get
            {
                return (LoginModel)HttpContext.Current.Session["UserLogin"];
            }
            set
            {
                HttpContext.Current.Session["UserLogin"] = value;
            }
        }
    }
    public class ClubGurgaonEnquiryModel
    {
        public string Name { set; get; }
        public string Mobile { set; get; }
        public string Email { set; get; }
        public string InterestedIn { set; get; }
        public string City { set; get; }
        public string Message { set; get; }
        public string FormType { set; get; }
        public DateTime FormSubmitOn { set; get; }
        public string OTP { get; set; }
    }

    public class LeadGenerationModel
    {
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(LeadGenerationModel))]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(LeadGenerationModel))]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(LeadGenerationModel))]
        public string SelectedLeadSource { get; set; }
        public List<SelectListItem> LeadSource { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(LeadGenerationModel))]
        public string SelectedLeadStatus { get; set; }
        public List<SelectListItem> LeadStatus { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(LeadGenerationModel))]
        public string SelectedGender { get; set; }
        public List<SelectListItem> Gender { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(LeadGenerationModel))]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(LeadGenerationModel))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(LeadGenerationModel))]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(LeadGenerationModel))]
        public string Mobile { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(LeadGenerationModel))]
        public string Profession { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(LeadGenerationModel))]
        public string InterestedIn { get; set; }
        public List<SelectListItem> InterestedInList { get; set; }

       // [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(LeadGenerationModel))]
        public string Membership { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(LeadGenerationModel))]
        public string Age { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(LeadGenerationModel))]
        public string Lead_Owner { get; set; }
        public List<SelectListItem> Lead_OwnersList { get; set; }
        

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(LeadGenerationModel))]
        public string City { get; set; }

        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(LeadGenerationModel))]
        public string Remarks { get; set; }

        public string Projects_Interested__c { get; set; }
        public string PropertyLocation { set; get; }
        public string sale_type { get; set; }
        public string Captcha { get; set; }
        public string OTP { get; set; }
        public string FormType { get; set; }
        public string PageInfo { get; set; }
        public string UTMSource { get; set; }
        public DateTime FormSubmitOn { get; set; }
        public string ReturnViewMessage { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Feedback/Required", "This field is required");
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Feedback/Invalid Contact", "Please enter a valid Mobile Number");
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Email Address", "Please enter a valid email address");
    }
}