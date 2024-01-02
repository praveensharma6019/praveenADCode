using Adani.SuperApp.Airport.Feature.FNB.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.LayoutServices
{
    public class CartFNBcontentresolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {

        private readonly ICartFNBservice _cartFNBData;

        public CartFNBcontentresolver(ICartFNBservice cartFNBData)
        {
            this._cartFNBData = cartFNBData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {

            return _cartFNBData.GetCartFNBAPIData(rendering, Convert.ToString(Sitecore.Context.Request.QueryString["isApp"]));
                                                          
        }
    }


}