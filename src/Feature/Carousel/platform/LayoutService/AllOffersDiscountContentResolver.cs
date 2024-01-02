using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService
{
    public class AllOffersDiscountContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IOfferDiscount offerDiscountService;

        public AllOffersDiscountContentResolver(IOfferDiscount _offerDiscountService)
        {
            this.offerDiscountService = _offerDiscountService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            //Code Changes For Ticket No 18569
            if (Sitecore.Context.Request.QueryString["sc_apptype"] != null &&
                !string.IsNullOrEmpty(Convert.ToString(Sitecore.Context.Request.QueryString["sc_apptype"])) &&
                Sitecore.Context.Request.QueryString["sc_location"] != null &&
                !string.IsNullOrEmpty(Convert.ToString(Sitecore.Context.Request.QueryString["sc_location"])) &&
                Sitecore.Context.Request.QueryString["sc_module"] != null &&
                !string.IsNullOrEmpty(Convert.ToString(Sitecore.Context.Request.QueryString["sc_module"])) &&
                    rendering.Parameters.Contains("IsExclusive") && 
                        !string.IsNullOrEmpty(rendering.Parameters["IsExclusive"]) && 
                            Convert.ToBoolean(rendering.Parameters["IsExclusive"]))
            {
                return null;
              //return  offerDiscountService.GetOfferList(rendering, false,string.Empty,string.Empty,
              //                                               Convert.ToString(rendering.Parameters["IsExclusive"]),
              //                                               Convert.ToString(Sitecore.Context.Request.QueryString["sc_module"]),
              //                                               string.Empty,
              //                                               string.Empty,
              //                                               Convert.ToString(Sitecore.Context.Request.QueryString["sc_apptype"]));
            }
            else if (Sitecore.Context.Request.QueryString["sc_apptype"] != null &&
                !string.IsNullOrEmpty(Convert.ToString(Sitecore.Context.Request.QueryString["sc_apptype"])) &&
                Sitecore.Context.Request.QueryString["sc_location"] != null &&
                !string.IsNullOrEmpty(Convert.ToString(Sitecore.Context.Request.QueryString["sc_location"])) &&
                Sitecore.Context.Request.QueryString["sc_module"] != null &&
                !string.IsNullOrEmpty(Convert.ToString(Sitecore.Context.Request.QueryString["sc_module"])) &&
                    rendering.Parameters.Contains("IsBannerOffer") &&
                        !string.IsNullOrEmpty(rendering.Parameters["IsBannerOffer"]) &&
                            Convert.ToBoolean(rendering.Parameters["IsBannerOffer"]))
            {
                return null;
                //return offerDiscountService.GetOfferList(rendering, false, Convert.ToString(Sitecore.Context.Request.QueryString["sc_location"]),
                //                                             string.Empty,
                //                                             string.Empty,
                //                                             Convert.ToString(Sitecore.Context.Request.QueryString["sc_module"]),
                //                                             string.Empty,
                //                                             Convert.ToString(rendering.Parameters["IsBannerOffer"]),
                //                                             Convert.ToString(Sitecore.Context.Request.QueryString["sc_apptype"]));
            }
            else if (Sitecore.Context.Request.QueryString["sc_apptype"] != null &&
                !string.IsNullOrEmpty(Convert.ToString(Sitecore.Context.Request.QueryString["sc_apptype"])) &&
                Sitecore.Context.Request.QueryString["sc_location"] != null &&
                !string.IsNullOrEmpty(Convert.ToString(Sitecore.Context.Request.QueryString["sc_location"])) &&
                Sitecore.Context.Request.QueryString["sc_module"] != null &&
                !string.IsNullOrEmpty(Convert.ToString(Sitecore.Context.Request.QueryString["sc_module"])) &&
                    rendering.Parameters.Contains("IsOfferDiscount") &&
                        !string.IsNullOrEmpty(rendering.Parameters["IsOfferDiscount"]) &&
                            Convert.ToBoolean(rendering.Parameters["IsOfferDiscount"]))
            {
                return null;
                //return offerDiscountService.GetOfferList(rendering, false,string.Empty,
                //                                             string.Empty,string.Empty,
                //                                             Convert.ToString(Sitecore.Context.Request.QueryString["sc_module"]),
                //                                             Convert.ToString(rendering.Parameters["IsOfferDiscount"]),
                //                                             string.Empty,
                //                                             Convert.ToString(Sitecore.Context.Request.QueryString["sc_apptype"]));
            }
            return null;
        }
    }
}