using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Navigation.Platform.Models
{
    public class SfdcModel
    {
        public string Name { get; set; }
        public string OTP { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string ProjectType { get; set; }
        public string Type { get; set; }
        public string Comments { get; set; }
        public string Configuration { get; set; }
        public string Location { get; set; }
        public bool agreement { get; set; }
        public string purpose { get; set; }
        public string Country { get; set; }
        public string FormType { get; set; }
        public string PageInfo { get; set; }
        public bool IsPreQualificationLeads { get; set; }
        public bool IsHomeLoanRequired { get; set; }
        public string timeSLot { get; set; }
        public DateTime PlanVisitDate { get; set; }
    }
}