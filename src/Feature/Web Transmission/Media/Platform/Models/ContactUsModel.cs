using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.BAU.Transmission.Feature.Media.Platform.Models
{
    public class NameValue
    {
        public string name { get; set; }
        public string value { get; set; }
        public HtmlString RichText { get; set; }
        public string ImageSrc { get; set; }
        public string ImageAlt { get; set; }
    }
    public class ContactUsModel
    {
        public List<NameValue> cityList { get; set; }
        public HtmlString RichText { get; set; }
        public string Title  { get; set; }
        public string ImageSrc { get; set; }
        public string ImageAlt { get; set; }
        public string FileURL { get; set; }
    }
}