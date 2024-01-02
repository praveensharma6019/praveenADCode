using Project.MiningRenderingHost.Website.Models.Common;
using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Project.MiningRenderingHost.Website.Models
{
    public class DiscoverMiningBrochureModel:ImageModel
    {

        [SitecoreComponentField]
        public string? Description { get; set; }
        [SitecoreComponentField]
        public string? Heading { get; set; }
        [SitecoreComponentField]
        public string? SubHeading { get; set; }
        [SitecoreComponentField]
        public LinkData? CTALink { get; set; }
        [SitecoreComponentField]
        public ImageData? CtaImage { get; set; }

        [SitecoreComponentField]
        public string? Class { get; set; }
    }
}

