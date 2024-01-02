using Sitecore.AspNet.RenderingEngine.Binding.Attributes;
using Project.MiningRenderingHost.Website.Models.Common;

namespace Project.MiningRenderingHost.Website.Models.Common
{
    public class Banner : ImageModel
    {
        [SitecoreComponentField]
        public LinkData? CtaLink { get; set; }
        [SitecoreComponentField]
        public string? Description { get; set; }
        [SitecoreComponentField]
        public string? SubHeading { get; set; }
        [SitecoreComponentField]
        public string? Heading { get; set; }
    }
}
