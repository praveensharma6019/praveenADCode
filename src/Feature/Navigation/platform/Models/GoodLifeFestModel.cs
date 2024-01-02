using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Navigation.Platform.Models
{
    public class GoodLifeFestEnquireModel
    {
        public string full_name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string projectname { get; set; }
        public string project_type { get; set; }
        public string Projects_Interested__c { get; set; }
        public string PropertyLocation { set; get; }
        public string PropertyCode { set; get; }
        public string RecordType { get; set; }
        public string Captcha { get; set; }
        public string OTP { get; set; }
        public string FormType { get; set; }
        public string PageInfo { get; set; }
        public string PageUrl { get; set; }
        public string UTMSource { get; set; }
        public DateTime FormSubmitOn { get; set; }
        public string ReturnViewMessage { get; set; }
        public string city { get; set; }
        public string LeadSource { get; set; }
        public string AdvertisementId { get; set; }
        public string UTMPlacement { get; set; }
        public string reResponse { get; set; }
        public bool isincludedquerystring { get; set; }
    }
}