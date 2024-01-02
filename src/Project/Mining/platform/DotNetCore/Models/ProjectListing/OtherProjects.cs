using Project.MiningRenderingHost.Website.Models.Common;
using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Project.MiningRenderingHost.Website.Models.ProjectListing
{
    public class OtherProjects : SectionHeadings
    {
        [SitecoreComponentField]
        public List<OurServicesData>? Projects { get; set; }
        [SitecoreComponentField]
        public string? BgColor { get; set; }
    }
    public class OurServicesData : ImageModel
    {
        [SitecoreComponentField]
        public string? SubHeading { get; set; }
        [SitecoreComponentField]
        public string? Heading { get; set; }
        [SitecoreComponentField]
        public LinkData? CTALink { get; set; }
        [SitecoreComponentField]
        public string? CTAClass { get; set; }
        [SitecoreComponentField]
        public string? BgColor { get; set; }
    }
}
