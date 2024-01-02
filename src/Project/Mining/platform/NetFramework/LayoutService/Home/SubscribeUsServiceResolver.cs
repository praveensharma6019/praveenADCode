using Project.Mining.Website.Services.Banner;
using Project.Mining.Website.Services.Home;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.LayoutService.Home
{
    public class SubscribeUsServiceResolver : RenderingContentsResolver
    {
        private readonly ISubscribeUsService _rootResolver;

        public SubscribeUsServiceResolver(ISubscribeUsService rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _rootResolver.GetSubscribeUs(rendering);

        }
    }
}