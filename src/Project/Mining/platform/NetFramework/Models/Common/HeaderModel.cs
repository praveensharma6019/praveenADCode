using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.Mining.Website.Models.Common;
using Sitecore.Publishing.Pipelines.PublishItem;
using System.Collections.Generic;

namespace Project.Mining.Website.Models
{
    public class GtmData
    {
        public virtual string Event { get; set; }
        public virtual string Category { get; set; }
        public virtual string Title { get; set; }
        public virtual string Label { get; set; }
        public virtual string Page_type { get; set; }
    }
    public class HeaderModel
    {
        public virtual string HeaderLogo { get; set; }
        public virtual string Logo { get; set; }
        public virtual string MobileLogo { get; set; }
        public virtual string BuLink { get; set; }
        public virtual string LinkTarget { get; set; }
        public virtual string BuLogoAltText { get; set; }
        public virtual bool IsAbsolute { get; set; }
        public virtual string AddOnClass { get; set; }
        [SitecoreChildren]
        public virtual List<NavDatum> NavData { get; set; }
        [SitecoreChildren]
        public virtual List<TopbarList> TopbarList { get; set; }
    }
    public class NavDatum
    {
        public virtual string Link { get; set; }
        public virtual string LinkText { get; set; }
        public virtual bool HeaderCallback { get; set; }
        public virtual string DefaultImage { get; set; }
        public virtual GtmData GtmData { get; set; }
        public virtual string Class { get; set; }
    }

    public class TopbarList
    {
        public virtual string LinkText { get; set; }
        public virtual string LinkIcon { get; set; }
        public virtual string Link { get; set; }
        public virtual string HeaderLeftIcon { get; set; }
        public virtual string HeaderRightIcon { get; set; }
        public virtual GtmData GtmData { get; set; }
    }

}
