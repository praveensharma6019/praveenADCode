using System;
using System.Collections.Generic;
using System.Linq;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Extensions;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public class FAQLandingPageService : IFAQLandingPageService
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public FAQLandingPageService(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetFAQData(Rendering rendering)
        {
            WidgetModel widgetList = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                widgetList.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                widgetList.widget.widgetItems = GetFAQDataList(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetFAQData in FAQLandingPageService gives -> " + ex.Message);
            }
            return widgetList;
        }

        private List<object> GetFAQDataList(Rendering rendering)
        {
            List<Object> FAQDataList = new List<Object>();
            try
            {
                var datasourceItem = rendering.Item;
                // Null Check for datasource
                if (datasourceItem != null && datasourceItem.GetChildren() != null && datasourceItem.GetChildren().Count() > 0)
                {
                    Category category = null;
                    foreach (Sitecore.Data.Items.Item item in datasourceItem.Children)
                    {
                        category = new Category();
                        MultilistField faqList = item.Fields[Constant.AllCategoriesList];
                        category.categoryTitle = item.Fields[Constant.Heading] != null ? item.Fields[Constant.Heading].Value : string.Empty;
                        category.seeAllLink = item.Fields[Constant.SeeAllLink] != null ? _helper.LinkUrl(item.Fields[Constant.SeeAllLink]) : string.Empty;
                        if (faqList != null)
                        {
                            Item[] faqsList = faqList.GetItems();
                            category.faqs = faqsList.Select(y => new Faq { question = y.Fields[Constant.Question] != null ? y.Fields[Constant.Question].Value : string.Empty
                                , redirectionLink = item.Fields[Constant.FAQQuestionRedirectLink] != null ? _helper.LinkUrl(item.Fields[Constant.FAQQuestionRedirectLink]) + "?" + y.DisplayName : string.Empty
                            }).ToList();
                        }
                        FAQDataList.Add(category);
                    }
                } 
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetFAQDataList in FAQLandingPageService gives -> " + ex.Message);
            }
            return FAQDataList;
        }

    }
}