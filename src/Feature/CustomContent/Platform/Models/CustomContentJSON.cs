using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.Models
{
    public class ContentJSON
    {
        public string title { get; set; }
        public List<LinewithLinks> lines { get; set; }

        public ContentJSON()
        {
            title = "";
            lines = new List<LinewithLinks>();
        }
    }

    public class ContentJSONList
    {       
        public List<ContentJSON> contentList { get; set; }

        public ContentJSONList()
        {
            contentList = new List<ContentJSON>();
        }
    }

    public class LinewithLinks
    {
        public string line { get; set; }
        public List<LineLinks> links { get; set; }

        public LinewithLinks()
        {
            line = string.Empty;
            links = new List<LineLinks>();
        }
    }

    public class LineLinks
    {
        public string link { get; set; }
        public string linkText { get; set; }
        public string linkURL { get; set; }

        public LineLinks()
        {
            linkText = string.Empty;
            link = string.Empty;
            linkURL = string.Empty;
        }
    }
}