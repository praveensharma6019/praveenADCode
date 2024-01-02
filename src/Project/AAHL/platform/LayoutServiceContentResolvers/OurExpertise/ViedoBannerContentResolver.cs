using Project.AAHL.Website.Services.Common;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers
{
    public class ViedoBannerContentResolver : RenderingContentsResolver
    {
        private readonly IOurExpertise rootResolver;

        public ViedoBannerContentResolver(IOurExpertise dataResolver)
        {
            this.rootResolver = dataResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return rootResolver.GetRetailVideoBannerModel(rendering);
        }
    }
}