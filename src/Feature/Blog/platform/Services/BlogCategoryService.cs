using Adani.SuperApp.Realty.Feature.Blog.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.Search.Platform.Services;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using static Adani.SuperApp.Realty.Feature.Blog.Platform.Templates;

namespace Adani.SuperApp.Realty.Feature.Blog.Platform.Services
{
    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly ILogRepository _logRepository;
        public BlogCategoryService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public BlogCategoryModel GetBlogCategoryData(Rendering rendering, string categoryName, int pageNo)
        {
            BlogCategoryModel blogCategoryModel = new BlogCategoryModel();
            try
            {
                Item renderingItem = rendering?.Item;
                if (renderingItem != null)
                {
                    Content content = new Content();
                    content.Heading = renderingItem?.Fields[BlogKeysTemplate.Fields.Title]?.Value;
                    content.LoadingText = renderingItem?.Fields[BlogKeysTemplate.Fields.LoadingText]?.Value;
                    blogCategoryModel.Content = content;
                    GenericObject blogAnchors = new GenericObject();
                    blogAnchors.Data = new List<object>();
                    MultilistField blogAnchorsField = renderingItem?.Fields[Templates.BlogAnchorsTemplate.Fields.BlogAnchorField];
                    blogAnchors.Data = GetBlogAnchors(blogAnchorsField);
                    blogCategoryModel.BlogAnchors = blogAnchors;
                }
                Int32 pageCount = Convert.ToInt32(renderingItem?.Fields[BlogKeysTemplate.Fields.PageCount]?.Value);
                //string lastPartUrl = HttpContext.Current.Request.Url.AbsoluteUri.Split('/').Last();
                blogCategoryModel.Blogs = GetCategoriesFromSolr(categoryName, pageCount, pageNo); ;
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return blogCategoryModel;
        }

        private BlogObject GetCategoriesFromSolr(string categoryName, Int32 pageCount, int pageNo)
        {
            var genericObject = new BlogObject();

            try
            {
                SearchBuilder searchBuilder = new SearchBuilder();
                var searchResults = searchBuilder.GetBlogCategoryResults();
                List<SearchResultItem> categories = new List<SearchResultItem>();
                Item blogAnchorsFolder = Sitecore.Context.Database.GetItem("{1C0D5047-2849-44A5-900F-79F2CE21124C}");
                Item blogCategoryItem = blogAnchorsFolder != null ? Sitecore.Context.Database.GetItem(string.Format("{0}/{1}", blogAnchorsFolder.Paths.FullPath, categoryName)) : null;
                var commonItem = Sitecore.Context.Database.GetItem(Templates.commonData.ItemID);
                var realtyLink = !string.IsNullOrEmpty(commonItem.Fields[Templates.commonData.Fields.blogLinkId].Value.ToString()) ?
                                commonItem.Fields[Templates.commonData.Fields.blogLinkId].Value.ToString() : "";
                string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;

                if (blogCategoryItem != null)
                {
                    foreach (var item in searchResults)
                    {
                        var category = item.Fields.Keys.Contains("category") ? item.Fields["category"] : null;
                        if (category != null)
                        {
                            List<Guid> cateGuids = category.ToString().Split('|').ToList().ConvertAll(Guid.Parse);
                            if (blogCategoryItem != null)
                            {
                                if (cateGuids.Contains(Guid.Parse(blogCategoryItem.ID.ToString().ToLower())))
                                {
                                    categories.Add(item);
                                }
                            }
                            else
                            {
                                categories.Add(item);
                            }

                        }
                    }
                }

                if (categories != null && categories.Count() > 0)
                {
                    CatBlog blog = new CatBlog();
                    blog.Keys = new List<BlogKey>();
                    foreach (var item in categories)
                    {
                        _logRepository.Info(string.Format("Blog Name:{0} ", MethodBase.GetCurrentMethod().Name + item?.Fields["title_t"].ToString()));
                        BlogKey blogKey = new BlogKey();
                        ID cateGuid = new ID(Guid.Parse(item.Fields["category_title"]?.ToString()));
                        Item categoryItem = !string.IsNullOrWhiteSpace(categoryName) ? blogCategoryItem : Sitecore.Context.Database?.GetItem(blogCategoryItem.ID);
                        blogKey.CategoryTitle = categoryItem?.Fields[BlogAnchorsTemplate.Fields.Keyword]?.Value;
                        blogKey.CategoryLink = string.Format("/blogs/category/{0}", categoryItem?.Fields[BlogAnchorsTemplate.Fields.Keyword]?.Value?.ToLower());
                        blogKey.Title = item.Fields.Keys.Contains("title_t") ? item?.Fields["title_t"].ToString() : "";
                        blogKey.Heading = item.Fields.Keys.Contains("summary") ? item?.Fields["summary"]?.ToString() : "";
                        blogKey.SubHeading = item.Fields.Keys.Contains("body") ? item?.Fields["body"]?.ToString() : "";
                        blogKey.Date = item.Fields.Keys.Contains("datetime") ? ConvertDateFormat(item?.Fields["datetime"]?.ToString()) : new DateTime();
                        blogKey.DateTime = item.Fields.Keys.Contains("datetime") && blogKey.Date != null && ConvertDateFormatTostring(blogKey.Date) != "Jan 01 0001" ? ConvertDateFormatTostring(blogKey.Date) : "";
                        blogKey.ReadTime = item.Fields.Keys.Contains("readtime") ? item?.Fields["readtime"]?.ToString() : "";
                        var image = item.Fields.Keys.Contains("blogimage") ? item?.Fields["blogimage"]?.ToString() : "";
                        blogKey.Src = Helper.GetSitecoreDomain() + image;
                        blogKey.imgTitle = image != "" ?
                                         Helper.GetSolrImageDetails(item, "blogimage").Fields[ImageFeilds.Fields.TitleFieldName].Value : "";
                        blogKey.Alt = image != "" ?
                                         Helper.GetSolrImageDetails(item, "blogimage").Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                        blogKey.Link = string.Format(strSitedomain + realtyLink + "/{0}/{1}", "blogs", item.Fields.Keys.Contains("slug") ? item?.Fields["slug"]?.ToString()?.ToLower() : "");
                        blog.Keys.Add(blogKey);
                    }
                    blog.Keys = blog.Keys.OrderByDescending(x => x.Date).ToList();
                    ////pageNo = pageNo == 0 ? 1 : pageNo;
                    blog.TotalBlogs = blog.Keys.Count();
                    blog.CurrentBlogs = pageNo * pageCount;
                    Int32 totalPages = blog.TotalBlogs / pageCount;
                    var takeNthRecored = new object();
                    if (pageNo == 0)
                    {

                        takeNthRecored = blog.Keys;
                        genericObject.TotalBlogs = blog.TotalBlogs;
                        genericObject.CurrentBlogs = blog.TotalBlogs;
                        genericObject.IsRecordsEnd = true;
                    }
                    else if (pageNo * pageCount <= blog.TotalBlogs)
                    {
                        takeNthRecored = pageNo == 1 ? blog.Keys.Take(pageCount) : blog.Keys.Skip((pageNo - 1) * pageCount).Take(pageCount);
                        genericObject.TotalBlogs = blog.TotalBlogs;
                        genericObject.CurrentBlogs = blog.CurrentBlogs;
                        genericObject.IsRecordsEnd = false;
                    }
                    else if (pageNo * pageCount > blog.TotalBlogs)
                    {
                        takeNthRecored = pageNo == 1 ? blog.Keys.Take(pageCount) : blog.Keys.Skip((pageNo - 1) * pageCount).Take(blog.TotalBlogs);
                        genericObject.TotalBlogs = blog.TotalBlogs;
                        genericObject.CurrentBlogs = blog.TotalBlogs;
                        genericObject.IsRecordsEnd = true;
                    }
                    else
                    {
                        //takeNthRecored = pageNo == 1 ? blog.Keys.Take(pageCount) : blog.Keys.Skip((pageNo - 1) * pageCount).Take(blog.TotalBlogs);
                        genericObject.TotalBlogs = 0;
                        genericObject.CurrentBlogs = 0;
                        genericObject.IsRecordsEnd = true;
                    }
                    genericObject.Data = takeNthRecored;
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return genericObject;
        }

        private DateTime ConvertDateFormat(string datetimeString)
        {

            DateTime utcdate = DateTime.Parse(datetimeString, CultureInfo.InvariantCulture);
            return TimeZoneInfo.ConvertTimeFromUtc(utcdate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        }
        private string ConvertDateFormatTostring(DateTime date)
        {
            return date.ToString("MMM dd yyyy");
        }

        private string GetSlugValue(Item catItem)
        {
            string value = string.Empty;
            try
            {
                if (catItem != null)
                {
                    value = catItem?.Fields[BlogAnchorsTemplate.Fields.SlugText]?.Value?.ToLower();
                }

            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return value;
        }

        private string GetLinkURLbyField(Field field)
        {
            string linkUrl = string.Empty;
            try
            {
                LinkField linkField = field;
                if (!String.IsNullOrEmpty(linkField.ToString()))
                {
                    switch (linkField.LinkType)
                    {
                        case "internal":
                        case "external":
                        case "mailto":
                        case "anchor":
                        case "javascript":
                            linkUrl = Helper.GetUrlDomain() + linkField.Url;
                            break;
                        default:
                            linkUrl = string.Empty;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return linkUrl;
        }

        private List<object> GetBlogAnchors(MultilistField blogAnchorsField)
        {
            List<Object> anchorList = new List<Object>();
            var commonItem = Sitecore.Context.Database.GetItem(Templates.commonData.ItemID);
            var realtyLink = !string.IsNullOrEmpty(commonItem.Fields[Templates.commonData.Fields.blogLinkId].Value.ToString()) ?
                            commonItem.Fields[Templates.commonData.Fields.blogLinkId].Value.ToString() : "";
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            var blogItem = Sitecore.Context.Database.GetItem(Templates.Communication.ItemID);
            var InnerblogItems = blogItem.Children.Where(x => x.TemplateID == Communication.Fields.innerTemplateID).ToList();
            var selectedBlog = InnerblogItems.Select(x => x.Fields[Communication.Fields.categoryID].Value).ToList();
            try
            {
                if (blogAnchorsField != null && blogAnchorsField.GetItems().Count() > 0)
                {
                    foreach (Item item in blogAnchorsField.GetItems())
                    {
                        var selectCat = selectedBlog.Select(x => x.Contains(item.ID.ToString().ToUpper()));
                        if (item.ID.ToString() == "{04EC5AAB-9231-4C3B-88D9-57753ADEF368}" || selectCat.Contains(true))
                        {
                            BlogAnchor blogAnchor = new BlogAnchor();
                            blogAnchor.CategoryTabTitle = item?.Fields[BlogAnchorsTemplate.Fields.Keyword]?.Value;
                            blogAnchor.CategoryTabID = item?.ID?.ToString();
                            blogAnchor.Link = item?.Fields[BlogAnchorsTemplate.Fields.HashData]?.Value;
                            blogAnchor.Slug = string.Format(strSitedomain + realtyLink + "/blogs/category/{0}", item?.Fields[BlogAnchorsTemplate.Fields.SlugText]?.Value?.ToLower());
                            anchorList?.Add(blogAnchor);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return anchorList;
        }
    }
}