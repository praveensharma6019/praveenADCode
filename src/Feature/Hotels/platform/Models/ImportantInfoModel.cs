using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Hotels.Platform.Models
{
    public class ImportantInfo
    {
        public string title { get; set; }

        public List<Information> lines { get; set; }

        public ImportantInfo()
        {
            lines = new List<Information>();
        }

    }

    public class Information
    {
        public string title { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public string autoId { get; set; }
        public List<LineLinks> links { get; set; }

        public Information()
        {
            links = new List<LineLinks>();
        }

    }

    public class LineLinks
    {
        public string link { get; set; }
        public string linkText { get; set; }
        public string linkURL { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public string uniqueId { get; set; }

        public LineLinks()
        {
            linkText = string.Empty;
            link = string.Empty;
            linkURL = string.Empty;
        }

    }
}