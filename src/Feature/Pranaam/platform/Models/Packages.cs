using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Models
{
    public class PranaamPackage
    {
        [JsonProperty(PropertyName = "pranaamPackage")]
        public Packages pranaamPackage { get; set; }
    }
    public class Packages
    {
        [JsonProperty(PropertyName = "title")]
        public string title { get; set; }
        [JsonProperty(PropertyName = "cards")]
        public PackageCard cards { get; set; }
    }

    public class PackageCard
    {
        [JsonProperty(PropertyName = "items")]
        public List<PackageItems> items { get; set; }
    }

    public class PackageItems 
    {
        [JsonProperty(PropertyName = "id")]
        public string PackageId { get; set; }
        [JsonProperty(PropertyName = "src")]
        public string src { get; set; }
        [JsonProperty(PropertyName = "alt")]
        public string alt { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string PackageTitle { get; set; }
        [JsonProperty(PropertyName = "cardDesc")]
        public string cardDesc { get; set; }
        [JsonProperty(PropertyName = "finalPrice")]
        public string finalPrice { get; set; }
        [JsonProperty(PropertyName = "btnText")]
        public string btnText { get; set; }
        [JsonProperty(PropertyName = "btnUrl")]
        public string btnUrl { get; set; }
        [JsonProperty(PropertyName = "btnVariant")]
        public string btnVariant { get; set; }

        [JsonProperty(PropertyName = "mobileImage")]
        public string mobileImage { get; set; }

        [JsonProperty(PropertyName = "mobileImageAlt")]
        public string mobileImageAlt { get; set; }

        [JsonProperty(PropertyName = "webImage")]
        public string webImage { get; set; }

        [JsonProperty(PropertyName = "webImageAlt")]
        public string webImageAlt { get; set; }

        [JsonProperty(PropertyName = "thumbnailImage")]
        public string thumbnailImage { get; set; }

        [JsonProperty(PropertyName = "thumbnailImageAlt")]
        public string thumbnailImageAlt { get; set; }

        [JsonProperty(PropertyName = "IsRecommended")]
        public bool IsRecommended { get; set; }
    }

    public class Packageswidgets
    {
        [JsonProperty(PropertyName = "widget")]
        public WidgetItem widget { get; set; }
    }
    public class AppPackage
    {
        [JsonProperty(PropertyName = "data")]
        public List<AppPackageDetails> data { get; set; }
    }
    public class AppPackageDetails
    {
        [JsonProperty(PropertyName = "packageId")]
        public string packageId { get; set; }

        [JsonProperty(PropertyName = "PackageName")]
        public string PackageName { get; set; }

        [JsonProperty(PropertyName = "IsRecommended")]
        public bool IsRecommended { get; set; }

        [JsonProperty(PropertyName = "finalPrice")]
        public string FinalPrice { get; set; }

        [JsonProperty(PropertyName = "ServicesList")]
        public List<NameValue> ServicesList { get; set; }
    }
    public class NameValue{
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "Value")]
        public bool Value { get; set; }
    }
}