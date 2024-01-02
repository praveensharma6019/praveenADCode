using Project.AmbujaCement.Website.Services.HomeBuilder;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService.HomeBuilder
{
    public class HomeBuilderContentResolver : RenderingContentsResolver
    {
        private readonly ISubNavService _subNav;

        public HomeBuilderContentResolver(ISubNavService subNavService)
        {
            _subNav = subNavService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _subNav.GetHomeBuilderCard(rendering);
        }
    }
}