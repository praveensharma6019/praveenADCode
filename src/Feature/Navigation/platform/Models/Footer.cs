using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Navigation.Platform.Models
{
    public class Footer
    {
        [JsonProperty(PropertyName = "mainNavigations")]
        public List<FooterHeading> MainNavigations { get; set; }
        [JsonProperty(PropertyName = "copyRight")]
        public List<CopyRightFooterHeading> CopyRight { get; set; }
        [JsonProperty(PropertyName = "socialLinks")]
        public List<FooterSocialHeading> FooterSocialLink { get; set; }
    }

    public class FooterHeading
    {
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }
        public bool isSeoFooter { get; set; }
        [JsonProperty(PropertyName = "items")]
        public List<object> Items { get; set; }
    }
    public class CopyRightFooterHeading
    {
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }
        [JsonProperty(PropertyName = "subHeading")]
        public string SubHeading { get; set; }
        [JsonProperty(PropertyName = "items")]
        public List<object> Items { get; set; }
    }
    public class FooterSocialHeading
    {
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }
        [JsonProperty(PropertyName = "items")]
        public List<SocialLink> Items { get; set; }
    }
    public class Link
    {
        [JsonProperty(PropertyName = "linkUrl")]
        public string LinkUrl { get; set; }
        [JsonProperty(PropertyName = "target")]
        public string target { get; set; }
        [JsonProperty(PropertyName = "LinkTitle")]
        public string LinkTitle { get; set; }
    }
    public class SocialLink
    {
        [JsonProperty(PropertyName = "linkUrl")]
        public string LinkUrl { get; set; }
        [JsonProperty(PropertyName = "linkTitle")]
        public string LinkTitle { get; set; }
        [JsonProperty(PropertyName = "itemicon")]
        public string ItemIcon { get; set; }
        [JsonProperty(PropertyName = "target")]
        public string target { get; set; }
    }

    public class DropdownLinks
    {
        public string linkTitle { get; set; }
        public List<Propertylinks> items { get; set; }
    }

    public class Propertylinks
    {
        public string link { get; set; }
        public string linkTitle { get; set; }
        public string target { get; set; }
    }
}