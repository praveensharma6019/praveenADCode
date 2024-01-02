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
    public class UnlockOffersController : ApiController
    {
        // GET: Offers
        private ILogRepository logRepository;
        private readonly ISearchBuilder searchBuilder;
        private readonly IHelper _helper;
        public UnlockOffersController(ILogRepository _logRepository, ISearchBuilder _searchBuilder, IHelper helper)
        {
            this.logRepository = _logRepository;
            this.searchBuilder = _searchBuilder;
            this._helper = helper;
        }

        [HttpPost]
        [Route("api/GetUnlockedOffersNew")]
        public IHttpActionResult GetUnlockOffers([FromBody] OfferFilters filter)
        {
            Services.OfferDataParser offerDataParser = new Services.OfferDataParser(logRepository, searchBuilder, _helper);
            ResponseData responseData = new ResponseData();
            ResultData resultData = new ResultData();
            if (!string.IsNullOrEmpty(filter.AirportCode) && Convert.ToBoolean(filter.isUnlockOffer))
            {
                IQueryable<SearchResultItem> results = searchBuilder.GetOfferSearchResults(Services.OfferPredicateBuilder.GetOfferPredicate(filter));
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

                    logRepository.Error("GetUnlockOffers in OffersController gives error -> " + ex.Message);
                }
            }


            return Json(responseData);
        }

        

    }
}