using Project.AmbujaCement.Website.Services.CostCalculatorClientAPI;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService.CostCalculatorClientAPI
{
    public class CostCalculatorClientAPIContentResolver : RenderingContentsResolver
    {
        private readonly ICostCalculatorClientAPIServices _costCalculatorclient;

        public CostCalculatorClientAPIContentResolver(ICostCalculatorClientAPIServices calculatorClientAPIServices)
        {
            _costCalculatorclient = calculatorClientAPIServices;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _costCalculatorclient.GetCostCalculatorClientAPIData(rendering);
        }
    }
}