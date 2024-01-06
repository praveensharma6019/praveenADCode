using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTrans.Website.Models
{
    public class Home
    {
        public Header HeaderData { get; set; }
        public Cookie CookieData { get; set; }
        public GenericBannerCarousel BannerCarouselData { get; set; }
        public HomeCommercialOperation CommercialData { get; set; }
        public GroupWebsite GroupWebsiteData { get; set; }
        public ContactFormData ContactFormData { get; set; }
        public Footer FooterData { get; set; }
    }
}