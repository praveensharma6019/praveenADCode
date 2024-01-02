using Project.AdaniInternationalSchool.Website.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniInternationalSchool.Website.LayoutService
{
    public class UpcomingEventsContentResolver : RenderingContentsResolver
    {
        private readonly IUpcomingEventsService _service;

        public UpcomingEventsContentResolver(IUpcomingEventsService service)
        {
            _service = service;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _service.Render(rendering);
        }
    }
}