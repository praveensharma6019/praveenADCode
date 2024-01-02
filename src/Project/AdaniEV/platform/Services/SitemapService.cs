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
    public class SitemapService : ISitemapService
    {
        public SitemapModel GetSitemapModel(Rendering rendering)
        {
            SitemapModel sitemapModel  = new SitemapModel();
            List<SitemapItem> sitemapItems = new List<SitemapItem>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetArticleVideoGalleryCarousel : Datasource is empty", this);
                return sitemapModel;
                //throw new NullReferenceException();
            }
            try
            {
                var multilist = Utils.GetMultiListValueItem(datasource, Templates.Sitemap.Fields.widgetItems);
                MultilistField galleryMultilistField = datasource.Fields[Templates.Sitemap.Fields.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        SitemapItem ItemsListData = new SitemapItem();
                        ItemsListData.title = galleryItem.Fields[Templates.SitemapItem.Fields.title].Value;                     
                        ItemsListData.imgUrl = Utils.GetImageURLByFieldId(galleryItem, Templates.SitemapItem.Fields.Image);            
                        sitemapItems.Add(ItemsListData);
                    }
                    sitemapModel.sitemapItems = sitemapItems;
                        }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return sitemapModel;
        }
    }
}