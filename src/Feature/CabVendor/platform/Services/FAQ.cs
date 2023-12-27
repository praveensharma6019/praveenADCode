
using Adani.SuperApp.Airport.Feature.CabVendor.Models;
using Adani.SuperApp.Airport.Feature.CabVendor.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Extensions;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adani.SuperApp.Airport.Feature.CabVendor.Platform.Services
{
    public class FAQ : IFAQ
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;
        public FAQ(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetFAQList(Rendering rendering, string location)
        {

            WidgetModel CarouselOutletData = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[VendorConstant.RenderingParamField]);
                CarouselOutletData.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                CarouselOutletData.widget.widgetItems = ParseFAQData(rendering,location);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetFAQList gives -> " + ex.Message);
            }


            return CarouselOutletData;
        }

        private List<object> ParseFAQData(Rendering rendering,  string location)
        {
            List<Object> outletsData = new List<Object>();
            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
               ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
               : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    _logRepository.Info("Datasource not selected");
                }
                else
                {
                    string Temp = FAQData.FAQTemplateID.ToString();

                     List<Item> faq_list = new List<Item>();

                    if (datasource.GetChildren().Where(x => x.TemplateID.ToString().Equals(Temp)).ToList() != null)
                    {
                        faq_list = datasource.GetChildren().Where(x => x.TemplateID.ToString().Equals(Temp)).ToList();
                    }

                    FAQModel _faqModel = null;
                    List<Item> FilteredOFAQData = GetFAQListLocationBasis(faq_list,  getLocationID(location));


                    foreach (Sitecore.Data.Items.Item outletItem in FilteredOFAQData)
                    {
                        _faqModel = new FAQModel();
                        _faqModel.title = !string.IsNullOrEmpty(outletItem.Fields[FAQData.Title].Value.ToString()) ? outletItem.Fields[FAQData.Title].Value.ToString() : string.Empty;
                            _faqModel.faqCtaURL = _helper.GetLinkURL(outletItem, FAQData.CTALink);
                        _faqModel.faqCtaText = !string.IsNullOrEmpty(outletItem.Fields[FAQData.CTAText].Value.ToString()) ? outletItem.Fields[FAQData.CTAText].Value.ToString() : string.Empty;
                        _faqModel.faqHTML = !string.IsNullOrEmpty(outletItem.Fields[FAQData.FAQHtml].Value.ToString()) ? outletItem.Fields[FAQData.FAQHtml].Value.ToString() : string.Empty;
                          Sitecore.Data.Fields.MultilistField multilistField = outletItem.Fields[FAQData.FAQList];
                       List<FAQCard> cardList= new List<FAQCard>();
                        FAQCard card = null;
                        List<Item> applicableOutlets = multilistField.GetItems().ToList();

                       
                        foreach (Sitecore.Data.Items.Item item in applicableOutlets)
                        {
                            card = new FAQCard();
                            card.title = !string.IsNullOrEmpty(item.Fields[FAQData.Question].Value.ToString()) ? item.Fields[FAQData.Question].Value.ToString() : string.Empty;
                            card.body = !string.IsNullOrEmpty(item.Fields[FAQData.Answer].Value.ToString()) ? item.Fields[FAQData.Answer].Value.ToString() : string.Empty;
                            cardList.Add(card);

                        }
                        _faqModel.list=cardList;
                     
                        outletsData.Add(_faqModel);

                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" ParseOutletData gives -> " + ex.Message);
            }

            return outletsData;
        }

        private List<Item> GetFAQListLocationBasis(List<Sitecore.Data.Items.Item> childList,string location)
        {

            List<Item> childlst = new List<Item>();

            childlst = childList.Where(p => p[FAQData.Location].Contains(location)).ToList();
                                     
            
                                             
            return childlst;
        }


        private string getLocationID(string location)
        {
            var locationSmaller = location.ToLower();
           // Airport_Location parsedLocation = (Airport_Location)Enum.Parse(typeof(Airport_Location), location);

            string LocationID = string.Empty;

            switch (locationSmaller)
            {
                case "amd":
                    LocationID = VendorConstant.Ahmedabad;
                    break;

                case "gau":
                    LocationID = VendorConstant.Guwahati;
                    break;
                case "jai":
                    LocationID = VendorConstant.Jaipur;
                    break;
                case "lko":
                    LocationID = VendorConstant.Lucknow;
                    break;
                case "trv":
                    LocationID = VendorConstant.Thiruvananthapuram;
                    break;
                case "ixe":
                    LocationID = VendorConstant.Mangaluru;
                    break;
                case "bom":
                    LocationID = VendorConstant.Mumbai;
                    break;
                case "adlone":
                    LocationID = VendorConstant.adaniOne;
                    break;
                default:
                    LocationID = VendorConstant.adaniOne;
                    break;


            }
            return LocationID;
        }

       
    }
}