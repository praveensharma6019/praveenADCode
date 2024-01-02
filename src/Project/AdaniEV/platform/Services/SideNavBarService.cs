using Adani.EV.Project.Helper;
using Adani.EV.Project.Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Services
{
    public class SideNavBarService : ISideNavBarService
    {
        public SidebarModel GetSideNavbarModel(Rendering rendering)
        {
            SidebarModel objsidebarModel = new SidebarModel();


            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetBannerCarousel : Datasource is empty", this);
                return objsidebarModel;
            }
            try
            {
                objsidebarModel.name = datasource.Fields[Templates.SideNavbar.Fields.Name] != null ? datasource.Fields[Templates.SideNavbar.Fields.Name].Value : "";
                objsidebarModel.widgetItems.Login.title = datasource.Fields[Templates.SideNavbar.LoginSideNavBarDetails.Fields.Title] != null ? datasource.Fields[Templates.SideNavbar.LoginSideNavBarDetails.Fields.Title].Value : "";
                objsidebarModel.widgetItems.Login.ctaLink = Utils.GetLinkURL(datasource?.Fields[Templates.SideNavbar.LoginSideNavBarDetails.Fields.CTALink]);
                objsidebarModel.widgetItems.Login.image = Utils.GetImageURLByFieldId(datasource, Templates.SideNavbar.LoginSideNavBarDetails.Fields.Image);
                objsidebarModel.widgetItems.Login.sideDrawerRightIcon = Utils.GetImageURLByFieldId(datasource, Templates.SideNavbar.LoginSideNavBarDetails.Fields.sideDrawerRightIcon);

                objsidebarModel.widgetItems.profile.title = datasource.Fields[Templates.SideNavbar.ProfileSideNavBarDetails.Fields.Title] != null ? datasource.Fields[Templates.SideNavbar.ProfileSideNavBarDetails.Fields.Title].Value : "";
                objsidebarModel.widgetItems.profile.ctaLink = Utils.GetLinkURL(datasource?.Fields[Templates.SideNavbar.ProfileSideNavBarDetails.Fields.CTALink]);
                objsidebarModel.widgetItems.profile.image = Utils.GetImageURLByFieldId(datasource, Templates.SideNavbar.ProfileSideNavBarDetails.Fields.Image);
                objsidebarModel.widgetItems.profile.sideDrawerRightIcon = Utils.GetImageURLByFieldId(datasource, Templates.SideNavbar.ProfileSideNavBarDetails.Fields.sideDrawerRightIcon);


                var multilist = Utils.GetMultiListValueItem(datasource, Templates.SideNavbar.Fields.SideNavbarTargetItems);
                MultilistField galleryMultilistField = datasource.Fields[Templates.SideNavbar.Fields.SideNavbarTargetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        SideNavbarTargetItem ItemsData = new SideNavbarTargetItem();
                        ItemsData.title = galleryItem.Fields[Templates.SideNavbarTargetItems.Fields.Title].Value;
                        ItemsData.image = Utils.GetImageURLByFieldId(galleryItem, Templates.SideNavbarTargetItems.Fields.Image);
                        ItemsData.sideDrawerRightIcon= Utils.GetImageURLByFieldId(galleryItem, Templates.SideNavbarTargetItems.Fields.sideDrawerRightIcon);

                       MultilistField SideNavTargetMultilistField = galleryItem.Fields[Templates.SideNavbarTargetItems.Fields.SideNavbarItem];

                        if (SideNavTargetMultilistField.Count > 0)
                        {
                            foreach (Item objitem in SideNavTargetMultilistField.GetItems())
                            {
                                SideNavbarItem sideNavbarItem = new SideNavbarItem();
                                sideNavbarItem.title = objitem.Fields[Templates.SideNavbarItem.Fields.Title].Value;
                                sideNavbarItem.image = Utils.GetImageURLByFieldId(objitem, Templates.SideNavbarItem.Fields.Image);
                                sideNavbarItem.ctaLink = Utils.GetLinkURL(objitem.Fields[Templates.SideNavbarItem.Fields.ctaLink]);
                                ItemsData.items.Add(sideNavbarItem);
                            }
                        }
                        objsidebarModel.widgetItems.targetItems.Add(ItemsData);
                    }
                }
                return objsidebarModel;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                //throw ex;
            }
            return objsidebarModel;
        }
    }
}