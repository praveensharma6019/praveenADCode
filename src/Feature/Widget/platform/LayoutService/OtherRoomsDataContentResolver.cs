using Adani.SuperApp.Realty.Feature.Widget.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Widget.Platform.LayoutService
{
    public class OtherRoomsDataContentResolver : RenderingContentsResolver
    {
        private readonly IWidgetService widgetService;
        public OtherRoomsDataContentResolver(IWidgetService widgetService)
        {
            this.widgetService = widgetService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return widgetService.GetOtherRoomsData(rendering);
        }
    }
}