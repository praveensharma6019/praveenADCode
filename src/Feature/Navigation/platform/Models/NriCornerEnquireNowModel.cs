using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Navigation.Platform.Models
{
    public class NriCornerEnquireNowModel
    {
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string RecordType { get; set; }  
        public string country { get; set; }  
        public string propertyType { get; set; }  
        public string MasterProjectID { get; set; }
        public string ProjectName { get; set; }
        public string FormType { get; set; }
        public string UTMSource { get; set; }
        public string UTMPlacement { get; set; }
        public string AdvertisementID { get; set; }
        public string TermsAndcondition { get; set; }
        public string HomeLoan { get; set; }
        public string PageInfo { get; set; }
        public string PageUrl { get; set; }
        public string Comment { get; set; }
        public string PropertyLocation { get; set; }
        public bool isincludedquerystring { get; set; }
    }
}