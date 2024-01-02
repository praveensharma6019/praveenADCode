using Adani.SuperApp.Airport.Feature.FNB.Platform.Models;
using Adani.SuperApp.Airport.Feature.FNB.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.LayoutServices
{
    public class TitleDescriptionResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {

        private readonly ITitleDescriptionAPI _titledescriptionApi;

        public TitleDescriptionResolver(ITitleDescriptionAPI titledescriptionApi)
        {
            this._titledescriptionApi = titledescriptionApi;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _titledescriptionApi.GetTitleDescriptionAPIData(rendering, Convert.ToString(Sitecore.Context.Request.QueryString["sc_storetype"]),
                                                      Convert.ToString(Sitecore.Context.Request.QueryString["sc_location"]),
                                                      Convert.ToString(Sitecore.Context.Request.QueryString["sc_terminaltype"]));
        }
    }
}