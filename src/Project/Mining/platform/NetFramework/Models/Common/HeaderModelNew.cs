using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Mining.Website.Models.Common
{
    public class HeaderModelNew  : GtmDataModel
    {

        public virtual Image HeaderLogo { get; set; }
        public virtual Image Logo { get; set; }
        public virtual Image MobileLogo { get; set; }
        public virtual Link BuLink { get; set; }
        public virtual string LinkTarget { get; set; }
        public virtual string BuLogoAltText { get; set; }
        public virtual bool IsAbsolute { get; set; }
        public virtual string AddOnClass { get; set; }

        [SitecoreField("NavData")]
        public virtual IEnumerable<NavDatumNew> NavData { get; set; }
        [SitecoreField("TopbarList")]
        public virtual IEnumerable<TopbarListNew> TopbarList { get; set; }

        [SitecoreField("HamburgerList")]
        public virtual IEnumerable<HamburgerList> HamburgerList { get; set; }
       
    }

    public class HamburgerList :GtmDataModel
    {
        public virtual string Heading { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<HamburgerMainNavigation> HamburgerMainNavigation { get; set; }
    }

    public class HamburgerMainNavigation : GtmDataModel
    {
        public virtual Link Link { get; set; }
        public virtual string CTAText { get; set; }
        public virtual Link CTALink { get; set; }
        public virtual string CtaClass { get; set; }
        public virtual string Heading { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<HamburgerSubNavigation> HamburgerSubNavigation { get; set; }
    }

    public class HamburgerSubNavigation
    {
        public virtual Link Link { get; set; }
        public virtual string CtaClass { get; set; }
        public virtual string Heading { get; set; }
    }

    public class NavDatumNew : GtmDataModel
    {
        public virtual Link Link { get; set; }
        public virtual string LinkText { get; set; }
        public virtual bool HeaderCallback { get; set; }
        public virtual Image DefaultImage { get; set; }
        //public virtual GtmData GtmData { get; set; }
        public virtual string Class { get; set; }
    }

    public class TopbarListNew : GtmDataModel
    {
        public virtual string LinkText { get; set; }
        public virtual string LinkIcon { get; set; }
        public virtual Link Link { get; set; }
        public virtual string HeaderLeftIcon { get; set; }
        public virtual string HeaderRightIcon { get; set; }
        // public virtual GtmData GtmData { get; set; }
    }
}