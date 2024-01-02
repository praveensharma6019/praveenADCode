using Project.AmbujaCement.Website.Services.AboutUs;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService.AboutUs
{
    public class AchievementsContentResolver : RenderingContentsResolver
    {
        private readonly IAboutUsService _aboutUsService;

        public AchievementsContentResolver(IAboutUsService aboutUsService)
        {
            _aboutUsService = aboutUsService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _aboutUsService.GetAchievementsModel(rendering);
        }
    }
}