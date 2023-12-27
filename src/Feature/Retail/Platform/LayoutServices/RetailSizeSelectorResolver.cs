using Adani.SuperApp.Airport.Feature.Retail.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;

namespace Adani.SuperApp.Airport.Feature.Retail.Platform.LayoutServices
{
    public class RetailSizeSelectorResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {

        private readonly ISizeSelect _sizesData;

        public RetailSizeSelectorResolver(ISizeSelect sizesData)
        {
            this._sizesData = sizesData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {

            return _sizesData.GetSizesData(rendering, Convert.ToString(Sitecore.Context.Request.QueryString["sc_outletcode"]), Convert.ToString(Sitecore.Context.Request.QueryString["sc_category"]), Convert.ToString(Sitecore.Context.Request.QueryString["isApp"]));

        }
    }
}