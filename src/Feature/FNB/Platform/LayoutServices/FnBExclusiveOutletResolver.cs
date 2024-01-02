using Adani.SuperApp.Airport.Feature.FNB.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.LayoutServices
{
    public class FnBExclusiveOutletResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {

        private readonly IFnBExclusiveOutlet _exclusiveOutlet;

        public FnBExclusiveOutletResolver(IFnBExclusiveOutlet exclusiveOutlet)
        {
            this._exclusiveOutlet = exclusiveOutlet;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {

            return _exclusiveOutlet.GetExclusiveOutletdata(rendering, Convert.ToString(Sitecore.Context.Request.QueryString["sc_storetype"]), 
                                                           Convert.ToString(Sitecore.Context.Request.QueryString["sc_location"]),
                                                           Convert.ToString(Sitecore.Context.Request.QueryString["sc_terminaltype"]),
                                                           Convert.ToString(Sitecore.Context.Request.QueryString["isApp"]));
        }
    }
}