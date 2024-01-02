using Project.AmbujaCement.Website.Services.AboutUs;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService.AboutUs
{
    public class ICanCardsContentResolver : RenderingContentsResolver
    {
        private readonly IAboutUsService _aboutUsService;

        public ICanCardsContentResolver(IAboutUsService aboutUsService)
        {
            _aboutUsService = aboutUsService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _aboutUsService.GetICanCardsModel(rendering);
        }
    }
}