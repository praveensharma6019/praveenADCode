using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.Models
{
    public class CommunicationCornerModel
    {
        [JsonProperty(PropertyName = "BlogAnchors")]
        public GenericObject BlogAnchors { get; set; }
        [JsonProperty(PropertyName = "Content")]
        public Content Content { get; set; }
        [JsonProperty(PropertyName = "OtherArticles")]
        public OtherArticleData OtherArticles { get; set; }
    }
    public class BlogAnchor
    {
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }
        [JsonProperty(PropertyName = "btntext")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "slug")]
        public string Slug { get; set; }
    }
    public class GenericObject
    {
        [JsonProperty(PropertyName = "data")]
        public List<object> Data { get; set; }
    }
    public class Content
    {
        [JsonProperty(PropertyName = "ComCorner")]
        public string ComCorner { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
    }
    public class OtherArticleData
    {
        [JsonProperty(PropertyName = "componentName")]
        public string ComponentName { get; set; }
        [JsonProperty(PropertyName = "data")]
        public List<OtherArticle> Data { get; set; }
    }
    public class OtherArticle
    {
        [JsonProperty(PropertyName = "articleType")]
        public string ArticleType { get; set; }
        [JsonProperty(PropertyName = "articleLink")]
        public string ArticleLink { get; set; }
        [JsonProperty(PropertyName = "articleLinkIcon")]
        public string ArticleLinkIcon { get; set; }
        [JsonProperty(PropertyName = "articleLinkTitle")]
        public string ArticleLinkTitle { get; set; }
        [JsonProperty(PropertyName = "articleThumb")]
        public string ArticleLinkThumb { get; set; }
        [JsonProperty(PropertyName = "articleThumbAlt")]
        public string ArticleLinkThumbAlt { get; set; }
        [JsonProperty(PropertyName = "articleTitle")]
        public string ArticleTitle { get; set; }
        [JsonProperty(PropertyName = "articleDescription")]
        public string ArticleDescription { get; set; }
    }
}