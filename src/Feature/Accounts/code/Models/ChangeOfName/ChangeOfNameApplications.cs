namespace Sitecore.Feature.Accounts.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;
    using System.Web;

    [Serializable]
    public class ChangeOfNameApplications
    {
        public string AccountNumber { get; set; }
        public string ApplicationRegistrationNumber { get; set; }
        public string AppliedDate { get; set; }
        public string DateOfSubmission { get; set; }
        public string ApplicationStatus { get; set; }
        public string EmployeeRemarks { get; set; }
        public string TempRegistrationNumber { get; set; }
        

        public List<CONApplicationDocumentDetail> GetExistingDocuments { get; set; }
        public List<DocumentCheckList> IDDocumentsList { get; set; }
        public List<DocumentCheckList> IDDocumentsListOnly1 { get; set; }
        public string SelectedIDDocumentOnly1 { get; set; }

        public List<DocumentCheckList> ID2DocumentsList { get; set; }
        public List<DocumentCheckList> ID2DocumentsListOnly1 { get; set; }
        public string SelectedID2DocumentOnly1 { get; set; }


        public List<DocumentCheckList> OTDocumentsList { get; set; }
        public List<DocumentCheckList> OTDocumentsListOnly1 { get; set; }
        public string SelectedOTDocumentOnly1 { get; set; }

        public List<DocumentCheckList> ODDocumentsList { get; set; }
        public List<DocumentCheckList> ODDocumentsListOnly1 { get; set; }
        public string SelectedODDocumentOnly1 { get; set; }

        public List<DocumentCheckList> PHDocumentsList { get; set; }
        public List<DocumentCheckList> PHDocumentsListOnly1 { get; set; }
        public string SelectedPHDocumentOnly1 { get; set; }

        public List<DocumentCheckList> SDDocumentsList { get; set; }
        public List<DocumentCheckList> SDDocumentsListOnly1 { get; set; }
        public string SelectedSDDocumentOnly1 { get; set; }

        public string ConsumerName { get; set; }

        public static string InvalidName => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Login Name", "Name contains alphbets only");
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        public string FirstName { get; set; }

        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        public string MiddleName { get; set; }

        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        public string LastName { get; set; }

        public static string InvalidInput => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Login Name", "Input is not valid.");
        [RegularExpression("^[a-zA-Z]([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        public string OrganizationName { get; set; }

        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        public string Name1Joint { get; set; }

        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        public string Name2Joint { get; set; }

        public string ExistingMobileNumber { get; set; }

        public string LECMobileNumber { get; set; }
        public string LECRegistrationNumber { get; set; }
        public bool IsLEC { get; set; }

        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a valid Mobile Number");
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        public string MobileNo { get; set; }

        public static string InvalidLandline => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Landline Number", "Please enter a valid Landline Number");
        [RegularExpression(@"^\d{6,9}$", ErrorMessageResourceName = nameof(InvalidLandline), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        public string Landline { get; set; }
        public string ApplicantType { get; set; }

        public string ExistingEmailId { get; set; }
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Email Address", "Please enter a valid email address");

        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        public string MeterNumber { get; set; }
        public string ConnectionType { get; set; }

        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        public string HouseNumber { get; set; }

        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        public string Street { get; set; }

        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        public string Area { get; set; }

        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        public string Landmark { get; set; }

        public string City { get; set; }

        public string Pincode { get; set; }

        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        public string BuildingName { get; set; }

        public string OldHouseNumber { get; set; }
        public string OldStreet { get; set; }
        public string OldArea { get; set; }
        public string OldLandmark { get; set; }
        public string OldCity { get; set; }
        public string OldPincode { get; set; }
        public string OldBuildingName { get; set; }
        public string OldSuburb { get; set; }

        public string SelectedSuburb { get; set; }
        public List<SelectListItem> SuburbSelectList { get; set; }

        public string SelectedCity { get; set; }
        public List<SelectListItem> CitySelectList { get; set; }

        public string SelectedPincode { get; set; }
        public List<SelectListItem> PincodeSelectList { get; set; }

        public string SelectedBillLanguage { get; set; }
        public List<SelectListItem> BillLanguageSelectList { get; set; }

        //For billing - if different
        public string BillingHouseNumber { get; set; }
        public string BillingStreet { get; set; }
        public string BillingArea { get; set; }
        public string BillingLandmark { get; set; }
        public string BillingCity { get; set; }
        public string BillingPincode { get; set; }
        public string BillingBuildingName { get; set; }

        public string BillingSelectedSuburb { get; set; }
        public List<SelectListItem> BillingSuburbSelectList { get; set; }

        public string BillingSelectedCity { get; set; }
        public List<SelectListItem> BillingCitySelectList { get; set; }

        public string BillingSelectedPincode { get; set; }
        public List<SelectListItem> BillingPincodeSelectList { get; set; }

        public string BillingSelectedBillLanguage { get; set; }
        public List<SelectListItem> BillingBillLanguageSelectList { get; set; }

        public string Address { get; set; }
        public string Vertrag_Contract { get; set; }

        public string IsStillLiving { get; set; }

        public string IsRentalProperty { get; set; }

        public string IsAddressCorrectionRequired { get; set; }

        public string IsBillingAddressDifferent { get; set; }

        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        public string LandlordName { get; set; }
        public string LandlordMobile { get; set; }

        [RegularExpression(@"^\d{6,9}$", ErrorMessageResourceName = nameof(InvalidLandline), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        public string LandlordLandline { get; set; }

        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        [DataType(DataType.EmailAddress)]
        public string LandlordEmail { get; set; }

        public string IsContinueWithExistingSD { get; set; }
        public decimal SecurityDepositeAmount { get; set; }
        public decimal ExistingSecurityDepositeAmount { get; set; }
        public string IsPaperlessBilling { get; set; }

        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        public string ConsumerRemark { get; set; }

        public string SelectedPremiseType { get; set; }
        public List<SelectListItem> PremiseTypeSelectList { get; set; }
        public string SelectedTitle { get; set; }
        public List<SelectListItem> TitleSelectList { get; set; }
        public string docnumber_ID { get; set; }
        public string docnumber_ID2 { get; set; }




        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter {0}");
    }

}