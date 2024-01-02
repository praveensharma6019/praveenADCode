using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Project.MiningRenderingHost.Website.Models.Common
{
    public class ImageModel
    {
        [SitecoreComponentField]
        public ImageData? Image { get; set; } 
        [SitecoreComponentField]
        public ImageData? MobileImage { get; set; }
        [SitecoreComponentField]
        public ImageData? TabletImage { get; set; }
    }
}
