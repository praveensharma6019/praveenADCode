using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Realty.Feature.Widget.Platform.Services;
using Sitecore.Mvc.Presentation;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Data.Items;
/// <summary>
/// read more articles component for blof detail page 
/// </summary>
namespace Adani.SuperApp.Realty.Feature.Widget.Platform.LayoutService
{
    public class OtherArticlesContentResolver : RenderingContentsResolver
    {
        private readonly IWidgetService widgetService;
        public OtherArticlesContentResolver(IWidgetService widgetService)
        {
            this.widgetService = widgetService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return widgetService.GetOtherArticlesList(rendering);

        }

    }
}