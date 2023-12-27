using System;
using System.Collections.Generic;
using System.Linq;
using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Services;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.ContentSearch.SearchTypes;
using System.Web.Http;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Controllers
{
    public class CategorySearchController : ApiController
    {
        private ILogRepository logRepository;
        private readonly ISearchBuilder searchBuilder;
        private readonly IHelper _helper;
        public CategorySearchController(ILogRepository _logRepository, ISearchBuilder _searchBuilder, IHelper helper)
        {
            this.logRepository = _logRepository;
            this.searchBuilder = _searchBuilder;
            this._helper = helper;
        }
               

       
        // GET: CategorySearch
        [HttpPost]
        [Route("api/GetDutyFreeCategory")]
        public IHttpActionResult GetDutyFreeCategory([FromBody] Filters filter)
        {
            filter.filterType = "Category";
            IQueryable<SearchResultItem> results = searchBuilder.GetSearchResults(Services.Search.GetSearchPredicate(filter));
            ResponseData responseData = new ResponseData();
            ResultData resultData = new ResultData();
            
            resultData.result = ParseCategory(results, filter);

            if (resultData.result != null)
            {
                resultData.count = resultData.result.Count;
                responseData.status = true;
                responseData.data = resultData;
            }
            return Json(responseData);
        }

        private List<Object> ParseCategory(IQueryable<SearchResultItem> results, Filters filter)
        {
            List<Object> CategoryMappingsList = new List<Object>();
           // LogRepository logRepository = new LogRepository();
            try
            {
                Category categoryMapping = null;
                string itemPath = string.Empty;
                foreach (var categoryData in results.ToList())
                {
                    itemPath = (categoryData.Fields.ContainsKey(Constant.categoryPath) && !string.IsNullOrEmpty(categoryData.Fields[Constant.categoryPath].ToString())) ?
                                                  categoryData.Fields[Constant.categoryPath].ToString() : string.Empty;
                    categoryMapping = new Category
                    {

                        title = (categoryData.Fields.ContainsKey(Constant.Name) && !string.IsNullOrEmpty(categoryData.Fields[Constant.Name].ToString())) ?
                                            categoryData.Fields[Constant.Name].ToString() : string.Empty,

                        cdnPath = (categoryData.Fields.ContainsKey(Constant.CategoryCDNPath) && !string.IsNullOrEmpty(categoryData.Fields[Constant.CategoryCDNPath].ToString())) ?
                                              categoryData.Fields[Constant.CategoryCDNPath].ToString() : string.Empty,

                        showOnHomepage = (categoryData.Fields.ContainsKey(Constant.CategoryShowOnHomepage) && !string.IsNullOrEmpty(categoryData.Fields[Constant.CategoryShowOnHomepage].ToString())) ?
                                                             categoryData.Fields[Constant.CategoryShowOnHomepage].ToString() : "false",

                        //iconImage = (categoryData.Fields.ContainsKey(Constant.CategoryIcon) && !string.IsNullOrEmpty(categoryData.Fields[Constant.CategoryIcon].ToString())) ?
                        //                         _helper.GetImageUrlfromSitecore(categoryData.Fields[Constant.Icon].ToString()) : string.Empty,

                        //thumbnailImage = (categoryData.Fields.ContainsKey(Constant.CategoryThumbnail) && !string.IsNullOrEmpty(categoryData.Fields[Constant.CategoryThumbnail].ToString())) ?
                        //                         _helper.GetImageUrlfromSitecore(categoryData.Fields[Constant.Thumbnail].ToString()) : string.Empty,

                        //mainImage = (categoryData.Fields.ContainsKey(Constant.CategoryMainImage) && !string.IsNullOrEmpty(categoryData.Fields[Constant.CategoryMainImage].ToString())) ?
                        //                         _helper.GetImageUrlfromSitecore(categoryData.Fields[Constant.MainImage].ToString()) : string.Empty,

                        code = (categoryData.Fields.ContainsKey(Constant.code) && !string.IsNullOrEmpty(categoryData.Fields[Constant.code].ToString())) ?
                                              categoryData.Fields[Constant.code].ToString() : string.Empty,

                        link = (categoryData.Fields.ContainsKey(Constant.CategoryLink) && !string.IsNullOrEmpty(categoryData.Fields[Constant.CategoryLink].ToString())) ?
                                              categoryData.Fields[Constant.CategoryLink].ToString() : string.Empty,

                        categoryPath = itemPath
                    };
                    if (!String.IsNullOrEmpty(categoryMapping.code))
                    {
                        if (filter.materialGroup == null)
                        {
                            CategoryMappingsList.Add(categoryMapping);
                        }
                        else if (filter.materialGroup.ToLower().Trim() == itemPath.Split('/')[itemPath.Split('/').Length - 2])
                        {
                            CategoryMappingsList.Add(categoryMapping);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("ParseProduct Method in Category Search Controller Class gives error -> " + ex.Message);
            }

            return CategoryMappingsList;
        }
    }
}