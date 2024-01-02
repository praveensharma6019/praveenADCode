using Project.AmbujaCement.Website.Services.AboutUs;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService.AboutUs
{
    public class VisionMissionContentResolver : RenderingContentsResolver
    {
        private readonly IAboutUsService _aboutUsService;

        public VisionMissionContentResolver(IAboutUsService aboutUsService)
        {
            _aboutUsService = aboutUsService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _aboutUsService.GetVisionMissionCardModel(rendering);
        }
    }
}