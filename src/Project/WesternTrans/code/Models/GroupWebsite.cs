using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTrans.Website.Models
{
    public class GroupWebsite
    {
        public string Heading { get; set; }
        public List<GroupWebsiteItems> Data { get; set; }
    }
}