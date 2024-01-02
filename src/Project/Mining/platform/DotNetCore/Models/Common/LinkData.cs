using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Project.MiningRenderingHost.Website.Models.Common
{
    public class LinkData
    {
        [SitecoreComponentField]
        public string? Anchor { get; set; }
        [SitecoreComponentField]
        public string? Class { get; set; }
        [SitecoreComponentField]
        public string? Text { get; set; }
        [SitecoreComponentField]
        public string? Query { get; set; }
        [SitecoreComponentField]
        public string? Title { get; set; }
        [SitecoreComponentField]
        public string? URL { get; set; }
        [SitecoreComponentField]
        public string? Target { get; set; }
        [SitecoreComponentField]
        public string? TargetId { get; set; }
        [SitecoreComponentField]
        public int? Type { get; set; }
        [SitecoreComponentField]
        public string? Style { get; set; }
    }
}
