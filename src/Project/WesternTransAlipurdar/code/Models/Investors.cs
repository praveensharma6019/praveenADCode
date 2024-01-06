using Sitecore.WesternTransAlipurdar.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTransAlipurdar.Website
{
    public class Investors
    {
        public Header HeaderData { get; set; }
        public Cookie CookieData { get; set; }
        public GenericBredcrumbNavigation GenericBredcrumbNavigation { get; set; }
        public InvestorTiles InvestorTitles { get; set; }
        public Footer FooterData { get; set; }
        public ContactFormData ContactFormData { get; set; }

    }

    public class InvestorTiles
    {
        public string Heading { get; set; }
        public List<PageContent> Data { get; set; }
    }
}