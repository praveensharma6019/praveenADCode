using Adani.SuperApp.Realty.Feature.Property.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.LayoutService
{
    public class MapLocationContentResolver : RenderingContentsResolver
    {
        private readonly IProperyDataBasicService propertyDataBasicService;
        public MapLocationContentResolver(IProperyDataBasicService properyDataBasicService)
        {
            this.propertyDataBasicService = properyDataBasicService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return propertyDataBasicService.GetMapLocationData(rendering);

        }
    }
}
