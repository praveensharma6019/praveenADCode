using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Electricity.Website.Model
{
    [Serializable]
    public class Tender
    {
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string TenderId { get; set; }
        public Guid? TenderIdDB { get; set; }
        public string NITNo { get; set; }
        public string TenderName { get; set; }
        public DateTime? AdvDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public string DocumentStatus { get; set; }
        public string CustomerCode { get; set; }
        public string VendorCode { get; set; }
        public bool IsTenderFeePaid { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Tender))]
        [RegularExpression(@"[A-Z]{5}\d{4}[A-Z]{1}", ErrorMessageResourceName = nameof(InvalidPANNumber), ErrorMessageResourceType = typeof(Tender))]
        public string PANNumber { get; set; }
        public HttpPostedFileBase PANDocument { get; set; }
        public bool IsPANUploaded { get; set; }

        public bool IsBuyer { get; set; }
        public bool IsFinancialAdmin { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Tender))]
        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$", ErrorMessageResourceName = nameof(InvalidGSTNumber), ErrorMessageResourceType = typeof(Tender))]
        public string GSTNumber { get; set; }
        public HttpPostedFileBase GSTDocument { get; set; }
        public bool IsGSTUploaded { get; set; }

        
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessageResourceName = nameof(InvalidBankRefName), ErrorMessageResourceType = typeof(Tender))]
        public string TenderFeeBankRef { get; set; }
        public HttpPostedFileBase TenderFeeBankRefDocument { get; set; }

        
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessageResourceName = nameof(InvalidBankBranchName), ErrorMessageResourceType = typeof(Tender))]
        public string TenderFeeBankBranch { get; set; }
        public string TenderFeeModeOfPayment { get; set; }

        
       
        public decimal TenderFeeAmount { get; set; }

        
        public string TenderFeeDateOfPayment { get; set; }
        public bool IsTenderFeeRefUploaded { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Tender))]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessageResourceName = nameof(InvalidBankRefName), ErrorMessageResourceType = typeof(Tender))]
        public string EMDBankRef { get; set; }
        public HttpPostedFileBase EMDBankRefDocument { get; set; }

        public static string InvalidBankRefName => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Login Name", "Please enter a Valid Ref");

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Tender))]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessageResourceName = nameof(InvalidBankBranchName), ErrorMessageResourceType = typeof(Tender))]
        public string EMDBankBranch { get; set; }
        public static string InvalidBankBranchName => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Login Name", "Please enter a Valid Branch name");

        public string EMDModeOfPayment { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Tender))]
        [Range(1, Double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal EMDAmount { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Tender))]
        public string EMDDateOfPayment { get; set; }
        public bool IsEMDRefUploaded { get; set; }

        public List<TenderDocument> tenderdocument { get; set; }
        public List<UserTenderDocument> userUpladdocument { get; set; }

        public HttpPostedFileBase Envelope11 { get; set; }
        public HttpPostedFileBase Envelope12 { get; set; }
        public HttpPostedFileBase Envelope13 { get; set; }
        public HttpPostedFileBase Envelope14 { get; set; }
        public HttpPostedFileBase Envelope15 { get; set; }

        public HttpPostedFileBase Envelope21 { get; set; }
        public HttpPostedFileBase Envelope22 { get; set; }
        public HttpPostedFileBase Envelope23 { get; set; }
        public HttpPostedFileBase Envelope24 { get; set; }
        public HttpPostedFileBase Envelope25 { get; set; }
        public HttpPostedFileBase Envelope26 { get; set; }
        public HttpPostedFileBase Envelope27 { get; set; }
        public HttpPostedFileBase Envelope28 { get; set; }
        public HttpPostedFileBase Envelope29 { get; set; }
        public HttpPostedFileBase Envelope30 { get; set; }
        public HttpPostedFileBase Envelope3 { get; set; }

        public string Business { get; set; }
        public string Category { get; set; }

        //public HttpPostedFileBase Envelope31 { get; set; }
        //public HttpPostedFileBase Envelope32 { get; set; }
        //public HttpPostedFileBase Envelope33 { get; set; }
        //public HttpPostedFileBase Envelope34 { get; set; }
        //public HttpPostedFileBase Envelope35 { get; set; }
        //public HttpPostedFileBase Envelope36 { get; set; }
        //public HttpPostedFileBase Envelope37 { get; set; }
        //public HttpPostedFileBase Envelope38 { get; set; }
        //public HttpPostedFileBase Envelope39 { get; set; }
        //public HttpPostedFileBase Envelope40 { get; set; }

        [Display(Name = nameof(OldPasswordCaption), ResourceType = typeof(Tender))]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = nameof(PasswordCaption), ResourceType = typeof(Tender))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = nameof(ConfirmPasswordCaption), ResourceType = typeof(Tender))]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare(nameof(Password), ErrorMessageResourceName = nameof(ConfirmPasswordMismatch), ErrorMessageResourceType = typeof(Tender))]
        public string ConfirmPassword { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/Required", "Please enter a value for {0}");
        public static string PasswordCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/PasswordCaption", "New Password");
        public static string OldPasswordCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/OldPasswordCaption", "Change Password");
        public static string ConfirmPasswordCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/ConfirmPasswordCaption", "Retype Password");
        public static string ConfirmPasswordMismatch => DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/Confirm Password Mismatch", "Your password confirmation does not match. Please enter a new password.");
        public static string MinimumPasswordLength => DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/Minimum Password Length", "Please enter a password with at lease {1} characters");

        public static string InvalidPANNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid PANNumber", "Please enter a valid PAN Number");
        public static string InvalidGSTNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid GSTNumber", "Please enter a valid GST Number");
    }
    [Serializable]
    public class TenderStatus
    {
        public TenderStatus()
        {
            OpenTender = new List<TenderDetails>();
            CloseTender = new List<TenderDetails>();
            CorrigendumTender = new List<CorrigendumDetails>();
        }
        public List<TenderDetails> OpenTender { get; set; }
        public List<TenderDetails> CloseTender { get; set; }
        public List<CorrigendumDetails> CorrigendumTender { get; set; }
    }
    [Serializable]
    public class CorrigendumDetails
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public List<TenderDetails> NITPRNo { get; set; }
        public List<TenderDetails> TenderDocument { get; set; }

    }

    [Serializable]
    public class TenderDetails
    {
        public Guid Id { get; set; }
        public string NITPRNo { get; set; }
        public string Business { get; set; }
        public string Description { get; set; }
        public DateTime? Adv_Date { get; set; }
        public string Estimated_Cost { get; set; }
        public string Cost_of_EMD { get; set; }
        public string Bid_Submision_ClosingDate { get; set; }
        public string FileName { get; set; }
        public string DocumentPath { get; set; }
        public bool isCorrigendumPresent { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public string Status { get; set; }
        public bool OnHold { get; set; }
        public string Location { get; set; }
        public List<TenderDocument> Tenderdocs { get; set; }

    }
    //public class RegistedUserHasDocument
    //{
    //    public string UserId { get; set; }
    //    public string Name { get; set; }
    //    public string Company { get; set; }
    //    public string Email { get; set; }
    //    public string Mobile { get; set; }
    //    public string HasDocumentUploaded { get; set; }
    //    public string CustomerCode { get; set; }
    //    public string IsTenderFeePaid { get; set; }
    //   public string TenderId { get; set; }
    //    public string Description { get; set; }
    //    public DateTime? DateOfUpload { get; set; }
    //}

    public class TenderBidderDetails
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string CustomerCode { get; set; }
        public string VendorCode { get; set; }
        public string HasDocumentUploaded { get; set; }
        public string IsTenderFeePaid { get; set; }
        public string PANNumber { get; set; }
        public string GSTNumber { get; set; }
        public string TenderFeeBankRef { get; set; }
        public string TenderFeeBankBranch { get; set; }
        public string TenderFeeModeOfPayment { get; set; }
        public string TenderFeeAmount { get; set; }
        public string TenderFeeDateOfPayment { get; set; }
        public string EMDBankRef { get; set; }
        public string EMDBankBranch { get; set; }
        public string EMDModeOfPayment { get; set; }
        public string EMDAmount { get; set; }
        public string EMDDateOfPayment { get; set; }
        public string TenderNITNo { get; set; }
        public string TenderName { get; set; }
        public DateTime? DateOfUpload { get; set; }
        public string TenderId { get; set; }
    }

    [Serializable]
    public class UserDetails
    {
        [Required(ErrorMessage = "Name Field is Required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Company Field is Required.")]
        public string Company { get; set; }

        //[RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Please Enter Valid Number")]
        //public string PhoneNo { get; set; }

        [RegularExpression(@"^((\+)?(\d{2}[-])?(\d{10}){1})?(\d{11}){0,1}?$", ErrorMessage = "Please Enter Valid Mobile Number.")]
        [Required(ErrorMessage = "Mobile Number is Required.")]
        public string MobileNo { get; set; }

        //[Required(ErrorMessage = "Fax Number is Required.")]
        [RegularExpression(@"^[0-9]{12,12}$", ErrorMessage = "Please Enter Valid Fax Number.")]
        public string FaxNo { get; set; }

        [Required(ErrorMessage = "Email is Required.")]
        [EmailAddress(ErrorMessage = "Please Enter Valid EmailAddress.")]
        public string Email { get; set; }

        public string UserType { get; set; }
        public Guid TenderID { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

    }

    public class EnvelopUserDetails
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name Field Cannot be Blank.")]
        public string Name { get; set; }
        public string Company { get; set; }

        [Required(ErrorMessage = "Please Enter Valid EmailAddress.")]
        [EmailAddress(ErrorMessage = "Please Enter Valid EmailAddress.")]
        public string Email { get; set; }

        [RegularExpression(@"^((\+)?(\d{2}[-])?(\d{10}){1})?(\d{11}){0,1}?$", ErrorMessage = "Please Enter Valid Mobile Number.")]
        [Required(ErrorMessage = "Please Enter Valid Mobile Number.")]
        public string MobileNo { get; set; }
        public string UserType { get; set; }
        public string SelectTenderId { get; set; }
        public List<SelectListItem> TenderList { get; set; }
        public string FaxNo { get; set; }
        public List<EnvelopName> EnvelopNameCheckboxs { get; set; }
        public string UserId { get; set; }
        public string TenderNumber { get; set; }

        public string EnvelopRight { get; set; }
        public bool IsBuyer { get; set; }
        public string BuyerType { get; set; }
    }

    public class EnvelopName
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsChecked { get; set; }
    }

    public class TenderUserDetailsForMail
    {
        public string TenderName { get; set; }
        public string TenderId { get; set; }
        public string TenderNIT { get; set; }
        public List<UserTenderMapping> TenderUsers { get; set; }
    }
}