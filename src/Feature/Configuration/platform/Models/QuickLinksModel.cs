using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.Models
{
    public class QuickLinksModel
    {
        public List<object> QuickLinks { get; set; }
        [JsonProperty(PropertyName = "pageSpecificBreadcrumbs")]
        public List<PageSpecificBreadCrumb> PageSpecificBreadCrumbs { get; set; }
        [JsonProperty(PropertyName = "PageSpecificSEOdata")]
        public List<SEOData> PageSpecificSEOdata { get; set; }

    }
    public class DisclaimerObject
    {
        [JsonProperty(PropertyName = "disclaimer")]
        public QuickLink Disclaimer { get; set; }

    }
    public class TermsConditionObject
    {
        [JsonProperty(PropertyName = "termsandCondition")]
        public QuickLink TermsandCondition { get; set; }

    }
    public class PrivacyPolicyObject
    {
        [JsonProperty(PropertyName = "privacyPolicy")]
        public QuickLink PrivacyPolicy { get; set; }

    }
    public class CookiePolicyObject
    {
        [JsonProperty(PropertyName = "cookiePolicy")]
        public QuickLink CookiePolicy { get; set; }
    }

    public class FaqSection
    {
        [JsonProperty(PropertyName = "faqs")]
        public Category Faqs { get; set; }
    }

    public class Category
    {
        [JsonProperty(PropertyName = "categories")]
        public List<QuickLinksFaq> QuickLinksFaq { get; set; }
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
    }


    public class QuickLinksFaq
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }
        [JsonProperty(PropertyName = "Links")]
        public string Links { get; set; }
        [JsonProperty(PropertyName = "Data")]
        public List<Data> FaqItems { get; set; }
    }
    public class Data
    {
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "Body")]
        public string Body { get; set; }
    }

    public class QuickLink
    {
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
    }
    public class PageSpecificBreadCrumb
    {
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }
    }
    public class SEOData
    {
        public string pageTitle { get; set; }
        public string metaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaTitle { get; set; }
        public string ogTitle { get; set; }
        public string robotsTags { get; set; }
        public string browserTitle { get; set; }
        public string ogImage { get; set; }
        public string ogDescription { get; set; }
        public string ogKeyword { get; set; }
        public string canonicalUrl { get; set; }
        public string googleSiteVerification { get; set; }
        public string msValidate { get; set; }
        public orgSchema orgSchema { get; set; }
    }
    public class orgSchema
    {
        public orgSchema()
        {
            sameAs = new List<string>();
        }
        public string telephone { get; set; }
        public string contactType { get; set; }
        public string areaServed { get; set; }
        public string streetAddress { get; set; }
        public string addressLocality { get; set; }
        public string addressRegion { get; set; }
        public string postalCode { get; set; }
        public List<string> sameAs { get; set; }
        public string contactOption { get; set; }
        public string logo { get; set; }
        public string url { get; set; }

    }

}