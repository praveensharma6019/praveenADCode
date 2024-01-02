using Project.AdaniOneSEO.Website.Services.CityToCityPage;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniOneSEO.Website.LayoutServices.CityToCityPagev2
{
    public class CityToCityBannerContentResolver : RenderingContentsResolver
    {
        private readonly ICityToCityBannerWidgetService _rootresolver;

        public CityToCityBannerContentResolver(ICityToCityBannerWidgetService RootResolver)
        {
            _rootresolver = RootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _rootresolver.GetCityToCityBannerWidgetModel(rendering);
        }
    }
}