using Project.AmbujaCement.Website.Services.Home;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService
{
    public class CardSliderContentResolver : RenderingContentsResolver
    {
        private readonly IHomeServices RootResolver;

        public CardSliderContentResolver(IHomeServices cardsliderData)
        {
            this.RootResolver = cardsliderData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetCardSlider(rendering);
        }
    }
}