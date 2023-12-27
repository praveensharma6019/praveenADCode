using Adani.SuperApp.Airport.Feature.CabVendor.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;

namespace Adani.SuperApp.Airport.Feature.CabVendor.Platform.LayoutServices
{
    public class FAQResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {

        private readonly IFAQ _details;

        public FAQResolver(IFAQ reasons)
        {
            this._details = reasons;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {

            return _details.GetFAQList(rendering, Convert.ToString(Sitecore.Context.Request.QueryString["sc_location"]));
        }
    }
}