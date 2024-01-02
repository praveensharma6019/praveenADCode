using Project.AdaniInternationalSchool.Website.Services.Academics;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniInternationalSchool.Website.LayoutService
{
    public class AcademicsImageGalleryContentResolver : RenderingContentsResolver
    {
        private readonly IAcademicsService _service;

        public AcademicsImageGalleryContentResolver(IAcademicsService service)
        {
            _service = service;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _service.RenderImageGallery(rendering);
        }
    }
}