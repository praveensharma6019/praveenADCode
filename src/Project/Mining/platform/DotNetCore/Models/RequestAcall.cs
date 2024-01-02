using Project.MiningRenderingHost.Website.Models.Common;
using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Project.MiningRenderingHost.Website.Models
{
    public class RequestAcall
    {
        [SitecoreComponentField]
        public string? Heading { get; set; }
        [SitecoreComponentField]
        public string? SubHeading { get; set; }
        [SitecoreComponentField]
        public string? Bgcolor { get; set; }
        [SitecoreComponentField]
        public LinkData? Link { get; set; }
    }
}
