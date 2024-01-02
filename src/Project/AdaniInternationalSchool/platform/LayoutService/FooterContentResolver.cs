using Project.AdaniInternationalSchool.Website.Services.Footer;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniInternationalSchool.Website.LayoutService
{
    public class FooterContentResolver : RenderingContentsResolver
    {
        private readonly IFooterService _service;

        public FooterContentResolver(IFooterService service)
        {
            _service = service;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _service.Render(rendering);
        }
    }
}