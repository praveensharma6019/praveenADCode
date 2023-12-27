using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.CarParking.Platform.Models
{
    public class VendorFilters
    {
        public string language { get; set; }
        public string cabCode { get; set; }
        public string terminal { get; set; }
        public string terminalGate { get; set; }
        public string airport { get; set; }
        public VendorFilters()
        {
            language = "en";
            airport = "";
            terminal = "";
            terminalGate = "";
            cabCode = "";
        }
    }

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
        public ContentJSON cabImpInfo { get; set; }

        public ContentJSON cabDetailedImpInfo { get; set; }

        public ContentJSON cabCancellationPolicy { get; set; }

        public ContentJSONList()
        {
            cabImpInfo = new ContentJSON();
            cabDetailedImpInfo = new ContentJSON();
            cabCancellationPolicy = new ContentJSON();
        }
    }

    public class LinewithLinks
    {
        public string title { get; set; }
        public string description { get; set; }
        public string image { get; set; }

        public string autoId { get; set; }
        public List<LineLinks> links { get; set; }

        public LinewithLinks()
        {
            title = string.Empty;
            description = string.Empty;
            image = string.Empty;
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