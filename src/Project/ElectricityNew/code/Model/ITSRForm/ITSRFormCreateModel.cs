namespace Sitecore.ElectricityNew.Website.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Foundation.Dictionary.Repositories;

    [Serializable]
    public class ITSRFormCreateModel
    {
        public static string Invalidinput => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Org Name", "Invalid input");
        public static string InvalidName => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Login Name", "Name contains alphabets only");
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Email Address", "Please enter a valid email address");

        [Required]
        [RegularExpression("^[a-zA-Z]([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(Invalidinput), ErrorMessageResourceType = typeof(ITSRFormCreateModel))]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Max length limit exceeded")]
        public string Title { get; set; }

        public string TenderNo { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }


        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max length limit exceeded")]
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(ITSRFormCreateModel))]
        public string ProposalOwnerName { get; set; }

        [Required]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(ITSRFormCreateModel))]
        [DataType(DataType.EmailAddress)]
        public string ProposalOwnerEmailId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max length limit exceeded")]
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(ITSRFormCreateModel))]
        public string BuyerOwnerName { get; set; }

        [Required]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(ITSRFormCreateModel))]
        [DataType(DataType.EmailAddress)]
        public string BuyerOwnerEmailId { get; set; }

        public string PastExperienceList { get; set; }
        public string PerformanceCertificate { get; set; }
        public string PastOrders { get; set; }

        public string Deviation { get; set; }

        public string AcceptanceOfBillOfQuantity { get; set; }

        public string GuaranteeTechnicalParticulars { get; set; }
        public string GeneralArrangement { get; set; }
        public string TestCertificate { get; set; }

        public string Deliveryschedule { get; set; }
        public string SiteVisitDone { get; set; }
        public string DemonstrationOfProduct { get; set; }

        public string OtherdocumentsSpecified { get; set; }
        public string ConsolidatedDeviationSheet { get; set; }

        public bool IsFormCreated { get; set; }
        public string FormId { get; set; }
        public string FormURL { get; set; }
    }


}