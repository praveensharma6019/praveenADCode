using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.Models
{
    public class BlogContentModel
    {
        [JsonProperty(PropertyName = "blog")]
        public BlogObject Blog { get; set; }
    }
    public class BlogObject
    {
        [JsonProperty(PropertyName = "data")]
        public List<object> Data { get; set; }
    }
    public class Blog
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "categoryId")]
        public string CategoryId { get; set; }
        [JsonProperty(PropertyName = "categoryPageLink")]
        public string CategoryPageLink { get; set; }
        [JsonProperty(PropertyName = "categoryPageLinkText")]
        public string CategoryPageLinkText { get; set; }
        [JsonProperty(PropertyName = "categorySectionTitle")]
        public string CategorySectionTitle { get; set; }
        [JsonProperty(PropertyName = "slug")]
        public string Slug { get; set; }
        [JsonProperty(PropertyName = "keys")]
        public List<BlogKey> Keys { get; set; }
    }
    public class BlogKey
    {
        [JsonProperty(PropertyName = "isDefault")]
        public bool IsDeafult { get; set; }
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }
        [JsonProperty(PropertyName = "linkText")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "src")]
        public string Src { get; set; }
        [JsonProperty(PropertyName = "alt")]
        public string Alt { get; set; }
        [JsonProperty(PropertyName = "imgTitle")]
        public string imgTitle { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }
        [JsonProperty(PropertyName = "subHeading")]
        public string SubHeading { get; set; }
        [JsonProperty(PropertyName = "date")]
        public DateTime date { get; set; }
        [JsonProperty(PropertyName = "dateTime")]
        public string DateTime { get; set; }
        [JsonProperty(PropertyName = "categoryLink")]
        public string CategoryLink { get; set; }
        [JsonProperty(PropertyName = "categoryTitle")]
        public string CategoryTitle { get; set; }
        [JsonProperty(PropertyName = "readTime")]
        public string ReadTime { get; set; }
    }
}