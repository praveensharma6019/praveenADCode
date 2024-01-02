using Project.Mining.Website.Home;
using Project.Mining.Website.Services.Banner;
using Project.Mining.Website.Services.Header;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.LayoutService
{
    public class ProjectDetailsServiceResolver : RenderingContentsResolver
    {
        private readonly IProjectDetailsService _rootResolver;

        public ProjectDetailsServiceResolver(IProjectDetailsService rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
          return _rootResolver.GetProjectDetail(rendering);
                    
        }
    }
}