using Adani.SuperApp.Airport.Feature.FNB.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.LayoutServices
{
    public class FNBbankoffersResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {

        private readonly IFnBbankoffers _bankoffers;

        public FNBbankoffersResolver(IFnBbankoffers bankoffers)
        {
            this._bankoffers = bankoffers;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {

            return _bankoffers.GetFnBbankoffersData(rendering, Convert.ToString(Sitecore.Context.Request.QueryString["sc_storetype"]),
                                                           Convert.ToString(Sitecore.Context.Request.QueryString["sc_location"]),
                                                           Convert.ToString(Sitecore.Context.Request.QueryString["sc_terminaltype"]),
                                                           Convert.ToString(Sitecore.Context.Request.QueryString["sc_outletcode"]));
        }
    }
}