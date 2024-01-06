using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTransAlipurdar.Website.Models
{
    public class Legal
    {
        public Header HeaderData { get; set; }
        public Cookie CookieData { get; set; }
        public GenericBredcrumbNavigation GenericBredcrumbNavigation { get; set; }
        public PageContent PageContent { get; set; }
        public Footer FooterData { get; set; }
        public ContactFormData ContactFormData { get; set; }
    }
}