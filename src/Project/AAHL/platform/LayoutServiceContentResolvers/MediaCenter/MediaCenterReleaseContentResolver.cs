using Project.AAHL.Website.Services.Common;
using Project.AAHL.Website.Services.MediaCenter;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.MediaCenter
{
    public class MediaCenterReleaseContentResolver : RenderingContentsResolver
    {
        private readonly IMediaCenterServices rootResolver;

        public MediaCenterReleaseContentResolver(IMediaCenterServices mediaCenterServices)
        {
            this.rootResolver = mediaCenterServices;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return rootResolver.GetCenterRelease(rendering);
        }
    }
}