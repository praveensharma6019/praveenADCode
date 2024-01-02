using Adani.SuperApp.Realty.Feature.Configuration.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Adani.SuperApp.Realty.Feature.Configuration.Platform.Templates;
using EnquireForm = Adani.SuperApp.Realty.Feature.Configuration.Platform.Models.EnquireForm;
using ModalText = Adani.SuperApp.Realty.Feature.Configuration.Platform.Models.ModalText;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.Services
{
    public class CommonDataService : ICommonDataService
    {
        private readonly ILogRepository _logRepository;
        public CommonDataService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }

        public CommonDataModel GetCommonData(Rendering rendering)
        {
            CommonDataModel commondataModel = new CommonDataModel();
            try
            {
                Item _commonDataItem = rendering?.Item;
                if (_commonDataItem != null)
                {
                    ObjectItems whyAdaniItems = new ObjectItems();
                    whyAdaniItems.Items = new List<object>();
                    MultilistField whyAdaniField = _commonDataItem?.Fields[Templates.CommmonData.Fields.WhyAdani];
                    whyAdaniItems.Items = GetImageTextList(whyAdaniField);
                    commondataModel.WhyAdani = whyAdaniItems;

                    ObjectItems ourStoriesItems = new ObjectItems();
                    ourStoriesItems.Items = new List<object>();
                    MultilistField ourStoriesField = _commonDataItem?.Fields[Templates.CommmonData.Fields.OurStories];
                    ourStoriesItems.Items = GetImageTextList(ourStoriesField);
                    commondataModel.OurStories = ourStoriesItems;

                    ObjectItems clubEvents = new ObjectItems();
                    clubEvents.Items = new List<object>();
                    MultilistField clubEventsField = _commonDataItem?.Fields[Templates.CommmonData.Fields.ClubEvents];
                    clubEvents.Items = GetImageTextList(clubEventsField);
                    commondataModel.ClubEvents = clubEvents;

                    ObjectItems ourStory = new ObjectItems();
                    ourStory.Items = new List<object>();
                    MultilistField ourStoryField = _commonDataItem?.Fields[Templates.CommmonData.Fields.OurStory];
                    ourStory.Items = GetImageTextList(ourStoryField);
                    commondataModel.OurStory = ourStory;

                    ObjectItems headerInner = new ObjectItems();
                    headerInner.Items = new List<object>();
                    MultilistField headerInnerField = _commonDataItem?.Fields[Templates.CommmonData.Fields.HeaderInner];
                    headerInner.Items = GetImageTextList(headerInnerField);
                    commondataModel.HeaderInner = headerInner;


                    ObjectItems ourValues = new ObjectItems();
                    ourValues.Items = new List<object>();
                    MultilistField ourValuesField = _commonDataItem?.Fields[Templates.CommmonData.Fields.OurValues];
                    ourValues.Items = GetImageTextList(ourValuesField);
                    commondataModel.OurValues = ourValues;

                    //ConfigurationItems configuration = new ConfigurationItems();
                    //configuration.Items = new List<object>();
                    //MultilistField configurationField = _commonDataItem?.Fields[Templates.CommmonData.Fields.Configuration];
                    //configuration.Items = GetConfigurationData(configurationField);
                    //commondataModel.Configuration = configuration;
                    MultilistField configurationField = _commonDataItem?.Fields[Templates.CommmonData.Fields.Configuration];
                    commondataModel.Configuration = GetCommonData(configurationField);

                    commondataModel.ModalText = new ModalText();
                    commondataModel.ModalText = GetModalTextData(_commonDataItem);

                    ProjectPropertyDataOptions projectPropertyData = new ProjectPropertyDataOptions();
                    projectPropertyData.Options = new List<object>();
                    MultilistField projectPropField = _commonDataItem?.Fields[Templates.CommmonData.Fields.ProjectPropertyData];
                    projectPropertyData.Options = GetProjectPropertyData(projectPropField);
                    commondataModel.ProjectProertyData = projectPropertyData;

                    commondataModel.EnquireForm = new EnquireForm();
                    commondataModel.EnquireForm = GetEnquireFormDetails(_commonDataItem);


                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return commondataModel;

        }

        private List<Models.Configuration> GetCommonData(MultilistField configurationField)
        {
            List<Models.Configuration> configurations = new List<Models.Configuration>();
            try
            {
                MultilistField multilistField = configurationField;


                foreach (Item item in multilistField?.GetItems())
                {
                    Models.Configuration configuration = new Models.Configuration();
                    configuration.Items = new List<object>();
                    configuration.City = item?.Fields[CommmonData.Fields.Title]?.Value;
                    Item[] childItems = item?.GetChildren()?.ToArray();
                    foreach (Item childItem in childItems)
                    {
                        configuration.Items.Add(GetConfigurationData(childItem));
                    }
                    configurations.Add(configuration);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return configurations;
        }

        private List<Object> GetConfigurationData(Item item)
        {
            List<Object> configItems = new List<Object>();

            try
            {
                ConfiturationItem configItem = new ConfiturationItem();
                configItem.Keys = new List<Key>();
                configItem.Title = item?.Fields[Templates.ConfigurationTemplate.Fields.Title]?.Value;
                MultilistField keyMultilistField = item?.Fields[Templates.ConfigurationTemplate.Fields.Keys];
                configItem.Keys = GetConfigurationKeys(keyMultilistField);
                configItems.Add(configItem);
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return configItems;
        }

        private List<Key> GetConfigurationKeys(MultilistField keyMultilistField)
        {
            List<Key> keys = new List<Key>();
            try
            {
                if (keyMultilistField != null && keyMultilistField.GetItems().Count() > 0)
                {
                    foreach (Item item in keyMultilistField.GetItems())
                    {
                        Key key = new Key();
                        key.Link = Helper.GetLinkURLbyField(item, item?.Fields[Templates.LinkandKeyword.Fields.Link]);
                        key.Keyword = item?.Fields[Templates.LinkandKeyword.Fields.Keyword].Value;
                        keys?.Add(key);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return keys;
        }


        private EnquireForm GetEnquireFormDetails(Item commonDataItem)
        {
            EnquireForm enquireForm = new EnquireForm();

            try
            {
                enquireForm.EnquireNow = commonDataItem?.Fields[Templates.EnquireForm.Fields.EnquireNow].Value;
                enquireForm.ShareContact = commonDataItem?.Fields[Templates.EnquireForm.Fields.ShareContact].Value;
                enquireForm.SendusQuery = commonDataItem?.Fields[Templates.EnquireForm.Fields.SendUsQuery].Value;
                enquireForm.FirstName = commonDataItem?.Fields[Templates.EnquireForm.Fields.FirstName].Value;
                enquireForm.LastName = commonDataItem?.Fields[Templates.EnquireForm.Fields.LastName].Value;
                enquireForm.Name = commonDataItem?.Fields[Templates.EnquireForm.Fields.Name].Value;
                enquireForm.Email = commonDataItem?.Fields[Templates.EnquireForm.Fields.Email].Value;
                enquireForm.ProjectTypeOptions = new List<Dropdown>();
                enquireForm.ProjectTypeOptions = GetDropdownList(commonDataItem?.Fields[Templates.EnquireForm.Fields.ProjectType]);
                enquireForm.PropertyTypeOptions = new List<Dropdown>();
                enquireForm.PropertyTypeOptions = GetDropdownList(commonDataItem?.Fields[Templates.EnquireForm.Fields.PropertyType]);
                enquireForm.AgreeToConnect = commonDataItem?.Fields[Templates.EnquireForm.Fields.AgreetoConnect].Value;
                enquireForm.OverrideRegistry = commonDataItem?.Fields[Templates.EnquireForm.Fields.OverrideRegistry].Value;
                enquireForm.SubmitDetail = commonDataItem?.Fields[Templates.EnquireForm.Fields.SubmitDetails].Value;
                enquireForm.submit = commonDataItem?.Fields[Templates.EnquireForm.Fields.SubmitButton].Value;
                enquireForm.ProjectPropertyOptions = new List<Dropdown>();
                enquireForm.ProjectPropertyOptions = GetDropdownList(commonDataItem?.Fields[Templates.EnquireForm.Fields.ProjectProperty]);
                enquireForm.StartDate = commonDataItem?.Fields[Templates.EnquireForm.Fields.StartDate].Value;
                enquireForm.HomeLoanInterested = commonDataItem?.Fields[Templates.EnquireForm.Fields.HomeLoanInterested].Value;
                enquireForm.TimeSlotsOptions = new List<Dropdown>();
                enquireForm.TimeSlotsOptions = GetDropdownList(commonDataItem?.Fields[Templates.EnquireForm.Fields.TimeSolts]);
                enquireForm.Date = commonDataItem?.Fields[Templates.EnquireForm.Fields.Date].Value;
                enquireForm.From = commonDataItem?.Fields[Templates.EnquireForm.Fields.From].Value;
                enquireForm.To = commonDataItem?.Fields[Templates.EnquireForm.Fields.To].Value;
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return enquireForm;
        }

        private List<Dropdown> GetDropdownList(Field field)
        {
            List<Dropdown> dropdownList = new List<Dropdown>();
            try
            {
                MultilistField multilistField = field;
                foreach (Item item in multilistField.GetItems())
                {
                    Dropdown dropdown = new Dropdown();
                    dropdown.Id = item?.Fields[Templates.DropDown.Fields.ID].Value;
                    dropdown.Value = item?.DisplayName != "" ? item.DisplayName : item.Name;
                    dropdown.controllerName = item?.Fields[Templates.DropDown.Fields.Value].Value;
                    dropdownList.Add(dropdown);
                }

            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return dropdownList;
        }

        private List<object> GetProjectPropertyData(MultilistField projectPropField)
        {
            List<Object> projectPropItems = new List<Object>();

            try
            {
                if (projectPropField != null && projectPropField.GetItems().Count() > 0)
                {
                    foreach (Item item in projectPropField.GetItems())
                    {
                        ProjectPropertyOptions projectPropOptions = new ProjectPropertyOptions();
                        projectPropOptions.Label = item?.Fields[Templates.ProjectPropertyData.Fields.Label]?.Value;
                        projectPropOptions.Location = item?.Fields[Templates.ProjectPropertyData.Fields.Location]?.Value;
                        projectPropOptions.Key = item?.Fields[Templates.ProjectPropertyData.Fields.Key]?.Value;
                        projectPropItems.Add(projectPropOptions);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return projectPropItems;
        }

        private ModalText GetModalTextData(Item commonDataItem)
        {
            ModalText modalText = new ModalText();
            modalText.ShareContact = commonDataItem?.Fields[Templates.ModalText.Fields.ShareContact]?.Value;
            modalText.AgreeText = commonDataItem?.Fields[Templates.ModalText.Fields.AgreeText]?.Value;
            modalText.HomeLoanCheck = commonDataItem?.Fields[Templates.ModalText.Fields.HomeLoadCheck]?.Value;
            return modalText;
        }



        private List<Object> GetImageTextList(MultilistField multiField)
        {
            List<Object> list = new List<Object>();
            var commonItem = Sitecore.Context.Database.GetItem(Templates.commonData.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            try
            {
                if (multiField != null && multiField.GetItems().Count() > 0)
                {
                    foreach (Item item in multiField.GetItems())
                    {
                        ImageTitleText imageTitleText = new ImageTitleText();
                        imageTitleText.Src = Helper.GetImageURLbyField(item?.Fields[Templates.Image.Fields.Thumb]);
                        imageTitleText.Alt = Helper.GetImageAltbyField(item?.Fields[Templates.Image.Fields.Thumb]);
                        imageTitleText.Title = item?.Fields[Templates.TitleDescription.Fields.Title]?.Value;
                        imageTitleText.Decription = item?.Fields[Templates.TitleDescription.Fields.Description]?.Value;
                        var ButtonLink = Helper.GetLinkURLbyField(item, item?.Fields[Templates.TitleDescription.Fields.Link]);
                        imageTitleText.ButtonLink = ButtonLink != "" & !ButtonLink.Contains(strSitedomain) ? strSitedomain + ButtonLink : ButtonLink;
                        imageTitleText.ButtonText = Helper.GetLinkTextbyField(item, item?.Fields[Templates.TitleDescription.Fields.Link]);
                        imageTitleText.imgType = item?.Fields[Templates.TitleDescription.Fields.imgType]?.Value;
                        list?.Add(imageTitleText);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return list;
        }
        public CommonText GetCommonText(Rendering rendering)
        {
            List<Object> Banner = new List<Object>();
            CommonText banner = new CommonText(); ;

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

                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {

                    banner.seeall = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.seeall].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.seeall].Value.ToString() : "";
                    banner.projectResidential = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.projectResidential].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.projectResidential].Value.ToString() : "";
                    banner.commercialProjects = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.commercialProjects].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.commercialProjects].Value.ToString() : "";
                    banner.all = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.all].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.all].Value.ToString() : "";
                    banner.readytomove = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.readytomove].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.readytomove].Value.ToString() : "";
                    banner.underconstruction = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.underconstruction].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.underconstruction].Value.ToString() : "";
                    banner.commInOtherCity = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.commInOtherCity].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.commInOtherCity].Value.ToString() : "";
                    banner.commOffer = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.commOffer].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.commOffer].Value.ToString() : "";
                    banner.ressInOtherCity = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.ressInOtherCity].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.ressInOtherCity].Value.ToString() : "";
                    banner.ressOffer = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.ressOffer].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.ressOffer].Value.ToString() : "";
                    banner.filters = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.filters].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.filters].Value.ToString() : "";
                    banner.projectfound = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.projectfound].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.projectfound].Value.ToString() : "";
                    banner.faq = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.faq].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.faq].Value.ToString() : "";
                    banner.planavisit = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.planavisit].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.planavisit].Value.ToString() : "";
                    banner.searchproject = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.searchproject].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.searchproject].Value.ToString() : "";
                    banner.copylink = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.copylink].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.copylink].Value.ToString() : "";
                    banner.email = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.email].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.email].Value.ToString() : "";
                    banner.twitter = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.twitter].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.twitter].Value.ToString() : "";
                    banner.facebook = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.facebook].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.facebook].Value.ToString() : "";
                    banner.whatsapp = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.whatsapp].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.whatsapp].Value.ToString() : "";
                    banner.search = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.search].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.search].Value.ToString() : "";
                    banner.submit = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.submit].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.submit].Value.ToString() : "";
                    banner.readless = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.readless].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.readless].Value.ToString() : "";
                    banner.readmore = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.readmore].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.readmore].Value.ToString() : "";
                    banner.livinggoodlife = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.livinggoodlife].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.livinggoodlife].Value.ToString() : "";
                    banner.print = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.print].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.print].Value.ToString() : "";
                    banner.done = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.done].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.done].Value.ToString() : "";
                    banner.saveaspdf = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.saveaspdf].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.saveaspdf].Value.ToString() : "";
                    banner.downloadBrochure = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.downloadBrochure].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.downloadBrochure].Value.ToString() : "";
                    banner.share = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.share].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.share].Value.ToString() : "";
                    banner.socialClubs = !string.IsNullOrEmpty(item.Fields[CommonTextTemp.Fields.socialClubs].Value.ToString()) ? item.Fields[CommonTextTemp.Fields.socialClubs].Value.ToString() : "";
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" HeroCarouselService GetCommonText gives -> " + ex.Message);
            }

            return banner;
        }
    }
}