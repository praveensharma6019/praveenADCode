using Adani.SuperApp.Realty.Feature.Property.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.LayoutService
{
    public class PropertyDataEmiCalculatorContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IProperyDataBasicService properyDataBasicService;
        public PropertyDataEmiCalculatorContentResolver(IProperyDataBasicService properyDataBasicService)
        {
            this.properyDataBasicService = properyDataBasicService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return properyDataBasicService.GetEmiCalculator(rendering.Item);

        }
    }
}