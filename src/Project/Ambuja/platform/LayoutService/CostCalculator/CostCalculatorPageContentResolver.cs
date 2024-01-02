using Project.AmbujaCement.Website.Services.AboutUs;
using Project.AmbujaCement.Website.Services.CostCalculator;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService.CostCalculator
{
    public class CostCalculatorPageContentResolver : RenderingContentsResolver
    {
        private readonly ICostCalculatorServices _costCalculator;

        public CostCalculatorPageContentResolver(ICostCalculatorServices costCalculatorServices)
        {
            _costCalculator = costCalculatorServices;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _costCalculator.GetCostCalculatorPageData(rendering);
        }
    }
}