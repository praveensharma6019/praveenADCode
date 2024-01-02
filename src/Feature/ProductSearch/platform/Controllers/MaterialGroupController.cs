using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Services;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.ContentSearch.SearchTypes;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Controllers
{
    public class MaterialGroupController : ApiController
    {
        private ILogRepository logRepository;
        private readonly ISearchBuilder searchBuilder;
        private readonly IHelper _helper;
        public MaterialGroupController(ILogRepository _logRepository,ISearchBuilder _searchBuilder, IHelper helper)
        {
            this.logRepository = _logRepository;
            this.searchBuilder = _searchBuilder;
            this._helper = helper;
        }
        

        
        [HttpPost]
        [Route("api/GetDutyFreeMaterialGroup")]
        public IHttpActionResult GetDutyFreeMaterialGroup([FromBody] Filters filter)
        {
            filter.filterType = "MaterialGroup";
            IQueryable<SearchResultItem> results = searchBuilder.GetSearchResults(Services.Search.GetSearchPredicate(filter));
            ResponseData responseData = new ResponseData();
            ResultData resultData = new ResultData();
            resultData.result = ParseMaterialGroup(results);
            
            if (resultData.result != null)
            {
                resultData.count = resultData.result.Count;
                responseData.status = true;
                responseData.data = resultData;
            }               
            return Json(responseData);
        }

        private List<Object> ParseMaterialGroup(IQueryable<SearchResultItem> results)
        {
            List<Object> materialgroupMappingsList = new List<Object>();
            //LogRepository logRepository = new LogRepository();
            try
            {
                MaterialGroup materialGroupMapping = null;
                foreach (var mGroup in results.ToList())
                {
                    materialGroupMapping = new MaterialGroup { 

                                            title = (mGroup.Fields.ContainsKey(Constant.Title) && !string.IsNullOrEmpty(mGroup.Fields[Constant.Title].ToString())) ?
                                                    mGroup.Fields[Constant.Title].ToString() : 
                                                    string.Empty,
                                            path = (mGroup.Fields.ContainsKey(Constant.Path) && !string.IsNullOrEmpty(mGroup.Fields[Constant.Path].ToString())) ? 
                                                    mGroup.Fields[Constant.path].ToString() : 
                                                    string.Empty,
                                            cdnPath = (mGroup.Fields.ContainsKey(Constant.CDNPath) && !string.IsNullOrEmpty(mGroup.Fields[Constant.CDNPath].ToString())) ? 
                                                    mGroup.Fields[Constant.CDNPath].ToString() :
                                                    string.Empty,
                                            link = (mGroup.Fields.ContainsKey(Constant.Link) && !string.IsNullOrEmpty(mGroup.Fields[Constant.Link].ToString())) ? 
                                                    mGroup.Fields[Constant.Link].ToString() : 
                                                    string.Empty,
                                            code = (mGroup.Fields.ContainsKey(Constant.Title) && !string.IsNullOrEmpty(mGroup.Fields[Constant.Title].ToString())) ?
                                                    mGroup.Fields[Constant.Title].ToString() :
                                                    string.Empty,
                    };
                    //materialGroupMapping.thumbnailImage = (mGroup.Fields.ContainsKey(Constant.Thumbnail) && !string.IsNullOrEmpty(mGroup.Fields[Constant.Thumbnail].ToString())) ?
                    //        _helper.GetImageUrlfromSitecore(mGroup.Fields[Constant.Thumbnail].ToString()) :
                    //        string.Empty;
                    //materialGroupMapping.iconImage = (mGroup.Fields.ContainsKey(Constant.Icon) && !string.IsNullOrEmpty(mGroup.Fields[Constant.Icon].ToString())) ?
                    //        _helper.GetImageUrlfromSitecore(mGroup.Fields[Constant.Icon].ToString()) :
                    //        string.Empty;
                    //materialGroupMapping.mainImage = (mGroup.Fields.ContainsKey(Constant.MainImage) && !string.IsNullOrEmpty(mGroup.Fields[Constant.MainImage].ToString())) ?
                    //        _helper.GetImageUrlfromSitecore(mGroup.Fields[Constant.MainImage].ToString()) :
                    //        string.Empty;
                    
                        //Active = (product.Fields.ContainsKey(Constant.SKUName) && !string.IsNullOrEmpty(product.Fields[Constant.SKUName].ToString())) ?
                        //          product.Fields[Constant.SKUName].ToString() :
                        //          string.Empty;

                    materialgroupMappingsList.Add(materialGroupMapping);
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("ParseProduct Method in Material Group Controller Class gives error -> " + ex.Message);
            }

            return materialgroupMappingsList;
        }

        
    }
}