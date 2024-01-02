using Adani.SuperApp.Realty.Feature.JSSComponents.Platform.Services.Common;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Realty.Feature.JSSComponents.Platform.LayoutService
{
    public class HomeFAQContentResolver : RenderingContentsResolver
    {
        private readonly ICommonComponents RootResolver;

        public HomeFAQContentResolver(ICommonComponents homeFAQData)
        {
            this.RootResolver = homeFAQData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetHomeFAQ(rendering);
        }
    }
}