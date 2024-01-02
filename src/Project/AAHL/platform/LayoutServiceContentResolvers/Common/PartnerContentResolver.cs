using Project.AAHL.Website.Services.AboutUs;
using Project.AAHL.Website.Services.Common;
using Sitecore.ContentSearch.Utilities;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.Common
{
    public class PartnerContentResolver : RenderingContentsResolver
    {
        private readonly IAdvertisingSponsorships RootResolver;

        public PartnerContentResolver(IAdvertisingSponsorships dataResolver)
        {
            this.RootResolver = dataResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetPartner(rendering);
        }
    }
}