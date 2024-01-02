using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.Loyalty.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.LayoutService
{
    public class Earn2XResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IEarn2XProduct earn2XLoyaltyProduct;
        public Earn2XResolver(IEarn2XProduct _earn2XLoyaltyProduct)
        {
            this.earn2XLoyaltyProduct = _earn2XLoyaltyProduct;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {

                if (Sitecore.Context.Request.QueryString["sc_type"] ==null && !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["sc_type"].ToString()))
                {
                    return null;
                }
                else

                return earn2XLoyaltyProduct.get2xLoyaltyProductData(rendering,Convert.ToString(Sitecore.Context.Request.QueryString["sc_type"]));

        }
    }
}