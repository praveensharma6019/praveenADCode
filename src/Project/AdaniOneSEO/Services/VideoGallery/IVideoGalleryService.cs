using Project.AdaniOneSEO.Website.Models.VideoGallery;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniOneSEO.Website.Services.VideoGallery
{
    public interface IVideoGalleryService
    {
        //VideoGalleryModel GetVideoGalleryModel(Rendering rendering);
        VideoGalleryModelNew GetVideoGalleryModel(Rendering rendering);
        //VideoDetailsPageModel GetVideoDetailsPageModel(Rendering rendering);
        VideoDetailsPageModelNew GetVideoDetailsPageModel(Rendering rendering);
    }
}
