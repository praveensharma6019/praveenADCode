using Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Services
{
    public class OurLocationRootResolverService : IOurLocationRootResolverService
    {

        private readonly ILogRepository _logRepository;
        public OurLocationRootResolverService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public OurLocationData GetOurLocationDataList(Rendering rendering)
        {
            OurLocationData ourLocationDataList = new OurLocationData();
            try
            {

                ourLocationDataList.data = GetOurLocationData(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return ourLocationDataList;
        }

        public List<Object> GetOurLocationData(Rendering rendering)
        {
            List<Object> getOurLocationDataList = new List<Object>();
            var commonItem = Sitecore.Context.Database.GetItem(Templates.commondataitem);
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
                OurLocationDataItem ourLocationdata;
                if (datasource.TemplateID == Templates.OurLocationFolder.TemplateID && datasource.Children.ToList().Count > 0)
                {
                    List<Item> children = datasource.Children.Where(x => x.TemplateID == Templates.OurLocation.TemplateID).ToList();
                    if (children != null && children.Count > 0)
                    {
                        foreach (Sitecore.Data.Items.Item item in children)
                        {
                            ourLocationdata = new OurLocationDataItem();

                            ourLocationdata.link = Helper.GetLinkURL(item, Templates.ILink.FieldsName.Link) != null ?
                                      Helper.GetLinkURL(item, Templates.ILink.FieldsName.Link) : "";

                            ourLocationdata.imageSrc = Helper.GetImageSource(item, Templates.Image.FieldsName.Image) != null ?
                                      Helper.GetImageSource(item, Templates.Image.FieldsName.Image) : "";

                            ourLocationdata.imageAlt = Helper.GetImageDetails(item, Templates.Image.FieldsName.Image) != null ?
                                      Helper.GetImageDetails(item, Templates.Image.FieldsName.Image).Fields["Alt"].Value : "";

                            ourLocationdata.imgTitle = Helper.GetImageDetails(item, Templates.Image.FieldsName.Image) != null ?
                                      Helper.GetImageDetails(item, Templates.Image.FieldsName.Image).Fields["TItle"].Value : "";

                            ourLocationdata.projectCity = item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";

                            Sitecore.Data.Fields.MultilistField multiselectField = item.Fields[Templates.OurLocation.Fields.FieldsName.ProjectList];
                            List<ProjectlistDataItem> objectprojectList = new List<ProjectlistDataItem>();
                            if (!string.IsNullOrEmpty(multiselectField.Value) && multiselectField.TargetIDs.Count() > 0 && multiselectField.GetItems().Count() > 0)
                            {
                                foreach (Item product in multiselectField.GetItems())
                                {
                                    ProjectlistDataItem projectListItem = new ProjectlistDataItem();

                                    if (product.TemplateID == Templates.Commercial.TemplateID || product.TemplateID == Templates.Residential.TemplateID || product.TemplateID == Templates.Club.TemplateID)
                                    {
                                        projectListItem.projectTitle = !string.IsNullOrEmpty(product.Fields[Templates.HasPageContent.Fields.FieldsName.HasPageContentTitle].ToString()) ? product.Fields[Templates.HasPageContent.Fields.FieldsName.HasPageContentTitle].ToString() : "";
                                        projectListItem.projectPrice = !string.IsNullOrEmpty(product.Fields[Templates.Property.Fields.FieldsID.PropertyPrice].Value) ? product.Fields[Templates.Property.Fields.FieldsID.PropertyPrice].Value.ToString() : "";
                                        projectListItem.projectType = !string.IsNullOrEmpty(product.Fields[Templates.Property.Fields.FieldsID.PropertyType].Value) ? product.Fields[Templates.Property.Fields.FieldsID.PropertyType].Value : "";
                                        projectListItem.propertyType = product.Parent.Parent.ID == Templates.Commercial.CommercialItemID ? "Commercial-Projects" : product.Parent.Parent.ID == Templates.Residential.ResidentialItemID ? "Residential-Projects" : "Clubs";
                                        projectListItem.Projectlink = !string.IsNullOrEmpty(product.Fields[Templates.HasPageContent.Fields.FieldsName.HasPageContentLink].Value) ? Helper.GetLinkURL(product, Templates.HasPageContent.Fields.FieldsName.HasPageContentLink) : "";
                                        projectListItem.linktarget = Helper.GetLinkURLTargetSpace(product, Templates.HasPageContent.Fields.FieldsName.HasPageContentLink) != null ? Helper.GetLinkURLTargetSpace(product, Templates.HasPageContent.Fields.FieldsName.HasPageContentLink) : "";
                                        projectListItem.AllInclusiveLabel = !string.IsNullOrEmpty(product.Fields[Templates.Property.Fields.FieldsID.allinclusivelabel].Value) ? product.Fields[Templates.Property.Fields.FieldsID.allinclusivelabel].Value : "";
                                        projectListItem.pricestartinglabel = !string.IsNullOrEmpty(product.Fields[Templates.Property.Fields.FieldsID.pricestartinglabel].Value) ? product.Fields[Templates.Property.Fields.FieldsID.pricestartinglabel].Value : "";
                                        projectListItem.condition = !string.IsNullOrEmpty(product.Fields[Templates.Property.Fields.FieldsID.condition].Value) ? product.Fields[Templates.Property.Fields.FieldsID.condition].Value : "";

                                    }
                                    objectprojectList.Add(projectListItem);
                                }
                            }
                            ourLocationdata.projectList = objectprojectList;
                            var HomeItem = rendering.RenderingItem.Database.GetItem(Templates.Home.ItemID);
                            List<string> projectTypemultiListForItem = new List<string>();
                            if (HomeItem != null)
                            {
                                foreach (Item propertyitem in HomeItem.Children.OrderByDescending(x => x.Name).ToList())
                                {
                                    if (propertyitem.ID == Templates.Residential.ResidentialItemID)
                                    {
                                        var propcheck = propertyitem.Children.Where(x => x.Name.ToLower() == item.Name.ToLower()).ToList();
                                        if (propcheck.Count() != 0)
                                        {
                                            var title = !string.IsNullOrEmpty(propertyitem.Fields[Templates.Property.Fields.FieldsID.PropertyTypeTitle].Value) ? propertyitem.Fields[Templates.Property.Fields.FieldsID.PropertyTypeTitle].Value.ToString() : "Residential";
                                            projectTypemultiListForItem.Add(title);
                                        }
                                    }
                                    if (propertyitem.ID == Templates.Commercial.CommercialItemID)
                                    {
                                        var propcheck = propertyitem.Children.Where(x => x.Name.ToLower() == item.Name.ToLower()).ToList();
                                        if (propcheck.Count() != 0)
                                        {
                                            var title = !string.IsNullOrEmpty(propertyitem.Fields[Templates.Property.Fields.FieldsID.PropertyTypeTitle].Value) ? propertyitem.Fields[Templates.Property.Fields.FieldsID.PropertyTypeTitle].Value.ToString() : "Commercial & Retail";
                                            projectTypemultiListForItem.Add(title);
                                        }
                                    }
                                    if (propertyitem.ID == Templates.Club.ClubItemID)
                                    {
                                        var propcheck = propertyitem.Children.Where(x => x.Name.ToLower() == item.Name.ToLower()).ToList();
                                        if (propcheck.Count() != 0)
                                        {
                                            var title = !string.IsNullOrEmpty(propertyitem.Fields[Templates.Property.Fields.FieldsID.PropertyTypeTitle].Value) ? propertyitem.Fields[Templates.Property.Fields.FieldsID.PropertyTypeTitle].Value.ToString() : "CLub";
                                            projectTypemultiListForItem.Add(title);
                                        }
                                    }

                                }
                            }
                            ourLocationdata.propertyType = projectTypemultiListForItem;

                            getOurLocationDataList.Add(ourLocationdata);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return getOurLocationDataList;
        }

    }
}