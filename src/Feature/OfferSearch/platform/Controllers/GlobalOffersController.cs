using System;
using System.Linq;
using System.Web.Http;
using Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Services;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.ContentSearch.SearchTypes;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Controllers
{
    public class GlobalOffersController : ApiController
    {
        // GET: Offers
        private ILogRepository logRepository;
        private readonly ISearchBuilder searchBuilder;
        private readonly IHelper _helper;
        public GlobalOffersController(ILogRepository _logRepository, ISearchBuilder _searchBuilder, IHelper helper)
        {
            this.logRepository = _logRepository;
            this.searchBuilder = _searchBuilder;
            this._helper = helper;
        }

        [HttpPost]
        [Route("api/GetAdaniOneOffers")]
        public IHttpActionResult GetAdaniOneOffers([FromBody] OfferFilters filter)
        {
            Services.OfferDataParser offerDataParser = new Services.OfferDataParser(logRepository,searchBuilder,_helper);
            ResponseData responseData = new ResponseData();
            ResultData resultData = new ResultData();
                IQueryable<SearchResultItem> results = searchBuilder.GetOfferSearchResults(Services.OfferPredicateBuilder.GetAdaniOneOfferPredicate(filter));
                try
                {
                    resultData.result = offerDataParser.ParseOffer(results, filter);
                    if (resultData.result != null)
                    {
                        resultData.count = resultData.result.Count;
                        responseData.status = true;
                        responseData.data = resultData;
                    }
                }
                catch (Exception ex)
                {

                    logRepository.Error("GetAdaniOneOffers in OffersController gives error -> " + ex.Message);
                }
             return Json(responseData);
        }


    }
}