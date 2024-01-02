using Project.AdaniInternationalSchool.Website.AdmissionPage;
using Project.AdaniInternationalSchool.Website.Helpers;
using Project.AdaniInternationalSchool.Website.Models;
using Project.AdaniInternationalSchool.Website.Templates;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Extensions;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using static Azure.Core.HttpHeader;
using static Project.AdaniInternationalSchool.Website.Templates.CardsTemplate.Fields;

namespace Project.AdaniInternationalSchool.Website.Services.AdmissionPage
{
    public class AISAdmissionPageServices : IAISAdmissionPageServices
    {

        public AdmissionOverview GetAdmissionOverview(Rendering rendering)
        {
            var overviewData = new AdmissionOverview();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                overviewData.Description = Utils.GetValue(datasource, BaseTemplates.DescriptionTemplate.DescriptionFieldId);
                overviewData.Heading = Utils.GetValue(datasource, BaseTemplates.HeadingTemplate.HeadingFieldId);
                overviewData.SectionID = Utils.GetValue(datasource, BaseTemplates.SectionIdTemplate.SectionIdFieldId);
                overviewData.LinkText = Utils.GetValue(datasource, BaseTemplates.LinkTemplate.LinkTextFieldId);
                overviewData.Link = Utils.GetLinkURL(datasource.Fields[BaseTemplates.LinkTemplate.LinkFieldId]);
                overviewData.BackgroundImage = Utils.GetImageURLByFieldId(datasource, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                overviewData.ImageAlt = Utils.GetValue(datasource, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);
                overviewData.GtmData = new GtmDataModel();
                overviewData.GtmData.Event = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmEventFieldId);
                overviewData.GtmData.Category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmCategoryFieldId);
                overviewData.GtmData.Sub_category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId);
                overviewData.GtmData.Banner_category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmBannerCategoryFieldId);
                overviewData.GtmData.Index = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmIndexFieldId);
                overviewData.GtmData.Label = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmLabelFieldId);
                overviewData.GtmData.Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item);

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return overviewData;
        }

        public AdmissionUpdatesModel GetAdmissionupdates(Rendering rendering)
        {
            AdmissionUpdatesModel overviewData = new AdmissionUpdatesModel();
            List<DescriptionModel> widgetItemsListData = new List<DescriptionModel>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                overviewData.Theme = Utils.GetValue(datasource, BaseTemplates.ThemeTemplate.ThemeFieldId);
                overviewData.Heading = Utils.GetValue(datasource, BaseTemplates.HeadingTemplate.HeadingFieldId);


                foreach (Item galleryItem in datasource.Children)
                {
                    DescriptionModel ItemsListData = new DescriptionModel
                    {
                        Description = galleryItem.Fields[BaseTemplates.DescriptionTemplate.DescriptionFieldId].Value
                    };
                    widgetItemsListData.Add(ItemsListData);
                }
                overviewData.Data = widgetItemsListData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return overviewData;
        }

        public AdmissionCardsRoot GetAdmissionCards(Rendering rendering)
        {
            AdmissionCardsRoot admissionCardsRootData = new AdmissionCardsRoot();
            var admissionCardsData = new SectionCards<AdmissionCardItemModel>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                admissionCardsData.Variant = Utils.GetValue(datasource, BaseTemplates.VariantTemplate.VariantFieldId);
                admissionCardsData.SectionID = Utils.GetValue(datasource, BaseTemplates.SectionIdTemplate.SectionIdFieldId);

                if (datasource.Children.Count > 0)
                {
                    foreach (Item galleryItem in datasource.Children)
                    {
                        var ItemsListData = new AdmissionCardItemModel
                        {
                            Theme = galleryItem.Fields[BaseTemplates.ThemeTemplate.ThemeFieldId].Value,
                            TextFirst = Utils.GetBoleanValue(galleryItem, BaseTemplates.TextFirstTemplate.TextFirstFieldId),
                            CardType = galleryItem.Fields[BaseTemplates.CardTypeTemplate.CardTypeFieldId].Value,
                            MediaType = galleryItem.Fields[BaseTemplates.MediaTypeTemplate.MediaTypeFieldId].Value,
                            SubHeading = Utils.GetValue(galleryItem, BaseTemplates.SubHeadingTemplate.SubHeadingFieldId),
                            Heading = Utils.GetValue(galleryItem, BaseTemplates.HeadingTemplate.HeadingFieldId),
                            SubDescription = galleryItem.Fields[BaseTemplates.DescriptionTemplate.SubDescriptionFieldId].Value,
                            Link = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),
                            LinkText = Utils.GetValue(galleryItem, BaseTemplates.LinkTemplate.LinkTextFieldId),
                            ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId),
                            SubLink = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]),
                            SubLinkText = Utils.GetValue(galleryItem, BaseTemplates.CtaTemplate.CtaTextFieldId),
                            ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                            ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                            ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId),
                        };
                        ItemsListData.GtmData = new GtmDataModel
                        {
                            Event = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                            Category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                            Banner_category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmBannerCategoryFieldId),
                            Label = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                            Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };
                        admissionCardsData.Data.Add(ItemsListData);
                    }

                }
                admissionCardsRootData.AdmissionCards = admissionCardsData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return admissionCardsRootData;
        }

        public Root GetCards(Rendering rendering)
        {
            Root root = new Root();
            root.Cards = new List<SectionCards<CardsData>>();
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                foreach (Item item in datasource.Children)
                {
                    var cards = new SectionCards<CardsData>();
                    cards.SectionID = Utils.GetValue(item, BaseTemplates.SectionIdTemplate.SectionIdFieldId);
                    cards.Variant = Utils.GetValue(item, BaseTemplates.VariantTemplate.VariantFieldId);
                    cards.Data = new List<CardsData>();

                    foreach (Item galleryItem in item.Children)
                    {
                        CardsData cardsData = new CardsData();
                        cardsData.Theme = galleryItem.Fields[BaseTemplates.ThemeTemplate.ThemeFieldId].Value;
                        cardsData.TextFirst = Utils.GetBoleanValue(galleryItem, BaseTemplates.TextFirstTemplate.TextFirstFieldId);
                        cardsData.CardType = galleryItem.Fields[BaseTemplates.CardTypeTemplate.CardTypeFieldId].Value;
                        cardsData.MediaType = galleryItem.Fields[BaseTemplates.MediaTypeTemplate.MediaTypeFieldId].Value;
                        cardsData.SubHeading = Utils.GetValue(galleryItem, BaseTemplates.SubHeadingTemplate.SubHeadingFieldId);
                        cardsData.Heading = Utils.GetValue(galleryItem, BaseTemplates.HeadingTemplate.HeadingFieldId);
                        cardsData.Description = galleryItem.Fields[BaseTemplates.DescriptionTemplate.DescriptionFieldId].Value;
                        cardsData.Link = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]);
                        cardsData.LinkText = Utils.GetValue(galleryItem, BaseTemplates.CtaTemplate.CtaTextFieldId);
                        cardsData.ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                        cardsData.ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
                        cardsData.ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);
                        cardsData.VideoSource = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceFieldId]);
                        cardsData.VideoSourceMobile = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceMobileFieldId]);
                        cardsData.VideoSourceTablet = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceTabletFieldId]);
                        cardsData.MapSource = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.MapSourceTemplate.MapSourceFieldId]);
                        cardsData.ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);
                        cardsData.GtmData = new GtmDataModel();
                        cardsData.GtmData.Event = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmEventFieldId);
                        cardsData.GtmData.Category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId);
                        cardsData.GtmData.Banner_category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmBannerCategoryFieldId);
                        cardsData.GtmData.Label = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmLabelFieldId);
                        cardsData.GtmData.Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item);
                        cardsData.ListItem = new List<CardslistItemStudentTimings>();


                        foreach (Item galleryItems in galleryItem.Children)
                        {
                            CardslistItemStudentTimings Studentinfo = new CardslistItemStudentTimings();
                            Studentinfo.Heading = Utils.GetValue(galleryItems, BaseTemplates.HeadingTemplate.HeadingFieldId);
                            Studentinfo.SubHeading = Utils.GetValue(galleryItems, BaseTemplates.SubHeadingTemplate.SubHeadingFieldId);
                            Studentinfo.Item = new List<CardsDataitem>();

                            foreach (Item studentTimings in galleryItems.Children)
                            {
                                CardsDataitem studentTiming = new CardsDataitem();
                                studentTiming.Description = Utils.GetValue(studentTimings, BaseTemplates.TitleTemplate.TitleFieldId);
                                studentTiming.Timing = Utils.GetValue(studentTimings, BaseTemplates.TitleTemplate.SubTitleFieldID);
                                Studentinfo.Item.Add(studentTiming);
                            }
                            cardsData.ListItem.Add(Studentinfo);
                        }
                        cards.Data.Add(cardsData);
                    }
                    root.Cards.Add(cards);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return root;
        }

        public SeoData GetSeoData(Rendering rendering)
        {
            SeoData seoData = new SeoData();
            SeoDataorgSchemaModel seoDataorgSchemaModelData = new SeoDataorgSchemaModel();
            List<string> SeoDataorgSchemadata = new List<string>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {

                seoData.PageTitle = Utils.GetValue(datasource, BaseTemplates.TitleTemplate.TitleFieldId);
                seoData.MetaDescription = Utils.GetValue(datasource, BaseTemplates.DescriptionTemplate.DescriptionFieldId);
                seoData.MetaKeywords = Utils.GetValue(datasource, BaseTemplates.TitleTemplate.SubTitleFieldID);
                seoData.MetaTitle = Utils.GetValue(datasource, SeoDataTemplate.Fields.heading);
                seoData.OgTitle = Utils.GetValue(datasource, SeoDataTemplate.Fields.ogTitle);
                seoData.RobotsTags = Utils.GetValue(datasource, SeoDataTemplate.Fields.robotsTags);
                seoData.BrowserTitle = Utils.GetValue(datasource, SeoDataTemplate.Fields.browserTitle);
                seoData.OgImage = Utils.Settings("domain") + Utils.GetImageURLByFieldId(datasource, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                seoData.OgDescription = Utils.GetValue(datasource, SeoDataTemplate.Fields.ogDescription);
                seoData.OgKeyword = Utils.GetValue(datasource, BaseTemplates.CtaTemplate.CtaTextFieldId);
                seoData.CanonicalUrl = Utils.GetLinkURL(datasource.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]);
                seoData.GoogleSiteVerification = Utils.GetValue(datasource, SeoDataTemplate.Fields.googleSiteVerification);
                seoData.MsValidate = Utils.GetValue(datasource, SeoDataTemplate.Fields.msValidate);


                foreach (Item galleryItem in datasource.Children)
                {
                    SeoDataorgSchemaModel ItemsListData = new SeoDataorgSchemaModel();

                    List<CardslistItemStudentTimings> CardslistItemStudentTimings = new List<CardslistItemStudentTimings>();

                    ItemsListData.Telephone = Utils.GetValue(galleryItem, BaseTemplates.TitleTemplate.TitleFieldId);
                    ItemsListData.ContactType = Utils.GetValue(galleryItem, BaseTemplates.TitleTemplate.SubTitleFieldID);
                    ItemsListData.AreaServed = galleryItem.Fields[BaseTemplates.DescriptionTemplate.DescriptionFieldId].Value;
                    ItemsListData.StreetAddress = Utils.GetValue(galleryItem, BaseTemplates.HeadingTemplate.HeadingFieldId);
                    ItemsListData.AddressLocality = galleryItem.Fields[SeoDataTemplate.Fields.SeoDataOrgSchema.addressLocality].Value;
                    ItemsListData.AddressRegion = galleryItem.Fields[SeoDataTemplate.Fields.SeoDataOrgSchema.addressRegion].Value;
                    ItemsListData.PostalCode = galleryItem.Fields[SeoDataTemplate.Fields.SeoDataOrgSchema.postalCode].Value;
                    ItemsListData.ContactOption = galleryItem.Fields[SeoDataTemplate.Fields.SeoDataOrgSchema.contactOption].Value;
                    ItemsListData.Logo = Utils.GetImageURLByFieldId(galleryItem, SeoDataTemplate.Fields.SeoDataOrgSchema.logo);
                    ItemsListData.Url = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]);

                    List<string> sameAsLinkList = new List<string>();
                    foreach (Item galleryItems in galleryItem.Children)
                    {
                        var sameAsLink = Utils.GetLinkURL(galleryItems.Fields[BaseTemplates.LinkTemplate.LinkFieldId]);
                        sameAsLinkList.Add(sameAsLink);
                    }
                    ItemsListData.sameAs = sameAsLinkList;
                    seoDataorgSchemaModelData = ItemsListData;
                }
                seoData.orgSchema = seoDataorgSchemaModelData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return seoData;
        }

    }
}