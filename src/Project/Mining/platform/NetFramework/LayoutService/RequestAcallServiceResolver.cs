using Project.Mining.Website.Services.ProjectListing;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.LayoutService
{
    public class RequestAcallServiceResolver : RenderingContentsResolver
    {
        private readonly IProjectListingService _rootResolver;

        public RequestAcallServiceResolver(IProjectListingService rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _rootResolver.GetRequestAcall(rendering);

        }
    }
}