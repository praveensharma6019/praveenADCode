using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Sitecore.Data.Items;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService
{
    public class OffersAndDiscountsResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IOffersAndDiscountsService data;
        private readonly ILogRepository logger;

        public OffersAndDiscountsResolver(IOffersAndDiscountsService offersanddiscount, ILogRepository _logging)
        {
            this.data = offersanddiscount;
            this.logger = _logging;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            try
            {
                var datasource = RenderingContext.Current.Rendering.Item;

                string queryString = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["application"]) ? Sitecore.Context.Request.QueryString["application"].ToLower() : "";
                string cityqueryString = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["citycode"]) ? Sitecore.Context.Request.QueryString["citycode"].ToLower() : "";
                return this.data.OffersAndDiscountsData(rendering, datasource, queryString, cityqueryString);
            }
            catch (Exception ex)
            {
                this.logger.Error("OffersAndDiscounts Datasource is null" + ex.Message);
            }

            return null;
        }
    }
}