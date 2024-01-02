using Project.AmbujaCement.Website.Services.Home;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService
{
    public class HomeCurriculumContentResolver : RenderingContentsResolver
    {
        private readonly IHomeServices RootResolver;

        public HomeCurriculumContentResolver(IHomeServices homeCurriculumData)
        {
            this.RootResolver = homeCurriculumData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetCurriculum(rendering);
        }
    }
}