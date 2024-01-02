using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService
{
    public class ServiceListResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IServiceListService _serviceList;
        public ServiceListResolver(IServiceListService servciceList)
        {
            this._serviceList = servciceList;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _serviceList.GetServiceListData(rendering);
        }
    }
}