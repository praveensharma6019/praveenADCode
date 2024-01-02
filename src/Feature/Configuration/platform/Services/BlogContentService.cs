using Adani.SuperApp.Realty.Feature.Configuration.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using static Adani.SuperApp.Realty.Feature.Configuration.Platform.Templates;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.Services
{
    public class BlogContentService : IBlogContentService
    {
        private readonly ILogRepository _logRepository;
        public BlogContentService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public BlogContentModel GetBlockContentData(Rendering rendering)
        {
            BlogContentModel bcModel = new BlogContentModel();
            try
            {
                Item renderingItem = rendering?.Item;
                if (renderingItem != null)
                {
                    BlogObject blogObj = new BlogObject();
                    blogObj.Data = new List<object>();
                    blogObj.Data = GetBlogs(renderingItem);
                    bcModel.Blog = blogObj;


                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return bcModel;

        }

        private List<object> GetBlogs(Item renderingItem)
        {
            List<object> blogs = new List<object>();
            try
            {
                var commonItem = Sitecore.Context.Database.GetItem(Templates.commonData.ItemID);
                var realtyLink = !string.IsNullOrEmpty(commonItem.Fields[Templates.commonData.Fields.blogLinkId].Value.ToString()) ?
                                commonItem.Fields[Templates.commonData.Fields.blogLinkId].Value.ToString() : "";
                Item[] blogItems = renderingItem.GetChildren().ToArray();
                string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
                foreach (Item item in blogItems)
                {
                    Blog blog = new Blog();
                    blog.Title = GetDropLinkFieldValue(item?.Fields[BlogTemplate.Fields.Title]);
                    blog.CategoryId = item?.ID?.ToString();
                    blog.CategoryPageLink = GetHashData(item?.Fields[BlogTemplate.Fields.Title]);
                    blog.CategoryPageLinkText = GetCategoryPageLinkText(item?.Fields[BlogTemplate.Fields.Title]);
                    blog.CategorySectionTitle = item?.Fields[BlogTemplate.Fields.PageTitle].Value;
                    var Slug = string.Format(realtyLink + "/blogs/category/{0}", GetSlugValue(item?.Fields[BlogTemplate.Fields.Title]));
                    blog.Slug = Slug != "" ? strSitedomain + Slug : Slug;
                    blog.Keys = GetBlogKeys(item?.Fields[BlogTemplate.Fields.Keys]);
                    blogs?.Add(blog);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return blogs;
        }

        private string GetSlugValue(Field field)
        {
            string value = string.Empty;
            try
            {
                LookupField lookupField = field;
                if (lookupField != null && lookupField?.TargetItem != null)
                {
                    Item categoryItem = lookupField.TargetItem;
                    value = categoryItem?.Fields[BlogAnchorsTemplate.Fields.SlugText]?.Value?.ToLower();
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
                    value = categoryItem?.Fields[BlogAnchorsTemplate.Fields.Keyword]?.ToString();
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return value;
        }

        private string GetCategoryPageLinkText(Field field)
        {
            string value = string.Empty;
            try
            {
                LookupField lookupField = field;
                if (lookupField != null && lookupField?.TargetItem != null)
                {
                    Item categoryItem = lookupField.TargetItem;
                    value = categoryItem?.Fields[BlogAnchorsTemplate.Fields.SeeAllText]?.Value;
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return value;
        }

        private string GetHashData(Field field)
        {
            string value = string.Empty;
            try
            {
                LookupField lookupField = field;
                if (lookupField != null && lookupField?.TargetItem != null)
                {
                    Item categoryItem = lookupField.TargetItem;
                    value = categoryItem?.Fields[BlogAnchorsTemplate.Fields.HashData]?.Value;
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return value;
        }

        private List<BlogKey> GetBlogKeys(Field field)
        {
            int intialCount = 1;
            List<BlogKey> blogKeys = new List<BlogKey>();
            try
            {
                var commonItem = Sitecore.Context.Database.GetItem(Templates.commonData.ItemID);
                var realtyLink = !string.IsNullOrEmpty(commonItem.Fields[Templates.commonData.Fields.blogLinkId].Value.ToString()) ?
                                commonItem.Fields[Templates.commonData.Fields.blogLinkId].Value.ToString() : "";
                MultilistField multiField = field;
                if (field != null && multiField.GetItems().Count() > 0)
                {
                    foreach (Item item in multiField.GetItems().OrderByDescending(x => x.Fields[BlogKeysTemplate.Fields.DateTime].Value))
                    {
                        BlogKey blogKey = new BlogKey();

                        blogKey.IsDeafult = intialCount == 1 ? true : false;
                        string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
                        var Link = string.Format(realtyLink + "/blogs/{0}", item?.Fields[BlogKeysTemplate.Fields.Slug]?.Value?.ToLower());
                        blogKey.Link = Link != "" ? strSitedomain + Link : strSitedomain;
                        blogKey.Text = Helper.GetLinkTextbyField(item, item?.Fields[BlogKeysTemplate.Fields.Link]);
                        blogKey.Src = Helper.GetImageURLbyField(item?.Fields[BlogKeysTemplate.Fields.ImageSrc]);
                        blogKey.Alt = Helper.GetImageAltbyField(item?.Fields[BlogKeysTemplate.Fields.ImageSrc]);
                        blogKey.imgTitle = Helper.GetImageDetails(item, BlogKeysTemplate.Fields.ImageSrcFieldName) != null ?
                              Helper.GetImageDetails(item, BlogKeysTemplate.Fields.ImageSrcFieldName).Fields[ImageFeilds.Fields.TitleFieldName].Value : "";
                        blogKey.Title = item?.Fields[BlogKeysTemplate.Fields.Title]?.Value;
                        //blogKey.Heading = item?.Fields[BlogKeysTemplate.Fields.Heading]?.Value;
                        //blogKey.SubHeading = item?.Fields[BlogKeysTemplate.Fields.SubHeading]?.Value;
                        blogKey.date = item?.Fields[BlogKeysTemplate.Fields.DateTime]?.Value != "" ? ConvertDateFormat(item?.Fields[BlogKeysTemplate.Fields.DateTime]?.ToString()) : new DateTime();
                        blogKey.DateTime = blogKey.date != null && ConvertDateFormatTostring(blogKey.date) != "Jan 01 0001" ? ConvertDateFormatTostring(blogKey.date) : "";
                        var CategoryLink = string.Format(realtyLink + "/blogs/category/{0}", GetSlugValue(item?.Fields[BlogKeysTemplate.Fields.CategoryTitle]));
                        blogKey.CategoryLink = CategoryLink != "" ? strSitedomain + CategoryLink : strSitedomain;
                        blogKey.CategoryTitle = GetDropLinkFieldValue(item?.Fields[BlogKeysTemplate.Fields.CategoryTitle]);
                        blogKey.ReadTime = item?.Fields[BlogKeysTemplate.Fields.ReadTime]?.Value;
                        blogKeys?.Add(blogKey);
                        intialCount++;
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return blogKeys;
        }
        private DateTime ConvertDateFormat(string datetimeString)
        {
            DateTime utcdate = DateTime.ParseExact(datetimeString, "yyyyMMdd'T'HHmmss'Z'", CultureInfo.InvariantCulture);
            return TimeZoneInfo.ConvertTimeFromUtc(utcdate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        }
        private string ConvertDateFormatTostring(DateTime date)
        {
            return date.ToString("MMM dd yyyy");
        }


        private string GetDropLinkFieldValue(Field field)
        {
            string value = string.Empty;
            try
            {
                LookupField lookupField = field;
                if (lookupField != null && lookupField?.TargetItem != null)
                {
                    Item categoryItem = lookupField.TargetItem;
                    value = categoryItem?.Fields[BlogAnchorsTemplate.Fields.Keyword]?.Value;
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