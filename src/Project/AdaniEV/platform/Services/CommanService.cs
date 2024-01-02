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
    public class CommanService : ICommanService
    {


        public FooterBannerModel GetFooterBannerModel(Rendering rendering)
        {

            FooterBannerModel footerBanner = new FooterBannerModel();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetFooterBannerModel : Datasource is empty", this);
                return footerBanner;
            }
            try
            {
                footerBanner.Id = datasource.Fields[Templates.BaseFieldTemplates.Fields.id] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.id].Value : "";
                footerBanner.Title = datasource.Fields[Templates.BaseFieldTemplates.Fields.title] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.title].Value : "";
                footerBanner.Type = datasource.Fields[Templates.BaseFieldTemplates.Fields.type] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.type].Value : "";
                footerBanner.CtaText = datasource.Fields[Templates.BaseFieldTemplates.Fields.ctaText] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.ctaText].Value : "";
                footerBanner.CtaLink = Utils.GetLinkURL(datasource.Fields[Templates.BaseFieldTemplates.Fields.ctaLink]);
                footerBanner.Imagesrc = Utils.GetImageURLByFieldId(datasource, Templates.BaseFieldTemplates.Fields.imageSrc);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return footerBanner;
        }

        public FooterNavigationModel GetFooterNavigationModel(Rendering rendering)
        {
            FooterNavigationModel footerBaseModel = new FooterNavigationModel();
            List<FooterNavigationWidgetModel> footerModels = new List<FooterNavigationWidgetModel>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetFooterNavigationModel : Datasource is empty", this);
                return footerBaseModel;
            }
            try
            {
                footerBaseModel.Id = datasource.Fields[Templates.BaseFieldTemplates.Fields.id] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.id].Value : "";
                footerBaseModel.Title = datasource.Fields[Templates.BaseFieldTemplates.Fields.title] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.title].Value : "";
                footerBaseModel.Type = datasource.Fields[Templates.BaseFieldTemplates.Fields.type] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.type].Value : "";
                footerBaseModel.CtaText = datasource.Fields[Templates.BaseFieldTemplates.Fields.ctaText] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.ctaText].Value : "";
                footerBaseModel.CtaLink = Utils.GetLinkURL(datasource.Fields[Templates.BaseFieldTemplates.Fields.ctaLink]);
                footerBaseModel.Imagesrc = Utils.GetImageURLByFieldId(datasource, Templates.BaseFieldTemplates.Fields.imageSrc);


                MultilistField galleryMultilistField = datasource.Fields[Templates.FooterNavigation.Fields.widgetItems];
                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        FooterNavigationWidgetModel ItemsData = new FooterNavigationWidgetModel();
                        ItemsData.Id = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.id] != null ? galleryItem.Fields[Templates.BaseFieldTemplates.Fields.id].Value : "";
                        ItemsData.Title = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.title] != null ? galleryItem.Fields[Templates.BaseFieldTemplates.Fields.title].Value : "";
                        ItemsData.SubTitle = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.subTitle] != null ? galleryItem.Fields[Templates.BaseFieldTemplates.Fields.subTitle].Value : "";
                        ItemsData.Imagesrc = Utils.GetImageURLByFieldId(galleryItem, Templates.BaseFieldTemplates.Fields.imageSrc);
                        MultilistField footeritemlist = galleryItem.Fields[Templates.FooterNavigationWidgetItem.Fields.widgetItems];
                        if (footeritemlist.Count > 0)
                        {
                            foreach (Item objitem in footeritemlist.GetItems())
                            {
                                FooterNavigationWidgetItemModel footerItems = new FooterNavigationWidgetItemModel();
                                footerItems.Id = objitem.Fields[Templates.BaseFieldTemplates.Fields.id] != null ? objitem.Fields[Templates.BaseFieldTemplates.Fields.id].Value : "";
                                footerItems.CtaText = objitem.Fields[Templates.BaseFieldTemplates.Fields.ctaText] != null ? objitem.Fields[Templates.BaseFieldTemplates.Fields.ctaText].Value : "";
                                footerItems.CtaLink = Utils.GetLinkURL(objitem.Fields[Templates.BaseFieldTemplates.Fields.ctaLink]);
                                ItemsData.items.Add(footerItems);
                            }
                        }
                        footerModels.Add(ItemsData);
                    }
                }
                footerBaseModel.widgetItems = footerModels;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                //throw ex;
            }
            return footerBaseModel;
        }

        public HeaderNavBarModel GetHeaderNavBarModel(Rendering rendering)
        {
            HeaderNavBarModel headerNavBar = new HeaderNavBarModel();
            List<HeaderNavBarWidgetItem> widgetItemsData = new List<HeaderNavBarWidgetItem>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetHeroCarousel : Datasource is empty", this);
                return headerNavBar;
            }
            try
            {
                headerNavBar.Id = datasource.Fields[Templates.BaseFieldTemplates.Fields.id] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.id].Value : "";
                MultilistField galleryMultilistField = datasource.Fields[Templates.NavBar.Fields.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        HeaderNavBarWidgetItem HeroCarouselwidgetItemsData = new HeaderNavBarWidgetItem();
                        HeroCarouselwidgetItemsData.Id = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.id].Value;
                        HeroCarouselwidgetItemsData.Name = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.ctaText].Value;
                        HeroCarouselwidgetItemsData.CtaLink = Utils.GetLinkURL(galleryItem.Fields[Templates.BaseFieldTemplates.Fields.ctaLink]);
                        widgetItemsData.Add(HeroCarouselwidgetItemsData);
                    }
                }
                headerNavBar.widgetItems = widgetItemsData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return headerNavBar;
        }

        public SocialMediaLinksModel GetSocialMediaLinksModel(Rendering rendering)
        {
            SocialMediaLinksModel socialMediaLinks = new SocialMediaLinksModel();
            List<SocialMediaLinksWidgetItem> widgetItemsData = new List<SocialMediaLinksWidgetItem>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("SocialMediaLinks : Datasource is empty", this);
                return socialMediaLinks;
            }
            try
            {
                socialMediaLinks.Id = datasource.Fields[Templates.BaseFieldTemplates.Fields.id] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.id].Value : "";
                MultilistField galleryMultilistField = datasource.Fields[Templates.SocialMediaLinks.Fields.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        SocialMediaLinksWidgetItem socialMediaLinksWidget = new SocialMediaLinksWidgetItem();
                        socialMediaLinksWidget.Id = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.id].Value;
                        socialMediaLinksWidget.Imagesrc = Utils.GetImageURLByFieldId(galleryItem, Templates.BaseFieldTemplates.Fields.imageSrc);
                        socialMediaLinksWidget.CtaText = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.ctaText].Value;
                        socialMediaLinksWidget.CtaLink = Utils.GetLinkURL(galleryItem.Fields[Templates.BaseFieldTemplates.Fields.ctaLink]);
                        widgetItemsData.Add(socialMediaLinksWidget);
                    }
                }
                socialMediaLinks.widgetItems = widgetItemsData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return socialMediaLinks;
        }

        public CopyrightModel GetCopyrightModel(Rendering rendering)
        {
            CopyrightModel copyright = new CopyrightModel();
            List<CopyrightWidgetItem> widgetItemsData = new List<CopyrightWidgetItem>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetCopyrightModel : Datasource is empty", this);
                return copyright;
            }
            try
            {
                copyright.Id = datasource.Fields[Templates.BaseFieldTemplates.Fields.id] != null ? datasource.Fields[Templates.BaseFieldTemplates.Fields.id].Value : "";
                MultilistField galleryMultilistField = datasource.Fields[Templates.Copyright.Fields.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        CopyrightWidgetItem socialMediaLinksWidget = new CopyrightWidgetItem();
                        socialMediaLinksWidget.Id = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.id].Value;
                        socialMediaLinksWidget.Title = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.title].Value; ;
                        socialMediaLinksWidget.CtaText = galleryItem.Fields[Templates.BaseFieldTemplates.Fields.ctaText].Value;
                        socialMediaLinksWidget.CtaLink = Utils.GetLinkURL(galleryItem.Fields[Templates.BaseFieldTemplates.Fields.ctaLink]);
                        widgetItemsData.Add(socialMediaLinksWidget);
                    }
                }
                copyright.widgetItems = widgetItemsData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return copyright;
        }

        public LanguageModel GetLanguageModel(Rendering rendering)
        {
            LanguageModel languageModel = new LanguageModel();
            List<LanguageWidgetModel> widgetItemsData = new List<LanguageWidgetModel>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetCopyrightModel : Datasource is empty", this);
                return languageModel;
            }
            try
            {
                LanguageWidgetModel socialMediaLinksWidget = new LanguageWidgetModel();
                socialMediaLinksWidget.Id = datasource.Fields[Templates.BaseFieldTemplates.Fields.id].Value;
                socialMediaLinksWidget.Imagesrc = Utils.GetImageURLByFieldId(datasource, Templates.BaseFieldTemplates.Fields.imageSrc);
                socialMediaLinksWidget.Title = datasource.Fields[Templates.BaseFieldTemplates.Fields.title].Value; ;
                socialMediaLinksWidget.CtaText = datasource.Fields[Templates.BaseFieldTemplates.Fields.ctaText].Value;
                socialMediaLinksWidget.CtaLink = Utils.GetLinkURL(datasource.Fields[Templates.BaseFieldTemplates.Fields.ctaLink]);
                widgetItemsData.Add(socialMediaLinksWidget);

                languageModel.widgetItems = widgetItemsData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return languageModel;
        }
    }
}