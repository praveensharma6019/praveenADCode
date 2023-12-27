using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Foundation.SitecoreExtension.Platform.Pipelines
{
    public class CustomLinkResolver : HttpRequestProcessor
    {
        /// <summary>
        /// This method used to resolve the item in sitecore using index based on the item name
        /// </summary>
        /// <param name="args"></param>
        public override void Process(HttpRequestArgs args)
        {
            if (Context.Item != null || Context.Database == null || args.Url == null || args.Url.ItemPath.Length == 0)
                return;


            var requestUrl = args.Url.ItemPath.TrimEnd('/');
            if (requestUrl.Contains("domestic-flights"))
            {
                var index = requestUrl.LastIndexOf('/');

                var itemName = requestUrl.Substring(index + 1);

                string contextDB = Sitecore.Context.Database.Name;
                string currentdb = GetContextIndexValue(contextDB, "domestic-flights");
                using (var searchContext = ContentSearchManager.GetIndex(currentdb).CreateSearchContext())
                {
                    //Find the item from the index using name and template id
                    ID itemTemplateID = new Sitecore.Data.ID(new Guid(Constants.Constant.CitytoCItyTemplateID));

                    if (!string.IsNullOrEmpty(itemName) && itemName == "domestic-flights")
                    {
                        var result = searchContext.GetQueryable<SearchResultItem>().
                            Where(
                                x => x.Name == itemName
                            ).FirstOrDefault();

                        if (result != null)
                        {
                            var item = result.GetItem();
                            if (item.Language == Context.Language)
                            {
                                Context.Item = result.GetItem();
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(itemName) && itemName != "domestic-flights")
                    {
                        var result = searchContext.GetQueryable<SearchResultItem>().
                            Where(
                                x => x.TemplateId == itemTemplateID &&
                                x.Name == itemName
                            ).FirstOrDefault();

                        if (result != null)
                        {
                            var item = result.GetItem();
                            if (item.Language == Context.Language)
                            {
                                Context.Item = result.GetItem();
                            }
                        }
                    }
                }
            }
            else if (requestUrl.Contains("international-flights"))
            {
                var index = requestUrl.LastIndexOf('/');

                var itemName = requestUrl.Substring(index + 1);

                string contextDB = Sitecore.Context.Database.Name;
                string currentdb = GetContextIndexValue(contextDB, "international-flights");
                using (var searchContext = ContentSearchManager.GetIndex(currentdb).CreateSearchContext())
                {
                    //Find the item from the index using name and template id
                    ID itemTemplateID = new Sitecore.Data.ID(new Guid(Constants.Constant.CitytoCItyTemplateID));

                    if (!string.IsNullOrEmpty(itemName) && itemName == "international-flights")
                    {
                        var result = searchContext.GetQueryable<SearchResultItem>().
                            Where(
                                x => x.Name == itemName
                            ).FirstOrDefault();

                        if (result != null)
                        {
                            var item = result.GetItem();
                            if (item.Language == Context.Language)
                            {
                                Context.Item = result.GetItem();
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(itemName) && itemName != "international-flights")
                    {
                        var result = searchContext.GetQueryable<SearchResultItem>().
                            Where(
                                x => x.TemplateId == itemTemplateID &&
                                x.Name == itemName
                            ).FirstOrDefault();

                        if (result != null)
                        {
                            var item = result.GetItem();
                            if (item.Language == Context.Language)
                            {
                                Context.Item = result.GetItem();
                            }
                        }
                    }
                }
            }
            else if (requestUrl.Contains("blog"))
            {
                var index = requestUrl.LastIndexOf('/');

                var pageName = requestUrl.Substring(index + 1);

                string contextDB = Sitecore.Context.Database.Name;
                string currentdb = GetContextIndexValue(contextDB);
                using (var searchContext = ContentSearchManager.GetIndex(currentdb).CreateSearchContext())
                {
                    //Find the item from the index using name and template id
                    ID itemTemplateID = new Sitecore.Data.ID(new Guid(Constants.Constant.BlogDetailPageTemplateID));

                    if (!string.IsNullOrEmpty(pageName) && pageName == "blog")
                    {
                        var result = searchContext.GetQueryable<SearchResultItem>().
                            Where(
                                x => x.Name == pageName
                            ).FirstOrDefault();

                        if (result != null)
                        {
                            var item = result.GetItem();
                            if (item.Language == Context.Language)
                            {
                                Context.Item = result.GetItem();
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(pageName) && pageName != "blog")
                    {
                        var result = searchContext.GetQueryable<SearchResultItem>().
                            Where(
                                x => x.TemplateId == itemTemplateID &&
                                x.Name == pageName
                            ).FirstOrDefault();

                        if (result != null)
                        {
                            var item = result.GetItem();
                            if (item.Language == Context.Language)
                            {
                                Context.Item = result.GetItem();
                            }
                        }
                    }
                }
            }
        }

        public string GetContextIndexValue(string contextDB, string flighttype)
        {
            if (contextDB == "master")
            {
                if (flighttype == "domestic-flights")
                {
                    return Constants.Constant.citytocity_domestic_masterindex;
                }
                else if (flighttype == "international-flights")
                {
                    return Constants.Constant.citytocity_international_masterindex;
                }
            }
            else if (contextDB == "web")
            {
                if (flighttype == "domestic-flights")
                {
                    return Constants.Constant.citytocity_domestic_webindex;
                }
                else if (flighttype == "international-flights")
                {
                    return Constants.Constant.citytocity_international_webindex;
                }
            }
            return Constants.Constant.citytocity_domestic_masterindex;
        }

        public string GetContextIndexValue(string contextDB)
        {

            if (contextDB == "master")
            {
                return Constants.Constant.SuperApp_masterindex;
            }
            else if (contextDB == "web")
            {
                return Constants.Constant.SuperApp_webindex;
            }
            return Constants.Constant.SuperApp_masterindex;
        }
    }
}