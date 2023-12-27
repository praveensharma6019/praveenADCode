using Adani.SuperApp.Airport.Feature.Retail.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;

namespace Adani.SuperApp.Airport.Feature.Retail.Platform.LayoutServices
{
    public class RetailBestOffersResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {

        private readonly IBestOffers _offersData;

        public RetailBestOffersResolver(IBestOffers offersData)
        {
            this._offersData = offersData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {

            return _offersData.GetBestOffersData(rendering, Convert.ToString(Sitecore.Context.Request.QueryString["sc_outletcode"]), Convert.ToString(Sitecore.Context.Request.QueryString["isApp"]));

        }
    }
}