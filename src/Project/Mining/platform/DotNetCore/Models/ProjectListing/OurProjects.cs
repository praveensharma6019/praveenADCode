using Project.MiningRenderingHost.Website.Models.Common;
using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Project.MiningRenderingHost.Website.Models.ProjectListing
{
    public class OurProjects : SectionHeadings
    {
        [SitecoreComponentField]
        public List<OurServicesDataItems>? OurServicesdata { get; set; }
    }
    public class OurServicesDataItems : ImageModel
    {
        [SitecoreComponentField]
        public string? Description { get; set; }
        [SitecoreComponentField]
        public string? Heading { get; set; }
        [SitecoreComponentField]
        public LinkData? Link { get; set; }       
    }
}
