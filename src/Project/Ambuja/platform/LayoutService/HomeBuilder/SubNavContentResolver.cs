using Project.AmbujaCement.Website.Services.AboutUs;
using Project.AmbujaCement.Website.Services.HomeBuilder;
using Project.AmbujaCement.Website.Services.Sitemap;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService.HomeBuilder
{
    public class SubNavContentResolver : RenderingContentsResolver
    {
        private readonly ISubNavService _subNav;

        public SubNavContentResolver(ISubNavService subNavService)
        {
            _subNav = subNavService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _subNav.GetSubNav(rendering);
        }
    }
}