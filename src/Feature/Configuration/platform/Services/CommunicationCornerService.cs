using Adani.SuperApp.Realty.Feature.Configuration.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using static Adani.SuperApp.Realty.Feature.Configuration.Platform.Templates;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.Services
{
    public class CommunicationCornerService : ICommunicationCornerService
    {
        private readonly ILogRepository _logRepository;
        public CommunicationCornerService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public CommunicationCornerModel GetCommunicationCornerData(Rendering rendering)
        {
            CommunicationCornerModel ccModel = new CommunicationCornerModel();
            try
            {
                Item renderingItem = rendering?.Item;
                if (renderingItem != null)
                {
                    GenericObject blogAnchors = new GenericObject();
                    blogAnchors.Data = new List<object>();
                    MultilistField blogAnchorsField = renderingItem?.Fields[Templates.BlogAnchorsTemplate.Fields.BlogAnchorField];
                    blogAnchors.Data = GetBlogAnchors(blogAnchorsField);
                    ccModel.BlogAnchors = blogAnchors;

                    Content content = new Content();
                    content.Title = renderingItem?.Fields[BlogContentTemplate.Fields.Title]?.Value;
                    content.ComCorner = renderingItem?.Fields[BlogContentTemplate.Fields.Description]?.Value;
                    ccModel.Content = content;

                    OtherArticleData otherArticleData = new OtherArticleData();
                    otherArticleData.ComponentName = renderingItem?.Fields[BlogAnchorsTemplate.Fields.OtherArticlesTitle]?.Value;
                    otherArticleData.Data = GetOtherArticles(renderingItem?.Fields[BlogAnchorsTemplate.Fields.OtherArticles]);
                    ccModel.OtherArticles = otherArticleData;
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return ccModel;

        }

        private List<OtherArticle> GetOtherArticles(Field field)
        {
            List<OtherArticle> otherArticles = new List<OtherArticle>();
            try
            {
                var commonItem = Sitecore.Context.Database.GetItem(Templates.commonData.ItemID);
                string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
                MultilistField mulField = field;
                if (mulField != null && mulField.GetItems().Count() > 0)
                {
                    foreach (Item item in mulField.GetItems())
                    {
                        OtherArticle otherArticle = new OtherArticle();
                        otherArticle.ArticleType = item?.Fields[OtherArticleTemplate.OtherArticleFields.ArticleType]?.Value;
                        var ArticleLink = Helper.GetLinkTextbyField(item, item?.Fields[OtherArticleTemplate.OtherArticleFields.ArticleLink]);
                        otherArticle.ArticleLink = ArticleLink;
                        otherArticle.ArticleLinkIcon = item?.Fields[OtherArticleTemplate.OtherArticleFields.ArticleLinkIcon]?.Value;
                        otherArticle.ArticleLinkTitle = item?.Fields[OtherArticleTemplate.OtherArticleFields.ArticleLinkTitle]?.Value;
                        otherArticle.ArticleLinkThumb = Helper.GetImageURLbyField(item?.Fields[OtherArticleTemplate.OtherArticleFields.ArticleThumb]);
                        otherArticle.ArticleLinkThumbAlt = Helper.GetImageAltbyField(item?.Fields[OtherArticleTemplate.OtherArticleFields.ArticleThumb]);
                        otherArticle.ArticleTitle = item?.Fields[OtherArticleTemplate.OtherArticleFields.ArticleTitle]?.Value;
                        otherArticle.ArticleDescription = item?.Fields[OtherArticleTemplate.OtherArticleFields.ArticleDescription]?.Value;
                        otherArticles?.Add(otherArticle);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return otherArticles;
        }

        private List<object> GetBlogAnchors(MultilistField blogAnchorsField)
        {
            List<Object> anchorList = new List<Object>();
            var commonItem = Sitecore.Context.Database.GetItem(Templates.commonData.ItemID);
            var blogItem = Sitecore.Context.Database.GetItem(Templates.Communication.ItemID);
            var InnerblogItems = blogItem.Children.Where(x => x.TemplateID == Communication.Fields.innerTemplateID).ToList();
            var selectedBlog = InnerblogItems.Select(x => x.Fields[Communication.Fields.categoryID].Value).ToList();
            var realtyLink = !string.IsNullOrEmpty(commonItem.Fields[Templates.commonData.Fields.blogLinkId].Value.ToString()) ?
                            commonItem.Fields[Templates.commonData.Fields.blogLinkId].Value.ToString() : "";
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
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
                            blogAnchor.Title = item?.Fields[BlogAnchorsTemplate.Fields.Keyword]?.Value;
                            blogAnchor.Link = item?.Fields[BlogAnchorsTemplate.Fields.HashData]?.Value;
                            blogAnchor.Text = Helper.GetLinkTextbyField(item, item?.Fields[Templates.TitleDescription.Fields.Link]);
                            var slug = string.Format(realtyLink + "/blogs/category/{0}", item?.Fields[BlogAnchorsTemplate.Fields.SlugText]?.Value?.ToLower());
                            blogAnchor.Slug = slug != "" ? strSitedomain + slug : strSitedomain;
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