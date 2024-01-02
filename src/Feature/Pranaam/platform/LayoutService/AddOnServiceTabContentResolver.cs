using System;
using Adani.SuperApp.Airport.Feature.Pranaam.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;

namespace Adani.SuperApp.Airport.Feature.Pranaam.LayoutService
{
    public class AddOnServiceTabContentResolver : RenderingContentsResolver
    {
        private readonly IAddOnServiceTab _addOnServiceTab;
        public AddOnServiceTabContentResolver(IAddOnServiceTab addOnServiceTab)
        {
            this._addOnServiceTab = addOnServiceTab;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            try
            {
                return _addOnServiceTab.GetServiceTab(rendering);
            }
            catch (Exception ex)
            {
                throw new Exception("AddOnServiceTabContentResolver throws Exception -> " + ex.Message);
            }
        }
    }
}