using Adani.SuperApp.Realty.Feature.Blog.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.Search.Platform.Services;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Blog.Platform.Services
{
    public class BlogSearchService : IBlogSearchService
    {
        private readonly ILogRepository _logRepository;
        public BlogSearchService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }

        public BlogSearchModel GetBlogSearch(string keyword)
        {
            BlogSearchModel blogSearchModel = new BlogSearchModel();

            try
            {
                var commonItem = Sitecore.Context.Database.GetItem(Templates.commonData.ItemID);
                string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;

                SearchBuilder searchBuilder = new SearchBuilder();
                var searchResults = searchBuilder.GetBlogCategoryResults(keyword);
                blogSearchModel.BlogSearchList = new List<BlogSearch>();
                //string baseUrl = "sitecore/api/layout/render/jss";//?item={AAFF728E-820B-4637-8427-8FFDC331B6B2};
                //string apiKey = "{471138C3-06D1-43D9-A38B-875E3823789C}";
                //string siteName = "adani-realty";
                foreach (SearchResultItem item in searchResults)
                {
                    BlogSearch blogSearch = new BlogSearch();
                    blogSearch.ItemId = item?.ItemId.ToString();
                    blogSearch.Role = item?.Fields["title_t"].ToString();
                    var realtyLink = !string.IsNullOrEmpty(commonItem.Fields[Templates.commonData.Fields.blogLinkId].Value.ToString()) ?
                                        commonItem.Fields[Templates.commonData.Fields.blogLinkId].Value.ToString() : "";
                    blogSearch.Slug = string.Format(strSitedomain + realtyLink + "/{0}/{1}", "blogs", item?.Fields["slug"].ToString());
                    blogSearchModel.BlogSearchList.Add(blogSearch);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return blogSearchModel;
        }
        public object GetBlogDisclaimer(Rendering rendering)
        {
            BlogDisclaimerModel blogDisclaimerModel = new BlogDisclaimerModel();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                blogDisclaimerModel.title = datasource?.Fields[Templates.CommunicationDisclaimer.Fields.titleID].Value;
                blogDisclaimerModel.disclaimer = datasource?.Fields[Templates.CommunicationDisclaimer.Fields.disclaimerID].Value;
            }
            catch (Exception ex)
            {

                _logRepository.Error(" BlogSearchService GetBlogDisclaimer gives -> " + ex.Message);
            }
            return blogDisclaimerModel;
        }
    }
}