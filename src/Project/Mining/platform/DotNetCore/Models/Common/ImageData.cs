using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Project.MiningRenderingHost.Website.Models.Common
{
    public class ImageData
    {
        [SitecoreComponentField]
        public string? Alt { get; set; }
        [SitecoreComponentField]
        public string? Border { get; set; }
        [SitecoreComponentField]
        public string? Class { get; set; }
        [SitecoreComponentField]
        public int? Height { get; set; }
        [SitecoreComponentField]
        public int? HSpace { get; set; }
        [SitecoreComponentField]
        public string? Src { get; set; }
        [SitecoreComponentField]
        public int? VSpace { get; set; }
        [SitecoreComponentField]
        public int? Width { get; set; }
        [SitecoreComponentField]
        public string? MediaId { get; set; }
        [SitecoreComponentField]
        public string? Title { get; set; }
        [SitecoreComponentField]
        public LanguageData? Language { get; set; }
        [SitecoreComponentField]
        public bool? MediaExists { get; set; }
    }

    public class LanguageData
    {
        [SitecoreComponentField]
        public string? Name { get; set; }
    }
}
