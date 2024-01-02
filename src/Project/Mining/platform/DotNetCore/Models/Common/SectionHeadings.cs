using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Project.MiningRenderingHost.Website.Models.Common
{
    public class SectionHeadings
    {
        [SitecoreComponentField]
        public string? Heading { get; set; }
        [SitecoreComponentField]
        public string? Description { get; set; }
        [SitecoreComponentField]
        public string? SubHeading { get; set; }
    }
}
