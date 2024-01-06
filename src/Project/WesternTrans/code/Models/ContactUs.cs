using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTrans.Website.Models
{
    public class ContactUs
    {
        public Header HeaderData { get; set; }
        public Cookie CookieData { get; set; }       
        public GenericBredcrumbNavigation GenericBredcrumbNavigation { get; set; }               
        public ContactDetail ContactDetailData { get; set; }
        public Footer FooterData { get; set; }
    }
}