using Project.Mining.Website.Services.Banner;
using Project.Mining.Website.Services.ProjectListing;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.LayoutService.ProjectListing
{
    public class ProjectListingServiceResolver : RenderingContentsResolver
    {
        private readonly IProjectListingService _rootResolver;

        public ProjectListingServiceResolver(IProjectListingService rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _rootResolver.GetProjectListing(rendering);

        }
    }
}