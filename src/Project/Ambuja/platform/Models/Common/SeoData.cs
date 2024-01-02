using System.Collections.Generic;

namespace Project.AmbujaCement.Website.Models
{
    public class SeoData
    {
        public string PageTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaTitle { get; set; }
        public string OgTitle { get; set; }
        public RobotTag RobotsTags { get; set; }
        public string BrowserTitle { get; set; }
        public string OgImage { get; set; }
        public string OgDescription { get; set; }
        public string OgKeyword { get; set; }
        public string CanonicalUrl { get; set; }
        public string GoogleSiteVerification { get; set; }
        public string MsValidate { get; set; }
        public SeoDataorgSchemaModel orgSchema { get; set; }
    }

    public class SeoDataorgSchemaModel
    {
        public string Telephone { get; set; }
        public string ContactType { get; set; }
        public string AreaServed { get; set; }
        public string StreetAddress { get; set; }
        public string AddressLocality { get; set; }
        public string AddressRegion { get; set; }
        public string PostalCode { get; set; }
        public string ContactOption { get; set; }
        public string Logo { get; set; }
        public string Url { get; set; }

        public List<string> sameAs { get; set; } = new List<string>(); 
    }
    public class RobotTag
    {
        public bool index { get; set; }
        public bool follow { get; set; }
    }
}