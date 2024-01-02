using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Blog.Platform.Models
{
    public class BlogSearchModel
    {
        [JsonProperty(PropertyName = "blogSearch")]
        public List<BlogSearch> BlogSearchList { get; set; }
    }
    public class BlogSearch
    {
        [JsonProperty(PropertyName = "id")]
        public string ItemId { get; set; }
        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; }
        [JsonProperty(PropertyName = "slug")]
        public string Slug { get; set; }
    }
}