using Project.AdaniOneSEO.Website.Services.VideoGallery;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniOneSEO.Website.LayoutServices.VideoGallery
{
    public class VideoGalleryContentResolver : RenderingContentsResolver
    {
        private readonly IVideoGalleryService _videoGalleryService;

        public VideoGalleryContentResolver(IVideoGalleryService videoGalleryService)
        {
            _videoGalleryService = videoGalleryService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _videoGalleryService.GetVideoGalleryModel(rendering);
        }
    }
}