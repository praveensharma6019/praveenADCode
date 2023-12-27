using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public class BestSellerDutyFreeService : IBestSellerDutyFreeService
    {
        private readonly ILogRepository _logRepository;

        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;
        public BestSellerDutyFreeService(ILogRepository logRepository, IHelper helper, IWidgetService widget)
        {

            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widget;
        }
        public WidgetModel GetBestSellerDutyFreeService(Sitecore.Mvc.Presentation.Rendering rendering, string queryString)
        {
            WidgetModel bestSellerDutyFree = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                if (widget != null)
                {
                    //WidgetService widgetService = new WidgetService();
                    bestSellerDutyFree.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    bestSellerDutyFree.widget = new WidgetItem();
                }
                bestSellerDutyFree.widget.widgetItems = GetSliderData(rendering, queryString);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" BestSellerDutyFreeService GetBestSellerDutyFreeService gives -> " + ex.Message);
            }


            return bestSellerDutyFree;
        }

        private List<Object> GetSliderData(Rendering rendering, string queryString)
        {
            List<Object> sliderDataList = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasource = RenderingContext.Current.Rendering.Item;
                // Null Check for datasource
                if (datasource == null)
                {
                    _logRepository.Error(" BestSellerDutyFreeService GetSliderData  data source is empty ");
                }

                BestSellerDutyFree slider;
                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    slider = new BestSellerDutyFree();
                    slider.isAirportSelectNeeded = _helper.GetCheckboxOption(item.Fields[Constant.isAirportSelectNeeded]);
                    slider.Title = item[Constant.CardTitle];
                    slider.ImageSrc = item.Fields[Constant.SellerCard] != null ? _helper.GetImageURL(item, Constant.SellerCard) : String.Empty;
                    slider.Price = item[Constant.Price];
                    slider.Amount = item[Constant.Amount];
                    slider.OfferText = item[Constant.OfferText];
                    slider.UniqueId = item[Constant.UniqueId];
                    slider.MobileImage = item.Fields[Constant.MobileImage] != null ? _helper.GetImageURL(item, Constant.MobileImage) : String.Empty;
                    slider.SKUCode = item.Fields[Constant.SKUCode].Value;
                    slider.StoreType = _helper.GetDropListValue(item.Fields[Constant.StoreTypeFiled]);
                    slider.apiUrl = item.Fields[Constant.apiURL].Value;

                    sliderDataList.Add(slider);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" BestSellerDutyFreeService GetSliderData gives -> " + ex.Message);
            }

            return sliderDataList;
        }
    }
}