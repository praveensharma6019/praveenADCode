using Project.AAHL.Website.Services.AboutUs;
using Project.AAHL.Website.Services.Common;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.Common
{
    public class CookiesContentResolver : RenderingContentsResolver
    {
        private readonly ICookiesService RootResolver;

        public CookiesContentResolver(ICookiesService cookiesServices)
        {
            this.RootResolver = cookiesServices;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetCookies(rendering);
        }
    }
}