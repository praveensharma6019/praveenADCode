using Adani.SuperApp.Realty.Feature.Faq.Platform.Models;
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
using static Adani.SuperApp.Realty.Feature.Faq.Platform.Templates;

namespace Adani.SuperApp.Realty.Feature.Faq.Platform.Services
{
    public class QuickLinksFaqService: IQuickLinksFaqService
    {
        private readonly ILogRepository _logRepository;
        public QuickLinksFaqService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public QuickLinksFaqModel GetQuickLinksFaqData(Rendering rendering)
        {
            QuickLinksFaqModel quickLinksFaqModel = new QuickLinksFaqModel();
            try
            {
                Item _faqItem = rendering?.Item;
                if (_faqItem != null)
                {
                    quickLinksFaqModel.QuickLinksFaq = new List<QuickLinksFaq>();
                    foreach (Item item in _faqItem.GetChildren())
                    {
                        QuickLinksFaq quickLinksFaq = new QuickLinksFaq();
                        quickLinksFaq.Category = item?.Fields[QuickLinsFaqTemplate.CategoryFields.Title]?.Value;
                        quickLinksFaq.Heading = item?.Fields[QuickLinsFaqTemplate.CategoryFields.Heading]?.Value;
                        quickLinksFaq.FaqItems = GetFaqData(item);
                        quickLinksFaqModel?.QuickLinksFaq?.Add(quickLinksFaq);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return quickLinksFaqModel;
        }


        private List<Data> GetFaqData(Item item)
        {
            List<Data> dataList = new List<Data>();
            try
            {
                foreach (Item innerItem in item.GetChildren())
                {
                    Data data = new Data();
                    data.Title = innerItem?.Fields[Templates.Faq.Fields.Title].Value;
                    data.Body = innerItem?.Fields[Templates.Faq.Fields.Content].Value;
                    dataList?.Add(data);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return dataList;
        }
    }
}