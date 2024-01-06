using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTrans.Website.Models
{
    public class AboutUs
    {
        public Header HeaderData { get; set; }
        public Cookie CookieData { get; set; }
        public GenericImageBanner GenericImageBanner { get; set;}
        public GenericBredcrumbNavigation GenericBredcrumbNavigation { get; set;}
        public PageContent PageContent { get; set;}
        public CultureValues CultureValues { get; set; }
        public InThisSection InThisSection { get; set;}
        public Footer FooterData { get; set; }
       
    }
}