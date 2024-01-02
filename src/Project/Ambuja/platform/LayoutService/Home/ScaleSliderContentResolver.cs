using Project.AmbujaCement.Website.Services.Home;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService
{
    public class ScaleSliderContentResolver : RenderingContentsResolver
    {
        private readonly IHomeServices RootResolver;

        public ScaleSliderContentResolver(IHomeServices scaleliderData)
        {
            this.RootResolver = scaleliderData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetScaleSlider(rendering);
        }
    }
}