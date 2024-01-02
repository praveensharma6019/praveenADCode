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

namespace Adani.SuperApp.Realty.Feature.Faq.Platform.Services
{
    public class FaqService : IFaqService
    {
        private readonly ILogRepository _logRepository;
        public FaqService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }

        public FaqList GetFaqData(Rendering rendering)
        {
            FaqList faqList = new FaqList();
            try
            {
                var commonItem = Sitecore.Context.Database.GetItem(Templates.CommonTextItem);
                var strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
                Item _faqItem = rendering?.Item;
                if (_faqItem != null)
                {
                    MultilistField faqField = _faqItem?.Fields[Templates.FAQList];
                    var renderinParams = rendering.Parameters;
                    faqList.FaqLink = renderinParams != null ? strSitedomain + renderinParams["SeeAllLink"] : string.Empty; //Helper.GetLinkURL(commonItem, Templates.faqLinkText);
                    faqList.FaqItems = new List<FaqItem>();

                    foreach (Item item in faqField.GetItems())
                    {
                        FaqItem faqItem = new FaqItem();
                        faqItem.Title = item?.Fields[Templates.Faq.Fields.Title].Value;
                        faqItem.Body = item?.Fields[Templates.Faq.Fields.Content].Value;
                        faqList.FaqItems?.Add(faqItem);
                    }
                    faqList.FaqItems = faqList.FaqItems.Count() > 8 ? faqList.FaqItems.Take(8).ToList() : faqList.FaqItems;

                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return faqList;

        }
        public LocationFaqList LocationPageFaqList(Rendering rendering)
        {
            LocationFaqList faqList = new LocationFaqList();
            string urlParam = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query).Get("Location");
            urlParam = !string.IsNullOrEmpty(urlParam) ? urlParam.Replace("property-in-", "") : "";
            try
            {
                var commonItem = Sitecore.Context.Database.GetItem(Templates.CommonTextItem);
                var strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
                Item _faqItem = rendering?.Item;
                if (_faqItem != null)
                {
                    var childen = _faqItem.Children.Where(x => x.TemplateID == Templates.FolderID).ToList();
                    List<LocationFaq> listoFlocationFaqItems = new List<LocationFaq>();
                    if (string.IsNullOrEmpty(urlParam))
                    {
                        faqList.FaqLink = strSitedomain + Helper.GetLinkURL(commonItem, Templates.faqLinkText);
                        foreach (Item item in childen)
                        {
                            LocationFaq locationFaqItem = new LocationFaq();
                            var nestedchild = item.Axes.GetDescendants().Where(x => x.TemplateID == Templates.Faq.Id).ToList();
                            List<LocationFaq> LocationFaqList = new List<LocationFaq>();
                            foreach (var nesteditem in nestedchild)
                            {
                                LocationFaq faqItem = new LocationFaq();
                                faqItem.Location = item.Name;
                                faqItem.Title = nesteditem?.Fields[Templates.Faq.Fields.Title].Value;
                                faqItem.Body = nesteditem?.Fields[Templates.Faq.Fields.Content].Value;
                                LocationFaqList.Add(faqItem);
                            }
                            faqList.FaqItems = LocationFaqList;
                        }
                    }
                    else
                    {
                        foreach (Item item in childen)
                        {
                            if (urlParam.ToString().ToLower() == item.Name.ToString().ToLower())
                            {
                                var faqListFolder = item.Children.Where(x => x.TemplateID == Templates.LocationFAQList).FirstOrDefault();
                                faqList.FaqLink = Helper.GetLinkURL(faqListFolder, Templates.seealltext) != null ?
                                   Helper.GetLinkURL(faqListFolder, Templates.seealltext) : "";
                            }
                        }
                        foreach (Item item in childen)
                        {
                            if (urlParam.ToString().ToLower() == item.Name.ToString().ToLower())
                            {
                                LocationFaq locationFaqItem = new LocationFaq();
                                var faqListFolder = item.Children.Where(x => x.TemplateID == Templates.LocationFAQList).FirstOrDefault();
                                MultilistField listofFAQ = faqListFolder.Fields[Templates.LocationFAQItem];
                                var nestedchild = item.Axes.GetDescendants().Where(x => x.TemplateID == Templates.Faq.Id).ToList();
                                List<LocationFaq> LocationFaqList = new List<LocationFaq>();
                                foreach (var nesteditem in listofFAQ.GetItems().Where(x => x.TemplateID == Templates.Faq.Id))
                                {
                                    LocationFaq faqItem = new LocationFaq();
                                    faqItem.Location = item.Name;
                                    faqItem.Title = nesteditem?.Fields[Templates.Faq.Fields.Title].Value;
                                    faqItem.Body = nesteditem?.Fields[Templates.Faq.Fields.Content].Value;
                                    LocationFaqList.Add(faqItem);
                                }
                                faqList.FaqItems = LocationFaqList;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return faqList;
        }
        public LocationFaqList SeoPageFaqList(Rendering rendering)
        {
            LocationFaqList faqList = new LocationFaqList();
            try
            {
                var commonItem = Sitecore.Context.Database.GetItem(Templates.CommonTextItem);
                var strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
                Item _faqItem = rendering?.Item;
                List<Item> _faqItemList = new List<Item>() { _faqItem };
                if (_faqItem != null)
                {
                        List<LocationFaq> listoFlocationFaqItems = new List<LocationFaq>();
                        faqList.FaqLink = Helper.GetLinkURL(commonItem, Templates.faqLinkText);
                        foreach (Item item in _faqItemList)
                        {
                            LocationFaq locationFaqItem = new LocationFaq();
                            var nestedchild = item.Axes.GetDescendants().Where(x => x.TemplateID == Templates.Faq.Id).ToList();
                            List<LocationFaq> LocationFaqList = new List<LocationFaq>();
                            foreach (var nesteditem in nestedchild)
                            {
                                LocationFaq faqItem = new LocationFaq();
                                //faqItem.Location = item.Name;
                                faqItem.Title = nesteditem?.Fields[Templates.Faq.Fields.Title].Value;
                                faqItem.Body = nesteditem?.Fields[Templates.Faq.Fields.Content].Value;
                                LocationFaqList.Add(faqItem);
                            }
                            faqList.FaqItems = LocationFaqList;
                        }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return faqList;
        }
    }
}