using Adani.SuperApp.Airport.Feature.FNB.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.LayoutServices
{
    public class OffersOutletResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {

        private readonly IOffersOutlet _offersData;

        public OffersOutletResolver(IOffersOutlet offersoutletData)
        {
            this._offersData = offersoutletData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {

            return _offersData.GetOffersOutletData(rendering, Convert.ToString(Sitecore.Context.Request.QueryString["sc_storetype"]),
                                                           Convert.ToString(Sitecore.Context.Request.QueryString["sc_location"]),
                                                           Convert.ToString(Sitecore.Context.Request.QueryString["sc_terminaltype"]));
        }
    }
}