using Project.AAHL.Website.Services.CompetetiveAdvantage;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.CompetetiveAdvantage
{
    public class SectionWithImagesContentResolver : RenderingContentsResolver
    {
        private readonly ICompetetiveAdvantageService _rootResolver;

        public SectionWithImagesContentResolver(ICompetetiveAdvantageService rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _rootResolver.GetSectionWithImages(rendering);
        }
    }
}