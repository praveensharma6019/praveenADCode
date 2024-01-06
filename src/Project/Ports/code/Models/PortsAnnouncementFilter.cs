using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Ports.Website.Models
{
    public class PortsAnnouncementFilter
    {
        public string Segment { set; get; }
        public string Category { set; get; }
        public string Date { set; get; }
        public string ToDate { set; get; }
    }
}