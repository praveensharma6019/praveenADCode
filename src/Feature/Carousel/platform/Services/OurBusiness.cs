using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public class OurBusiness : IOurBusiness
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public OurBusiness(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel OurBusinessData(Rendering rendering, Item datasource)
        {
            WidgetModel OurBusinessListData = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);

                if (widget != null)
                {

                    OurBusinessListData.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    OurBusinessListData.widget = new WidgetItem();
                }
                OurBusinessListData.widget.widgetItems = GetBusinesData(datasource);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetBusinesData gives -> " + ex.Message);
            }
            return OurBusinessListData;
        }

        public List<object> GetBusinesData(Item datasource)
        {
            List<object> BusinessDataList = new List<object>();
            OurBusinessModel ourBusinessModel;


            if (datasource.Children != null && datasource.Children.Count > 0)
            {
                ourBusinessModel = new OurBusinessModel();
                foreach (Item folderData in datasource.Children)
                {

                    ourBusinessModel.title = folderData.Fields[Constant.OBTitle].Value;
                    ourBusinessModel.description = folderData.Fields[Constant.OBDescription].Value;
                    ourBusinessModel.Url = _helper.LinkUrl(folderData.Fields[Constant.OBUrl]);
                    ourBusinessModel.UrlTarget = _helper.LinkUrlTarget(folderData.Fields[Constant.OBUrl]);
                    ourBusinessModel.UrlName = folderData.Fields[Constant.OBUrlName].Value;
                    ourBusinessModel.image = !string.IsNullOrEmpty(folderData.Fields[Constant.Image].Value) ? _helper.GetImageURL(folderData.Fields[Constant.Image]) : string.Empty;
                    List<BusinessDataList> sliderList = new List<BusinessDataList>();
                    BusinessDataList slider;
                    Sitecore.Data.Fields.MultilistField multiselectField = folderData.Fields[Constant.OBListData];
                    Sitecore.Data.Items.Item[] items = multiselectField.GetItems();

                    if (items != null && items.Length > 0)
                    {
                        foreach (Item item in items)
                        {
                            slider = new BusinessDataList();
                            slider.ImageTitle = item.Fields[Constant.OBImageTitle].Value;
                            slider.ImageDescripton = item.Fields[Constant.OBImageDescripton].Value;
                            slider.WebImage = item.Fields[Constant.OBWebImage] != null ? _helper.GetImageURL(item, Constant.OBWebImage) : String.Empty;
                            slider.MobileImage = item.Fields[Constant.OBMobileImage] != null ? _helper.GetImageURL(item, Constant.OBMobileImage) : String.Empty;
                            slider.CTAUrl = _helper.LinkUrl(item.Fields[Constant.OBCTAUrl]);
                            slider.CTAUrlTarget = _helper.LinkUrlTarget(item.Fields[Constant.OBCTAUrl]);
                            sliderList.Add(slider);
                        }
                        ourBusinessModel.BuisnessDataList = sliderList;
                    }

                }
                BusinessDataList.Add(ourBusinessModel);


            }

            return BusinessDataList;
        }
    }
}