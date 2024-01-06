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
    public class ComplaintReportModel
    {
        public ComplaintReportModel()
        {
            ComplaintPortalService complaintPortalService = new ComplaintPortalService();
            var listZoneDivision = complaintPortalService.GetCGRFComplaintZoneDivisionList();
            ConsumerZoneSelectList = new List<SelectListItem>();
            ConsumerDivisionSelectList = new List<SelectListItem>();

            var divisions = listZoneDivision.Select(c => c.ZoneName).Distinct(); // group by Zone head
            foreach (var div in divisions)
            {
                ConsumerDivisionSelectList.Add(new SelectListItem
                {
                    Value = div,
                    Text = div
                });
            }

            var zones = listZoneDivision.GroupBy(x => x.ZoneName); // group by Zone head
            foreach (var group in zones)
            {

                var optionGroup = new SelectListGroup() { Name = group.Key.ToString() };
                foreach (var item in group)
                {
                    ConsumerZoneSelectList.Add(new SelectListItem()
                    {
                        Value = item.Id.ToString(),
                        Text = item.DivisionName,
                        Group = optionGroup
                    });
                }
            }

            ComplaintCategorySelectList = new List<SelectListItem> {
                new SelectListItem{ Value="Non Supply", Text="Non Supply"},
                new SelectListItem{ Value="New Connection", Text="New Connection"},
                new SelectListItem{ Value="Disconnection", Text="Disconnection "},
                new SelectListItem{ Value="Reconnection", Text="Reconnection"},
                new SelectListItem{ Value="Billing Related", Text="Billing Related"},
                new SelectListItem{ Value="Others", Text="Others"},
            };

            ComplaintStatusSelecteList = new List<SelectListItem> {
                new SelectListItem{ Text="SAVED", Value="1"},
                new SelectListItem{ Text="SUBMITTED", Value="2"},
                new SelectListItem{ Text="NODAL REPLY IN PROGRESS", Value="3"},
                new SelectListItem{ Text="RESUBMIT", Value="4"},
                new SelectListItem{ Text="REJOINDER BY COMPLAINANT IS IN PROCESS", Value="5"},
                new SelectListItem{ Text="GRIEVANCE REDRESSAL IN PROCESS ", Value="6"},
                new SelectListItem{ Text="COMPLAINT IS CLOSED", Value="7"},
            };
        }

        public List<SelectListItem> ConsumerDivisionSelectList { get; set; }
        public List<SelectListItem> ConsumerZoneSelectList { get; set; }
        public List<SelectListItem> ComplaintCategorySelectList { get; set; }
        public List<SelectListItem> ComplaintStatusSelecteList { get; set; }
        public string SelectedComplaintCategory { get; set; }
        public string SelectedConsumerZone { get; set; }
        public string SelectedConsumerDivision { get; set; }
        public string SelectedComplaintStatus { get; set; }

        public string LoginName { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string ComplaintCategory { get; set; }

        public string ComplaintStatus { get; set; }

        public string Zone { get; set; }

        //public List<ComplaintPortalRegisteredComplaint> ComplaintList { get; set; }

        public List<ComplaintDetailsForReport> ComplaintList { get; set; }
    }

    public class ComplaintDetailsForReport
    {
        public long ComplaintId { get; set; }
        public string AccountNumber { get; set; }
        public string MobileNumber { get; set; }
        public string RegistrationTrackingNumber { get; set; }
        public string CaseNumber { get; set; }
        public string ConsumerName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string Zone { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string ConsumerCategory { get; set; }
        public string ComplaintCategory { get; set; }
        public string ComplaintCategoryOtherText { get; set; }
        public string ComplaintDescription { get; set; }
        public string ComplaintStatusDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public string StartDate { get; set; }
        public string ClosedDate { get; set; }
        public string ComplaintReason { get; set; }
        public string NodalReplyDueDate { get; set; }
        public string SecretaryActionDueDate { get; set; }
    }
}