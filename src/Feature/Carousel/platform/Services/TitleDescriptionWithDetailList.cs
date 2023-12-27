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


namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public class TitleDescriptionWithDetailList: ITitleDescriptionWithDetailList
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public TitleDescriptionWithDetailList(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel ItemDetails(Rendering rendering, Item datasource)
        {
            WidgetModel ItemdetailsData = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);

                if (widget != null)
                {

                    ItemdetailsData.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    ItemdetailsData.widget = new WidgetItem();
                }
                ItemdetailsData.widget.widgetItems = GetBusinesData(datasource);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetBusinesData gives -> " + ex.Message);
            }
            return ItemdetailsData;
        }

        public List<object> GetBusinesData(Item datasource)
        {
            List<object> DetailDataList = new List<object>();
            TitleDescriptionWithDetailModel ourBusinessModel;


            if (datasource.Children != null && datasource.Children.Count > 0)
            {
                ourBusinessModel = new TitleDescriptionWithDetailModel();
                foreach (Item folderData in datasource.Children)
                {

                    ourBusinessModel.title = folderData.Fields[Constant.Title].Value;
                    ourBusinessModel.detail = folderData.Fields[Constant.Description].Value;                   
                    List<DetailsList> sliderList = new List<DetailsList>();
                    DetailsList slider;
                    Sitecore.Data.Fields.MultilistField multiselectField = folderData.Fields[Constant.OBListData];
                    Sitecore.Data.Items.Item[] items = multiselectField.GetItems();

                    if (items != null && items.Length > 0)
                    {
                        foreach (Item item in items)
                        {
                            slider = new DetailsList();
                            slider.value = item.Fields[Constant.value].Value;
                            slider.sign = item.Fields[Constant.sign].Value;
                            slider.detail = item.Fields[Constant.detail].Value;

                            sliderList.Add(slider);
                        }
                        ourBusinessModel.DetailsDataList = sliderList;
                    }

                }
                DetailDataList.Add(ourBusinessModel);


            }

            return DetailDataList;
        }
    }
}