using Project.AmbujaCement.Website.Services.Common;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService
{
    public class ChecklistServiceResolver : RenderingContentsResolver
    {
        private readonly IChecklistService _rootResolver;

        public ChecklistServiceResolver(IChecklistService rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _rootResolver.GetChecklist(rendering);

        }
    }
}