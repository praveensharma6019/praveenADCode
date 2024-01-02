using Adani.EV.Project.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.LayoutService
{
    public class CostCalulatorResolver : RenderingContentsResolver
    {
        private readonly ICostCalculatorService RootResolver;

        public CostCalulatorResolver(ICostCalculatorService costCalculatorService)
        {
            this.RootResolver = costCalculatorService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetCostCalulatorSectionsModel(rendering);

        }
    }
}