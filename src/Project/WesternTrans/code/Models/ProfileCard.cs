using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTrans.Website.Models
{
    public class ProfileCard
    {
        public string Heading { get; set; }
        public List<ProfileCardItems> Data { get; set; }
    }

    public class ProfileCardItems
    {
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Link { get; set; }
        public bool isExternalLink { get; set; }
        public string Image { get; set; }
        public string ImageAltText { get; set; }
    }
}