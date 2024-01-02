 using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Airport.Feature.Media.Platform.Services;
using System;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.Media.Platform.LayoutService
{
    public class ServiceListContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IServiceList serviceList;

        public ServiceListContentResolver(IServiceList serviceList)
        {
            this.serviceList = serviceList;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            string queryString = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["application"]) ? Sitecore.Context.Request.QueryString["application"].ToLower() : "";
            return this.serviceList.GetServicesData(rendering, queryString);
        }
    }
}