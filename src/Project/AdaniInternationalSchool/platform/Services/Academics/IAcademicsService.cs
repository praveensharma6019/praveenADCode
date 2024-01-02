using Project.AdaniInternationalSchool.Website.Models.Academics;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Services.Academics
{
    public interface IAcademicsService
    {
        AcademicsModel RenderDetails(Rendering rendering);
        List<ImageGalleryItemModel> RenderImageGallery(Rendering rendering);
    }
}