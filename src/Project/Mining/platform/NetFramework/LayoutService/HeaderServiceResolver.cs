using Project.Mining.Website.Services.Header;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.LayoutService
{
    public class HeaderServiceResolver : RenderingContentsResolver
    {
        private readonly IHeaderService _rootResolver;

        public HeaderServiceResolver(IHeaderService rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
          return _rootResolver.GetHeader(rendering);
                    
        }
    }
}