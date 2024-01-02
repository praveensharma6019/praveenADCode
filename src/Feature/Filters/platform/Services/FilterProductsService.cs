using Adani.SuperApp.Realty.Feature.Filters.Platform.Models;
using Adani.SuperApp.Realty.Feature.Widget.Platform.Services;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Adani.SuperApp.Realty.Feature.Filters.Platform.Templates;

namespace Adani.SuperApp.Realty.Feature.Filters.Platform.Services
{
    public class FilterProductsService : IFilterProductsService
    {
        private readonly ILogRepository _logRepository;
        public FilterProductsService(ILogRepository logRepository)
        {

            this._logRepository = logRepository;
        }

        public FilterProductsWidgets GetProductFilters(Rendering rendering)
        {
            FilterProductsWidgets filterProductsWidgets = new FilterProductsWidgets();

            //try
            //{
            //    Item widget = null;
            //    IDictionary<string, string> paramDictionary = rendering.Parameters.ToDictionary(pair => pair.Key, pair => pair.Value);
            //    foreach (string key in paramDictionary.Keys.ToList())
            //    {
            //        if (Sitecore.Data.ID.TryParse(paramDictionary[key], out var itemId))
            //        {
            //            widget = rendering.RenderingItem.Database.GetItem(itemId);
            //        }
            //    }
            //    if (widget != null)
            //    {
            //        WidgetService widgetService = new WidgetService();
            //        filterProductsWidgets.widget = widgetService.GetWidgetItem(widget);
            //    }
            //    else
            //    {
            //        filterProductsWidgets.widget = new Feature.Widget.Platform.Models.WidgetItem();
            //    }
            //    filterProductsWidgets.widget.widgetItems = GetProductFilterList(rendering);
            //}
            //catch (Exception ex)
            //{
            //    _logRepository.Error(" FilterProductsService GetProductFilters gives -> " + ex.Message);
            //}

            return filterProductsWidgets;
        }
        public NoresultFounfEnquiryForm NoResultFounfItem(Rendering rendering)
        {
            NoresultFounfEnquiryForm filterProductsWidgets = new NoresultFounfEnquiryForm();
            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                filterProductsWidgets.heading = !string.IsNullOrEmpty(datasource.Fields[AboutUsCommonText.Fields.headingID].Value.ToString()) ? datasource.Fields[AboutUsCommonText.Fields.headingID].Value.ToString() : "";
                filterProductsWidgets.description = !string.IsNullOrEmpty(datasource.Fields[AboutUsCommonText.Fields.subheadingID].Value.ToString()) ? datasource.Fields[AboutUsCommonText.Fields.subheadingID].Value.ToString() : "";
                filterProductsWidgets.buttonText = !string.IsNullOrEmpty(datasource.Fields[AboutUsCommonText.Fields.btnTextID].Value.ToString()) ? datasource.Fields[AboutUsCommonText.Fields.btnTextID].Value.ToString() : "";
            }
            catch (Exception ex)
            {
                _logRepository.Error(" FilterProductsService NoResultFounfItem gives -> " + ex.Message);
            }

            return filterProductsWidgets;
        }

        private List<Object> GetProductFilterList(Rendering rendering)
        {
            List<Object> productFilterList = new List<Object>();

            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                FilterProducts filterProducts;
                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    filterProducts = new FilterProducts();
                    filterProducts.title = !string.IsNullOrEmpty(item.Fields[Constant.displayName].Value.ToString()) ? item.Fields[Constant.displayName].Value.ToString() : "";
                    filterProducts.apiUrl = !string.IsNullOrEmpty(item.Fields[Constant.apiUrl].Value.ToString()) ? item.Fields[Constant.apiUrl].Value.ToString() : "";

                    filterProducts.materialGroup = !string.IsNullOrEmpty(item.Fields[Constant.MaterialGroup].Value.ToString()) ? item.Fields[Constant.MaterialGroup].Value.ToString() : "";
                    filterProducts.category = !string.IsNullOrEmpty(item.Fields[Constant.Category].Value.ToString()) ? item.Fields[Constant.Category].Value.ToString() : "";
                    filterProducts.subCategory = !string.IsNullOrEmpty(item.Fields[Constant.SubCategory].Value.ToString()) ? item.Fields[Constant.SubCategory].Value.ToString() : "";
                    filterProducts.brand = !string.IsNullOrEmpty(item.Fields[Constant.Brand].Value.ToString()) ? item.Fields[Constant.Brand].Value.ToString() : "";
                    filterProducts.showOnHomepage = (item.Fields[Constant.ShowOnHomepage].Value.ToString() == "1") ? true : false;
                    filterProducts.newArrival = (item.Fields[Constant.NewArrival].Value.ToString() == "1") ? true : false;
                    filterProducts.popular = (item.Fields[Constant.Popular].Value.ToString() == "1") ? true : false;


                    productFilterList.Add(filterProducts);
                }

            }
            catch (Exception ex)
            {
                _logRepository.Error(" FilterProductsService GetProductFilterList gives -> " + ex.Message);
            }
            return productFilterList;
        }
        public CityDescriptionList GetCityDescriptionList(Rendering rendering)
        {
            CityDescriptionList cityDescriptionList = new CityDescriptionList();
            try
            {

                cityDescriptionList.data = GetCityDescriptionItem(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" FilterProductsService GetCityDescriptionList gives -> " + ex.Message);
            }


            return cityDescriptionList;
        }
        public List<Object> GetCityDescriptionItem(Rendering rendering)
        {
            List<Object> cityDescriptionItem = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                string location = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query).Get("Location");
                location = !string.IsNullOrEmpty(location) ? location.Replace("property-in-", "") : "";
                //var currentItem = Sitecore.Context.Item;
                //if(datasource.Children.FirstOrDefault().TemplateID())
                var multiList = datasource.TemplateID.ToString() == CityDescriptionFilter.TemplateID.ToString() ? datasource.GetMultiListValueItem(CityDescriptionFilter.CitiesID) : null;
                // Null Check for datasource
                if (multiList == null)
                {
                    throw new NullReferenceException();
                }
                CityDescriptionItem cityDescription;
                foreach (Sitecore.Data.Items.Item item in multiList)
                {
                    if (item.TemplateID == CityDescriptionFilter.ItemID)
                    {
                        if (!string.IsNullOrEmpty(location))
                        {

                            cityDescription = new CityDescriptionItem();
                            cityDescription.src = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, CityDescriptionFilter.Fields.ThumbID.ToString()) != null ?
                                                 Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, CityDescriptionFilter.Fields.ThumbID.ToString()) : "";
                            cityDescription.cityname = item.Name;
                            if (item.Name.ToLower() == location.ToLower())
                            {
                                cityDescription.citydetail = !string.IsNullOrEmpty(item.Fields[CityDescriptionFilter.Fields.SummaryID].Value.ToString()) ? item.Fields[CityDescriptionFilter.Fields.SummaryID].Value.ToString() : "";
                                cityDescription.readmore = !string.IsNullOrEmpty(item.Fields[CityDescriptionFilter.Fields.BodyID].Value.ToString()) ? item.Fields[CityDescriptionFilter.Fields.BodyID].Value.ToString() : "";
                            }
                            cityDescription.slug = Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, CityDescriptionFilter.Fields.LinkName) != null ?
                                Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, CityDescriptionFilter.Fields.LinkName) : "";
                            cityDescriptionItem.Add(cityDescription);
                        }

                    }
                }

            }
            catch (Exception ex)
            {

                _logRepository.Error(" FilterProductsService GetCityDescriptionItem gives -> " + ex.Message);
            }

            return cityDescriptionItem;
        }
        public CommonTextForAboutAdani GetCommonTextForAboutAdani(Rendering rendering)
        {
            CommonTextForAboutAdani commonTextForAboutAdani = new CommonTextForAboutAdani();

            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                if (datasource.TemplateID.ToString() == AboutUsCommonText.TemplateID.ToString())
                {
                    commonTextForAboutAdani.adaniRealty = !string.IsNullOrEmpty(datasource.Fields[AboutUsCommonText.Fields.TitleID].Value) ? datasource.Fields[AboutUsCommonText.Fields.TitleID].Value.ToString() : "";
                    commonTextForAboutAdani.journey = !string.IsNullOrEmpty(datasource.Fields[AboutUsCommonText.Fields.headingID].Value) ? datasource.Fields[AboutUsCommonText.Fields.headingID].Value.ToString() : "";
                    commonTextForAboutAdani.aboutAdaniRealty = !string.IsNullOrEmpty(datasource.Fields[AboutUsCommonText.Fields.readmoreID].Value) ? datasource.Fields[AboutUsCommonText.Fields.readmoreID].Value.ToString() : "";
                    commonTextForAboutAdani.ourLocation = !string.IsNullOrEmpty(datasource.Fields[AboutUsCommonText.Fields.subheadingID].Value) ? datasource.Fields[AboutUsCommonText.Fields.subheadingID].Value.ToString() : "";
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" FilterProductsService GetCommonTextForAboutAdani gives -> " + ex.Message);
            }

            return commonTextForAboutAdani;
        }
        //public LocationSearchDataList GetLocationSearchData(Rendering rendering)
        //{
        //    LocationSearchDataList commonTextForAboutAdani = new LocationSearchDataList();
        //    try
        //    {

        //        commonTextForAboutAdani.data = GetLocationSerchData(rendering);
        //    }
        //    catch (Exception ex)
        //    {

        //        _logRepository.Error(" FilterProductsService GetLocationSearchDataContentResolver gives -> " + ex.Message);
        //    }
        //    return commonTextForAboutAdani;
        //}
        public List<Object> GetLocationSerchData(Rendering rendering)
        {
            List<Object> cityDescriptionItem = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                LocationSearchDataItem locationSearchDataItem;
                if (datasource.TemplateID == LocationSearch.TemplateID)
                {
                    propertype propertype = new propertype();
                    propertyConfiguration propertyConfiguration = new propertyConfiguration();
                    List<propertype> newListOfPropertyType = new List<propertype>();
                    List<propertyConfiguration> propertyConfigurations = new List<propertyConfiguration>();
                    locationSearchDataItem = new LocationSearchDataItem();
                    locationSearchDataItem.propertStatus = !string.IsNullOrEmpty(datasource.Fields[LocationSearch.Fields.propertStatusID].Value.ToString()) ? datasource.Fields[LocationSearch.Fields.propertStatusID].Value.ToString() : "";
                    var propertyTypeList = datasource.TemplateID.ToString() == LocationSearch.TemplateID.ToString() ? datasource.GetMultiListValueItem(LocationSearch.Fields.propertyTypeID) : null;
                    foreach (var type in propertyTypeList)
                    {
                        propertype.id = type.ID.ToString();
                        propertype.projectType = type.Fields[LocationSearch.PropertyFields.TextID].Value;
                        if (propertype.id != null && propertype.projectType != null)
                        {
                            newListOfPropertyType.Add(new propertype() { id = propertype.id, projectType = propertype.projectType });
                        }
                    }
                    locationSearchDataItem.propertyType = newListOfPropertyType;
                    locationSearchDataItem.configuration = !string.IsNullOrEmpty(datasource.Fields[LocationSearch.Fields.configurationID].Value.ToString()) ? datasource.Fields[LocationSearch.Fields.configurationID].Value.ToString() : "";
                    var propertyConfigurationList = datasource.TemplateID.ToString() == LocationSearch.TemplateID.ToString() ? datasource.GetMultiListValueItem(LocationSearch.Fields.propertyConfigurationID) : null;
                    foreach (var type in propertyConfigurationList)
                    {
                        propertyConfiguration.id = type.ID.ToString();
                        propertyConfiguration.configuration = type.Fields[LocationSearch.PropertyFields.TextID].Value;
                        if (propertyConfiguration.id != null && propertyConfiguration.configuration != null)
                        {
                            propertyConfigurations.Add(new propertyConfiguration() { id = propertyConfiguration.id, configuration = propertyConfiguration.configuration });
                        }
                    }
                    locationSearchDataItem.propertyConfiguration = propertyConfigurations;
                    locationSearchDataItem.priceRange = !string.IsNullOrEmpty(datasource.Fields[LocationSearch.Fields.priceRangeID].Value.ToString()) ? datasource.Fields[LocationSearch.Fields.priceRangeID].Value.ToString() : "";
                    locationSearchDataItem.allInclusion = !string.IsNullOrEmpty(datasource.Fields[LocationSearch.Fields.allInclusionID].Value.ToString()) ? datasource.Fields[LocationSearch.Fields.allInclusionID].Value.ToString() : "";
                    locationSearchDataItem.rangeStart = !string.IsNullOrEmpty(datasource.Fields[LocationSearch.Fields.rangeStartID].Value.ToString()) ? datasource.Fields[LocationSearch.Fields.rangeStartID].Value.ToString() : null;
                    locationSearchDataItem.rangeEnd = !string.IsNullOrEmpty(datasource.Fields[LocationSearch.Fields.rangeEndID].Value.ToString()) ? datasource.Fields[LocationSearch.Fields.rangeEndID].Value.ToString() : "";
                    locationSearchDataItem.residential = !string.IsNullOrEmpty(datasource.Fields[LocationSearch.Fields.residentialID].Value.ToString()) ? datasource.Fields[LocationSearch.Fields.residentialID].Value.ToString() : "";
                    locationSearchDataItem.commercialandRetail = !string.IsNullOrEmpty(datasource.Fields[LocationSearch.Fields.commercialandRetailID].Value.ToString()) ? datasource.Fields[LocationSearch.Fields.commercialandRetailID].Value.ToString() : "";
                    locationSearchDataItem.clearAll = !string.IsNullOrEmpty(datasource.Fields[LocationSearch.Fields.clearAllID].Value.ToString()) ? datasource.Fields[LocationSearch.Fields.clearAllID].Value.ToString() : "";
                    locationSearchDataItem.apply = !string.IsNullOrEmpty(datasource.Fields[LocationSearch.Fields.applyID].Value.ToString()) ? datasource.Fields[LocationSearch.Fields.applyID].Value.ToString() : "";
                    cityDescriptionItem.Add(locationSearchDataItem);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" FilterProductsService GetLocationSerchData gives -> " + ex.Message);
            }
            return cityDescriptionItem;
        }
        public List<object> GetPropperyTypesList(Rendering rendering)
        {
            List<object> propertylist = new List<object>();

            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                if (datasource.HasChildren && datasource.ID == PropertyTypesCOmponent.ItemID)
                {
                    foreach (Item item in datasource.Children.ToList())
                    {
                        if (item.ID == PropertyTypesCOmponent.ResidentialItemID || item.ID == PropertyTypesCOmponent.CommercialItemID)
                        {
                            var title = !string.IsNullOrEmpty(item.Fields[PropertyTypesCOmponent.Fields.TitleID].Value) ? item.Fields[PropertyTypesCOmponent.Fields.TitleID].Value.ToString() : "";
                            propertylist.Add(title);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" HeroCarouselService GetPropperyTypesList gives -> " + ex.Message);
            }

            return propertylist;
        }
        public List<SearchData> GetSearchDataList(Rendering rendering)
        {
            List<SearchData> searchDatasList = new List<SearchData>();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }

                if (datasource.HasChildren && datasource.TemplateID == SearchDataTemp.TemplateID)
                {
                    foreach (Item item in datasource.Children.ToList())
                    {
                        SearchData searchData = new SearchData();
                        if (item.TemplateID == SearchDataTemp.LocationDataID || item.TemplateID == SearchDataTemp.PropertyDataID || item.TemplateID == SearchDataTemp.StatusDataID)
                        {
                            searchData.type = !string.IsNullOrEmpty(item.Fields[SearchDataTemp.Fields.TypeID].Value) ? item.Fields[SearchDataTemp.Fields.TypeID].Value.ToString() : "";
                            if (item.TemplateID == SearchDataTemp.LocationDataID)
                            {
                                List<OptionClass> list = new List<OptionClass>();
                                var multiList = item.GetMultiListValueItem(SearchDataTemp.internalOption.LocationOptionID);
                                foreach (var subitem in multiList)
                                {
                                    OptionClass optionClass = new OptionClass();
                                    //optionClass.key = Sitecore.Links.LinkManager.GetItemUrl(subitem);
                                    optionClass.key = Helper.GetLinkURL(subitem, SearchDataTemp.internalOption.LinkName) != null ?
                                                 Helper.GetLinkURL(subitem, SearchDataTemp.internalOption.LinkName) : "";
                                    optionClass.label = subitem.Fields[SearchDataTemp.internalOption.TitleID].Value;
                                    optionClass.latitude = subitem.Fields[SearchDataTemp.internalOption.LatitudeID].Value;
                                    optionClass.longitude = subitem.Fields[SearchDataTemp.internalOption.LongitudeID].Value;
                                    list.Add(optionClass);
                                }
                                searchData.options = list;
                            }
                            else if (item.TemplateID == SearchDataTemp.PropertyDataID)
                            {
                                List<OptionClass> list = new List<OptionClass>();
                                var multiList = item.GetMultiListValueItem(SearchDataTemp.internalOption.PropertyOptionID);
                                foreach (var subitem in multiList)
                                {
                                    OptionClass optionClass = new OptionClass();
                                    optionClass.key = subitem.Fields[SearchDataTemp.internalOption.PropertyLabelID].Value.Replace(" ", "_").ToLower();
                                    optionClass.label = subitem.Fields[SearchDataTemp.internalOption.PropertyLabelID].Value;
                                    list.Add(optionClass);
                                }
                                searchData.options = list;
                            }
                            else if (item.TemplateID == SearchDataTemp.StatusDataID)
                            {
                                List<OptionClass> list = new List<OptionClass>();
                                var multiList = item.GetMultiListValueItem(SearchDataTemp.internalOption.StatusOptionID);
                                foreach (var subitem in multiList)
                                {
                                    OptionClass optionClass = new OptionClass();
                                    //optionClass.key = Helper.GetLinkTargetID(subitem, SearchDataTemp.internalOption.LinkName) != null ?
                                    //             Helper.GetLinkTargetID(subitem, SearchDataTemp.internalOption.LinkName) : "";
                                    optionClass.key = subitem.Fields[SearchDataTemp.internalOption.TextID].Value.Replace(" ", "_").ToLower();
                                    optionClass.label = subitem.Fields[SearchDataTemp.internalOption.TextID].Value;
                                    list.Add(optionClass);
                                }
                                searchData.options = list;
                            }
                            searchData.placeholder = !string.IsNullOrEmpty(item.Fields[SearchDataTemp.Fields.PLaceholderID].Value) ? item.Fields[SearchDataTemp.Fields.PLaceholderID].Value.ToString() : "";
                            searchData.label = !string.IsNullOrEmpty(item.Fields[SearchDataTemp.Fields.LabelID].Value) ? item.Fields[SearchDataTemp.Fields.LabelID].Value.ToString() : "";
                            searchData.errorMessage = !string.IsNullOrEmpty(item.Fields[SearchDataTemp.Fields.messageID].Value) ? item.Fields[SearchDataTemp.Fields.messageID].Value.ToString() : "";
                        }
                        searchDatasList.Add(searchData);
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" HeroCarouselService GetSearchDataList gives -> " + ex.Message);
            }
            return searchDatasList;
        }
        public FilterData GetLocationFilterData(Rendering rendering)
        {
            List<FilterTabsData> listOfFilterTabsDatas = new List<FilterTabsData>();
            List<Item> cityList = new List<Item>();
            List<Item> PropertyList = new List<Item>();
            FilterData filterData = new FilterData();
            try
            {
                var dataSource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                if (dataSource == null)
                {
                    throw new NullReferenceException();
                }
                string location = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query).Get("Location");
                location = !string.IsNullOrEmpty(location) ? location.Replace("property-in-", "") : "";
                filterData.heading = !string.IsNullOrEmpty(dataSource.Fields[LocationFilter.Fields.FilterHeadingID].Value) ? dataSource.Fields[LocationFilter.Fields.FilterHeadingID].Value.ToString() : "";
                filterData.clearAllLabel = !string.IsNullOrEmpty(dataSource.Fields[LocationFilter.Fields.ClearallLabelID].Value) ? dataSource.Fields[LocationFilter.Fields.ClearallLabelID].Value.ToString() : "";
                filterData.applyFilterLabel = !string.IsNullOrEmpty(dataSource.Fields[LocationFilter.Fields.ApplyFilterLabelID].Value) ? dataSource.Fields[LocationFilter.Fields.ApplyFilterLabelID].Value.ToString() : "";
                var propertytypes = dataSource.TemplateID.ToString() == LocationFilter.TemplateID.ToString() ? dataSource.GetMultiListValueItem(LocationFilter.Fields.PropertiesID) : null;
                foreach (var propertyType in propertytypes)
                {
                    var list = propertyType.GetChildren().Where(x => x.TemplateID == CityDescriptionFilter.ItemID && location.ToLower() == x.Name.ToString().ToLower()).FirstOrDefault();
                    if (list != null)
                    {
                        cityList.Add(list);
                    }
                }
                foreach (var property in cityList)
                {
                    var properties = property.GetChildren().Where(x => x.TemplateID == LocationFilter.residentialTemplateID || x.TemplateID == LocationFilter.CommercialTemplateID);
                    PropertyList.AddRange(properties);
                }
                if (propertytypes == null)
                {
                    throw new NullReferenceException();
                }
                foreach (Item propType in propertytypes.OrderByDescending(x => x.Name))
                {
                    List<filterButtons> listOfFilterButton = new List<filterButtons>();
                    FilterTabsData filterTabsData = new FilterTabsData();
                    filterTabsData.tabID = propType.ID == LocationFilter.CommercialItemId ? Constant.CommercialItemName.ToLower() : Constant.ResidentialItemName.ToLower();
                    filterTabsData.tabHeading = propType.ID == LocationFilter.CommercialItemId ? Constant.NewCommercialItemName : Constant.ResidentialItemName;
                    var loopCount = 0;
                    while (loopCount < 3)
                    {
                        if (loopCount == 0)
                        {
                            List<buttons> listOfButtonTabsDatas = new List<buttons>();
                            var residentialStatus = Sitecore.Context.Database.GetItem(LocationFilter.ProjectsStatusDataSourceID);
                            var configurationList = residentialStatus.Children.ToList();
                            if (configurationList == null)
                            {
                                throw new NullReferenceException();
                            }
                            filterButtons filterButtons = new filterButtons();
                            foreach (var configurationItem in configurationList)
                            {
                                filterButtons.type = Constant.Propertystatus.Replace(" ", "");
                                filterButtons.For = filterTabsData.tabHeading;
                                filterButtons.filterHeading = !string.IsNullOrEmpty(dataSource.Fields[LocationFilter.Fields.StatusLabelID].Value) ? dataSource.Fields[LocationFilter.Fields.StatusLabelID].Value.ToString() : "";
                                buttons buttons = new buttons();
                                if (configurationItem.ID.ToString() != "{BFA62551-B9A1-49C1-8730-618A206A4CAF}")
                                {
                                    buttons.id = configurationItem.ID.ToString();
                                    buttons.buttonLabel = configurationItem.Name;
                                    buttons.controllerName = configurationItem.Name.Replace(" ", "");
                                    listOfButtonTabsDatas.Add(buttons);
                                }
                                filterButtons.buttons = listOfButtonTabsDatas;
                            }
                            listOfFilterButton.Add(filterButtons);
                        }
                        else if (loopCount == 1 && propType.ID != LocationFilter.CommercialItemId)
                        {
                            List<buttons> listOfButtonTabsDatas = new List<buttons>();
                            var configuration = Sitecore.Context.Database.GetItem(LocationFilter.ResidentialConfigurationID);
                            var configurationList = configuration.Children.ToList();
                            if (configurationList == null)
                            {
                                throw new NullReferenceException();
                            }
                            filterButtons filterButtons = new filterButtons();
                            foreach (var configurationItem in configurationList)
                            {
                                filterButtons.type = Constant.propertyconfiguration.Replace(" ", "");
                                filterButtons.For = filterTabsData.tabHeading;
                                filterButtons.filterHeading = !string.IsNullOrEmpty(dataSource.Fields[LocationFilter.Fields.ConfigurationLabel].Value) ? dataSource.Fields[LocationFilter.Fields.ConfigurationLabel].Value.ToString() : "";
                                buttons buttons = new buttons();
                                buttons.id = configurationItem.ID.ToString();
                                buttons.buttonLabel = configurationItem.Fields["Text"].Value;
                                buttons.controllerName = configurationItem.Fields["Summary"].Value;
                                listOfButtonTabsDatas.Add(buttons);
                                filterButtons.buttons = listOfButtonTabsDatas;
                            }
                            listOfFilterButton.Add(filterButtons);
                        }
                        else if (loopCount == 2)
                        {
                            List<buttons> listOfButtonTabsDatas = new List<buttons>();
                            filterButtons filterButtons = new filterButtons();
                            filterButtons.type = !string.IsNullOrEmpty(dataSource.Fields[LocationFilter.Fields.RangeFilterID].Value) ? dataSource.Fields[LocationFilter.Fields.RangeFilterID].Value.ToString() : "";
                            filterButtons.For = propType.Name;
                            filterButtons.filterHeading = !string.IsNullOrEmpty(dataSource.Fields[LocationFilter.Fields.PriceRangeLabelID].Value) ? dataSource.Fields[LocationFilter.Fields.PriceRangeLabelID].Value.ToString() : "";
                            filterButtons.allInclusive = !string.IsNullOrEmpty(dataSource.Fields[LocationFilter.Fields.AllInclusiveLabelID].Value) ? dataSource.Fields[LocationFilter.Fields.AllInclusiveLabelID].Value.ToString() : "";
                            var propertyPriceRange = PropertyList.Where(x => x.Fields[LocationFilter.Fields.propertyPricefilter] != null && x.Fields[LocationFilter.Fields.propertyPricefilter].Value != "").ToList();
                            if (propertyPriceRange.Count() != 0)
                            {
                                #region for Min Price 
                                var minRange = propertyPriceRange.OrderBy(x => Convert.ToInt32(x.Fields[LocationFilter.Fields.propertyPricefilter].Value)).FirstOrDefault();
                                var minRangeValue = minRange != null ? minRange.Fields[LocationFilter.Fields.propertyPricefilter].Value : "";

                                var lower = minRangeValue.Substring(0, 2);
                                var lowerZeroSize = minRangeValue.Length - lower.Length;
                                string lowerZero = "";
                                for (int i = 0; i < lowerZeroSize; i++)
                                {
                                    lowerZero = lowerZero + "0";
                                }
                                var insideLower = Convert.ToInt32(lower);
                                filterButtons.minRangeValue = insideLower + lowerZero;
                                #endregion

                                #region for Max Price
                                var maxRange = propertyPriceRange.OrderByDescending(x => Convert.ToInt32(x.Fields[LocationFilter.Fields.propertyPricefilter].Value)).FirstOrDefault();
                                var maxRangeValue = maxRange != null ? maxRange.Fields[LocationFilter.Fields.propertyPricefilter].Value : "";
                                string stringPosition = maxRangeValue.Substring(0, 2);
                                var upperZeroSize = maxRangeValue.Length - stringPosition.Length;
                                string upperZero = "";
                                for (int i = 0; i < upperZeroSize; i++)
                                {
                                    upperZero = upperZero + "0";
                                }
                                if (maxRangeValue.Substring(1, maxRangeValue.Length - 1) == upperZero)
                                {
                                    var upper = Convert.ToInt32(stringPosition);
                                    filterButtons.maxRangeValue = upper + upperZero;
                                }
                                else
                                {
                                    var upper = Convert.ToInt32(stringPosition) + 1;
                                    filterButtons.maxRangeValue = upper + upperZero;
                                }
                                int CompareValue = Convert.ToInt32(stringPosition + upperZero);
                                if (CompareValue == Convert.ToInt32(maxRangeValue))
                                {
                                    filterButtons.maxRangeValue = maxRangeValue;
                                }
                                #endregion
                            }
                            else
                            {
                                filterButtons.minRangeValue = "0";
                                filterButtons.maxRangeValue = "0";
                            }
                            filterButtons.Rs = !string.IsNullOrEmpty(dataSource.Fields[LocationFilter.Fields.CurrencytypeID].Value) ? dataSource.Fields[LocationFilter.Fields.CurrencytypeID].Value.ToString() : "";
                            filterButtons.addsign = !string.IsNullOrEmpty(dataSource.Fields[LocationFilter.Fields.additionalsign].Value) ? dataSource.Fields[LocationFilter.Fields.additionalsign].Value.ToString() : "";
                            listOfFilterButton.Add(filterButtons);
                        }
                        loopCount = loopCount + 1;
                    }
                    filterTabsData.filterButtons = listOfFilterButton;
                    listOfFilterTabsDatas.Add(filterTabsData);
                    filterData.filterTabsData = listOfFilterTabsDatas;
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" HeroCarouselService GetLocationFilterData gives -> " + ex.Message);
            }
            return filterData;
        }
        public SEOHeadingDescription GetHeadingDescriptionList(Rendering rendering)
        {
            SEOHeadingDescription headingDescription = new SEOHeadingDescription();
            try
            {
                var commonItem = Sitecore.Context.Database.GetItem(Templates.commonData.ItemID);
                var dataSource = !string.IsNullOrEmpty(rendering.DataSource)
                   ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                   : null;
                if (dataSource == null)
                {
                    throw new NullReferenceException();
                }

                headingDescription.src = Helper.GetImageSource(dataSource, SEOheadingDescription.Fields.src.ToString()) != null ? Helper.GetImageSource(dataSource, SEOheadingDescription.Fields.src.ToString()) : "";
                //headingDescription.slug = Helper.GetLinkURL(dataSource, SEOheadingDescription.Fields.slug.ToString()) != null ?
                //               Helper.GetLinkURL(dataSource, SEOheadingDescription.Fields.slug.ToString()) : "";
                //if (headingDescription.slug == Helper.GetSitecoreDomain())
                //{
                //    headingDescription.slug = "";
                //}
                headingDescription.cityName = !string.IsNullOrEmpty(dataSource.Fields[SEOheadingDescription.Fields.cityName].Value) ? dataSource.Fields[SEOheadingDescription.Fields.cityName].Value.ToString() : "";
                headingDescription.cityDetail = !string.IsNullOrEmpty(dataSource.Fields[SEOheadingDescription.Fields.cityDetail].Value) ? dataSource.Fields[SEOheadingDescription.Fields.cityDetail].Value.ToString() : "";
                headingDescription.readmore = commonItem != null ? commonItem.Fields["readmore"].Value : string.Empty;
                headingDescription.heading = !string.IsNullOrEmpty(dataSource.Fields[SEOheadingDescription.Fields.heading].Value) ? dataSource.Fields[SEOheadingDescription.Fields.heading].Value.ToString() : "";
                headingDescription.IsSEOPage = dataSource.Fields[SEOheadingDescription.Fields.IsSEOPage] != null ? Helper.GetCheckBoxSelection(dataSource.Fields[SEOheadingDescription.Fields.IsSEOPage]) : false;
            }
            catch (Exception ex)
            {
                _logRepository.Error(" HeroCarouselService GetHeadingDescriptionList gives -> " + ex.Message);
            }
            return headingDescription;
        }
    }
}