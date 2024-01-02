using System;
using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.LayoutService
{
    public class BrandListingServices : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IBrandListing brandListing;

        public BrandListingServices(IBrandListing brandListing)
        {
            this.brandListing = brandListing;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            string restricted = (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["sc_restricted"]))? System.Web.HttpContext.Current.Request.QueryString["sc_restricted"].Trim().ToLower() : "false";
            string storeType = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["sc_storetype"]) ? Sitecore.Context.Request.QueryString["sc_storetype"].Trim().ToLower() : "departure";
            string airport = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["sc_airport"]) ? Sitecore.Context.Request.QueryString["sc_airport"].Trim().ToLower() : "BOM";
            return brandListing.GetBrandListing(rendering, restricted, storeType, airport);

        }
    }
}