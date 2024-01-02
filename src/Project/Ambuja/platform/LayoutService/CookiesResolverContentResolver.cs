using Project.AmbujaCement.Website.Services.Home;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService
{

    public class CookiesResolverContentResolver : RenderingContentsResolver
    {
        private readonly IHomeServices RootResolver;
        public CookiesResolverContentResolver(IHomeServices cookiesData)
        {
            this.RootResolver = cookiesData;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetCookies(rendering);
        }
    }

}
