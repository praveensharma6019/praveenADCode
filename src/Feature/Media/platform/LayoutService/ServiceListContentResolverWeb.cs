 using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Airport.Feature.Media.Platform.Services;
using System;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.Media.Platform.LayoutService
{
    public class ServiceListContentResolverWeb : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IServiceListWeb serviceList;

        public ServiceListContentResolverWeb(IServiceListWeb serviceList)
        {
            this.serviceList = serviceList;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return this.serviceList.GetServicesData(rendering);
        }
    }
}