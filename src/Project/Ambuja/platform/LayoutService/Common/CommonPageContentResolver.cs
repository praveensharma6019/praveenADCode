using Project.AmbujaCement.Website.Services.Common;
using Project.AmbujaCement.Website.Services.Home;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService.Common
{
    public class CommonPageContentResolver : RenderingContentsResolver
    {
        private readonly IChecklistService RootResolver;

        public CommonPageContentResolver(IChecklistService checklistService)
        {
            this.RootResolver = checklistService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetItemData(rendering);
        }
    }
}