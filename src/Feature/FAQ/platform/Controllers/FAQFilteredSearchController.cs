using Adani.SuperApp.Airport.Foundation.DataAPI.Platform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Feature.FAQ.Models;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;
using System.Web.Http;
using Adani.SuperApp.Airport.Feature.FAQ.Interfaces;
using Adani.SuperApp.Airport.Feature.FAQ.Services;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.FAQ.Controlles
{
    public class FAQFilteredSearchController : ApiController
    {
        private ILogRepository logRepository;
        private readonly ISearchBuilder searchBuilder;
        private IFAQSearchItems faqSearchItems;

        public FAQFilteredSearchController(ILogRepository _logRepository, ISearchBuilder _searchBuilder, IFAQSearchItems _faqSearchItems)
        {
            this.logRepository = _logRepository;
            this.searchBuilder = _searchBuilder;
            faqSearchItems = _faqSearchItems;
        }

        [HttpPost]
        [Route("api/GetFaqs")]
        public IHttpActionResult GetFaqs([FromBody] Filters filter)
        {
            FAQResponseData responseData = new FAQResponseData();
            if (filter != null)
                responseData = faqSearchItems.GetSolrFAQData(ref filter);

            return Json(responseData);
        }
    }
}