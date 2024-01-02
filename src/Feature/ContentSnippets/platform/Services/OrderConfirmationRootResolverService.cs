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
    public class OrderConfirmationRootResolverService : IOrderConfirmationRootResolverService
    {

        private readonly ILogRepository _logRepository;
        public OrderConfirmationRootResolverService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        #region ConfirmBanner
        public ConfirmBannerData GetConfirmBannerDataList(Rendering rendering)
        {
            ConfirmBannerData confirmBannerDataList = new ConfirmBannerData();
            try
            {
                confirmBannerDataList.bookingConfirmed= GetConfirmBannerDataItem(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return confirmBannerDataList;
        }

        public ConfirmBannerDataItem GetConfirmBannerDataItem(Rendering rendering)
        {
            ConfirmBannerDataItem confirmBannerDataItem = new ConfirmBannerDataItem();
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
                if (datasource.TemplateID == Templates.ConfirmBanner.TemplateID)
                {
                    Item item = datasource;
                    if (item != null)
                    {
                        confirmBannerDataItem.Heading = item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";

                        confirmBannerDataItem.Detail = item.Fields[Templates.ISummary.FieldsName.Summary].Value != null ? item.Fields[Templates.ISummary.FieldsName.Summary].Value : "";                       
                    }
                }

            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return confirmBannerDataItem;
        }

        #endregion

        #region OrderDetails

        public OrderDetailsData GetOrderDetailsDataList(Rendering rendering)
        {
            OrderDetailsData orderDetailsDataList = new OrderDetailsData();
            try
            {
                orderDetailsDataList.data = GetOrderDetailsDataListItem(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return orderDetailsDataList;
        }

        public List<Object> GetOrderDetailsDataListItem(Rendering rendering)
        {
            List<Object> orderDetailsDataItemList = new List<Object>();
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
                OrderDetailsDataItem orderDetailsDataItem;

                if (datasource.TemplateID == Templates.OrderDetails.TemplateID)
                {
                 
                    List<Item> children = datasource.GetMultiListValueItem(Templates.OrderDetails.Fields.FieldsID.OrderDetailsItems).ToList();
                    if (children != null && children.Count > 0)
                    {
                        foreach (Item item in children)
                        {
                            orderDetailsDataItem = new OrderDetailsDataItem();
                            if (item.TemplateID == Templates.OrderDetailsItem.TemplateID)
                            {
                                
                                orderDetailsDataItem.Label =  item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";
                                orderDetailsDataItem.Detail = item.Fields[Templates.ISummary.FieldsName.Summary].Value != null ? item.Fields[Templates.ISummary.FieldsName.Summary].Value : "";
                            }

                            orderDetailsDataItemList.Add(orderDetailsDataItem);
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return orderDetailsDataItemList;
        }

        #endregion

        #region SaveDetails

        public SaveDetailsData GetSaveDetailsDataList(Rendering rendering)
        {
            SaveDetailsData saveDetailsData = new SaveDetailsData();
            try
            {
                saveDetailsData.saveDetails = GetSaveDetailsDataListItem(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return saveDetailsData;
        }

        public List<Object> GetSaveDetailsDataListItem(Rendering rendering)
        {
            List<Object> saveDetailsDataItemList = new List<Object>();
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
                SaveDetailsDataItem saveDetailsDataItem;

                if (datasource.TemplateID == Templates.SaveDetails.TemplateID)
                {

                    List<Item> children = datasource.GetMultiListValueItem(Templates.SaveDetails.Fields.FieldsID.SaveDetailsItems).ToList();
                    if (children != null && children.Count > 0)
                    {
                        foreach (Item item in children)
                        {
                            saveDetailsDataItem = new SaveDetailsDataItem();
                            if (item.TemplateID == Templates.SaveDetailsItems.TemplateID)
                            {

                                saveDetailsDataItem.Label = item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";
                                
                                saveDetailsDataItem.Icon = item.Fields[Templates.ISubTitle.FieldsName.SubTitle].Value != null ? item.Fields[Templates.ISubTitle.FieldsName.SubTitle].Value : "";
                                
                                saveDetailsDataItem.ImgSrc = Helper.GetImageSource(item, Templates.Image.FieldsName.Image) != null ?
                                      Helper.GetImageSource(item, Templates.Image.FieldsName.Image) : "";

                                saveDetailsDataItem.ImgAlt = Helper.GetImageDetails(item, Templates.Image.FieldsName.Image) != null ?
                                          Helper.GetImageDetails(item, Templates.Image.FieldsName.Image).Fields["Alt"].Value : "";

                                saveDetailsDataItem.ImgTitle = Helper.GetImageDetails(item, Templates.Image.FieldsName.Image) != null ?
                                          Helper.GetImageDetails(item, Templates.Image.FieldsName.Image).Fields["TItle"].Value : "";
                            }

                            saveDetailsDataItemList.Add(saveDetailsDataItem);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return saveDetailsDataItemList;
        }

        #endregion

        #region Explore

        public ExploreData GetExploreData(Rendering rendering)
        {
            ExploreData exploreData = new ExploreData();
            try
            {
                exploreData.Explore = GetExploreDataItem(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return exploreData;
        }

        public ExploreDataItem GetExploreDataItem(Rendering rendering)
        {
            ExploreDataItem exploreDataItem = new ExploreDataItem();

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
                if (datasource.TemplateID == Templates.Explore.TemplateID)
                {
                    Item item = datasource;
                    if (item != null)
                    {
                        exploreDataItem.Heading = item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";
                      
                    }
                }

            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return exploreDataItem;
        }

        #endregion

        #region Configuration

        public ConfigurationData GetConfigurationData(Rendering rendering)
        {
            ConfigurationData configurationData = new ConfigurationData();
            try
            {
                configurationData.configuration = GetConfigurationDataListItem(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return configurationData;
        }

        public List<Object> GetConfigurationDataListItem(Rendering rendering)
        {
            List<Object> configurationDataItemList = new List<Object>();
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
                ConfigurationDataItem configurationDataItem;

                if (datasource.TemplateID == Templates.ConfigurationFolder.TemplateID)
                {

                    List<Item> children = datasource.GetChildren().Where(x=>x.TemplateID == Templates.Configuration.TemplateID).ToList();
                    if (children != null && children.Count > 0)
                    {
                        foreach (Item item in children)
                        {
                            configurationDataItem = new ConfigurationDataItem();
                            if (item.TemplateID == Templates.Configuration.TemplateID)
                            {
                                configurationDataItem.Title = item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";

                                List<ConfigurationKeysItem> objectconfigurationList = new List<ConfigurationKeysItem>();

                                var multiListForItem = item.GetMultiListValueItem(Templates.Configuration.Fields.FieldsID.KeysItems);
                                if (multiListForItem != null && multiListForItem.Count() > 0)
                                {
                                    foreach (var selectedKeywords in multiListForItem)
                                    {
                                        ConfigurationKeysItem configurationKeysItem = new ConfigurationKeysItem();
                                        if (selectedKeywords.TemplateID == Templates.ConfigurationKeysItem.TemplateID)
                                        {
                                            
                                            configurationKeysItem.Link = Helper.GetLinkURL(selectedKeywords, Templates.ILink.FieldsName.Link) != null ?
                                                Helper.GetLinkURL(selectedKeywords, Templates.ILink.FieldsName.Link) : null;

                                            configurationKeysItem.LinkText = Helper.GetLinkTextbyField(selectedKeywords, selectedKeywords.Fields[Templates.ILink.FieldsName.Link]) != null ?
                                                 Helper.GetLinkTextbyField(selectedKeywords, selectedKeywords.Fields[Templates.ILink.FieldsName.Link]) : "";

                                            configurationKeysItem.keyword = selectedKeywords.Fields[Templates.ITitle.FieldsName.Title].Value != null ? selectedKeywords.Fields[Templates.ITitle.FieldsName.Title].Value : "";

                                            objectconfigurationList.Add(configurationKeysItem);
                                        }
                                    }
                                }

                                configurationDataItem.Keys = objectconfigurationList;

                            }

                            configurationDataItemList.Add(configurationDataItem);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return configurationDataItemList;
        }

        #endregion
    }
}
