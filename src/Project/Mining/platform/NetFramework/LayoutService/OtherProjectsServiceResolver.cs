using Project.Mining.Website.Home;
using Project.Mining.Website.Services;
using Project.Mining.Website.Services.Banner;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.LayoutService
{
    public class OtherProjectsServiceResolver : RenderingContentsResolver
    {
        private readonly IOtherProjectsService _rootResolver;

        public OtherProjectsServiceResolver(IOtherProjectsService rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
          return _rootResolver.GetOtherProject(rendering);
                    
        }
    }
}