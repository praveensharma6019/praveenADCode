using Project.AdaniInternationalSchool.Website.Services.EnrollNow;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniInternationalSchool.Website.LayoutService.EnrollNow
{
    public class EnrollNowOverviewContentResolver : RenderingContentsResolver
    {
        private readonly IEnrollNowService _enrollNowService;
        public EnrollNowOverviewContentResolver(IEnrollNowService enrollNowService)
        {
            _enrollNowService = enrollNowService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _enrollNowService.GetOverviewModel(rendering);
        }
    }
}