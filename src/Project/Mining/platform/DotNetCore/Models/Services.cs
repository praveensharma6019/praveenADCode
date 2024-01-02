using Project.MiningRenderingHost.Website.Models.Common;
using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Project.MiningRenderingHost.Website.Models
{

    public class Services : SectionHeadings
    {
        [SitecoreComponentField]
        public List<OurServicesItemdata>? OurServicesdata { get; set; }      
        [SitecoreComponentField]
        public string? LeftArrowIcon { get; set; }
        [SitecoreComponentField]
        public string? RightArrowIcon { get; set; }
    }
    public class OurServicesItemdata : ImageModel
    {
        [SitecoreComponentField]
        public string? Description { get; set; }
        [SitecoreComponentField]
        public LinkData? Link { get; set; }       
        [SitecoreComponentField]
        public string? BgColor { get; set; }
        [SitecoreComponentField]
        public bool? IsSelected { get; set; }
    }
}
