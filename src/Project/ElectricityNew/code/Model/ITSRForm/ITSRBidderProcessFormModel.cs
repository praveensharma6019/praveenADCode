namespace Sitecore.ElectricityNew.Website.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using Sitecore.Foundation.Dictionary.Repositories;

    [Serializable]
    public class ITSRBidderProcessFormSessionModel
    {
        public bool IsExists { get; set; }
        public string FormId { get; set; }
        public bool IsOTPSent { get; set; }
        public string OTPNumber { get; set; }
        public bool IsOTPValid { get; set; }

        public string CompanyName { get; set; }
        public string Location { get; set; }
        public string SPOCPersonName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }

        public string Title { get; set; }
        public string TenderNo { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public string ProposalOwnerName { get; set; }
        public string ProposalOwnerEmailId { get; set; }

        public string BuyerOwnerName { get; set; }
        public string BuyerOwnerEmailId { get; set; }
    }

    [Serializable]
    public class ITSRBidderProcessFormModel
    {
        public string Message { get; set; }
        public bool IsExistingUser { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsSaved { get; set; }
        public bool IsExists { get; set; }
        public string FormId { get; set; }
        public bool IsOTPSent { get; set; }
        public string OTPNumber { get; set; }
        public bool IsOTPValid { get; set; }

        public static string Invalidinput => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Org Name", "Invalid input");

        [Required]
        [RegularExpression("^[a-zA-Z]([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(Invalidinput), ErrorMessageResourceType = typeof(ITSRBidderProcessFormModel))]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Max length limit exceeded")]
        public string CompanyName { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z]([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(Invalidinput), ErrorMessageResourceType = typeof(ITSRBidderProcessFormModel))]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Max length limit exceeded")]
        public string Location { get; set; }

        public static string InvalidName => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Login Name", "Name contains alphabets only");

        [Required]
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(ITSRBidderProcessFormModel))]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max length limit exceeded")]
        public string SPOCPersonName { get; set; }

        [Required]
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a valid Mobile Number");
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(ITSRBidderProcessFormModel))]
        public string MobileNumber { get; set; }

        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Email Address", "Please enter a valid email address");
        [Required]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(ITSRBidderProcessFormModel))]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max length limit exceeded")]
        public string EmailId { get; set; }

        public string Title { get; set; }
        public string TenderNo { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public string ProposalOwnerName { get; set; }
        public string ProposalOwnerEmailId { get; set; }

        public string BuyerOwnerName { get; set; }
        public string BuyerOwnerEmailId { get; set; }

        public List<ITSR_BidderFormSubmissions_Document> UploadedDocuments { get; set; }

        //[Required]
        public HttpPostedFileBase[] PastExperienceList { get; set; }
        public HttpPostedFileBase[] PerformanceCertificate { get; set; }
        public HttpPostedFileBase[] PastOrders { get; set; }

        public string Deviation { get; set; }

        public string AcceptanceOfBillOfQuantity { get; set; }

        //[Required]
        public HttpPostedFileBase[] GuaranteeTechnicalParticulars { get; set; }
        //[Required]
        public HttpPostedFileBase[] GeneralArrangement { get; set; }
        public HttpPostedFileBase[] TestCertificate { get; set; }

        //[Required]
        [RegularExpression("^[a-zA-Z0-9]([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(Invalidinput), ErrorMessageResourceType = typeof(ITSRBidderProcessFormModel))]
        [StringLength(2000, MinimumLength = 1, ErrorMessage = "Max length limit exceeded")]
        public string Deliveryschedule { get; set; }

        public string SiteVisitDone { get; set; }
        public string DemonstrationOfProduct { get; set; }

        public HttpPostedFileBase[] OtherdocumentsSpecified { get; set; }
        public HttpPostedFileBase[] ConsolidatedDeviationSheet { get; set; }

    }


}