using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Sitecore.InspireBkc.Website.Models
{
  public class LeadGenerationModel
{
	[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	public string FirstName { get; set; }

	[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	public string LastName { get; set; }

	[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	public string SelectedLeadRecordType { get; set; }

	public List<SelectListItem> LeadRecordType { get; set; }

	[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	public string SelectedAssignmentCity { get; set; }

	public List<SelectListItem> AssignmentCity { get; set; }

	[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	public string SelectedLeadSource { get; set; }

	public List<SelectListItem> LeadSource { get; set; }

	public string LeadSubSource { get; set; }

	[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	public string SelectedLeadStatus { get; set; }

	public List<SelectListItem> LeadStatus { get; set; }

	[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	public string SelectedLeadSubStatus { get; set; }

	public List<SelectListItem> LeadSubStatus { get; set; }

	[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	public string SelectedGender { get; set; }

	public List<SelectListItem> LeadGender { get; set; }

	[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	[EmailAddress(ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	public string Email { get; set; }

	[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	[DataType(DataType.PhoneNumber)]
	[RegularExpression("^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessageResourceName = "InvalidMobile", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	public string Mobile { get; set; }

	[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	public string Budget { get; set; }

	[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	public string Profession { get; set; }

	[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	public string SelectedReasonforPurchase { get; set; }

	public List<SelectListItem> ReasonforPurchase { get; set; }

	[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	public string SelectedPropertyStage { get; set; }

	public List<SelectListItem> PropertyStage { get; set; }

	[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	public string Configuration { get; set; }

	[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	public string SelectedProjectName { get; set; }

	public List<SelectListItem> ProjectList { get; set; }

	[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	public string CountryCode { get; set; }

	public List<SelectListItem> CountryList { get; set; }

	[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	public string StateCode { get; set; }

	public List<SelectListItem> StateList { get; set; }

	[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	public string City { get; set; }

	public string InboundCall { get; set; }

	[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	public string PreSalesAgent { get; set; }

	public string LeadLostReasons { get; set; }

	public string ScheduledFollowUp { get; set; }

	[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(LeadGenerationModel))]
	public string Remarks { get; set; }

	public string Projects_Interested__c { get; set; }

	public string PropertyLocation { get; set; }

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
