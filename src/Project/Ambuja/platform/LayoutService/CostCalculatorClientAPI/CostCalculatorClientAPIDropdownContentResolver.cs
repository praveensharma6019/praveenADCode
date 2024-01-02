using Project.AmbujaCement.Website.Services.CostCalculatorClientAPI;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService.CostCalculatorClientAPI
{
    public class CostCalculatorClientAPIDropdownContentResolver : RenderingContentsResolver
    {
        private readonly ICostCalculatorClientAPIServices _costCalculatorclient;

        public CostCalculatorClientAPIDropdownContentResolver(ICostCalculatorClientAPIServices calculatorClientAPIServices)
        {
            _costCalculatorclient = calculatorClientAPIServices;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _costCalculatorclient.GetCostCalculatorClientAPIDropDownData(rendering);
        }
    }
}