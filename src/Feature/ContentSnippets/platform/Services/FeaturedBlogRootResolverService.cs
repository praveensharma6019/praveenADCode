using Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Services
{
    public class FeaturedBlogRootResolverService : IFeaturedBlogRootResolverService
    {

        private readonly ILogRepository _logRepository;
        public FeaturedBlogRootResolverService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public FeaturedBlogData GetFeaturedBlogDataList(Rendering rendering)
        {
            FeaturedBlogData featuredBlogDataList = new FeaturedBlogData();
            try
            {
                featuredBlogDataList.Title = GetFeaturedBlogDataItem(rendering).Title;
                featuredBlogDataList.data = GetFeaturedBlogDataItem(rendering).data;
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return featuredBlogDataList;
        }

        public FeaturedBlogData GetFeaturedBlogDataItem(Rendering rendering)
        {
            List<Object> featuredBlogDataItemList = new List<Object>();
            FeaturedBlogData featuredBlogData = new FeaturedBlogData();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                var commonItem = Sitecore.Context.Database.GetItem(Templates.commonData.ItemID);
                var realtyLink = !string.IsNullOrEmpty(commonItem.Fields[Templates.commonData.Fields.blogLinkId].Value.ToString()) ?
                                commonItem.Fields[Templates.commonData.Fields.blogLinkId].Value.ToString() : "";
                string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                FeaturedBlogDataItem featuredBlogdataItem;

                if (datasource.TemplateID == Templates.FeaturesItem.TemplateID)
                {
                    featuredBlogData.Title = datasource.Fields[Templates.ITitle.FieldsName.Title].Value != null ? datasource.Fields[Templates.ITitle.FieldsName.Title].Value : "";

                    List<Item> children = datasource.GetMultiListValueItem(Templates.FeaturesItem.Fields.FieldsID.FeaturesItem).ToList();
                    if (children != null && children.Count > 0)
                    {
                        foreach (Item item in children.OrderByDescending(x => x.Fields[Templates.FeaturedBlog.Fields.FieldsID.DateTime].Value))
                        {
                            featuredBlogdataItem = new FeaturedBlogDataItem();

                            List<FeaturedBlogCategoryListDataItem> objectcategorytList = new List<FeaturedBlogCategoryListDataItem>();

                            var multiListForItem = item.GetMultiListValueItem(Templates.FeaturedBlog.Fields.FieldsID.Category);
                            if (multiListForItem != null && multiListForItem.Count() > 0)
                            {
                                foreach (var selectedCatagory in multiListForItem)
                                {
                                    FeaturedBlogCategoryListDataItem featuredBlogCategoryListItem = new FeaturedBlogCategoryListDataItem();
                                    if (selectedCatagory.TemplateID == Templates.FeaturedBlog.Fields.FieldsID.BlogAnchors)
                                    {
                                        featuredBlogCategoryListItem.CategoryTitle = !string.IsNullOrEmpty(selectedCatagory.Fields[Templates.FeaturedBlogCategoryListDataItem.Fields.FieldsID.CategoryTitle].Value.ToString()) ?
                                                                                        selectedCatagory.Fields[Templates.FeaturedBlogCategoryListDataItem.Fields.FieldsID.CategoryTitle].Value : null;
                                        var CategoryLink = string.Format(realtyLink + "/blogs/category/{0}", GetSlugValue(selectedCatagory));
                                        featuredBlogCategoryListItem.CategoryLink = CategoryLink != "" ? strSitedomain + CategoryLink : "";

                                        featuredBlogCategoryListItem.CategoryLinkText = Helper.GetLinkTextbyField(selectedCatagory, selectedCatagory.Fields[Templates.FeaturedBlogCategoryListDataItem.Fields.FieldsName.CategoryLink]) != null ?
                                             Helper.GetLinkTextbyField(selectedCatagory, selectedCatagory.Fields[Templates.FeaturedBlogCategoryListDataItem.Fields.FieldsName.CategoryLink]) : "";

                                        if (featuredBlogCategoryListItem.CategoryTitle != null && featuredBlogCategoryListItem.CategoryLink != null)
                                        {
                                            objectcategorytList.Add(featuredBlogCategoryListItem);
                                        }
                                    }
                                }
                            }

                            featuredBlogdataItem.FeaturedBlogCategoryListDataItemList = objectcategorytList;

                            featuredBlogdataItem.ImgSrc = Helper.GetImageSource(item, Templates.HasPageContent.Fields.FieldsName.HasPageContentImage) != null ?
                                      Helper.GetImageSource(item, Templates.HasPageContent.Fields.FieldsName.HasPageContentImage) : "";

                            featuredBlogdataItem.ImgAlt = Helper.GetImageDetails(item, Templates.HasPageContent.Fields.FieldsName.HasPageContentImage) != null ?
                                      Helper.GetImageDetails(item, Templates.HasPageContent.Fields.FieldsName.HasPageContentImage).Fields["Alt"].Value : "";

                            featuredBlogdataItem.ImgTitle = Helper.GetImageDetails(item, Templates.HasPageContent.Fields.FieldsName.HasPageContentImage) != null ?
                                      Helper.GetImageDetails(item, Templates.HasPageContent.Fields.FieldsName.HasPageContentImage).Fields["TItle"].Value : "";

                            featuredBlogdataItem.Title = item.Fields[Templates.HasPageContent.Fields.FieldsName.HasPageContentTitle].Value != null ? item.Fields[Templates.HasPageContent.Fields.FieldsName.HasPageContentTitle].Value : "";

                            featuredBlogdataItem.Heading = item.Fields[Templates.HasPageContent.Fields.FieldsName.HasPageContentSummary].Value != null ? item.Fields[Templates.HasPageContent.Fields.FieldsName.HasPageContentSummary].Value : "";

                            var Link = string.Format(realtyLink + "/blogs/{0}", item?.Fields[Templates.HasPageContent.Fields.FieldsID.Slug]?.Value?.ToLower());
                            featuredBlogdataItem.Link = Link != "" ? strSitedomain + Link : Link;
                            featuredBlogdataItem.LinkText = Helper.GetLinkTextbyField(item, item.Fields[Templates.HasPageContent.Fields.FieldsName.HasPageContentLink]) != null ?
                                      Helper.GetLinkTextbyField(item, item.Fields[Templates.HasPageContent.Fields.FieldsName.HasPageContentLink]) : "";


                            featuredBlogDataItemList.Add(featuredBlogdataItem);
                            featuredBlogData.data = featuredBlogDataItemList;
                        }
                    }
                }

            }
            catch (Exception ex)
            {


                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return featuredBlogData;
        }

        private string GetSlugValue(Item item)
        {
            string value = string.Empty;
            try
            {
                if (item != null)
                {
                    value = item?.Fields[Templates.HasPageContent.Fields.FieldsID.SlugText]?.Value?.ToLower();
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return value;
        }

        private string GetSlugID(Field field)
        {
            string value = string.Empty;
            try
            {
                LookupField lookupField = field;
                if (lookupField != null && lookupField?.TargetItem != null)
                {
                    Item categoryItem = lookupField.TargetItem;
                    value = categoryItem?.ID?.ToString();
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return value;
        }
    }
}
