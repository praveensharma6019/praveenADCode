using Adani.SuperApp.Airport.Feature.Retail.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;

namespace Adani.SuperApp.Airport.Feature.Retail.Platform.LayoutServices
{
    public class RetailRefundPolicyResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {

        private readonly IRefundPolicy _refundData;

        public RetailRefundPolicyResolver(IRefundPolicy refundData)
        {
            this._refundData = refundData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {

            return _refundData.GetRefundPolicyData(rendering, Convert.ToString(Sitecore.Context.Request.QueryString["sc_outletcode"]));

        }
    }
}