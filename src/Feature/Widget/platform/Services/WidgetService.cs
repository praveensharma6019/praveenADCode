using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Adani.SuperApp.Realty.Feature.Widget.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Sitecore.Mvc.Presentation;
using static Adani.SuperApp.Realty.Feature.Carousel.Platform.Templates;
using System.Reflection;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using System.Net;
using System.Globalization;

namespace Adani.SuperApp.Realty.Feature.Widget.Platform.Services
{
    public class WidgetService : IWidgetService
    {
        private readonly ILogRepository _logRepository;
        public WidgetService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public WidgetItem GetWidgetItem(Item widget)
        {
            WidgetItem widgetItem = new WidgetItem();
            try
            {
                widgetItem.widgetId = !string.IsNullOrEmpty(widget.Fields[Constant.widgetId].Value.ToString()) ? Convert.ToInt16(widget.Fields[Constant.widgetId].Value.Trim()) : 0;
                widgetItem.widgetType = !string.IsNullOrEmpty(widget.Fields[Constant.widgetType].Value.ToString()) ? widget.Fields[Constant.widgetType].Value.ToString() : "";
                widgetItem.title = !string.IsNullOrEmpty(widget.Fields[Constant.title].Value.ToString()) ? widget.Fields[Constant.title].Value.ToString() : "";
                widgetItem.subTitle = !string.IsNullOrEmpty(widget.Fields[Constant.subTitle].Value.ToString()) ? widget.Fields[Constant.subTitle].Value.ToString() : "";
                widgetItem.subItemRadius = !string.IsNullOrEmpty(widget.Fields[Constant.subItemRadius].Value.ToString()) ? Convert.ToDouble(widget.Fields[Constant.subItemRadius].Value.Trim()) : 0;
                widgetItem.subItemWidth = !string.IsNullOrEmpty(widget.Fields[Constant.subItemWidth].Value.ToString()) ? Convert.ToDouble(widget.Fields[Constant.subItemWidth].Value.Trim()) : 0;
                widgetItem.gridColumn = !string.IsNullOrEmpty(widget.Fields[Constant.gridColumn].Value.ToString()) ? Convert.ToInt16(widget.Fields[Constant.gridColumn].Value.Trim()) : 0;
                widgetItem.aspectRatio = !string.IsNullOrEmpty(widget.Fields[Constant.aspectRatio].Value.ToString()) ? Convert.ToDouble(widget.Fields[Constant.aspectRatio].Value.Trim()) : 0;

                foreach (Sitecore.Data.Items.Item childItem in widget.GetChildren())
                {
                    if (childItem.Name == "itemMargin")
                    {
                        ItemCSS itemCss = new ItemCSS();
                        itemCss.left = !string.IsNullOrEmpty(childItem.Fields[Constant.left].Value.ToString()) ? Convert.ToDouble(childItem.Fields[Constant.left].Value.Trim()) : 0;
                        itemCss.right = !string.IsNullOrEmpty(childItem.Fields[Constant.right].Value.ToString()) ? Convert.ToDouble(childItem.Fields[Constant.right].Value.Trim()) : 0;
                        itemCss.bottom = !string.IsNullOrEmpty(childItem.Fields[Constant.bottom].Value.ToString()) ? Convert.ToDouble(childItem.Fields[Constant.bottom].Value.Trim()) : 0;
                        itemCss.top = !string.IsNullOrEmpty(childItem.Fields[Constant.top].Value.ToString()) ? Convert.ToDouble(childItem.Fields[Constant.top].Value.Trim()) : 0;
                        widgetItem.itemMargin = itemCss;
                    }
                    if (childItem.Name == "subItemMargin")
                    {
                        ItemCSS subitemCss = new ItemCSS();
                        subitemCss.left = !string.IsNullOrEmpty(childItem.Fields[Constant.left].Value.ToString()) ? Convert.ToDouble(childItem.Fields[Constant.left].Value.Trim()) : 0;
                        subitemCss.right = !string.IsNullOrEmpty(childItem.Fields[Constant.right].Value.ToString()) ? Convert.ToDouble(childItem.Fields[Constant.right].Value.Trim()) : 0;
                        subitemCss.bottom = !string.IsNullOrEmpty(childItem.Fields[Constant.bottom].Value.ToString()) ? Convert.ToDouble(childItem.Fields[Constant.bottom].Value.Trim()) : 0;
                        subitemCss.top = !string.IsNullOrEmpty(childItem.Fields[Constant.top].Value.ToString()) ? Convert.ToDouble(childItem.Fields[Constant.top].Value.Trim()) : 0;
                        widgetItem.subItemMargin = subitemCss;
                    }
                    if (childItem.Name == "actionTitle")
                    {
                        ActionTitle actionTitle = new ActionTitle();
                        actionTitle.name = !string.IsNullOrEmpty(childItem.Fields[Constant.name].Value.ToString()) ? childItem.Fields[Constant.name].Value.ToString() : "";
                        actionTitle.deeplink = !string.IsNullOrEmpty(childItem.Fields[Constant.deeplink].Value.ToString()) ? childItem.Fields[Constant.deeplink].Value.ToString() : "";
                        actionTitle.actionId = !string.IsNullOrEmpty(childItem.Fields[Constant.actionId].Value.ToString()) ? Convert.ToInt16(childItem.Fields[Constant.actionId].Value.Trim()) : 0;
                        widgetItem.actionTitle = actionTitle;
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetWidgetItem Method in class WidgetService -> " + ex.Message);
            }


            return widgetItem;
        }
        public FeedsdataList GetFeedsList(Rendering rendering)
        {
            FeedsdataList feedsList = new FeedsdataList();
            try
            {

                feedsList.data = FeedsList(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" FeedsDataContentResolver GetFeedsList gives -> " + ex.Message);
            }


            return feedsList;
        }
        public List<Object> FeedsList(Rendering rendering)
        {
            List<Object> FeedsList = new List<Object>();
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
                FeedsdataItem feedsdata;
                int i = 1;

                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    feedsdata = new FeedsdataItem();
                    feedsdata.src = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, FeedsData.Fields.srcFieldName) != null ?
                                        Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, FeedsData.Fields.srcFieldName) : "";
                    feedsdata.link = Helper.GetLinkURL(item, FeedsData.Fields.LinkFieldName) != null ?
                                       Helper.GetLinkURL(item, FeedsData.Fields.LinkFieldName) : "";
                    feedsdata.linktarget = Helper.GetLinkURLTargetSpace(item, FeedsData.Fields.LinkFieldName) != null ?
                                      Helper.GetLinkURLTargetSpace(item, FeedsData.Fields.LinkFieldName) : "";
                    feedsdata.imgalt = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, FeedsData.Fields.srcFieldName) != null ?
                                        Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, FeedsData.Fields.srcFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                    feedsdata.imgtitle = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, FeedsData.Fields.srcFieldName) != null ?
                                        Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, FeedsData.Fields.srcFieldName).Fields[ImageFeilds.Fields.TitleFieldName].Value : "";
                    feedsdata.heading = !string.IsNullOrEmpty(item.Fields[FeedsData.Fields.headingFieldName].Value.ToString()) ?
                                        item.Fields[FeedsData.Fields.headingFieldName].Value.ToString() : "";
                    feedsdata.columns = i++;
                    FeedsList.Add(feedsdata);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" WidgetService FeedsList gives -> " + ex.Message);
            }

            return FeedsList;
        }
        public AllaccoladesList GetAllaccoladesList(Rendering rendering)
        {
            AllaccoladesList Allaccolades = new AllaccoladesList();
            try
            {

                Allaccolades.data = AllaccoladesItem(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" AllaccoladesContentResolver GetAllaccoladesList gives -> " + ex.Message);
            }


            return Allaccolades;
        }
        public List<Object> AllaccoladesItem(Rendering rendering)
        {
            List<Object> AllaccoladesList = new List<Object>();
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
                AllaccoladesItem allaccolades;
                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    allaccolades = new AllaccoladesItem();
                    allaccolades.src = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, AllaccoladeDate.Fields.ImageFieldName) != null ?
                                        Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, AllaccoladeDate.Fields.ImageFieldName) : "";
                    allaccolades.caption = !string.IsNullOrEmpty(item.Fields[AllaccoladeDate.Fields.TitleFieldName].Value.ToString()) ?
                                        item.Fields[AllaccoladeDate.Fields.TitleFieldName].Value : "";
                    allaccolades.winner = !string.IsNullOrEmpty(item.Fields[AllaccoladeDate.Fields.winnerFieldName].Value) ?
                                        item.Fields[AllaccoladeDate.Fields.winnerFieldName].Value : "";

                    string date = item.Fields[AllaccoladeDate.Fields.DateFieldName].ToString();
                    if (date != null && date != "")
                    {
                        string format = "yyyyMMdd'T'HHmmss'Z'";
                        var datedata = DateTime.ParseExact(date, format, System.Globalization.CultureInfo.InvariantCulture);
                        allaccolades.date = datedata.ToString("dd MMMM yyyy");
                    }
                    allaccolades.imgalt = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, AllaccoladeDate.Fields.ImageFieldName) != null ?
                                        Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, AllaccoladeDate.Fields.ImageFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                    allaccolades.imgtitle = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, AllaccoladeDate.Fields.ImageFieldName) != null ?
                                        Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, AllaccoladeDate.Fields.ImageFieldName).Fields[ImageFeilds.Fields.TitleFieldName].Value : "";
                    AllaccoladesList.Add(allaccolades);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" AllaccoladesContentResolver AllaccoladesItem gives -> " + ex.Message);
            }

            return AllaccoladesList;
        }

        public TownshipList GetTownshipItem(Rendering rendering)
        {
            TownshipList townshipList1 = new TownshipList();
            List<TownshipComponent> townshipList = new List<TownshipComponent>();
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
                TownshipComponent township;
                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    if (item.TemplateID == TownshipData.bannerTemplateID)
                    {
                        townshipList1.bannerImage = Helper.GetImageSource(item, TownshipData.Fields.ThumbieldName) != null ?
                                      Helper.GetImageSource(item, TownshipData.Fields.ThumbieldName) : "";
                        townshipList1.ImgType = !string.IsNullOrEmpty(item.Fields[TownshipData.Fields.imgType].Value) ?
                                                       item.Fields[TownshipData.Fields.imgType].Value.ToString() : "";
                        if (item.HasChildren)
                        {
                            foreach (Item childItem in item.Children)
                            {
                                if (childItem.TemplateID == TownshipData.bannerTemplateID)
                                {
                                    township = new TownshipComponent();
                                    township.start = !string.IsNullOrEmpty(childItem.Fields[TownshipData.Fields.class1ID].Value) ?
                                                        childItem.Fields[TownshipData.Fields.class1ID].Value.ToString() : "";
                                    township.count = !string.IsNullOrEmpty(childItem.Fields[TownshipData.Fields.headingFieldName].Value.ToString()) ?
                                                        childItem.Fields[TownshipData.Fields.headingFieldName].Value.ToString() : "";
                                    township.detail = !string.IsNullOrEmpty(childItem.Fields[TownshipData.Fields.subheadingFieldName].Value.ToString()) ?
                                                        childItem.Fields[TownshipData.Fields.subheadingFieldName].Value.ToString() : "";
                                    township.delay = !string.IsNullOrEmpty(childItem.Fields[TownshipData.Fields.class2ID].Value) ?
                                                        childItem.Fields[TownshipData.Fields.class2ID].Value.ToString() : "";
                                    township.imgType = !string.IsNullOrEmpty(childItem.Fields[TownshipData.Fields.imgType].Value) ?
                                                        childItem.Fields[TownshipData.Fields.imgType].Value.ToString() : "";
                                    townshipList.Add(township);
                                }
                            }
                        }
                        townshipList1.data = townshipList;
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" TownshipContentResolver townshipList gives -> " + ex.Message);
            }

            return townshipList1;
        }
        public TimelineList GetTimelineList(Rendering rendering)
        {
            TimelineList timelineList = new TimelineList();
            try
            {

                timelineList.data = GetTimelineItem(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" TimelineContentResolver GetTimelineList gives -> " + ex.Message);
            }


            return timelineList;
        }
        public List<Object> GetTimelineItem(Rendering rendering)
        {
            List<Object> timelineList = new List<Object>();
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
                TimelineComponent timeline;
                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    timeline = new TimelineComponent();
                    timeline.src = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, Timeline.Fields.ThumbieldName) != null ?
                                        Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, Timeline.Fields.ThumbieldName) : "";
                    timeline.Alt = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, Timeline.Fields.ThumbieldName) != null ?
                                       Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, Timeline.Fields.ThumbieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                    timeline.year = !string.IsNullOrEmpty(item.Fields[Timeline.Fields.TitleFieldName].Value.ToString()) ?
                                                            item.Fields[Timeline.Fields.TitleFieldName].Value.ToString() : "";
                    timeline.name = !string.IsNullOrEmpty(item.Fields[Timeline.Fields.headingFieldName].Value.ToString()) ?
                                        item.Fields[Timeline.Fields.headingFieldName].Value.ToString() : "";
                    timeline.highlight = !string.IsNullOrEmpty(item.Fields[Timeline.Fields.subheadingFieldName].Value.ToString()) ?
                                     item.Fields[Timeline.Fields.subheadingFieldName].Value.ToString() : "";
                    timeline.imgType = !string.IsNullOrEmpty(item.Fields[Timeline.Fields.imgType].Value.ToString()) ?
                                     item.Fields[Timeline.Fields.imgType].Value.ToString() : "";
                    timeline.mobileImage = Helper.GetImageSource(item, Timeline.Fields.mobileImage.ToString()) != null ?
                                        Helper.GetImageSource(item, Timeline.Fields.mobileImage.ToString()) : "";

                    timelineList.Add(timeline);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" TimelineContentResolver GetTimelineItem gives -> " + ex.Message);
            }

            return timelineList;
        }
        public OurPartners GetOurPartners(Rendering rendering)
        {
            OurPartners ourPartners = new OurPartners();
            try
            {
                if (rendering != null && rendering.Item != null)
                {
                    var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                            ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                            : null;
                    // Null Check for datasource
                    if (datasource == null)
                    {
                        throw new NullReferenceException();
                    }
                    ourPartners.heading = datasource.Name;
                    ourPartners.Data = GetOurBrands(rendering.Item);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return ourPartners;
        }
        public OurBrand GetOurBrandsList(Rendering rendering)
        {
            OurBrand ourBrands = new OurBrand();
            try
            {
                if (rendering != null && rendering.Item != null)
                {
                    InnerData innerData = new InnerData();
                    innerData.Data = GetOurBrands(rendering.Item);
                    ourBrands.OurBrands = innerData;
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return ourBrands;
        }

        private List<object> GetOurBrands(Item item)
        {
            List<object> ourBrands = new List<object>();
            try
            {
                foreach (Item childItem in item.GetChildren())
                {
                    Brand brandData = new Brand();
                    brandData.Src = Helper.GetImageURLbyField(childItem?.Fields[Image.Fields.Thumb]);
                    brandData.Alt = Helper.GetImageAltbyField(childItem?.Fields[Image.Fields.Thumb]);
                    brandData.Title = childItem?.Fields[TitleDescription.Fields.Title].Value;
                    ourBrands.Add(brandData);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return ourBrands;
        }
        public Location GetLocationDataList(Rendering rendering)
        {
            Location location = new Location();
            try
            {
                if (rendering != null && rendering.Item != null)
                {
                    HomeLocationData homeLocationData = new HomeLocationData();
                    homeLocationData.Options = GetLocations(rendering.Item);
                    location.LocationData = homeLocationData;
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return location;
        }

        private List<object> GetLocations(Item item)
        {
            List<object> locations = new List<object>();
            try
            {
                foreach (Item childItem in item.GetChildren())
                {
                    Options options = new Options();
                    options.Label = childItem?.Fields[LocationData.Fields.Label].Value;
                    options.Key = childItem?.Fields[LocationData.Fields.Key].Value;

                    locations.Add(options);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return locations;
        }
        public EmployeeCard GetEmployeeCardDataList(Rendering rendering)
        {
            EmployeeCard employeeCard = new EmployeeCard();
            try
            {
                if (rendering != null && rendering.Item != null)
                {
                    EmployeeDataArray employeeDataArray = new EmployeeDataArray();
                    employeeDataArray.Data = GetEmployeeData(rendering.Item);
                    employeeCard.EmployeeCareCard = employeeDataArray;
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return employeeCard;
        }

        private List<object> GetEmployeeData(Item item)
        {
            List<object> employeeCards = new List<object>();
            try
            {
                foreach (Item childItem in item.GetChildren())
                {
                    EmployeeData employeeData = new EmployeeData();
                    employeeData.ImageSrc = Helper.GetImageURLbyField(childItem?.Fields[EmployeeCardTemplate.Fields.Image]);
                    employeeData.ImageAlt = Helper.GetImageAltbyField(childItem?.Fields[EmployeeCardTemplate.Fields.Image]);
                    employeeData.Heading = childItem?.Fields[EmployeeCardTemplate.Fields.Heading].Value;
                    employeeData.Detail = childItem?.Fields[EmployeeCardTemplate.Fields.Detail].Value;
                    employeeData.Logo = Helper.GetImageURLbyField(childItem?.Fields[EmployeeCardTemplate.Fields.Logo]);
                    employeeData.LogoAlt = Helper.GetImageAltbyField(childItem?.Fields[EmployeeCardTemplate.Fields.Logo]);
                    employeeCards.Add(employeeData);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return employeeCards;
        }
        public PageContent GetPageContentData(Rendering rendering)
        {
            PageContent pageContent = new PageContent();
            Content content = new Content();
            try
            {
                content.PageData = rendering?.Item?.Fields[ContentTemplate.Fields.PageData].Value;
                content.PageHeading = rendering?.Item?.Fields[ContentTemplate.Fields.PageHeading].Value;
                content.Upcoming = rendering?.Item?.Fields[ContentTemplate.Fields.Upcoming].Value;
                content.Year2020 = rendering?.Item?.Fields[ContentTemplate.Fields.Year2020].Value;
                content.Year2021 = rendering?.Item?.Fields[ContentTemplate.Fields.Year2021].Value;
                content.Events = rendering?.Item?.Fields[ContentTemplate.Fields.Events].Value;
                pageContent.PageInnerContent = content;
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return pageContent;
        }
        public CategoryLifestyleList GetCatagoryLIfestylItem(Rendering rendering)
        {
            CategoryLifestyleList categoryLifestyleList = new CategoryLifestyleList();
            try
            {
                categoryLifestyleList.data = GetCatagoryLIfestylList(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return categoryLifestyleList;
        }
        private List<object> GetCatagoryLIfestylList(Rendering rendering)
        {
            var count = 0;
            CatagotyType propertype = new CatagotyType();
            List<object> catagory = new List<object>();
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
                var lifeSTyleSelection = datasource.GetMultiListValueItem(CommunicationCornerTemp.BLogSelectionID);
                if (lifeSTyleSelection != null && lifeSTyleSelection.Count() > 0)
                {
                    lifeSTyleSelection = lifeSTyleSelection.OrderByDescending(x => x.Fields[CommunicationCornerTemp.Fields.datetime].Value);
                    foreach (Item childItem in lifeSTyleSelection)
                    {
                        if (childItem.TemplateID == CommunicationCornerTemp.TemplateId)
                        {
                            List<CatagotyType> list = new List<CatagotyType>();
                            LifestyleItem options = new LifestyleItem();
                            options.IsDefault = false;
                            if (count == 0)
                            {
                                options.IsDefault = true;
                            }
                            options.link = Helper.GetLinkURL(childItem, CommunicationCornerTemp.Fields.linkName) != null ?
                                        Helper.GetLinkURL(childItem, CommunicationCornerTemp.Fields.linkName) : "";
                            var slugValueCheck = childItem.Name.Replace(" ", "-");
                            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
                            var realtyLink = !string.IsNullOrEmpty(commonItem.Fields[Commondata.Fields.blogLinkId].Value.ToString()) ?
                                        commonItem.Fields[Commondata.Fields.blogLinkId].Value.ToString() : "";
                            options.Slug = commonItem.Fields["Site Domain"].Value + realtyLink + "/blogs/" + slugValueCheck;
                            options.src = Helper.GetImageSource(childItem, CommunicationCornerTemp.Fields.srcName) != null ?
                                                Helper.GetImageSource(childItem, CommunicationCornerTemp.Fields.srcName) : "";
                            options.imgalt = Helper.GetImageDetails(childItem, CommunicationCornerTemp.Fields.srcName) != null ?
                                            Helper.GetImageDetails(childItem, CommunicationCornerTemp.Fields.srcName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                            options.imgtitle = Helper.GetImageDetails(childItem, CommunicationCornerTemp.Fields.srcName) != null ?
                                                Helper.GetImageDetails(childItem, CommunicationCornerTemp.Fields.srcName).Fields[ImageFeilds.Fields.TitleFieldName].Value : "";
                            options.title = !string.IsNullOrEmpty(childItem.Fields[CommunicationCornerTemp.Fields.title].Value.ToString()) ?
                                                                childItem.Fields[CommunicationCornerTemp.Fields.title].Value.ToString() : "";
                            options.description = !string.IsNullOrEmpty(childItem.Fields[CommunicationCornerTemp.Fields.description].Value.ToString()) ?
                                                                childItem.Fields[CommunicationCornerTemp.Fields.description].Value.ToString() : "";
                            string date = childItem.Fields[CommunicationCornerTemp.Fields.datetime].ToString();
                            if (date != null && date != "")
                            {
                                string format = "yyyyMMdd'T'HHmmss'Z'";
                                var datedata = DateTime.ParseExact(date, format, System.Globalization.CultureInfo.InvariantCulture);
                                options.datetime = datedata.ToString("MMM dd yyyy");
                            }
                            var multiListForItem = childItem.GetMultiListValueItem(CommunicationCornerTemp.Fields.category);
                            foreach (var selectedCatagory in multiListForItem)
                            {

                                if (selectedCatagory.TemplateID == CommunicationCornerTemp.BlogAnchorsID)
                                {
                                    propertype.categorytitle = !string.IsNullOrEmpty(selectedCatagory.Fields[CommunicationCornerTemp.Fields.CatagorytitleID].Value.ToString()) ? selectedCatagory.Fields[CommunicationCornerTemp.Fields.CatagorytitleID].Value : null;
                                    propertype.categorylink = commonItem.Fields["Site Domain"].Value + string.Format(realtyLink + "/blogs/category/{0}", GetSlugValue(selectedCatagory));
                                    if (propertype.categorytitle != null && propertype.categorylink != null)
                                    {
                                        list.Add(new CatagotyType() { categorytitle = propertype.categorytitle, categorylink = propertype.categorylink });
                                        options.category = list;
                                    }
                                }
                            }
                            options.readtime = !string.IsNullOrEmpty(childItem.Fields[CommunicationCornerTemp.Fields.readtime].Value.ToString()) ?
                                                                childItem.Fields[CommunicationCornerTemp.Fields.readtime].Value.ToString() : "";
                            options.blogSchema = !string.IsNullOrEmpty(childItem.Fields[CommunicationCornerTemp.Fields.blogSchema].Value.ToString()) ?
                                                                childItem.Fields[CommunicationCornerTemp.Fields.blogSchema].Value.ToString() : "";
                            count = count + 1;
                            catagory.Add(options);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return catagory;
        }

        private string GetSlugValue(Item item)
        {
            string value = string.Empty;
            try
            {
                if (item != null)
                {
                    value = item?.Fields[CommunicationCornerTemp.Fields.SlugText]?.Value?.ToLower();
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return value;
        }

        private string GetSlugID(Item item)
        {
            string value = string.Empty;
            try
            {
                if (item != null)
                {
                    value = item?.ID?.ToString();
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return value;
        }

        public CommunicationItems GetCommunicationCornerItem(Rendering rendering)
        {
            CommunicationItems categoryLifestyleList = new CommunicationItems();
            try
            {
                categoryLifestyleList.Item = GetConerdata(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return categoryLifestyleList;
        }
        public List<CommunicationItemsData> GetConerdata(Rendering rendering)
        {
            List<CommunicationItemsData> dataObject = new List<CommunicationItemsData>();
            CommunicationItemsData itemsData = new CommunicationItemsData();
            CatagotyType propertype = new CatagotyType();
            List<CatagotyType> list = new List<CatagotyType>();
            LifestyleItem options = new LifestyleItem();
            List<object> catagory = new List<object>();
            try
            {
                var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
                string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;

                var datasource = Sitecore.Context.Item;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                itemsData.ItemID = datasource.ID.ToString();
                itemsData.ItemName = datasource.Name;
                var redirectUrl = Helper.GetLinkURL(datasource, CommunicationCornerTemp.Fields.redirectUrl.ToString()) != null ?
                                       Helper.GetLinkURL(datasource, CommunicationCornerTemp.Fields.redirectUrl.ToString()) : "";
                itemsData.redirectUrl = redirectUrl == strSitedomain ? "" : redirectUrl;
                itemsData.heading = !string.IsNullOrEmpty(datasource.Fields[CommunicationCornerTemp.Fields.title].Value.ToString()) ?
                                                        datasource.Fields[CommunicationCornerTemp.Fields.title].Value.ToString() : "";
                itemsData.blogSchema = !string.IsNullOrEmpty(datasource.Fields[CommunicationCornerTemp.Fields.blogSchema].Value.ToString()) ?
                                                        datasource.Fields[CommunicationCornerTemp.Fields.blogSchema].Value.ToString() : "";

                //itemsData.description = !string.IsNullOrEmpty(datasource.Fields[CommunicationCornerTemp.Fields.Body].Value.ToString()) ?
                //                                            datasource.Fields[CommunicationCornerTemp.Fields.Body].Value.ToString() : "";
                string date = datasource.Fields[CommunicationCornerTemp.Fields.datetime].ToString();
                if (date != null && date != "")
                {
                    string format = "yyyyMMdd'T'HHmmss'Z'";
                    var datedata = DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
                    var indTime = TimeZoneInfo.ConvertTimeFromUtc(datedata, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                    itemsData.date = indTime.ToString("MMMM dd, yyyy");
                }
                var stringvalue = !string.IsNullOrEmpty(datasource.Fields[CommunicationCornerTemp.Fields.Body].Value.ToString()) ?
                                                            WebUtility.HtmlDecode(datasource.Fields[CommunicationCornerTemp.Fields.Body].Value.Replace("-/media/", Helper.GetSitecoreDomain() + '/' + "~/media/")) : "";
                if (!string.IsNullOrEmpty(stringvalue))
                {
                    itemsData.description = stringvalue.ToLower().Contains("-/https") ? stringvalue.Replace("-/https", "https") : stringvalue;
                }
                CheckboxField checkboxField = datasource.Fields[CommunicationCornerTemp.Fields.QuoteCheckBox];
                if (checkboxField != null && checkboxField.Checked)
                {
                    itemsData.quote = !string.IsNullOrEmpty(datasource.Fields[CommunicationCornerTemp.Fields.Quote].Value.ToString()) ?
                                                            datasource.Fields[CommunicationCornerTemp.Fields.Quote].Value.ToString() : "";
                }
                var realtyLink = !string.IsNullOrEmpty(commonItem.Fields[Commondata.Fields.blogLinkId].Value.ToString()) ?
                                        commonItem.Fields[Commondata.Fields.blogLinkId].Value.ToString() : "";
                var slugValueCheck = datasource.Name.Replace(" ", "-");
                itemsData.Slug = strSitedomain + realtyLink + "/blogs/" + slugValueCheck;
                var multiListForItem = datasource.GetMultiListValueItem(CommunicationCornerTemp.Fields.category);
                foreach (var selectedCatagory in multiListForItem)
                {
                    if (selectedCatagory.TemplateID == CommunicationCornerTemp.BlogAnchorsID)
                    {
                        propertype.categorytitle = !string.IsNullOrEmpty(selectedCatagory.Fields[CommunicationCornerTemp.Fields.CatagorytitleID].Value.ToString()) ? selectedCatagory.Fields[CommunicationCornerTemp.Fields.CatagorytitleID].Value : null;
                        propertype.categorylink = string.Format(strSitedomain + realtyLink + "/blogs/category/{0}", GetSlugValue(selectedCatagory));
                        if (propertype.categorytitle != null && propertype.categorylink != null)
                        {
                            list.Add(new CatagotyType() { categorytitle = propertype.categorytitle, categorylink = propertype.categorylink });
                            options.category = list;
                        }
                    }
                }
                itemsData.category = options.category;
                dataObject.Add(itemsData);
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return dataObject;

        }
        public RestaurantInformation GetRestaurantInformation(Rendering rendering)
        {
            RestaurantInformation restosdata = new RestaurantInformation();
            try
            {
                //Get the datasource for the item
                var item = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (item == null)
                {
                    throw new NullReferenceException();
                }
                if (item.TemplateID == RestaurantInformationTemp.TemplateID)
                {
                    restosdata.Title = !string.IsNullOrEmpty(item.Fields[RestaurantInformationTemp.Fields.TitleID].Value.ToString()) ?
                                        item.Fields[RestaurantInformationTemp.Fields.TitleID].Value.ToString() : "";
                    restosdata.location = !string.IsNullOrEmpty(item.Fields[RestaurantInformationTemp.Fields.locationID].Value.ToString()) ?
                                        item.Fields[RestaurantInformationTemp.Fields.locationID].Value.ToString() : "";
                    restosdata.foodCategories = !string.IsNullOrEmpty(item.Fields[RestaurantInformationTemp.Fields.FoodCatagoriesID].Value.ToString()) ?
                                        item.Fields[RestaurantInformationTemp.Fields.FoodCatagoriesID].Value.ToString() : "";
                    restosdata.foodPrice = !string.IsNullOrEmpty(item.Fields[RestaurantInformationTemp.Fields.foodPricesID].Value.ToString()) ?
                                        item.Fields[RestaurantInformationTemp.Fields.foodPricesID].Value.ToString() : "";
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" WidgetService GetRestaurantInformation gives -> " + ex.Message);
            }
            return restosdata;
        }
        public List<ReasaurantCard> GetReasaurantCard(Rendering rendering)
        {
            List<ReasaurantCard> restodata = new List<ReasaurantCard>();
            try
            {
                //Get the datasource for the item
                var item = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (item == null)
                {
                    throw new NullReferenceException();
                }
                if (item.TemplateID == RestaurantInformationTemp.TemplateID)
                {
                    var multiListForItem = item.GetMultiListValueItem(RestaurantInformationTemp.Fields.OtherRestaurantID);
                    foreach (var selectedItem in multiListForItem)
                    {
                        if (selectedItem.TemplateID == RestaurantInformationTemp.TemplateID)
                        {
                            ReasaurantCard reasaurantCard = new ReasaurantCard();
                            reasaurantCard.src = Helper.GetImageSource(selectedItem, RestaurantInformationTemp.Fields.srcName) != null ?
                                            Helper.GetImageSource(selectedItem, RestaurantInformationTemp.Fields.srcName) : ""; ;
                            reasaurantCard.title = !string.IsNullOrEmpty(item.Fields[RestaurantInformationTemp.Fields.TitleID].Value.ToString()) ?
                                        item.Fields[RestaurantInformationTemp.Fields.TitleID].Value.ToString() : "";
                            reasaurantCard.logo = Helper.GetImageSource(selectedItem, RestaurantInformationTemp.Fields.ThumbnailName) != null ?
                                            Helper.GetImageSource(selectedItem, RestaurantInformationTemp.Fields.ThumbnailName) : "";
                            reasaurantCard.price = !string.IsNullOrEmpty(item.Fields[RestaurantInformationTemp.Fields.foodPricesID].Value.ToString()) ?
                                        item.Fields[RestaurantInformationTemp.Fields.foodPricesID].Value.ToString() : "";
                            reasaurantCard.discount = !string.IsNullOrEmpty(item.Fields[RestaurantInformationTemp.Fields.DiscountID].Value.ToString()) ?
                                        item.Fields[RestaurantInformationTemp.Fields.DiscountID].Value.ToString() : "";
                            reasaurantCard.status = Helper.GetSelectedItemFromDroplistField(selectedItem, RestaurantInformationTemp.Fields.StatusID) != null ?
                                         Helper.GetSelectedItemFromDroplistField(selectedItem, RestaurantInformationTemp.Fields.StatusID) : null;
                            restodata.Add(reasaurantCard);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" WidgetService GetReasaurantCard gives -> " + ex.Message);
            }
            return restodata;
        }
        public List<ReasaurantMenu> GetReasaurantMenu(Rendering rendering)
        {
            List<ReasaurantMenu> restodata = new List<ReasaurantMenu>();
            try
            {
                //Get the datasource for the item
                var item = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (item == null)
                {
                    throw new NullReferenceException();
                }
                if (item.TemplateID == RestaurantInformationTemp.TemplateID)
                {
                    var multiListForItem = item.GetMultiListValueItem(RestaurantMenuTemp.Fields.RestaurantMenuSelectionID);
                    foreach (var selectedItem in multiListForItem)
                    {
                        if (selectedItem.TemplateID == RestaurantMenuTemp.TemplateID)
                        {
                            ReasaurantMenu reasaurantCard = new ReasaurantMenu();
                            reasaurantCard.src = Helper.GetImageSource(selectedItem, RestaurantMenuTemp.Fields.thumbName) != null ?
                                            Helper.GetImageSource(selectedItem, RestaurantMenuTemp.Fields.thumbName) : "";
                            reasaurantCard.alt = Helper.GetImageDetails(selectedItem, RestaurantMenuTemp.Fields.thumbName) != null ?
                                        Helper.GetImageDetails(selectedItem, RestaurantMenuTemp.Fields.thumbName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                            reasaurantCard.title = selectedItem.DisplayName;
                            restodata.Add(reasaurantCard);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" WidgetService GetReasaurantCard gives -> " + ex.Message);
            }
            return restodata;
        }

        public RestaurantTabData GetRestaurantTabData(Rendering rendering)
        {
            RestaurantTabData restaurantTabData = new RestaurantTabData();
            restaurantTabData.RestaurantTabsData = new List<RestaurantTab>();
            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
               ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
               : null;

                var tabs = datasource.GetMultiListValueItem(RestaurantTabDataItem.Fields.Tabs);

                if (tabs != null)
                {
                    foreach (var item in tabs)
                    {
                        RestaurantTab restaurantTab = new RestaurantTab();
                        restaurantTab.Title = item.Fields[RestaurantTabItem.Fields.Title].Value;
                        restaurantTab.Link = item.Fields[RestaurantTabItem.Fields.Link].Value;
                        restaurantTab.LinkTitle = item.Fields[RestaurantTabItem.Fields.LinkTitle].Value;
                        restaurantTabData.RestaurantTabsData.Add(restaurantTab);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return restaurantTabData;
        }

        public ReasaurantContent GetRestaurantContentData(Rendering rendering)
        {
            ReasaurantContent reasaurantContent = new ReasaurantContent();

            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
               ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
               : null;

                reasaurantContent.about = datasource.Fields[RestaurantContentData.Fields.about].Value;
                reasaurantContent.menu = datasource.Fields[RestaurantContentData.Fields.menu].Value;
                reasaurantContent.otherInfo = datasource.Fields[RestaurantContentData.Fields.otherInfo].Value;
                reasaurantContent.moreReastaurant = datasource.Fields[RestaurantContentData.Fields.moreReastaurant].Value;
                reasaurantContent.restrofor = datasource.Fields[RestaurantContentData.Fields.Restaurantfor].Value;
                reasaurantContent.count = datasource.Fields[RestaurantContentData.Fields.count].Value;
                reasaurantContent.member = datasource.Fields[RestaurantContentData.Fields.member].Value;
                reasaurantContent.menuDownload = datasource.Fields[RestaurantContentData.Fields.menuDownload].Value;
                reasaurantContent.share = datasource.Fields[RestaurantContentData.Fields.share].Value;
                reasaurantContent.aboutReastaurant = datasource.Fields[RestaurantContentData.Fields.aboutReastaurant].Value;
                reasaurantContent.callUs = datasource.Fields[RestaurantContentData.Fields.callUs].Value;
                reasaurantContent.contactNum = datasource.Fields[RestaurantContentData.Fields.contactNum].Value;
                reasaurantContent.openNow = datasource.Fields[RestaurantContentData.Fields.openNow].Value;
                reasaurantContent.timeSlot1 = datasource.Fields[RestaurantContentData.Fields.timeSlot1].Value;
                reasaurantContent.enquireNow = datasource.Fields[RestaurantContentData.Fields.enquireNow].Value;
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return reasaurantContent;
        }

        public TopBarModel GetTopBarData(Rendering rendering)
        {

            TopBar topBar = new TopBar();

            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
               ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
               : null;

                topBar.about = datasource.Fields[TopBarItem.Fields.About].Value;
                topBar.link = datasource.Fields[TopBarItem.Fields.Link].Value;
                topBar.location = datasource.Fields[TopBarItem.Fields.Location].Value;
                topBar.roomPrice = datasource.Fields[TopBarItem.Fields.RoomPrice].Value;
                topBar.title = datasource.Fields[TopBarItem.Fields.Title].Value;
                topBar.linkTitle = datasource.Fields[TopBarItem.Fields.LinkTitle].Value;
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            TopBarModel topBarModel = new TopBarModel() { TopBar = topBar };
            return topBarModel;
        }

        public RoomTitleModel GetRoomTitleData(Rendering rendering)
        {
            RoomTitle roomTitle = new RoomTitle();

            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
               ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
               : null;

                roomTitle.title = datasource.Fields[RoomTitleItem.Fields.Title].Value;
                roomTitle.features = new List<Models.Feature>();

                var features = datasource.GetMultiListValueItem(RoomTitleItem.Fields.Features);

                if (features != null)
                {
                    foreach (var item in features)
                    {
                        Models.Feature feature = new Models.Feature();
                        feature.title = item.Fields[FeatureItem.Fields.Title].Value;
                        feature.icon = item.Fields[FeatureItem.Fields.Icon].Value;
                        roomTitle.features.Add(feature);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            RoomTitleModel roomTitleModel = new RoomTitleModel() { RoomTitle = roomTitle };
            return roomTitleModel;
        }

        public RoomInfoTabInfos GetRoomInfoTabData(Rendering rendering)
        {
            RoomInfoTabInfos roomInfoTabInfos = new RoomInfoTabInfos();

            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
               ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
               : null;


                roomInfoTabInfos.roomInfoTabs = new List<RoomInfoTab>();

                var tabs = datasource.GetMultiListValueItem(RoomInfoTabDataItem.Fields.Tabs);

                if (tabs != null)
                {
                    foreach (var item in tabs)
                    {
                        RoomInfoTab roomInfoTab = new RoomInfoTab();
                        roomInfoTab.tabTitle = item.Fields[RoomInfoTabItem.Fields.TabTitle].Value;
                        roomInfoTab.target = item.Fields[RoomInfoTabItem.Fields.Target].Value;
                        roomInfoTabInfos.roomInfoTabs.Add(roomInfoTab);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return roomInfoTabInfos;
        }

        public RoomFacilities GetFacilitiesData(Rendering rendering)
        {
            RoomFacilities roomFacilities = new RoomFacilities();

            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
               ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
               : null;


                roomFacilities.facilities = new List<Facility>();

                var facilities = datasource.GetMultiListValueItem(FacilityDataItem.Fields.Facilities);

                if (facilities != null)
                {
                    foreach (var item in facilities)
                    {
                        Facility facility = new Facility();
                        facility.title = item.Fields[FacilityItem.Fields.Title].Value;
                        facility.icon = item.Fields[FacilityItem.Fields.Icon].Value;
                        roomFacilities.facilities.Add(facility);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return roomFacilities;
        }

        public OtherRoomsModel GetOtherRoomsData(Rendering rendering)
        {
            OtherRooms otherRooms = new OtherRooms();

            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
               ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
               : null;

                otherRooms.title = datasource.Fields[OtherRoomsItem.Fields.Title].Value;
                otherRooms.src = datasource.Fields[OtherRoomsItem.Fields.Source].Value;
                otherRooms.nonmemberPrice = datasource.Fields[OtherRoomsItem.Fields.NonMemberPrice].Value;
                otherRooms.memberPrice = datasource.Fields[OtherRoomsItem.Fields.MemberPrice].Value;

            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            OtherRoomsModel otherRoomsModel = new OtherRoomsModel() { OtherRooms = otherRooms };

            return otherRoomsModel;
        }

        public OpentimingsData GetOpenTimingsData(Rendering rendering)
        {
            OpentimingsData opentimingsData = new OpentimingsData();
            opentimingsData.OpenTiming = new OpenTiming();

            opentimingsData.OpenTiming.Data = new List<TimeingData>();
            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
               ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
               : null;

                var data = datasource.GetMultiListValueItem(OpentimingItem.Fields.Data);

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        TimeingData timeingData = new TimeingData();
                        timeingData.Day = item.Fields[TimingsItem.Fields.Day].Value;
                        timeingData.Time = item.Fields[TimingsItem.Fields.Time].Value;
                        opentimingsData.OpenTiming.Data.Add(timeingData);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return opentimingsData;
        }
        private DateTime ConvertDateFormat(string datetimeString)
        {

            DateTime utcdate = DateTime.ParseExact(datetimeString, "yyyyMMdd'T'HHmmss'Z'", CultureInfo.InvariantCulture);
            return TimeZoneInfo.ConvertTimeFromUtc(utcdate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        }
        public ReadMoreArticles GetOtherArticlesList(Rendering rendering)
        {
            ReadMoreArticles readMoreArticles = new ReadMoreArticles();
            List<Articles> listOfArticles = new List<Articles>();
            var ArticlesCount = 1;
            try
            {
                var currentItem = Sitecore.Context.Item;
                if (currentItem == null)
                {
                    throw new NullReferenceException();
                }
                var nearestArticless = new List<Item>();
                var ListOfNextItemflag = true;
                var ListOfPreviousItemFlag = true;
                var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
                string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;

                string date = currentItem.Fields[CommunicationCornerTemp.Fields.datetime].ToString();
                var datedata = currentItem.Fields[CommunicationCornerTemp.Fields.datetime].ToString() != "" ? ConvertDateFormat(currentItem.Fields[CommunicationCornerTemp.Fields.datetime].ToString()) : new DateTime();
                var currentItemCategory = currentItem.GetMultiListValueItem(CommunicationCornerTemp.Fields.category).ToList();
                var blogItem = Sitecore.Context.Database.GetItem(CommunicationCornerTemp.ItemID);
                var nearestArticles = blogItem.Axes.GetDescendants().Where(x => x.TemplateID == CommunicationCornerTemp.TemplateId).ToList();
                foreach (var article in nearestArticles)
                {
                    var a = article.GetMultiListValueItem(CommunicationCornerTemp.Fields.category).ToList();
                    foreach (var nearestArticlesCategory in a)
                    {
                        foreach (var inneritem in currentItemCategory)
                        {
                            if (inneritem.ID == nearestArticlesCategory.ID)
                            {
                                if (!nearestArticless.Contains(article))
                                {
                                    nearestArticless.Add(article);
                                }
                            }
                        }
                    }
                }
                //Contains(currentItemCategory.Where(y => y.ID == x.ID).FirstOrDefault())).ToList();
                var articlesSelection = currentItem.GetMultiListValueItem(CommunicationCornerTemp.BLogSelectionID).ToList();
                if (articlesSelection.Count() != 0)
                {
                    foreach (Item Article in articlesSelection)
                    {
                        Articles articles = new Articles();
                        if (ArticlesCount <= 2 && Article.TemplateID == CommunicationCornerTemp.TemplateId)
                        {
                            if (ArticlesCount == 1)
                            {
                                articles.articleType = "prevArticle";
                                articles.articleLinkIcon = "AngleLeft";
                                articles.articleLinkTitle = "Previous";
                            }
                            else
                            {
                                articles.articleType = "nextArticle";
                                articles.articleLinkIcon = "AngleRight";
                                articles.articleLinkTitle = "Next";
                            }
                            var slugValueCheck = Article.Name.Replace(" ", "-");
                            var realtyLink = !string.IsNullOrEmpty(commonItem.Fields[Commondata.Fields.blogLinkId].Value.ToString()) ?
                                        commonItem.Fields[Commondata.Fields.blogLinkId].Value.ToString() : "";
                            articles.articleLink = strSitedomain + realtyLink + "/blogs/" + slugValueCheck;
                            articles.articleThumb = Helper.GetImageSource(Article, CommunicationCornerTemp.Fields.srcName) != null ?
                                        Helper.GetImageSource(Article, CommunicationCornerTemp.Fields.srcName) : "";
                            articles.articleThumbAlt = Helper.GetImageDetails(Article, CommunicationCornerTemp.Fields.srcName) != null ?
                                        Helper.GetImageDetails(Article, CommunicationCornerTemp.Fields.srcName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                            articles.articleTitle = !string.IsNullOrEmpty(Article.Fields[CommunicationCornerTemp.Fields.title].Value.ToString()) ?
                                        Article.Fields[CommunicationCornerTemp.Fields.title].Value.ToString() : "";
                            articles.articleDescription = !string.IsNullOrEmpty(Article.Fields[CommunicationCornerTemp.Fields.description].Value.ToString()) ?
                                        Article.Fields[CommunicationCornerTemp.Fields.description].Value.ToString() : "";
                            ArticlesCount = ArticlesCount + 1;
                            listOfArticles.Add(articles);
                        }
                    }
                    if (listOfArticles != null)
                    {
                        readMoreArticles.componentname = !string.IsNullOrEmpty(commonItem.Fields[Commondata.Fields.morearticlesID].Value.ToString()) ?
                                            commonItem.Fields[Commondata.Fields.morearticlesID].Value.ToString() : "";
                    }
                    readMoreArticles.data = listOfArticles;
                }
                else
                {
                    List<DateTime> dateTimeList = new List<DateTime>();
                    List<Item> ListOfNextItem = new List<Item>();
                    List<Item> ListOfPreviousItem = new List<Item>();
                    List<Item> dynamicArticlesSelection = new List<Item>();
                    if (!string.IsNullOrEmpty(date))
                    {
                        foreach (var artc in nearestArticless)
                        {
                            if (artc.ID != currentItem.ID)
                            {
                                if (artc.Fields[CommunicationCornerTemp.Fields.datetime] != null)
                                {
                                    var blogDatedata = artc.Fields[CommunicationCornerTemp.Fields.datetime].ToString() != "" ? ConvertDateFormat(artc.Fields[CommunicationCornerTemp.Fields.datetime].ToString()) : new DateTime();
                                    var previousfield = blogDatedata <= datedata;
                                    if (previousfield)
                                    {
                                        ListOfPreviousItem.Add(artc);
                                    }
                                    else
                                    {
                                        ListOfNextItem.Add(artc);
                                    }
                                }
                            }
                        }
                        var priviousItem = ListOfPreviousItem != null ? ListOfPreviousItem.Where(x => x.Fields[CommunicationCornerTemp.Fields.datetime].Value != "").OrderByDescending(x => x.Fields[CommunicationCornerTemp.Fields.datetime].Value).FirstOrDefault() : null;
                        var nextItem = ListOfNextItem != null ? ListOfNextItem.Where(x => x.Fields[CommunicationCornerTemp.Fields.datetime].Value != "").OrderBy(x => x.Fields[CommunicationCornerTemp.Fields.datetime].Value).FirstOrDefault() : null;
                        if (priviousItem != null)
                        {
                            dynamicArticlesSelection.Add(priviousItem);
                        }
                        else
                        {
                            ListOfPreviousItemFlag = false;
                        }
                        if (nextItem != null)
                        {
                            dynamicArticlesSelection.Add(nextItem);
                        }
                        else
                        {
                            ListOfNextItemflag = false;
                        }
                        if (dynamicArticlesSelection.Count() != 0)
                        {
                            foreach (Item Article in dynamicArticlesSelection)
                            {
                                Articles articles = new Articles();
                                if (ArticlesCount <= 2 && Article.TemplateID == CommunicationCornerTemp.TemplateId)
                                {
                                    if (ListOfPreviousItemFlag)
                                    {
                                        articles.articleType = "prevArticle";
                                        articles.articleLinkIcon = "AngleLeft";
                                        articles.articleLinkTitle = "Previous";
                                        ListOfPreviousItemFlag = false;
                                    }
                                    else if (ListOfNextItemflag)
                                    {
                                        articles.articleType = "nextArticle";
                                        articles.articleLinkIcon = "AngleRight";
                                        articles.articleLinkTitle = "Next";
                                        ListOfNextItemflag = false;
                                    }
                                    var slugValueCheck = Article.Name.Replace(" ", "-");
                                    var realtyLink = !string.IsNullOrEmpty(commonItem.Fields[Commondata.Fields.blogLinkId].Value.ToString()) ?
                                                commonItem.Fields[Commondata.Fields.blogLinkId].Value.ToString() : "";
                                    articles.articleLink = strSitedomain + realtyLink + "/blogs/" + slugValueCheck;
                                    articles.articleThumb = Helper.GetImageSource(Article, CommunicationCornerTemp.Fields.srcName) != null ?
                                                Helper.GetImageSource(Article, CommunicationCornerTemp.Fields.srcName) : "";
                                    articles.articleThumbAlt = Helper.GetImageDetails(Article, CommunicationCornerTemp.Fields.srcName) != null ?
                                                Helper.GetImageDetails(Article, CommunicationCornerTemp.Fields.srcName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                                    articles.articleTitle = !string.IsNullOrEmpty(Article.Fields[CommunicationCornerTemp.Fields.title].Value.ToString()) ?
                                                Article.Fields[CommunicationCornerTemp.Fields.title].Value.ToString() : "";
                                    articles.articleDescription = !string.IsNullOrEmpty(Article.Fields[CommunicationCornerTemp.Fields.description].Value.ToString()) ?
                                                Article.Fields[CommunicationCornerTemp.Fields.description].Value.ToString() : "";
                                    ArticlesCount = ArticlesCount + 1;
                                    listOfArticles.Add(articles);
                                }
                            }
                            if (listOfArticles != null)
                            {
                                readMoreArticles.componentname = !string.IsNullOrEmpty(commonItem.Fields[Commondata.Fields.morearticlesID].Value.ToString()) ?
                                                    commonItem.Fields[Commondata.Fields.morearticlesID].Value.ToString() : "";
                            }
                            readMoreArticles.data = listOfArticles;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return readMoreArticles;
        }

        public ShantigramData GetShantigramData(Rendering rendering)
        {
            Shantigram shantigram = new Shantigram();

            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
               ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
               : null;

                shantigram.title = datasource.Fields[ShantigramItem.Fields.Title].Value;
                shantigram.src = Helper.GetLinkURLbyField(datasource, datasource.Fields[ShantigramItem.Fields.Imgsrc]);
                shantigram.alt = datasource.Fields[ShantigramItem.Fields.Imgalt].Value;
                shantigram.heading = datasource.Fields[ShantigramItem.Fields.Heading].Value;
                shantigram.subheading = datasource.Fields[ShantigramItem.Fields.SubHeading].Value;
                shantigram.thumbImg = Helper.GetLinkURLbyField(datasource, datasource.Fields[ShantigramItem.Fields.ThumbImg]);
                shantigram.class1 = datasource.Fields[ShantigramItem.Fields.Class1].Value;
                shantigram.class2 = datasource.Fields[ShantigramItem.Fields.Class2].Value;
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            ShantigramData shantigramData = new ShantigramData() { shantigram = shantigram };
            return shantigramData;
        }

        public InfoTabsData GetInfoTabsData(Rendering rendering)
        {
            InfoTabsData infoTabs = new InfoTabsData();

            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
               ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
               : null;


                infoTabs.infoTabs = new InfoTabs();
                infoTabs.infoTabs.data = new List<InfoTabLink>();

                var tabs = datasource.GetMultiListValueItem(InfoTabsItem.Fields.Data);

                if (tabs != null)
                {
                    foreach (var item in tabs)
                    {
                        InfoTabLink tab = new InfoTabLink();
                        tab.title = item.Fields[InfoTabLinkItem.Fields.Title].Value;
                        tab.linktitle = item.Fields[InfoTabLinkItem.Fields.LinkTitle].Value;
                        tab.link = item.Fields[InfoTabLinkItem.Fields.Link].Value;
                        infoTabs.infoTabs.data.Add(tab);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return infoTabs;
        }
    }
}