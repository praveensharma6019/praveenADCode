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
    public class HomeService : IHomeService
    {     

        public HeroCarouselModel GetHeroCarousel(Rendering rendering)
        {
            HeroCarouselModel heroCarouselData = new HeroCarouselModel();
            List<HeroCarouselWidgetItem> widgetItemsData = new List<HeroCarouselWidgetItem>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetHeroCarousel : Datasource is empty", this);
                return heroCarouselData;
            }
            try
            {
                heroCarouselData.Id = datasource.Fields[Templates.BaseFieldTemplates.Fields.id] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.id].Value : "";
                heroCarouselData.Type = datasource.Fields[Templates.BaseFieldTemplates.Fields.type] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.type].Value : "";             
                MultilistField galleryMultilistField = datasource.Fields[Templates.HeroCarousel.Fields.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        HeroCarouselWidgetItem HeroCarouselwidgetItemsData = new HeroCarouselWidgetItem();
                        HeroCarouselwidgetItemsData.Id = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.id].Value;
                        HeroCarouselwidgetItemsData.Title = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.title].Value;
                        HeroCarouselwidgetItemsData.SubTitle = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.subTitle].Value;
                        HeroCarouselwidgetItemsData.Imagesrc = Utils.GetImageURLByFieldId(galleryItem, Templates.BaseFieldTemplates.Fields.imageSrc); ;
                        widgetItemsData.Add(HeroCarouselwidgetItemsData);
                    }
                }
                heroCarouselData.widgetItems = widgetItemsData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return heroCarouselData;
        }

        public LatestEVNewsModel GetLatestEVNewsModel(Rendering rendering)
        {
            LatestEVNewsModel  latestEVNews = new LatestEVNewsModel();
            List<LatestEVNewsWidgetItem> widgetItemsData = new List<LatestEVNewsWidgetItem>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetLatestEVNewsModel : Datasource is empty", this);
                return latestEVNews;
            }
            try
            {
                latestEVNews.id = datasource.Fields[Templates.BaseFieldTemplates.Fields.id] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.id].Value : "";
                latestEVNews.type = datasource.Fields[Templates.BaseFieldTemplates.Fields.type] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.type].Value : "";
                latestEVNews.title = datasource.Fields[Templates.BaseFieldTemplates.Fields.title] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.title].Value : "";
                latestEVNews.backgroundColor = datasource.Fields[Templates.BaseFieldTemplates.Fields.backgroundColor] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.backgroundColor].Value : "";
                MultilistField galleryMultilistField = datasource.Fields[Templates.LatestEVNews.Fields.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        LatestEVNewsWidgetItem HeroCarouselwidgetItemsData = new LatestEVNewsWidgetItem();
                        HeroCarouselwidgetItemsData.id = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.id].Value;
                        HeroCarouselwidgetItemsData.title = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.title].Value;
                        HeroCarouselwidgetItemsData.subTitle = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.subTitle].Value;
                        HeroCarouselwidgetItemsData.imageSrc = Utils.GetImageURLByFieldId(galleryItem, Templates.BaseFieldTemplates.Fields.imageSrc); ;
                        widgetItemsData.Add(HeroCarouselwidgetItemsData);
                    }
                }
                latestEVNews.widgetItems = widgetItemsData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return latestEVNews;
        }

        public QuickInfoModel GetQuickInfoModel(Rendering rendering)
        {
            QuickInfoModel heroCarouselData = new QuickInfoModel();
            List<QuickInfoWidgetItem> widgetItemsData = new List<QuickInfoWidgetItem>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetQuickInfoModel : Datasource is empty", this);
                return heroCarouselData;
            }
            try
            {
                heroCarouselData.Id = datasource.Fields[Templates.BaseFieldTemplates.Fields.id] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.id].Value : "";
                heroCarouselData.Type = datasource.Fields[Templates.BaseFieldTemplates.Fields.type] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.type].Value : "";
                heroCarouselData.Title = datasource.Fields[Templates.BaseFieldTemplates.Fields.title] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.title].Value : "";
                heroCarouselData.BackgroundColor = datasource.Fields[Templates.BaseFieldTemplates.Fields.backgroundColor] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.backgroundColor].Value : "";
                MultilistField galleryMultilistField = datasource.Fields[Templates.QuickInfo.Fields.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        QuickInfoWidgetItem HeroCarouselwidgetItemsData = new QuickInfoWidgetItem();
                        HeroCarouselwidgetItemsData.Id = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.id].Value;
                        HeroCarouselwidgetItemsData.Title = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.title].Value;
                        HeroCarouselwidgetItemsData.SubTitle = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.subTitle].Value;
                        HeroCarouselwidgetItemsData.Imagesrc = Utils.GetImageURLByFieldId(galleryItem, Templates.BaseFieldTemplates.Fields.imageSrc); ;
                        widgetItemsData.Add(HeroCarouselwidgetItemsData);
                    }
                }
                heroCarouselData.widgetItems = widgetItemsData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return heroCarouselData;
        }

        public QuickLinkModel GetQuickLinkModel(Rendering rendering)
        {
            QuickLinkModel quickLinkModel = new QuickLinkModel();
            List<QuickLinkWidgetItem> widgetItemsData = new List<QuickLinkWidgetItem>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetQuickInfoModel : Datasource is empty", this);
                return quickLinkModel;
            }
            try
            {
                quickLinkModel.Id = datasource.Fields[Templates.BaseFieldTemplates.Fields.id] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.id].Value : "";
                quickLinkModel.Type = datasource.Fields[Templates.BaseFieldTemplates.Fields.type] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.type].Value : "";
                quickLinkModel.Title = datasource.Fields[Templates.BaseFieldTemplates.Fields.title] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.title].Value : "";
                MultilistField galleryMultilistField = datasource.Fields[Templates.QuickLink.Fields.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        QuickLinkWidgetItem HeroCarouselwidgetItemsData = new QuickLinkWidgetItem();
                        HeroCarouselwidgetItemsData.Id = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.id].Value;
                        HeroCarouselwidgetItemsData.Title = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.title].Value;
                        HeroCarouselwidgetItemsData.SubTitle = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.subTitle].Value;
                        HeroCarouselwidgetItemsData.Imagesrc = Utils.GetImageURLByFieldId(galleryItem, Templates.BaseFieldTemplates.Fields.imageSrc);
                        HeroCarouselwidgetItemsData.CtaText = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.ctaText].Value;
                        HeroCarouselwidgetItemsData.CtaLink = Utils.GetLinkURL(galleryItem.Fields[Templates.BaseFieldTemplates.Fields.ctaLink]);
                        widgetItemsData.Add(HeroCarouselwidgetItemsData);
                    }
                }
                quickLinkModel.widgetItems = widgetItemsData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return quickLinkModel;
        }

        public WhySearchWithUsModel GetWhySearchWithUsModel(Rendering rendering)
        {
            WhySearchWithUsModel heroCarouselData = new WhySearchWithUsModel();
            List<WhySearchWithUsWidgetItem> widgetItemsData = new List<WhySearchWithUsWidgetItem>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetWhySearchWithUsModel : Datasource is empty", this);
                return heroCarouselData;
            }
            try
            {
                heroCarouselData.Id = datasource.Fields[Templates.BaseFieldTemplates.Fields.id] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.id].Value : "";
                heroCarouselData.Type = datasource.Fields[Templates.BaseFieldTemplates.Fields.type] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.type].Value : "";
                heroCarouselData.Title = datasource.Fields[Templates.BaseFieldTemplates.Fields.title] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.title].Value : "";
                MultilistField galleryMultilistField = datasource.Fields[Templates.WhySearchWithUs.Fields.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        WhySearchWithUsWidgetItem HeroCarouselwidgetItemsData = new WhySearchWithUsWidgetItem();
                        HeroCarouselwidgetItemsData.Id = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.id].Value;
                        HeroCarouselwidgetItemsData.Title = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.title].Value;
                        HeroCarouselwidgetItemsData.SubTitle = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.subTitle].Value;
                        HeroCarouselwidgetItemsData.Imagesrc = Utils.GetImageURLByFieldId(galleryItem, Templates.BaseFieldTemplates.Fields.imageSrc); ;
                        widgetItemsData.Add(HeroCarouselwidgetItemsData);
                    }
                }
                heroCarouselData.widgetItems = widgetItemsData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return heroCarouselData;
        }

        public EVNearBannerModel GetEVNearBannerModelModel(Rendering rendering)
        {
            EVNearBannerModel eVNearBanner = new EVNearBannerModel();
            List<EVNearBannerWidgetItem> widgetItemsData = new List<EVNearBannerWidgetItem>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetEVNearBannerModelModel : Datasource is empty", this);
                return eVNearBanner;
            }
            try
            {
                eVNearBanner.Id = datasource.Fields[Templates.BaseFieldTemplates.Fields.id] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.id].Value : "";
                MultilistField galleryMultilistField = datasource.Fields[Templates.EVNearBanner.Fields.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        EVNearBannerWidgetItem HeroCarouselwidgetItemsData = new EVNearBannerWidgetItem();
                        HeroCarouselwidgetItemsData.Id = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.id].Value;
                        HeroCarouselwidgetItemsData.Title = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.title].Value;
                        HeroCarouselwidgetItemsData.SubTitle = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.subTitle].Value;
                        HeroCarouselwidgetItemsData.Imagesrc = Utils.GetImageURLByFieldId(galleryItem, Templates.BaseFieldTemplates.Fields.imageSrc); ;
                        widgetItemsData.Add(HeroCarouselwidgetItemsData);
                    }
                }
                eVNearBanner.widgetItems = widgetItemsData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return eVNearBanner;
        }


        public ChargingStationBannerModel GetChargingStationBannerModel(Rendering rendering)
        {
            ChargingStationBannerModel eVNearBanner = new ChargingStationBannerModel();
            List<ChargingStationBannerWidgetItem> widgetItemsData = new List<ChargingStationBannerWidgetItem>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetChargingStationBannerModel : Datasource is empty", this);
                return eVNearBanner;
            }
            try
            {
                MultilistField galleryMultilistField = datasource.Fields[Templates.ChargingStationBanner.Fields.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        ChargingStationBannerWidgetItem HeroCarouselwidgetItemsData = new ChargingStationBannerWidgetItem();
                        HeroCarouselwidgetItemsData.Id = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.id].Value;
                        HeroCarouselwidgetItemsData.Title = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.title].Value;
                        HeroCarouselwidgetItemsData.CtaLink = Utils.GetLinkURL(galleryItem.Fields[Templates.BaseFieldTemplates.Fields.ctaLink]); 
                        HeroCarouselwidgetItemsData.CtaText = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.ctaText].Value;
                        HeroCarouselwidgetItemsData.Imagesrc = Utils.GetImageURLByFieldId(galleryItem, Templates.BaseFieldTemplates.Fields.imageSrc); ;
                        widgetItemsData.Add(HeroCarouselwidgetItemsData);
                    }
                }
                eVNearBanner.widgetItems = widgetItemsData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return eVNearBanner;
        }

        public FaqModel GetFaqModel(Rendering rendering)
        {
            FaqModel faqModel = new FaqModel();
            List<FaqWidgetItems> widgetItemsData = new List<FaqWidgetItems>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetFaqModel : Datasource is empty", this);
                return faqModel;
            }
            try
            {
                faqModel.Id = datasource.Fields[Templates.BaseFieldTemplates.Fields.id] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.id].Value : "";
                faqModel.Type = datasource.Fields[Templates.BaseFieldTemplates.Fields.type] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.type].Value : "";
                faqModel.Title = datasource.Fields[Templates.BaseFieldTemplates.Fields.title] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.title].Value : "";
                faqModel.CtaText = datasource.Fields[Templates.BaseFieldTemplates.Fields.ctaText] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.ctaText].Value : "";
                faqModel.CtaLink = Utils.GetLinkURL(datasource.Fields[Templates.BaseFieldTemplates.Fields.ctaLink]);
                MultilistField galleryMultilistField = datasource.Fields[Templates.FAQ.Fields.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        FaqWidgetItems HeroCarouselwidgetItemsData = new FaqWidgetItems();
                        HeroCarouselwidgetItemsData.id = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.id].Value;
                        HeroCarouselwidgetItemsData.title = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.title].Value;
                        HeroCarouselwidgetItemsData.subTitle = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.subTitle].Value;
                        widgetItemsData.Add(HeroCarouselwidgetItemsData);
                    }
                }
                faqModel.widgetItems = widgetItemsData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return faqModel;
        }

        public ContactusInfoModel GetContactusInfoModel(Rendering rendering)
        {
            ContactusInfoModel latestEVNews = new ContactusInfoModel();
            List<ContactusInfoItemModel> widgetItemsData = new List<ContactusInfoItemModel>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetContactusInfoModel : Datasource is empty", this);
                return latestEVNews;
            }
            try
            {
                latestEVNews.Id = datasource.Fields[Templates.BaseFieldTemplates.Fields.id] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.id].Value : "";
                latestEVNews.Type = datasource.Fields[Templates.BaseFieldTemplates.Fields.type] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.type].Value : "";
                latestEVNews.Title = datasource.Fields[Templates.BaseFieldTemplates.Fields.title] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.title].Value : "";
                latestEVNews.CtaText = datasource.Fields[Templates.BaseFieldTemplates.Fields.ctaText] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.ctaText].Value : "";
                latestEVNews.CtaLink = Utils.GetLinkURL(datasource.Fields[Templates.BaseFieldTemplates.Fields.ctaLink]);
                MultilistField galleryMultilistField = datasource.Fields[Templates.ContactusInfo.Fields.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        ContactusInfoItemModel  contactusInfoItem = new ContactusInfoItemModel();
                        contactusInfoItem.Id = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.id].Value;
                        contactusInfoItem.Title = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.title].Value;
                        contactusInfoItem.SubTitle = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.subTitle].Value;
                        contactusInfoItem.Imagesrc = Utils.GetImageURLByFieldId(galleryItem, Templates.BaseFieldTemplates.Fields.imageSrc); ;
                        widgetItemsData.Add(contactusInfoItem);
                    }
                }
                latestEVNews.widgetItems = widgetItemsData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return latestEVNews;
        }
    }
}