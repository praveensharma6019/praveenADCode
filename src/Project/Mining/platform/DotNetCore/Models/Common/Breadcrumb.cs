using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Project.MiningRenderingHost.Website.Models.Common
{
    public class Breadcrumb
    {
        [SitecoreComponentField]
        public List<Breadcrumblist>? breadcrumbData { get; set; }

        public class Breadcrumblist
        {
            [SitecoreComponentField]
            public LinkData? CTALink { get; set; }
            [SitecoreComponentField]
            public string? Class { get; set; }

        }

    }
}
