using Adani.SuperApp.Airport.Feature.Loyalty.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.LayoutService
{
    public class SocialMediaResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ISocialMediaLoyalty socialMediaLoyalty;
        public SocialMediaResolver(ISocialMediaLoyalty _socialMediaLoyalty)
        {
            this.socialMediaLoyalty = _socialMediaLoyalty;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {

            return socialMediaLoyalty.getMediaItems(rendering);

        }
    }
}