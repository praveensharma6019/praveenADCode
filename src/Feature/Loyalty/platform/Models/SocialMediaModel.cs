using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.Models
{
    public class SocialMediaModel
    {
        public string ctaLink { get; set; }
        public string ctaText { get; set; }
        public List<Media> Media { get; set; }

    }

    public class Media
    {
        public string Title { get; set; }
        public string MediaText { get; set; }
    }
}