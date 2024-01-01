using Project.AdaniOneSEO.Website.Helpers;
using Project.AdaniOneSEO.Website.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;

namespace Project.AdaniOneSEO.Website.Services
{
    public class BannerWidget : IBannerWidget
    {
        public BannerWidgetModel GetBannerWidget(Rendering rendering)
        {
            BannerWidgetModel BannerWidgetDataModel = null;

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                BannerWidgetDataModel = new BannerWidgetModel();


                widget widgetobj = new widget();
                widgetobj.widgetId = Utils.GetIntValue(datasource, BaseTemplates.BannerWidgetTemplate.widgetId);
                widgetobj.widgetType = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.widgetType);
                widgetobj.title = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.title);
                widgetobj.subTitle = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.subTitle);
                widgetobj.subItemRadius = Utils.GetIntValue(datasource, BaseTemplates.BannerWidgetTemplate.subItemRadius);
                widgetobj.subItemWidth = Utils.GetIntValue(datasource, BaseTemplates.BannerWidgetTemplate.subItemWidth);
                widgetobj.gridColumn = Utils.GetIntValue(datasource, BaseTemplates.BannerWidgetTemplate.gridColumn);
                widgetobj.aspectRatio = Utils.GetIntValue(datasource, BaseTemplates.BannerWidgetTemplate.aspectRatio);
                widgetobj.borderRadius = Utils.GetIntValue(datasource, BaseTemplates.BannerWidgetTemplate.borderRadius);
                widgetobj.backgroundColor = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.backgroundColor);
                //widgetobj.itemMargin = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.itemMargin);
                //widgetobj.subItemMargin = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.subItemMargin);
                //widgetobj.actionTitle = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.actionTitle);
                //widgetobj.carouselParam = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.carouselParam);
                //widgetobj.tabConfiguration = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.GalleryMobileBanner);
                //widgetobj.gradientConfiguration = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.gradientConfiguration);
                //widgetobj.gridConfiguration = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.gridConfiguration);
                //widgetobj.subItemColors = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.subItemColors);

                if (datasource.HasChildren)
                {
                    List<widgetItems> widgetItems = new List<widgetItems>();
                    foreach (Item child in datasource.Children)
                    {
                        widgetItems widgetItemsobj = new widgetItems();
                        widgetItemsobj.title = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.Title);
                        widgetItemsobj.description = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.Description);
                        widgetItemsobj.autoid = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.AutoId);
                        widgetItemsobj.imagesrc = Utils.GetImageURLByFieldId(child, BaseTemplates.BannerWidgetTemplate.Image);
                        widgetItemsobj.bannerlogo = Utils.GetImageURLByFieldId(child, BaseTemplates.BannerWidgetTemplate.BannerLogo);
                        widgetItemsobj.subtitle = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.SubTitle);
                        widgetItemsobj.uniqueid = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.UniqueId);
                        widgetItemsobj.mobileimage = Utils.GetImageURLByFieldId(child, BaseTemplates.BannerWidgetTemplate.MobileImage);
                        widgetItemsobj.btnText = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.btnText);
                        widgetItemsobj.isAirportSelectNeeded = Utils.GetCheckBoxSelection(child.Fields["isAirportSelectNeeded"]);
                        widgetItemsobj.link = Utils.GetLinkURL(child, BaseTemplates.BannerWidgetTemplate.Link.ToString());
                        widgetItemsobj.linkTarget = Utils.GetLinkURLTarget(child, BaseTemplates.BannerWidgetTemplate.Link.ToString());
                        widgetItemsobj.isAgePopup = Utils.GetCheckBoxSelection(child.Fields["isAgePopup"]);
                        widgetItemsobj.offerEligibility = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.OfferEligibility);
                        widgetItemsobj.cardBgColor = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.cardBgColor);
                        widgetItemsobj.gridNumber = Utils.GetIntValue(child, BaseTemplates.BannerWidgetTemplate.gridNumber);
                        widgetItemsobj.listClass = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.listClass);
                        widgetItemsobj.checkValidity = Utils.GetCheckBoxSelection(child.Fields["checkValidity"]);
                        widgetItemsobj.effectiveFrom = Utils.GetDate(child, BaseTemplates.BannerWidgetTemplate.EffectiveFrom);
                        widgetItemsobj.effectiveTo = Utils.GetDate(child, BaseTemplates.BannerWidgetTemplate.EffectiveTo);

                        tags tagssobj = new tags();
                        tagssobj.bannerCategory = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.BannerCategory);
                        tagssobj.businessUnit = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.BusinessUnit);
                        tagssobj.category = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.Category);
                        tagssobj.faqCategory = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.FaqCategory);
                        tagssobj.label = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.Label);
                        tagssobj.source = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.Source);
                        tagssobj.subCategory = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.SubCategory);
                        tagssobj.type = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.Type);
                        tagssobj.eventName = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.Event);
                        widgetItemsobj.tags = tagssobj;
                        widgetItems.Add(widgetItemsobj);
                    }
                    widgetobj.widgetItems = widgetItems;
                }

                BannerWidgetDataModel.widget = widgetobj;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return BannerWidgetDataModel;
        }
    }
}