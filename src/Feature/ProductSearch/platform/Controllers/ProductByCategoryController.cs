using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models;
using Adani.SuperApp.Airport.Foundation.DataAPI.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Newtonsoft.Json;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Controllers
{
    public class ProductByCategoryController : SitecoreController
    {
        private readonly IAPIResponse productResponse;
        private readonly ILogRepository logRepository;
       

        public ProductByCategoryController(IAPIResponse _productResponse,ILogRepository _logRepository)
        {
            this.productResponse = _productResponse;
            this.logRepository = _logRepository;
            
        }
        
        
        public JsonResult GetProducts(string queryText)
        {
            SearchResults<ProductList> results = null;
            List<SearchResults<ProductList>> resultsList = new List<SearchResults<ProductList>>();

            StringCollection queryValues=new StringCollection();
            try
            {
                if (!string.IsNullOrWhiteSpace(queryText) && !queryText.Contains(","))
                {
                    using (var context = ContentSearchManager.GetIndex(Constant.SitecoreWebIndex).CreateSearchContext())
                    {
                        var searchQuery = context.GetQueryable<ProductList>()
                            .Where(x => x.ProductCategory.Contains(queryText));

                        results = searchQuery.GetResults();
                    }

                    return Json(results, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    queryValues.AddRange(queryText.Split(',')?.ToArray());

                    for (int i = 0; i < queryValues.Count; i++)
                    {
                        
                        using (var context = ContentSearchManager.GetIndex(Constant.SitecoreWebIndex).CreateSearchContext())
                        {
                            var searchQuery = context.GetQueryable<ProductList>()
                                .Where(x => x.ProductCategory.Contains(queryValues[i]));

                            resultsList.Add(searchQuery.GetResults());
                        }
                    }
                    
                    return Json(resultsList?.ToList()?.Distinct(), JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                logRepository.Error("ParseProduct Method GetProducts in ProductSearchController gives error -> " + ex.Message);
            }
            

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        
    }
}