using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.Loyalty.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.LayoutService
{
    public class TerminalResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ITerminal terminal;
        public TerminalResolver(ITerminal _terminal)
        {
            this.terminal = _terminal;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {

                if (Sitecore.Context.Request.QueryString["sc_location"] ==null && !string.IsNullOrEmpty(Convert.ToString(Sitecore.Context.Request.QueryString["sc_location"])))
                {
                    return null;
                }
                else

                return terminal.GetTerminal(rendering,Convert.ToString(Sitecore.Context.Request.QueryString["sc_location"]));

        }
    }
}