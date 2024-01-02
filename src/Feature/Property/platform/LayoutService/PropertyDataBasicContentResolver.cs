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
    public class PropertyDataBasicContentResolver : RenderingContentsResolver
    {
        private readonly IProperyDataBasicService properyDataBasicService;
        public PropertyDataBasicContentResolver(IProperyDataBasicService properyDataBasicService)
        {
            this.properyDataBasicService = properyDataBasicService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return properyDataBasicService.GetPropertyBasicInfo(rendering.Item);

        }
    }
}