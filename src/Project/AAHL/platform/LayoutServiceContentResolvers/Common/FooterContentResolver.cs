using Project.AAHL.Website.Services.Common;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.Common
{
    public class FooterContentResolver : RenderingContentsResolver
    {
        private readonly IFooter rootResolver;

        public FooterContentResolver(IFooter HomedataResolver)
        {
            this.rootResolver = HomedataResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return rootResolver.GetFooter(rendering);
        }
    }
}