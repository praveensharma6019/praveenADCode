using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Blog.Platform.Models
{
    public class BlogCategoryModel
    {
        [JsonProperty(PropertyName = "Content")]
        public Content Content { get; set; }
        [JsonProperty(PropertyName = "BlogAnchors")]
        public GenericObject BlogAnchors { get; set; }
        [JsonProperty(PropertyName = "blogs")]
        public BlogObject Blogs { get; set; }

    }

    public class Content
    {
        [JsonProperty(PropertyName = "Heading")]
        public string Heading { get; set; }
        [JsonProperty(PropertyName = "LoadingText")]
        public string LoadingText { get; set; }
    }

    public class GenericObject
    {
        [JsonProperty(PropertyName = "data")]
        public Object Data { get; set; }
    }
    public class BlogObject
    {
        [JsonProperty(PropertyName = "totalBlogs")]
        public Int32 TotalBlogs { get; set; }
        [JsonProperty(PropertyName = "currentBlogs")]
        public Int32 CurrentBlogs { get; set; }
        [JsonProperty(PropertyName = "IsRecordsEnd")]
        public bool IsRecordsEnd { get; set; }
        [JsonProperty(PropertyName = "data")]
        public Object Data { get; set; }
    }



    public class BlogAnchor
    {
        [JsonProperty(PropertyName = "title")]
        public string CategoryTabTitle { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string CategoryTabID { get; set; }
        [JsonProperty(PropertyName = "Link")]
        public string Link { get; set; }
        [JsonProperty(PropertyName = "slug")]
        public string Slug { get; set; }
    }

    public class CatBlog
    {
        [JsonProperty(PropertyName = "total-blogs")]
        public Int32 TotalBlogs { get; set; }
        [JsonProperty(PropertyName = "current-Blogs")]
        public Int32 CurrentBlogs { get; set; }
        [JsonProperty(PropertyName = "keys")]
        public List<BlogKey> Keys { get; set; }
    }
    public class BlogKey
    {
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }
        [JsonProperty(PropertyName = "linkText")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "src")]
        public string Src { get; set; }
        [JsonProperty(PropertyName = "imgTitle")]
        public string imgTitle { get; set; }
        [JsonProperty(PropertyName = "alt")]
        public string Alt { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }
        [JsonProperty(PropertyName = "subHeading")]
        public string SubHeading { get; set; }
        [JsonProperty(PropertyName = "Date")]
        public DateTime Date { get; set; }
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