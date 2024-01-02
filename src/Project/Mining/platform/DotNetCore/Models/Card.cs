using Sitecore.AspNet.RenderingEngine.Binding.Attributes;
using Sitecore.LayoutService.Client.Response;

namespace Project.MiningRenderingHost.Website.Models
{
    public class Card
    {
        public SitecoreLayoutResponse? Response { get; set; }
        [SitecoreComponentField]
        public string? Title { get; set; }
        [SitecoreComponentField]
        public string? Description { get; set; }
        [SitecoreComponentField]
        public string? Link { get; set; }
        [SitecoreComponentField]
        public List<string>? multiList { get; set; }
        [SitecoreComponentField]
        public List<string>? checkList { get; set; }
        [SitecoreComponentField]
        public List<City>? City { get; set; }
    }
    public class City
    {
        public string? Value { get; set; }
        public string? Link { get; set; }
    }
}
