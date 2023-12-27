using Adani.SuperApp.Airport.Feature.CustomContent.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services
{
    public class FaqJSON : IFaqJSON
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetservice;

        public FaqJSON(ILogRepository logRepository, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._widgetservice = widgetService;
        }

        FaqJSONWidget IFaqJSON.GetFAQJSONList(Rendering rendering)
        {
            FaqJSONWidget faqListingWidgits = new FaqJSONWidget();
            try
            {
                Item widget = null;
                widget = Sitecore.Context.Database.GetItem(rendering.Parameters["Widget"]);
                faqListingWidgits.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                faqListingWidgits.widget.widgetItems = GetFaqListdata(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error(" FaqJSON GetFAQJSONList gives -> " + ex.Message);

            }

            return faqListingWidgits;
        }

        private List<Object> GetFaqListdata(Rendering rendering)
        {
            List<Object> faqJSONList = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? RenderingContext.Current.Rendering.Item
                : null;
                // Null Check for datasource
                if (datasource == null && datasource.Children.Count() == 0)
                {
                    throw new NullReferenceException("Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services.GetFaqListdata => Rendering Datasource is Empty");
                }
                var faqItems = datasource.Children.Where(a => a.TemplateID == Constants.FAQTemplateID).ToList();
                if (faqItems != null && faqItems.Count > 0)
                {
                    foreach (Item faq in faqItems)
                    {
                        FaqItem faqItem = new FaqItem();
                        faqItem.question = faq.Fields.Contains(Constants.Question) ? faq.Fields[Constants.Question].Value.ToString() : "";
                        faqItem.answer = faq.Fields.Contains(Constants.Answer) ? faq.Fields[Constants.Answer].Value.ToString() : "";
                        faqJSONList.Add(faqItem);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" Adani.SuperApp.Airport.Feature.CustomContent.Platform.CustomContent GetFaqListdata gives -> " + ex.Message);
            }

            return faqJSONList;
        }
    }
}