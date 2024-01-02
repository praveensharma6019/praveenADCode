using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Realty.Feature.Widget.Platform.Services;
using Sitecore.Mvc.Presentation;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Realty.Feature.Widget.Platform.LayoutService
{
    public class CategoryLifestyleContentResolver : RenderingContentsResolver
    {
        private readonly IWidgetService widgetService;
        public CategoryLifestyleContentResolver(IWidgetService widgetService)
        {
            this.widgetService = widgetService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return widgetService.GetCatagoryLIfestylItem(rendering);
        }
    }
}