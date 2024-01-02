using Project.AAHL.Website.Services.Common;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers
{
    public class AwardsAccoladesContentResolver : RenderingContentsResolver
    {
        private readonly IAwardsService rootResolver;

        public AwardsAccoladesContentResolver(IAwardsService HomedataResolver)
        {
            this.rootResolver = HomedataResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return rootResolver.GetAwardsAccolades(rendering);
        }
    }
}