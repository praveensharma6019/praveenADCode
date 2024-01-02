using Project.AmbujaCement.Website.Services.Header;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService
{
    public class HeaderServiceResolver : RenderingContentsResolver
    {
        private readonly IHeaderServices rootResolver;

        public HeaderServiceResolver(IHeaderServices HeaderdataResolver)
        {
            this.rootResolver = HeaderdataResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
          return rootResolver.GetHeader(rendering);
                    
        }
    }
}