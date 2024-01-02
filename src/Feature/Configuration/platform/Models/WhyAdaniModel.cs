using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.Models
{
    public class WhyAdaniModel
    {
        [JsonProperty(PropertyName = "whyushighlights")]
        public WhyUsHighlights WhyUsHighlights { get; set; }
        [JsonProperty(PropertyName = "whyAdani")]
        public WhyAdani WhyAdani { get; set; }
        [JsonProperty(PropertyName = "aboutAdani")]
        public AboutAdani AboutAdani { get; set; }
        [JsonProperty(PropertyName = "bottomQuote")]
        public BottomQuote BottomQuote { get; set; }
    }
    public class WhyUsHighlights
    {
        [JsonProperty(PropertyName = "data")]
        public List<WhyusHighlightsData> Data { get; set; }
    }
    public class WhyusHighlightsData
    {
        [JsonProperty(PropertyName = "sectionHeading")]
        public string SectionHeading { get; set; }
        [JsonProperty(PropertyName = "headingAsset")]
        public string HeadingAsset { get; set; }
        [JsonProperty(PropertyName = "headingAssetAlt")]
        public string HeadingAssetAlt { get; set; }
        [JsonProperty(PropertyName = "src")]
        public string Src { get; set; }
        [JsonProperty(PropertyName = "imageAlt")]
        public string ImageAlt { get; set; }
        [JsonProperty(PropertyName = "ImageTitle")]
        public string ImageTitle { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "subHeading")]
        public string SubHeading { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "dataAlign")]
        public string DataAligh { get; set; }
        [JsonProperty(PropertyName = "imgType")]
        public string imgType { get; set; }
    }
    public class WhyAdani
    {
        [JsonProperty(PropertyName = "data")]
        public List<WhyAdaniData> Data { get; set; }
    }
    public class WhyAdaniData
    {
        [JsonProperty(PropertyName = "alignCol")]
        public string AlignCol { get; set; }
        [JsonProperty(PropertyName = "sectionHeading")]
        public string SectionHeading { get; set; }
        [JsonProperty(PropertyName = "iconprimary")]
        public string IconPrimary { get; set; }
        [JsonProperty(PropertyName = "iconPrimaryAlt")]
        public string IconPrimaryAlt { get; set; }
        [JsonProperty(PropertyName = "iconsecondary")]
        public string IconSecondary { get; set; }
        [JsonProperty(PropertyName = "iconSecondaryAlt")]
        public string IconSecondaryAlt { get; set; }
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }
        [JsonProperty(PropertyName = "subHeading")]
        public string SubHeading { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        
    }
    public class AboutAdani
    {
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }
        [JsonProperty(PropertyName = "about")]
        public string About { get; set; }
        [JsonProperty(PropertyName = "readmore")]
        public string Readmore { get; set; }
        [JsonProperty(PropertyName = "blockHeading")]
        public List<BlockHeadingData> BlockHeading { get; set; }
    }
    public class BlockHeadingData
    {
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }
    }
    public class BottomQuote
    {
        [JsonProperty(PropertyName = "data")]
        public List<BottomQuoteData> Data { get; set; }
    }
    public class BottomQuoteData
    {
        [JsonProperty(PropertyName = "sectionHeading")]
        public string SectionHeading { get; set; }
        [JsonProperty(PropertyName = "iconPrimary")]
        public string IconPrimary { get; set; }
        [JsonProperty(PropertyName = "iconPrimaryAlt")]
        public string IconPrimaryAlt { get; set; }
        [JsonProperty(PropertyName = "iconSecondary")]
        public string IconSecondary { get; set; }
        [JsonProperty(PropertyName = "iconSecondaryAlt")]
        public string IconSecondaryAlt { get; set; }
    }
}
