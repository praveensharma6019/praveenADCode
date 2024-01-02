using Adani.SuperApp.Airport.Feature.FNB.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.LayoutServices
{
    public class CancellationResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {

        private readonly ICancellationReasons _cancellationReasons;

        public CancellationResolver(ICancellationReasons reasons)
        {
            this._cancellationReasons = reasons;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {

            return _cancellationReasons.GetCancellationReasons(rendering, Convert.ToString(Sitecore.Context.Request.QueryString["isApp"]));
        }
    }
}