using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using Adani.SuperApp.Airport.Feature.Retail.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.Retail.Platform.Services
{
    public class SizeSelect : ISizeSelect
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;
        public SizeSelect(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetSizesData(Rendering rendering, string outletcode,string category,string isApp)
        {

            WidgetModel retailOutletData = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constants.RenderingParamField]);
                retailOutletData.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                retailOutletData.widget.widgetItems = SizeChartData(rendering,outletcode,category,isApp);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetSizesData gives -> " + ex.Message);
            }


            return retailOutletData;
        }

        private List<Object> SizeChartData(Rendering rendering, string outletcode,string category,string isApp)
        {
            List<Object> SizeChartDataList = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasource = RenderingContext.Current.Rendering.Item;
                // Null Check for datasource
                if (datasource == null)
                {
                    _logRepository.Error(" GetSizesData SizeChartData  data source is empty ");
                }
                var filteredData = GetFilteredSizeDetails(datasource.GetChildren().ToList(), outletcode, category);
                SizeChart sizesData;
                string[] emptyArr = new string[] { };
                foreach (Sitecore.Data.Items.Item item in filteredData)
                {
                    sizesData = new SizeChart();
                    sizesData.SizeTitle = !string.IsNullOrEmpty(item.Fields[Constants.SizeTitle].Value.ToString()) ? item.Fields[Constants.SizeTitle].Value.ToString() : string.Empty;
                    sizesData.SizeChartTitle = !string.IsNullOrEmpty(item.Fields[Constants.SizeChartTitle].Value.ToString()) ? item.Fields[Constants.SizeChartTitle].Value.ToString() : string.Empty;
                    Sitecore.Data.Fields.MultilistField applicableCategories = ((Sitecore.Data.Fields.MultilistField)item.Fields[Constants.ApplicableCategories]);
                    List<Item> categoryList = applicableCategories.GetItems().ToList();
                    var retailAll = (from c in categoryList
                                  where c.Fields[Constants.Title].Value == "retail-all"
                                 select c).Any();
                    List<sizeChartImages> sizeChartImagesList = new List<sizeChartImages>();

                    if (retailAll)
                    {
                        foreach (Sitecore.Data.Items.Item sizeItem in applicableCategories.GetItems().Where((a => (a.Fields[Constants.Title].Value=="retail-all"))).ToList())
                        {
                            foreach (Item tabNames in sizeItem.Children)
                            {
                                sizeChartImages sizeChartImage = new sizeChartImages();
                                if ((Sitecore.Data.Fields.ImageField)tabNames.Fields[Constants.ImageSrc] != null)
                                    sizeChartImage.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)tabNames.Fields[Constants.ImageSrc]);
                                if ((Sitecore.Data.Fields.ImageField)tabNames.Fields[Constants.MobileImage] != null)
                                    sizeChartImage.MobileImage = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)tabNames.Fields[Constants.MobileImage]);
                                if (isApp == "true")
                                {
                                    if ((Sitecore.Data.Fields.ImageField)tabNames.Fields[Constants.MobileImage] != null)
                                    { sizeChartImage.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)tabNames.Fields[Constants.MobileImage]); }
                                    else { sizeChartImage.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)tabNames.Fields[Constants.ImageSrc]); }
                                }
                                sizeChartImagesList.Add(sizeChartImage);
                            }
                        }
                    }
                    else
                    {
                        foreach (Sitecore.Data.Items.Item sizeItem in applicableCategories.GetItems().Where((a => category.ToLower().Contains(a.Fields[Constants.Title].Value))).ToList())
                        {
                            foreach (Item tabNames in sizeItem.Children)
                            {
                                sizeChartImages sizeChartImage = new sizeChartImages();
                                if ((Sitecore.Data.Fields.ImageField)tabNames.Fields[Constants.ImageSrc] != null)
                                    sizeChartImage.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)tabNames.Fields[Constants.ImageSrc]);
                                if ((Sitecore.Data.Fields.ImageField)tabNames.Fields[Constants.MobileImage] != null)
                                    sizeChartImage.MobileImage = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)tabNames.Fields[Constants.MobileImage]);
                                if (isApp == "true")
                                {
                                    if ((Sitecore.Data.Fields.ImageField)tabNames.Fields[Constants.MobileImage] != null)
                                    { sizeChartImage.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)tabNames.Fields[Constants.MobileImage]); }
                                    else { sizeChartImage.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)tabNames.Fields[Constants.ImageSrc]); }
                                }
                                sizeChartImagesList.Add(sizeChartImage);
                            }
                        }
                    }
                    sizesData.SizeChartImages = sizeChartImagesList;
                    SizeChartDataList.Add(sizesData);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetSizesData SizeChartData gives -> " + ex.Message);
            }

            return SizeChartDataList;
        }
        private List<Item> GetFilteredSizeDetails(List<Sitecore.Data.Items.Item> childList, string outletCode, string category)
        {

            List<Item> childlst = new List<Item>();
            foreach (Sitecore.Data.Items.Item sizeItem in childList)
            {
                Sitecore.Data.Fields.MultilistField multilistField = sizeItem.Fields[Constants.ApplicableOutlets];
                List<Item> applicableOutlets = multilistField.GetItems().ToList();
                var list = applicableOutlets.Where(a => a.Fields[Constants.OutletCode].Value.Equals(outletCode)).ToList();
                if (list != null && list.Count != 0)
                {
                    childlst.Add(sizeItem);
                }
            }
            return childlst;
        }



    }
}