using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using Project.Mining.Website.Services;

namespace Project.Mining.Website.LayoutService
{
    public class WhoWeAreServiceResolver : RenderingContentsResolver
    {
        private readonly IWhoWeAreService _rootResolver;

        public WhoWeAreServiceResolver(IWhoWeAreService rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
          return _rootResolver.GetWhoWeAreModel(rendering);
                    
        }
    }
}