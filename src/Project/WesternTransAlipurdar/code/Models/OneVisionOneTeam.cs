using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTransAlipurdar.Website.Models
{
    public class OneVisionOneTeam
    {
        public Header HeaderData { get; set; }
        public Cookie CookieData { get; set; }
        public GenericBredcrumbNavigation GenericBredcrumbNavigation { get; set; }
        public ProfileCard ProfileCard { get; set; }
        public Footer FooterData { get; set; }
        public ContactFormData ContactFormData { get; set; }
    }
}