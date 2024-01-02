using Project.MiningRenderingHost.Website.Models.Common;
using Sitecore.AspNet.RenderingEngine.Binding;
using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Project.MiningRenderingHost.Website.Models
{
    public class Footer : ImageModel
    {       
        [SitecoreComponentField]
        public List<NavigationList>? MainNavigations { get; set; }
        [SitecoreComponentField]
        public string? Copyright { get; set; }
    }
    public class NavigationList
    {
        [SitecoreComponentField]
        public string? Heading { get; set; }
        [SitecoreComponentField]
        public List<NavigationListItems>? Items { get; set; }
    }
    public class NavigationListItems : ImageModel
    {
        [SitecoreComponentField]
        public LinkData? Link { get; set; }
    }
}
