using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Feature.Navigation.Platform.Models;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using static Adani.SuperApp.Realty.Feature.Navigation.Platform.Templates;
using FooterHeading = Adani.SuperApp.Realty.Feature.Navigation.Platform.Models.FooterHeading;
using Sitecore.Mvc.Extensions;

namespace Adani.SuperApp.Realty.Feature.Navigation.Platform.Services
{
    public class FooterService : IFooterService
    {
        private readonly ILogRepository _logRepository;
        public FooterService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public Footer GetFooterData(Rendering rendering, string location = null)
        {
            Footer footer = new Footer();
            try
            {
                Item renderingItem = rendering?.Item;
                if (renderingItem != null)
                {
                    footer.MainNavigations = new List<FooterHeading>();
                    footer.MainNavigations = GetMainNavData(renderingItem?.Fields[FooterTemplate.FooterFields.MainNavigations] , location);
                    footer.CopyRight = GetFooterCopyRightData(renderingItem?.Fields[FooterTemplate.FooterFields.CopyRight]);
                    footer.FooterSocialLink = GetFooterSocialLink(renderingItem?.Fields[FooterTemplate.FooterFields.SocialLinks]);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return footer;
        }

        private List<CopyRightFooterHeading> GetFooterCopyRightData(Field field)
        {
            List<CopyRightFooterHeading> footerList = new List<CopyRightFooterHeading>();
            try
            {
                MultilistField mulField = field;
                foreach (Item item in mulField.GetItems())
                {
                    CopyRightFooterHeading footerCopyRightHeading = new CopyRightFooterHeading();
                    footerCopyRightHeading.Heading = item?.Fields[FooterTemplate.FooterHeading.Heading]?.Value;
                    footerCopyRightHeading.SubHeading = item?.Fields[FooterTemplate.FooterHeading.SubHeading]?.Value;
                    footerCopyRightHeading.Items = GetFooterLinks(item?.Fields[FooterTemplate.FooterHeading.Links]);
                    footerList?.Add(footerCopyRightHeading);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return footerList;
        }

        private List<FooterSocialHeading> GetFooterSocialLink(Field field)
        {
            List<FooterSocialHeading> footerList = new List<FooterSocialHeading>();
            try
            {
                MultilistField mulField = field;
                foreach (Item item in mulField.GetItems())
                {
                    FooterSocialHeading footerSocialHeading = new FooterSocialHeading();
                    footerSocialHeading.Heading = item?.Fields[FooterTemplate.FooterHeading.Heading]?.Value;
                    footerSocialHeading.Items = GetFooterSocialLinks(item?.Fields[FooterTemplate.FooterHeading.Links]);
                    footerList?.Add(footerSocialHeading);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return footerList;
        }

        private List<FooterHeading> GetMainNavData(MultilistField field, string location = null )
        {
            List<FooterHeading> footerList = new List<FooterHeading>();

            try
            {
                //MultilistField mulField = field;
                foreach (Item item in field.GetItems())
                {
                    FooterHeading footerHeading = new FooterHeading();
                    List<object> locationList = new List<object>();

                    if (item.ID == FooterTemplate.SEOFooter)
                    {
                        if (Sitecore.Context.Item.TemplateID == FooterTemplate.ResidentialTemplateID || Sitecore.Context.Item.TemplateID == FooterTemplate.CommercialTemplateID || Sitecore.Context.Item.TemplateID == FooterTemplate.SeoTemplateID || Sitecore.Context.Item.TemplateID == FooterTemplate.LocationTempID || Sitecore.Context.Item.TemplateID == FooterTemplate.TownshipTempID)
                        {
                            var heading = item?.Fields[FooterTemplate.FooterHeading.Heading]?.Value;
                            if (Sitecore.Context.Item.TemplateID == FooterTemplate.ResidentialTemplateID || Sitecore.Context.Item.TemplateID == FooterTemplate.CommercialTemplateID)
                            {
                                var Location = Helper.GetSelectedItemFromDroplistFieldValue(Sitecore.Context.Item, FooterTemplate.Location);
                                footerHeading.isSeoFooter = Helper.GetCheckBoxSelection(item.Fields[Templates.FooterTemplate.FooterHeading.isSeoFooter]);
                                footerHeading.Heading = heading + Location;
                                //footerHeading.Heading = !string.IsNullOrEmpty(Location.ToString()) ? (heading + Location).ToString() : null;
                                locationList.Add(Location);
                            }
                            else if (Sitecore.Context.Item.TemplateID == FooterTemplate.SeoTemplateID)
                            {
                                var Location = Sitecore.Context.Item.Fields["CityName"].Value;
                                footerHeading.isSeoFooter = Helper.GetCheckBoxSelection(item.Fields[Templates.FooterTemplate.FooterHeading.isSeoFooter]);
                                footerHeading.Heading = heading + Location;
                                //footerHeading.Heading = !string.IsNullOrEmpty(Location.ToString()) ? (heading + Location).ToString() : null;
                                locationList.Add(Location);
                            }
                            
                            else if (Sitecore.Context.Item.TemplateID == FooterTemplate.LocationTempID)
                            {
                                var CityLocation = location;
                                var Location = char.ToUpper(CityLocation[0]) + CityLocation.Substring(1);
                                footerHeading.isSeoFooter = Helper.GetCheckBoxSelection(item.Fields[Templates.FooterTemplate.FooterHeading.isSeoFooter]);
                                footerHeading.Heading = heading + Location;
                                //footerHeading.Heading = !string.IsNullOrEmpty(Location.ToString()) ? (heading + Location).ToString() : null;
                                locationList.Add(Location);
                            }
                            
                            else if (Sitecore.Context.Item.TemplateID == FooterTemplate.TownshipTempID)
                            {
                                var Location = Sitecore.Context.Item.Fields[FooterTemplate.FooterHeading.TownshipLocation].Value; ;
                                footerHeading.isSeoFooter = Helper.GetCheckBoxSelection(item.Fields[Templates.FooterTemplate.FooterHeading.isSeoFooter]);
                                footerHeading.Heading = heading + Location;
                                //footerHeading.Heading = !string.IsNullOrEmpty(Location.ToString()) ? (heading + Location).ToString() : null;
                                locationList.Add(Location);
                            }
                            footerHeading.Items = GetSeoFooterLinks(item?.Fields[FooterTemplate.FooterHeading.Links] , location);
                            if (footerHeading.Items.Count > 0)
                            {
                                footerList.Add(footerHeading);
                            }
                        }
                    }
                    else
                    {
                        footerHeading.Heading = item?.Fields[FooterTemplate.FooterHeading.Heading]?.Value;
                        footerHeading.Items = GetFooterLinks(item?.Fields[FooterTemplate.FooterHeading.Links]);
                        footerList.Add(footerHeading);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return footerList;
        }
        private List<object> GetSeoFooterLinks(Field field , string location = null)
        {
            List<object> SeolinkList = new List<object>();
            try
            {
                List<string> locationList = new List<string>();
                List<Item> listOfProperty = new List<Item>();
                List<Item> prop = new List<Item>();

                if (Sitecore.Context.Item.TemplateID == FooterTemplate.ResidentialTemplateID || Sitecore.Context.Item.TemplateID == FooterTemplate.CommercialTemplateID)
                {
                    var Location = Helper.GetSelectedItemFromDroplistFieldValue(Sitecore.Context.Item, FooterTemplate.Location);
                    locationList.Add(Location);
                }
                else if (Sitecore.Context.Item.TemplateID == FooterTemplate.SeoTemplateID)
                {
                    var Location = Sitecore.Context.Item.Fields["CityName"].Value;
                    locationList.Add(Location);
                }
                else if (Sitecore.Context.Item.TemplateID == FooterTemplate.LocationTempID)
                {
                    var CityLocation = location;
                    var Location = char.ToUpper(CityLocation[0]) + CityLocation.Substring(1);
                    locationList.Add(Location);
                }
                else if (Sitecore.Context.Item.TemplateID == FooterTemplate.TownshipTempID)
                {
                    var Location = Sitecore.Context.Item.Fields[FooterTemplate.FooterHeading.TownshipLocation].Value;
                    locationList.Add(Location);
                }

                var dataItem = Sitecore.Context.Database.GetItem("{AE2AC42E-61EF-4A35-AC1E-EBBC160496AA}");
                var propList = dataItem.GetMultiListValueItem(FooterTemplate.FooterHeading.Links);
                foreach (var propertyTypeItem in propList)
                {
                    var y = propertyTypeItem.Axes.GetDescendants().Where(x => x.TemplateID == FooterTemplate.SeoTemplateID);

                    listOfProperty.AddRange(y);
                }
                foreach (var item in listOfProperty)
                {
                   
                    var property = !string.IsNullOrEmpty(item.Fields["CityName"].Value.ToString()) ? item.Fields["CityName"].Value.ToString() : null;
                    if (property != null && locationList.Contains(property))
                    {
                        prop.Add(item);
                    }
                }
                
                foreach (var item in prop)
                {
                    Link link = new Link();
                    link.LinkTitle = Helper.GetLinkDescriptionByField(item?.Fields[FooterTemplate.LinkFields.PropertyLinkID]);
                    link.LinkUrl = Helper.GetLinkURLbyField(item, item?.Fields[FooterTemplate.LinkFields.PropertyLinkID]);
                    link.target = Helper.GetLinkURLTargetSpace(item, FooterTemplate.LinkFields.PropertyTarget);
                    SeolinkList.Add(link);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return SeolinkList;
        }
        private List<object> GetFooterLinks(Field field)
        {
            List<object> linkList = new List<object>();
            try
            {
                MultilistField mulField = field;

                foreach (Item item in mulField.GetItems())
                {
                    Link link = new Link();
                    link.LinkTitle = Helper.GetLinkDescriptionByField(item?.Fields[FooterTemplate.LinkFields.Link]);
                    link.LinkUrl = Helper.GetLinkURLbyField(item, item?.Fields[FooterTemplate.LinkFields.Link]);
                    link.target = Helper.GetLinkURLTargetSpace(item, FooterTemplate.LinkFields.LinkName);
                    linkList?.Add(link);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return linkList;
        }
        private List<Propertylinks> GetPropertyLinks(Field field)
        {
            List<Propertylinks> linkList = new List<Propertylinks>();
            try
            {
                MultilistField mulField = field;
                foreach (Item item in mulField.GetItems())
                {
                    Propertylinks link = new Propertylinks();
                    link.linkTitle = Helper.GetLinkDescriptionByField(item?.Fields[FooterTemplate.LinkFields.Link]);
                    link.link = Helper.GetLinkURLbyField(item, item?.Fields[FooterTemplate.LinkFields.Link]);
                    link.target = Helper.GetLinkURLTargetSpace(item, FooterTemplate.LinkFields.LinkName);
                    linkList?.Add(link);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return linkList;
        }
        private List<SocialLink> GetFooterSocialLinks(Field field)
        {
            List<SocialLink> linkList = new List<SocialLink>();
            try
            {
                MultilistField mulField = field;
                foreach (Item item in mulField.GetItems())
                {
                    SocialLink link = new SocialLink();
                    link.LinkTitle = Helper.GetLinkDescriptionByField(item?.Fields[FooterTemplate.LinkFields.Link]);
                    link.LinkUrl = Helper.GetLinkURLbyField(item, item?.Fields[FooterTemplate.LinkFields.Link]);
                    link.target = Helper.GetLinkURLTargetSpace(item, FooterTemplate.LinkFields.LinkName) != null ?
                                                        Helper.GetLinkURLTargetSpace(item, FooterTemplate.LinkFields.LinkName) : "";
                    link.ItemIcon = Helper.GetLinkStyle(item?.Fields[FooterTemplate.LinkFields.Link]);
                    linkList?.Add(link);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return linkList;
        }
        public List<FooterHeading> GetConfigurationData(Rendering rendering)
        {
            List<FooterHeading> result = new List<FooterHeading>();
            Item renderingItem = rendering?.Item;
            if (renderingItem != null)
            {
                result = GetMainNavData(renderingItem?.Fields[FooterTemplate.FooterFields.MainNavigations]);
            }
            return result;
        }
    }
}
