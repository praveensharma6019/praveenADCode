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
    public class TrackComplaintModel
    {
        public string LoginName { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string ZoneName { get; set; }

        public string ExcalationRemarks { get; set; }

        public string ExcalationComplaintId { get; set; }

        public List<SelectListItem> Zones { get; set; }

        public List<SelectListItem> Months { get; set; }

        public List<ComplaintDetails> ComplaintList { get; set; }
        public List<ComplaintDetails> SavedComplaintList { get; set; }
    }

    public class ComplaintDetails
    {
        public int TATPassedDays { get; set; }
        public int TATLeftDays { get; set; }
        public int OrangeRedGreen { get; set; }

        public string ConsumerName { get; set; }
        public string MobileNumber { get; set; }
        public bool IsIGR { get; set; }
        public string M20ComplaintId { get; set; }
        public long ComplaintId { get; set; }
        public string AccountNumber { get; set; }
        public string ComplaintRegistrationNumber { get; set; }
        public string CGRFCaseNumber { get; set; }
        public string ComplaintLevel { get; set; }
        public string ComplaintNumber { get; set; }
        public string ComplaintCategory { get; set; }
        public string ComplaintSubCategory { get; set; }
        public string ComplaintDescription { get; set; }
        public string ComplaintZone { get; set; }
        public string ComplaintStatusCode { get; set; }
        public string ComplaintStatusName { get; set; }
        public string ComplaintStatusDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string CompletionDate { get; set; }
        public DateTime CreatedOnSAPDateType { get; set; }
        public string CreatedOnSAP { get; set; }
        public string Level1ComplaintNumber { get; set; }
        public string ModifiedOnSAP { get; set; }
        public string TATDate { get; set; }

        public string EscalatedToCGRF { get; set; }

        public string Division { get; set; }
        public string PMActivityType { get; set; }
    }

    public class ComplaintDetailsToExport
    {
       
        public string ComplaintZone { get; set; }
        public string Division { get; set; }
        public string ComplaintRegistrationNumber { get; set; }

        public string AccountNumber { get; set; }
        public string ConsumerName { get; set; }
        public string MobileNumber { get; set; }

        public string PMActivityType { get; set; }

        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string ComplaintDescription { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CompletionDate { get; set; }
        public string TATDate { get; set; }
        public string TrackingStatus { get; set; }
        public int TATPassedDays { get; set; }
        public string ComplaintStatusName { get; set; }

        public string Level1ComplaintNumber { get; set; }
        public string EscalatedToCGRF { get; set; }

        //public long ComplaintId { get; set; }
        //public string M20ComplaintId { get; set; }
        //public string ComplaintNumber { get; set; }
        //public string ComplaintStatusCode { get; set; }
        //public string ComplaintStatusDescription { get; set; }
        //public DateTime? StartDate { get; set; }
        //public DateTime? ClosedDate { get; set; }
        //public DateTime CreatedOnSAPDateType { get; set; }
        //public string CreatedOnSAP { get; set; }
        //public string ModifiedOnSAP { get; set; }
        
    }
}