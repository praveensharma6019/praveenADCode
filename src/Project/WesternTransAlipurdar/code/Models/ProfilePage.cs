using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTransAlipurdar.Website.Models
{
    public class ProfilePage
    {
        public Header HeaderData { get; set; }
        public Cookie CookieData { get; set; }
        public Footer FooterData { get; set; }
        public GenericBredcrumbNavigation GenericBredcrumbNavigation { get; set; }
        public string Heading { get; set; }
        public string HTMLText { get; set; }
        public string Image { get; set; }
        public string ImageAltText { get; set; }
        public string Designation { get; set; }
        public ContactFormData ContactFormData { get; set; }
    }
}