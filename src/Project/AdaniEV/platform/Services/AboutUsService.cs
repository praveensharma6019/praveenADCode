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
    public class AboutUsService : IAboutUsService
    {
        public AboutusModel GetAboutUsModel(Rendering rendering)
        {
            AboutusModel aboutusModel = new AboutusModel();
            List<AboutusItemModel> objAboutusItemModellist = new List<AboutusItemModel>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetAboutUsModel : Datasource is empty", this);
                return default;
                //throw new NullReferenceException();
            }
            try
            {
                aboutusModel.Title = datasource.Fields[Templates.Aboutus.Fields.title] != null ? datasource.Fields[Templates.Aboutus.Fields.title].Value : "";
                aboutusModel.Description = datasource.Fields[Templates.Aboutus.Fields.Description] != null ? datasource.Fields[Templates.Aboutus.Fields.Description].Value : "";
                var multilist = Utils.GetMultiListValueItem(datasource, Templates.Aboutus.Fields.widgetItems);
                MultilistField galleryMultilistField = datasource.Fields[Templates.Aboutus.Fields.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        AboutusItemModel ItemsListData = new AboutusItemModel();
                        ItemsListData.Title = galleryItem.Fields[Templates.AboutusItem.Fields.title].Value;
                        ItemsListData.Description = galleryItem.Fields[Templates.AboutusItem.Fields.Description].Value;
                        ItemsListData.ImgUrl = Utils.GetImageURLByFieldId(galleryItem, Templates.AboutusItem.Fields.Image);
                        ItemsListData.readMore = galleryItem.Fields[Templates.AboutusItem.Fields.ReadMore].Value;
                        objAboutusItemModellist.Add(ItemsListData);
                    }
                }

                aboutusModel.widgetItems = objAboutusItemModellist;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return aboutusModel;
        }
    }
}