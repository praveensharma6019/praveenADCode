using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using Project.Mining.Website.Services.OurProjects;

namespace Project.Mining.Website.LayoutService
{
    public class DiscoverMiningBrochureServiceResolver : RenderingContentsResolver
    {
        private readonly IDiscoverMiningBrochureServices rootResolver;

        public DiscoverMiningBrochureServiceResolver(IDiscoverMiningBrochureServices OurProjectsdataResolver)
        {
            this.rootResolver = OurProjectsdataResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
          return rootResolver.GetDiscoverMiningBrochure(rendering);
                    
        }
    }
}