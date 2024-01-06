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
    public class CGRFComplaintFileRegistrationModel
    {
        public CGRFComplaintFileRegistrationModel()
        {
            ComplaintPortalService complaintPortalService = new ComplaintPortalService();
            //ComplaintCategorySelectList = new List<SelectListItem>();
            //ConsumerCategorySelectList = new List<SelectListItem> {
            //    new SelectListItem{ Value="HT Commercial", Text="HT Commercial"},
            //    new SelectListItem{ Value="Industrial", Text="Industrial"},
            //    new SelectListItem{ Value="LT - Residential", Text="LT - Residential"},
            //    new SelectListItem{ Value="HT Commercial", Text="HT Commercial"},
            //    new SelectListItem{ Value="Commercial", Text="Commercial"},
            //    new SelectListItem{ Value="Industrial", Text="Industrial"},
            //    new SelectListItem{ Value="AG", Text="AG"},
            //    new SelectListItem{ Value="Others", Text="Others"}
            //};

            ReasonToApplySelectList = new List<SelectListItem>();
            ReasonToApplySelectList = new List<SelectListItem> {
                new SelectListItem{ Value="Complaint Closure without consent", Text="Complaint Closure without consent"},
                new SelectListItem{ Value="Not satisfied by the resolution provided", Text="Not satisfied by the resolution provided"},
                new SelectListItem{ Value="Non-admission in ICRS (Internal Complaint Redressal System)", Text="Non-admission in ICRS (Internal Complaint Redressal System)"},
                new SelectListItem{ Value="Unredressed within resolution period", Text="Unredressed within resolution period"},
            };

            ReasonToApplySubSelectList = new List<SelectListItem>();
            ReasonToApplySubSelectList = new List<SelectListItem> {
                new SelectListItem{ Value="Disconnection- Reconnection Complaint : 03 Days", Text="Disconnection- Reconnection Complaint : 03 Working Days"},
                new SelectListItem{ Value="No supply Complaint : 03 Days", Text="No supply Complaint : 03 Working Days"},
                new SelectListItem{ Value="New Connection Complaint : 03 Days", Text="New Connection Complaint : 03 Working Days"},
                new SelectListItem{ Value="High Consumption Complaint : 15 Days", Text="High Consumption Complaint : 15 Working Days"},
                new SelectListItem{ Value="Others type complaint : 15 Days", Text="Others type complaint : 15 Working Days"}
            };

            //var listZoneDivision = complaintPortalService.GetCGRFComplaintZoneDivisionList();
            //ConsumerZoneSelectList = new List<SelectListItem>();
            //var zones = listZoneDivision.GroupBy(x => x.ZoneName); // group by Zone head
            //foreach (var group in zones)
            //{
            //    var optionGroup = new SelectListGroup() { Name = group.Key.ToString() };
            //    foreach (var item in group)
            //    {
            //        ConsumerZoneSelectList.Add(new SelectListItem()
            //        {
            //            Value = item.Id.ToString(),
            //            Text = item.DivisionName,
            //            Group = optionGroup
            //        });
            //    }
            //}

            ComplaintCategorySelectList = new List<SelectListItem>();
            ComplaintCategorySelectList = new List<SelectListItem> {
                new SelectListItem{ Value="Non Supply", Text="Non Supply"},
                new SelectListItem{ Value="New Connection", Text="New Connection"},
                new SelectListItem{ Value="Disconnection", Text="Disconnection "},
                new SelectListItem{ Value="Reconnection", Text="Reconnection"},
                new SelectListItem{ Value="Billing Related", Text="Billing Related"},
                new SelectListItem{ Value="Others", Text="Others"},
            };

        }

        public static string InvalidInput => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Login Name", "Input is not valid.");

        public List<SelectListItem> ComplaintCategorySelectList { get; set; }
        public string SelectedComplaintCategory { get; set; }

        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(CGRFComplaintFileRegistrationModel))]
        public string OtherCategoryText { get; set; }

        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(CGRFComplaintFileRegistrationModel))]
        public string AdminRemarks { get; set; }

        public bool IsEscalated { get; set; }

        //public string SelectedComplaintSubCategory { get; set; }

        public List<SelectListItem> ConsumerCategorySelectList { get; set; }
        public string SelectedConsumerCategory { get; set; }

        public List<SelectListItem> ConsumerZoneSelectList { get; set; }
        public string SelectedConsumerZone { get; set; }

        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(CGRFComplaintFileRegistrationModel))]
        public string LoginName { get; set; }

        public string ComplaintTrackingNumber { get; set; }

        public string ComplaintId { get; set; }
        public DateTime AppliedDate { get; set; }
        public string ComplaintStatus { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsDocumentUploaded { get; set; }
        public string DocumentName { get; set; }
        public Guid Id { get; set; }

        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(CGRFComplaintFileRegistrationModel))]
        public string ComplaintFromPreviousLevel { get; set; }
        public string ComplaintFromPreviousLevelDivision { get; set; }
        public bool IsComplaintFromPreviousLevelValid { get; set; }
        public string RemarksFromPreviousLevel { get; set; }
        public string ComplaintDescriptionFromPreviousLevel { get; set; }
        public string ReasonToApply { get; set; }
        public List<SelectListItem> ReasonToApplySelectList { get; set; }

        public string ReasonToApplySubType { get; set; }
        public List<SelectListItem> ReasonToApplySubSelectList { get; set; }
        public string ReasonToApplyOtherText { get; set; }
        public string ComplaintFromPreviousLevelAppliedDate { get; set; }

        public string CGRFCaseNumber { get; set; }
        //public DateTime? ComplaintAppliedDate { get; set; }
        public DateTime? TantetiveHearingDate { get; set; }

        [RegularExpression("^([a-zA-Z0-9]|[@.,])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(CGRFComplaintFileRegistrationModel))]
        public string ForwardDetailsEmailTo { get; set; }

        [RegularExpression(@"^[0-9]{8,15}$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(CGRFComplaintFileRegistrationModel))]
        public string AccountNumber { get; set; }
        public string ConsumerName { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string DivisionName { get; set; }
        public string ZoneName { get; set; }
        public string ConsumerCategory { get; set; }

        public string ComplaintStatusDescription { get; set; }
        public string ComplaintRegistrationNumber { get; set; }

        public string ComplaintCategory { get; set; }
        public string ComplaintSubCategory { get; set; }

        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(CGRFComplaintFileRegistrationModel))]
        public string ComplaintDescription { get; set; }

        public List<CGRFComplaintDoc> ComplaintSupportingDocuments { get; set; }

        public HttpPostedFileBase ComplaintScheduleAFormFile { get; set; }

        public HttpPostedFileBase[] SupportingDocuments { get; set; }

        public List<ComplaintPortalCGRFComplaintHearingSchedule> ComplaintHearings { get; set; }
        public List<ComplaintPortalCGRFComplaintHearingSchedule> ComplaintHearingsOrderReviewRequest { get; set; }
        public string ComplaintHearingSelectedDate { get; set; }

        public string IsAppliedWithin30Days { get; set; }
        public string IsAppealPreferred { get; set; }
        public string IsErrorApparent { get; set; }
        public string IsImportantMatterDiscovery { get; set; }

        public bool IsReviewRequestRaised { get; set; }

        public string Captcha { get; set; }
        public List<ComplaintPortalCGRFComplaintHistory> ComplaintHistoryRecords { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter {0}");
    }

    public class CGRFComplaintDoc
    {
        public string DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
        public bool IsReviewDocument { get; set; }
    }
}