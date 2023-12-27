using Adani.SuperApp.Airport.Feature.Retail.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;

namespace Adani.SuperApp.Airport.Feature.Retail.Platform.LayoutServices
{
    public class RetailOutletResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {

        private readonly IRetailOutlet _outletData;

        public RetailOutletResolver(IRetailOutlet outletData)
        {
            this._outletData = outletData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {

            return _outletData.GetRetailOutletData(rendering, Convert.ToString(Sitecore.Context.Request.QueryString["sc_storetype"]),
                                                           Convert.ToString(Sitecore.Context.Request.QueryString["sc_location"]),
                                                           Convert.ToString(Sitecore.Context.Request.QueryString["sc_terminaltype"]),
                                                           Convert.ToString(Sitecore.Context.Request.QueryString["sc_outletcode"]),
                                                           Convert.ToString(Sitecore.Context.Request.QueryString["isApp"]));
        }
    }
}