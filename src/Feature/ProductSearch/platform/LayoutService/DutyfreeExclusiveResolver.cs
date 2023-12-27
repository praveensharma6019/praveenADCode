using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.LayoutService
{
    public class DutyfreeExclusiveResolver: Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IDutyfreeExclusive exclusiveOfferService;

        public DutyfreeExclusiveResolver(IDutyfreeExclusive exclusiveOfferService)
        {
            this.exclusiveOfferService = exclusiveOfferService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            string queryString = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["application"]) ? Sitecore.Context.Request.QueryString["application"].Trim().ToLower() : "web";
            string storeType = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["sc_storetype"]) ? Sitecore.Context.Request.QueryString["sc_storetype"].Trim().ToLower() : "departure";
            string airport = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["sc_airport"]) ? Sitecore.Context.Request.QueryString["sc_airport"].Trim().ToUpper() : "BOM";
            string language = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["sc_lang"]) ? Sitecore.Context.Request.QueryString["sc_lang"].Trim().ToLower() : "en";
            return exclusiveOfferService.GetExclusiveOffers(rendering, queryString, language, airport, storeType);
        }
    }
}