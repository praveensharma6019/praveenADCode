 using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Airport.Feature.Media.Platform.Services;
using System;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.Media.Platform.LayoutService
{
    public class CustomContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ICustomContent customContent;

        public CustomContentResolver(ICustomContent _customContent)
        {
            this.customContent = _customContent;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            string queryString = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["application"]) ? Sitecore.Context.Request.QueryString["application"].ToLower() : "";
            string cityqueryString = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["citycode"]) ? Sitecore.Context.Request.QueryString["citycode"].ToLower() : "";
            return this.customContent.GetCustomContentData(rendering, queryString, cityqueryString);
        }
    }
}