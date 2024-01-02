using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.LayoutService
{
    public class ProductFiltersResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IFilterProductsService filterProductsService;

        public ProductFiltersResolver(IFilterProductsService filterProductsService)
        {
            this.filterProductsService = filterProductsService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            string queryString = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["application"]) ? Sitecore.Context.Request.QueryString["application"].Trim().ToLower() : "web";
            string storeType = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["sc_storetype"]) ? Sitecore.Context.Request.QueryString["sc_storetype"].Trim().ToLower() : "departure";
            string airport = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["sc_airport"]) ? Sitecore.Context.Request.QueryString["sc_airport"].Trim().ToUpper() : "BOM";
            string language = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["sc_lang"]) ? Sitecore.Context.Request.QueryString["sc_lang"].Trim().ToLower() : "en";
            bool isAirportHome = (!string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["item"]) && Sitecore.Context.Request.QueryString["item"].ToLower().Trim().IndexOf("airport-web") >-1) ? true : false;
            bool restricted = (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["sc_restricted"]) && System.Web.HttpContext.Current.Request.QueryString["sc_restricted"].Trim().ToLower().Equals("true")) ? true : false;
            return filterProductsService.GetProductFilters(rendering, queryString, language, airport, storeType, isAirportHome, restricted);
        }
    }
}