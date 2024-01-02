using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class ContentModel
    {
        public Content Content { get; set; }
    }

    public class Content
    {
        public string title { get; set; }
        public string pageData { get; set; }
        public string heading { get; set; }
    }
        
}