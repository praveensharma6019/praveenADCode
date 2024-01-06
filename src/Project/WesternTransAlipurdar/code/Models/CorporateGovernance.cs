using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTransAlipurdar.Website.Models
{
    public class CorporateGovernance
    {       
        public Header HeaderData { get; set; }
        public Cookie CookieData { get; set; }
        public GenericBredcrumbNavigation GenericBredcrumbNavigation { get; set; }               
        public CorporateGoveranceProfiles CorporateGoveranceProfiles { get; set; }
        public PageContent PageContent { get; set; }
        public InThisSection InThisSection { get; set; }
        public Footer FooterData { get; set; }
        public ContactFormData ContactFormData { get; set; }

    }
    public class CorporateGoveranceProfiles
    {
        public string Heading { get; set; }
        public List<ProfileSections> ProfileSections { get; set; }
    }

    public class ProfileSections
    {
        public string Heading { get; set; }
        public string HTMLText { get; set; }
        public List<ProfileSectionItems> ProfileSectionItems { get; set; }
    }
    public class ProfileSectionItems
    {
        public string Heading { get; set; }
        public string HTMLText { get; set; }
        public string Image { get; set; }
        public string ImageAltText { get; set; }
    }
}