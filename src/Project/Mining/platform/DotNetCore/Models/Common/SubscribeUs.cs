using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Project.MiningRenderingHost.Website.Models.Common
{
    public class SubscribeUs : ImageModel
    {
        [SitecoreComponentField]
        public string? Heading { get; set; }
        [SitecoreComponentField]
        public string? Description { get; set; }
        [SitecoreComponentField]
        public LinkData? Link { get; set; }
    }
}
