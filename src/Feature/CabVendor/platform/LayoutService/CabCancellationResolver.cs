using Adani.SuperApp.Airport.Feature.CabVendor.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.CabVendor.Platform.LayoutServices
{
    public class CabCancellationResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ICabCancellation _cancellationdata;

        public CabCancellationResolver(ICabCancellation cancellationdata)
        {
            this._cancellationdata = cancellationdata;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            try
            {
                return _cancellationdata.GetCancellationData(rendering);
            }
            catch (Exception ex)
            {
                throw new Exception("CabCancellationResolver throws Exception -> " + ex.Message);
            }
        }
    }
}