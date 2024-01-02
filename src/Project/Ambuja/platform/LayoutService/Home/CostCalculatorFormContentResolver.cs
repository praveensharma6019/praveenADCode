using Project.AmbujaCement.Website.Services.Home;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService
{
    public class CostCalculatorFormContentResolver : RenderingContentsResolver
    {
        private readonly IHomeServices RootResolver;

        public CostCalculatorFormContentResolver(IHomeServices categoryData)
        {
            this.RootResolver = categoryData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetCostCalculatorFormData(rendering);
        }
    }
}