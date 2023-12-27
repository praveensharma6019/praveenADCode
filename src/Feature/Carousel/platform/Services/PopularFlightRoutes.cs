using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public class PopularFlightRoutes : IPopularFlightRoutes
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public PopularFlightRoutes(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel PopularFlightData(Sitecore.Mvc.Presentation.Rendering rendering, Item datasource)
        {
            WidgetModel PopularFlightListData = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);

                if (widget != null)
                {

                    PopularFlightListData.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    PopularFlightListData.widget = new WidgetItem();
                }
                PopularFlightListData.widget.widgetItems = GetPopularFlightData(datasource);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetOfferList gives -> " + ex.Message);
            }
            return PopularFlightListData;
        }

        public List<object> GetPopularFlightData(Item datasource)
        {
            List<object> PopularFlightWidgetList = new List<object>();
            PopularRoutesModel popularRoutesModel;


            if (datasource.Children != null && datasource.Children.Count > 0)
            {
                foreach (Item folderData in datasource.Children)
                {
                    popularRoutesModel = new PopularRoutesModel();
                    popularRoutesModel.TabName = folderData.Name;
                    List<PopularRoutesDataList> sliderList = new List<PopularRoutesDataList>();
                    PopularRoutesDataList slider;
                    if (folderData.Children != null && folderData.Children.Count > 0)
                    {
                        foreach (Item RoutesData in folderData.Children)
                        {
                           
                            Sitecore.Data.Fields.MultilistField multiselectField = RoutesData.Fields["OtherServices"];
                            Sitecore.Data.Items.Item[] items = multiselectField.GetItems();
                            if (items != null && items.Length > 0)
                            {
                                foreach (Item item in items)
                                {
                                    slider = new PopularRoutesDataList();
                                    slider.title = item.Fields["CardTitle"].Value;
                                    slider.WebImage = item.Fields[Constant.LWWebImage] != null ? _helper.GetImageURL(item, Constant.LWWebImage) : String.Empty;
                                    slider.MobileImage = item.Fields[Constant.LWMobileImage] != null ? _helper.GetImageURL(item, Constant.LWMobileImage) : String.Empty;
                                    slider.description = item.Fields[Constant.LWDescription].Value;
                                    slider.RedirectLink = _helper.LinkUrl(item.Fields[Constant.LWRedirectLink]);
                                    sliderList.Add(slider);
                                }
                            }

                        }

                        popularRoutesModel.PopularRouteList = sliderList;
                    }
                    PopularFlightWidgetList.Add(popularRoutesModel);
                }

               
            }

            return PopularFlightWidgetList;
        }
    }
}

