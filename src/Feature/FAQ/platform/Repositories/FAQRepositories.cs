using Adani.SuperApp.Airport.Feature.FAQ.Interfaces;
using Adani.SuperApp.Airport.Feature.FAQ.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.FAQ.Repositories
{
    public class FAQRepositories : IFAQ
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        public FAQRepositories(ILogRepository logRepository, IHelper helper)
        {

            this._logRepository = logRepository;
            this._helper = helper;
        }       

        public FAQData GetFaqData(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            _logRepository.Info("GetFaqData Initiated");
            FAQData _obj = new FAQData();
            try
            {
                var faqDatasource = RenderingContext.Current.Rendering.Item != null ? RenderingContext.Current.Rendering.Item : null;
                if (faqDatasource != null)
                {
                    _logRepository.Info("FAQ Datasource not null");
                    _obj.title = !string.IsNullOrEmpty(faqDatasource.Fields[Templates.FAQ.Fields.Title].Value.ToString()) ? faqDatasource.Fields[Templates.FAQ.Fields.Title].Value.ToString() : string.Empty;
                    _obj.faqHTML= !string.IsNullOrEmpty(faqDatasource.Fields[Templates.FAQ.Fields.FAQHTML].Value.ToString()) ? faqDatasource.Fields[Templates.FAQ.Fields.FAQHTML].Value.ToString() : string.Empty;
                    _obj.ctaURL=_helper.GetLinkURL(faqDatasource, Templates.FAQ.Fields.CTAKey);
                    _obj.ctaText = _helper.GetLinkText(faqDatasource, Templates.FAQ.Fields.CTAKey);
                    List<FAQCard> faqList = new List<FAQCard>();
                    if (((Sitecore.Data.Fields.MultilistField)faqDatasource.Fields[Templates.FAQ.Fields.FAQList]).Count > 0)
                    {
                        _logRepository.Info("FAQ list being populated");
                        foreach (Sitecore.Data.Items.Item item in ((Sitecore.Data.Fields.MultilistField)faqDatasource.Fields[Templates.FAQ.Fields.FAQList]).GetItems())
                        {
                            FAQCard cardItem = new FAQCard();
                            cardItem.title = !string.IsNullOrEmpty(item.Fields[Templates.FAQCard.Fields.Question].Value.ToString()) ? item.Fields[Templates.FAQCard.Fields.Question].Value.ToString() : "";
                            cardItem.body = !string.IsNullOrEmpty(item.Fields[Templates.FAQCard.Fields.Answer].Value.ToString()) ? item.Fields[Templates.FAQCard.Fields.Answer].Value.ToString() : "";
                            faqList.Add(cardItem);
                        }
                        _obj.list = faqList;
                    }
                }
                else return null;
                _logRepository.Info("GetFaqData Ended");
            }
            catch(Exception ex)
            {
                _logRepository.Error("GetFaqData throws Exception -> " + ex.Message);
            }
            
            return _obj;
        }
    }
}