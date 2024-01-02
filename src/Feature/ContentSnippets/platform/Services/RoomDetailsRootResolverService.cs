using Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Services
{
    public class RoomDetailsRootResolverService : IRoomDetailsRootResolverService
    {

        private readonly ILogRepository _logRepository;
        public RoomDetailsRootResolverService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        #region RoomTitle
        public RoomTitleData GetRoomTitleDataList(Rendering rendering)
        {
            RoomTitleData roomTitleDataList = new RoomTitleData();
            try
            {
                roomTitleDataList.roomTitle = GetRoomTitleDataItem(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return roomTitleDataList;
        }

        public RoomTitleDataItem GetRoomTitleDataItem(Rendering rendering)
        {
            RoomTitleDataItem roomTitleDataItemList = new RoomTitleDataItem();

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
                if (datasource.TemplateID == Templates.RoomTitle.TemplateID)
                {
                    Item item = datasource;
                    if (item != null)
                    {
                        roomTitleDataItemList.Title = item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";

                        MultilistField multiselectField = item.Fields[Templates.RoomTitle.Fields.FieldsName.Features];
                        List<FeaturesListDataItem> objectfeaturesList = new List<FeaturesListDataItem>();
                        if (!string.IsNullOrEmpty(multiselectField.Value) && multiselectField.TargetIDs.Count() > 0 && multiselectField.GetItems().Count() > 0)
                        {
                            foreach (Item featuresListItem in multiselectField.GetItems())
                            {
                                FeaturesListDataItem featuresListDataItem = new FeaturesListDataItem();
                                if (featuresListItem.TemplateID == Templates.Features.TemplateID)
                                {

                                    featuresListDataItem.Title = featuresListItem.Fields[Templates.ITitle.FieldsName.Title].Value != null ? featuresListItem.Fields[Templates.ITitle.FieldsName.Title].Value : "";

                                    featuresListDataItem.Icon = featuresListItem.Fields[Templates.ISubTitle.FieldsName.SubTitle].Value != null ? featuresListItem.Fields[Templates.ISubTitle.FieldsName.SubTitle].Value : "";

                                }
                                objectfeaturesList.Add(featuresListDataItem);
                            }
                        }
                        roomTitleDataItemList.Features = objectfeaturesList;                        
                    }
                }

            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return roomTitleDataItemList;
        }

        #endregion

        #region RoomTabInfo

        public RoomInfoTabsData GetRoomInfoTabsDataList(Rendering rendering)
        {
            RoomInfoTabsData roomInfoTabsDataList = new RoomInfoTabsData();
            try
            {
                roomInfoTabsDataList.roomInfoTabs = GetRoomTabInfoDataItem(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return roomInfoTabsDataList;
        }
       
        public List<Object> GetRoomTabInfoDataItem(Rendering rendering)
        {
            List<Object> roomTabInfoDataItemList = new List<Object>();

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
                RoomInfoTabsDataItem roominfoTabsDataItem;

                if (datasource.TemplateID == Templates.RoomInfoTabsFolder.TemplateID && datasource.Children.ToList().Count > 0)
                {
                    List<Item> children = datasource.Children.Where(x => x.TemplateID == Templates.RoomInfoTabs.TemplateID).ToList();
                    if (children != null && children.Count > 0)
                    {
                        foreach (Item item in children)
                        {

                            roominfoTabsDataItem = new RoomInfoTabsDataItem();

                            roominfoTabsDataItem.TabTitle = item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";
                            
                            roominfoTabsDataItem.Target = Helper.GetLinkURL(item, Templates.ILink.FieldsName.Link) != null ?
                                                          Helper.GetLinkURL(item, Templates.ILink.FieldsName.Link) : "";

                            roominfoTabsDataItem.TargetText = Helper.GetLinkTextbyField(item, item.Fields[Templates.ILink.FieldsName.Link]) != null ?
                                                              Helper.GetLinkTextbyField(item, item.Fields[Templates.ILink.FieldsName.Link]) : "";

                            roomTabInfoDataItemList.Add(roominfoTabsDataItem);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return roomTabInfoDataItemList;
        }

        #endregion

        #region MostFacilitiesData

        public MostFacilitiesData GeMostFacilitiesDataList(Rendering rendering)
        {
            MostFacilitiesData mostFacilitiesDataList = new MostFacilitiesData();
            try
            {
                mostFacilitiesDataList.facilities = GetMostFacilitiesDataItem(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return mostFacilitiesDataList;
        }

        public List<Object> GetMostFacilitiesDataItem(Rendering rendering)
        {
            List<Object> mostFacilitiesDataItemList = new List<Object>();

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
                MostFacilitiesDataItem mostFacilitiesDataItem;

                if (datasource.TemplateID == Templates.RoomDetailsFacilitiesFolder.TemplateID && datasource.Children.ToList().Count > 0)
                {
                    List<Item> children = datasource.Children.Where(x => x.TemplateID == Templates.RoomDetailsFacilities.TemplateID).ToList();
                    if (children != null && children.Count > 0)
                    {
                        foreach (Item item in children)
                        {
                           if(item!=null && item.Fields[Templates.RoomDetailsFacilities.Fields.FieldsName.IsMostPopularFacilities]!=null
                                && !string.IsNullOrEmpty(item.Fields[Templates.RoomDetailsFacilities.Fields.FieldsName.IsMostPopularFacilities].Value))
                            {
                                bool IsMostFacilitiesCat = item.Fields[Templates.RoomDetailsFacilities.Fields.FieldsName.IsMostPopularFacilities].Value.Equals("1") ? true:false;
                                if(IsMostFacilitiesCat)
                                {
                                    mostFacilitiesDataItem = new MostFacilitiesDataItem();

                                    mostFacilitiesDataItem.Title = item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";

                                    mostFacilitiesDataItem.Icon = item.Fields[Templates.ISubTitle.FieldsName.SubTitle].Value != null ? item.Fields[Templates.ISubTitle.FieldsName.SubTitle].Value : "";

                                    mostFacilitiesDataItemList.Add(mostFacilitiesDataItem);
                                }                               
                            }
                            
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return mostFacilitiesDataItemList;
        }

        #endregion

        #region FacilitiescategoriesData

        public FacilitiesCategoriesData GetFacilitiesCategoriesDataList(Rendering rendering)
        {
            FacilitiesCategoriesData facilitiesCategoriesDataList = new FacilitiesCategoriesData();
            try
            {
                facilitiesCategoriesDataList.facilities = GetFacilitiesCategoriesDataItem(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return facilitiesCategoriesDataList;
        }

        public List<Object> GetFacilitiesCategoriesDataItem(Rendering rendering)
        {
            List<Object> facilitiesCategoriesDataItemList = new List<Object>();

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
                FacilitiesCategoriesDataItem facilitiesCategoriesDataItem;

                if (datasource.TemplateID == Templates.RoomDetailsFacilitiesCategoriesFolder.TemplateID && datasource.Children.ToList().Count > 0)
                {
                    List<Item> children = datasource.Children.Where(x => x.TemplateID == Templates.RoomDetailsFacilitiesCategories.TemplateID).ToList();
                    if (children != null && children.Count > 0)
                    {
                        foreach (Item item in children)
                        {
                            facilitiesCategoriesDataItem = new FacilitiesCategoriesDataItem();

                            facilitiesCategoriesDataItem.Title = item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";

                            MultilistField multiselectField = item.Fields[Templates.RoomDetailsFacilitiesCategories.Fields.FieldsName.Facilities];
                            List<FeaturesListDataItem> objectfeaturesList = new List<FeaturesListDataItem>();
                            if (!string.IsNullOrEmpty(multiselectField.Value) && multiselectField.TargetIDs.Count() > 0 && multiselectField.GetItems().Count() > 0)
                            {
                                foreach (Item featuresListItem in multiselectField.GetItems())
                                {
                                    FeaturesListDataItem featuresListDataItem = new FeaturesListDataItem();
                                    if (featuresListItem.TemplateID == Templates.Features.TemplateID)
                                    {

                                        featuresListDataItem.Title = featuresListItem.Fields[Templates.ITitle.FieldsName.Title].Value != null ? featuresListItem.Fields[Templates.ITitle.FieldsName.Title].Value : "";

                                        featuresListDataItem.Icon = featuresListItem.Fields[Templates.ISubTitle.FieldsName.SubTitle].Value != null ? featuresListItem.Fields[Templates.ISubTitle.FieldsName.SubTitle].Value : "";

                                    }
                                    objectfeaturesList.Add(featuresListDataItem);
                                }
                                facilitiesCategoriesDataItem.facilitiesCategoriesDataItem = objectfeaturesList;
                            }
                            

                            facilitiesCategoriesDataItemList.Add(facilitiesCategoriesDataItem);

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return facilitiesCategoriesDataItemList;
        }

        #endregion

        #region OtherRooms

        public otherRoomsData GetOtherRoomsData(Rendering rendering)
        {
            otherRoomsData otherRoomsData = new otherRoomsData();
            try
            {
                otherRoomsData.otherRooms = GetOtherRoomsDataItem(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return otherRoomsData;
        }

        public OtherRoomsDataItem GetOtherRoomsDataItem(Rendering rendering)
        {
            OtherRoomsDataItem otherroomsDataItem = new OtherRoomsDataItem();

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
                if (datasource.TemplateID == Templates.OtherRooms.TemplateID)
                {
                    Item item = datasource;
                    otherroomsDataItem.ImgSrc = Helper.GetImageSource(item, Templates.Image.FieldsName.Image) != null ?
                                       Helper.GetImageSource(item, Templates.Image.FieldsName.Image) : "";

                    otherroomsDataItem.ImgAlt = Helper.GetImageDetails(item, Templates.Image.FieldsName.Image) != null ?
                                       Helper.GetImageDetails(item, Templates.Image.FieldsName.Image).Fields["Alt"].Value : "";

                    otherroomsDataItem.ImgTitle = Helper.GetImageDetails(item, Templates.Image.FieldsName.Image) != null ?
                                       Helper.GetImageDetails(item, Templates.Image.FieldsName.Image).Fields["TItle"].Value : "";

                    otherroomsDataItem.Title = item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";

                    otherroomsDataItem.NonmemberPrice = item.Fields[Templates.ISubTitle.FieldsName.SubTitle].Value != null ? item.Fields[Templates.ISubTitle.FieldsName.SubTitle].Value : "";

                    otherroomsDataItem.MemberPrice = item.Fields[Templates.ISummary.FieldsName.Summary].Value != null ? item.Fields[Templates.ISummary.FieldsName.Summary].Value : "";
                }

            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return otherroomsDataItem;
        }

        #endregion

    }
}
