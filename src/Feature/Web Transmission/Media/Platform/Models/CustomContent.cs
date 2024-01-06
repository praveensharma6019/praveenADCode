using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.BAU.Transmission.Feature.Media.Platform.Models
{
    public class CustomContentList
    {       
       public List<CustomContentItem> contentItems { get; set; }
    }
    public class CustomContentItem
    {
        public string name { get; set; }
        public string title { get; set; }

        public string richText { get; set; }

    }
}