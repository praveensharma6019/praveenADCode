using Sitecore.AspNet.RenderingEngine.Binding.Attributes;
using Project.MiningRenderingHost.Website.Models.Common;

namespace Project.MiningRenderingHost.Website.Models
{
    public class MainBannerCarousel
    {
        [SitecoreComponentField]
        public List<BannermodelDetails>? MainBannerData { get; set; }
    }

    public class BannermodelDetails : ImageModel
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
