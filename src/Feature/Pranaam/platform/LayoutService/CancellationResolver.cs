using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.Pranaam.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Pranaam.LayoutService
{
    public class CancellationResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ICancellation _obj;
        public CancellationResolver(ICancellation obj)
        {
            this._obj = obj;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            try
            {
                return _obj.GetCancellationData(rendering);
            }
            catch (Exception ex)
            {
                throw new Exception("CancellationResolver Resolver throws Exception -> " + ex.Message);
            }
        }
    }
}