using Project.AmbujaCement.Website.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService
{
    public class FooterServiceResolver : RenderingContentsResolver
    {
        private readonly IFooterServices rootResolver;

        public FooterServiceResolver(IFooterServices FooterdataResolver)
        {
            this.rootResolver = FooterdataResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return rootResolver.GetFooter(rendering);

        }
    }
}