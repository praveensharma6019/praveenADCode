using Project.MiningRenderingHost.Website.Models.Common;
using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Project.MiningRenderingHost.Website.Models
{
    public class GtmData
    {
        [SitecoreComponentField]
        public string? Event { get; set; }
        [SitecoreComponentField]
        public string? Category { get; set; }
        [SitecoreComponentField]
        public string? Title { get; set; }
        [SitecoreComponentField]
        public string? Label { get; set; }
        [SitecoreComponentField]
        public string? Page_type { get; set; }
    }
    public class Header
    {
        [SitecoreComponentField]
        public string? HeaderLogo { get; set; }
        [SitecoreComponentField]
        public string? Logo { get; set; }
        [SitecoreComponentField]
        public string? MobileLogo { get; set; }
        [SitecoreComponentField]
        public string? BuLink { get; set; }
        [SitecoreComponentField]
        public string? LinkTarget { get; set; }
        [SitecoreComponentField]
        public string? BuLogoAltText { get; set; }
        [SitecoreComponentField]
        public bool? IsAbsolute { get; set; }
        [SitecoreComponentField]
        public string? AddOnClass { get; set; }
        [SitecoreComponentField]
        public List<NavDatum>? NavData { get; set; }
        [SitecoreComponentField]
        public List<TopbarList>? TopbarList { get; set; }
    }

    public class NavDatum
    {
        [SitecoreComponentField]
        public string? Link { get; set; }
        [SitecoreComponentField]
        public string? Class { get; set; }
        [SitecoreComponentField]
        public string? LinkText { get; set; }
        [SitecoreComponentField]
        public bool? HeaderCallback { get; set; }
        [SitecoreComponentField]
        public string? DefaultImage { get; set; }
        [SitecoreComponentField]
        public GtmData? GtmData { get; set; }
    }

    public class TopbarList
    {
        [SitecoreComponentField]
        public string? LinkIcon { get; set; }
        [SitecoreComponentField]
        public string? Link { get; set; }
        [SitecoreComponentField]
        public string? LinkText { get; set; }
        [SitecoreComponentField]
        public virtual string? HeaderLeftIcon { get; set; }
        [SitecoreComponentField]
        public virtual string? HeaderRightIcon { get; set; }
        [SitecoreComponentField]
        public GtmData? GtmData { get; set; }
    }
}
