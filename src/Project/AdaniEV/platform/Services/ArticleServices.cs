using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Adani.EV.Project.Services;
using Sitecore.Data.Fields;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Adani.EV.Project.Helper;
using static Adani.EV.Project.Templates.NavbarArtical;
using Adani.EV.Project.Models;
using Sitecore.Shell.Applications.ContentEditor;
using DateTime = System.DateTime;
using Sitecore.Data.DataProviders.ReadOnly.Protobuf.Data;
using System.Web.DynamicData;

namespace Adani.EV.Project.Services
{
    public class ArticleServices : IArticleServices
    {

        public ArticleBannerCarousel GetBannerCarousel(Rendering rendering)
        {
            ArticleBannerCarousel aboutCareerData = new ArticleBannerCarousel();
            List<widgetItems> widgetItemsData = new List<widgetItems>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetBannerCarousel : Datasource is empty", this);
                return aboutCareerData;
            }
            try
            {
                aboutCareerData.id = datasource.Fields[Templates.NavbarArtical.Feilds.id] != null ? datasource.Fields[Templates.NavbarArtical.Feilds.id].Value : "";
                aboutCareerData.type = datasource.Fields[Templates.NavbarArtical.Feilds.type] != null ? datasource.Fields[Templates.NavbarArtical.Feilds.type].Value : "";
                var multilist = Utils.GetMultiListValueItem(datasource, Templates.NavbarArtical.Feilds.widgetItems);
                MultilistField galleryMultilistField = datasource.Fields[Templates.NavbarArtical.Feilds.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        widgetItems ItemsData = new widgetItems();
                        ItemsData.id = galleryItem.Fields[Templates.NavbarArtical.Feilds.widgetItem.id].Value;
                        ItemsData.imageSrc = Utils.GetImageURLByFieldId(galleryItem, Templates.NavbarArtical.Feilds.widgetItem.imageSrc);
                        ItemsData.title = galleryItem.Fields[Templates.NavbarArtical.Feilds.widgetItem.title].Value;
                        ItemsData.subTitle = galleryItem.Fields[Templates.NavbarArtical.Feilds.widgetItem.subTitle].Value;
                        ItemsData.ctaText = galleryItem.Fields[Templates.NavbarArtical.Feilds.widgetItem.ctaText].Value;
                        ItemsData.ctaLink = Utils.GetLinkURL(galleryItem?.Fields[Templates.NavbarArtical.Feilds.widgetItem.ctaLink]);
                        widgetItemsData.Add(ItemsData);
                    }
                }
                aboutCareerData.widgetItems = widgetItemsData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                //throw ex;
            }
            return aboutCareerData;
        }

        public ArticleFeaturedFilters GetArticleFeaturedFilters(Rendering rendering)
        {
            ArticleFeaturedFilters articleFeaturedFiltersData = new ArticleFeaturedFilters();
            List<widgetItemsList> widgetItemsListData = new List<widgetItemsList>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetArticleFeaturedFilters : Datasource is empty", this);
                return articleFeaturedFiltersData;

                // throw new NullReferenceException();
            }
            try
            {
                articleFeaturedFiltersData.id = datasource.Fields[Templates.ArticleFeaturedFilters.Fields.id] != null ? datasource.Fields[Templates.ArticleFeaturedFilters.Fields.id].Value : "";
                articleFeaturedFiltersData.type = datasource.Fields[Templates.ArticleFeaturedFilters.Fields.type] != null ? datasource.Fields[Templates.ArticleFeaturedFilters.Fields.type].Value : "";
                var multilist = Utils.GetMultiListValueItem(datasource, Templates.ArticleFeaturedFilters.Fields.widgetItems);
                MultilistField galleryMultilistField = datasource.Fields[Templates.ArticleFeaturedFilters.Fields.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        widgetItemsList ItemsListData = new widgetItemsList();
                        ItemsListData.id = galleryItem.Fields[Templates.ArticleFeaturedFilters.Fields.widgetItem.id].Value;
                        ItemsListData.ctaText = galleryItem.Fields[Templates.ArticleFeaturedFilters.Fields.widgetItem.ctaText].Value;
                        ItemsListData.ctaLink = Utils.GetLinkURL(galleryItem?.Fields[Templates.ArticleFeaturedFilters.Fields.widgetItem.ctaLink]);
                        widgetItemsListData.Add(ItemsListData);
                    }
                }
                articleFeaturedFiltersData.widgetItems = widgetItemsListData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return articleFeaturedFiltersData;
        }

        public ArticleVideoGalleryCarousel GetArticleVideoGalleryCarousel(Rendering rendering)
        {
            ArticleVideoGalleryCarousel articleVideoGalleryFiltersData = new ArticleVideoGalleryCarousel();
            List<ArticleVideoGalleryList> articleVideoGalleryListData = new List<ArticleVideoGalleryList>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetArticleVideoGalleryCarousel : Datasource is empty", this);
                return articleVideoGalleryFiltersData;
                //throw new NullReferenceException();
            }
            try
            {
                articleVideoGalleryFiltersData.id = datasource.Fields[Templates.ArticleVideoGalleryCarousel.Fields.id] != null ? datasource.Fields[Templates.ArticleVideoGalleryCarousel.Fields.id].Value : "";
                articleVideoGalleryFiltersData.type = datasource.Fields[Templates.ArticleVideoGalleryCarousel.Fields.type] != null ? datasource.Fields[Templates.ArticleVideoGalleryCarousel.Fields.type].Value : "";
                var multilist = Utils.GetMultiListValueItem(datasource, Templates.ArticleVideoGalleryCarousel.Fields.widgetItems);
                MultilistField galleryMultilistField = datasource.Fields[Templates.ArticleVideoGalleryCarousel.Fields.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        ArticleVideoGalleryList ItemsListData = new ArticleVideoGalleryList();
                        ItemsListData.id = galleryItem.Fields[Templates.ArticleVideoGalleryCarousel.Fields.widgetItem.id].Value;
                        ItemsListData.title = galleryItem.Fields[Templates.ArticleVideoGalleryCarousel.Fields.widgetItem.title].Value;
                        ItemsListData.imageSrc = Utils.GetImageURLByFieldId(galleryItem, Templates.ArticleVideoGalleryCarousel.Fields.widgetItem.imageSrc);
                        ItemsListData.videoSrc = galleryItem.Fields[Templates.ArticleVideoGalleryCarousel.Fields.widgetItem.videoSrc].Value;
                        articleVideoGalleryListData.Add(ItemsListData);
                    }
                }

                articleVideoGalleryFiltersData.widgetItems = articleVideoGalleryListData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return articleVideoGalleryFiltersData;
        }

        public ArticleFeaturedCardModel ArticleFeaturedCardList(Rendering rendering)
        {
            ArticleFeaturedCardModel articleFeaturedCardModelData = new ArticleFeaturedCardModel();
            List<ArticleFeaturedCardItems> widgetItemsListData = new List<ArticleFeaturedCardItems>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("ArticleFeaturedCardList : Datasource is empty", this);
                return articleFeaturedCardModelData;
                //   throw new NullReferenceException();
            }
            try
            {
                articleFeaturedCardModelData.id = datasource.Fields[Templates.ArticleFeaturedCard.Fields.id] != null ? datasource.Fields[Templates.ArticleFeaturedCard.Fields.id].Value : "";
                articleFeaturedCardModelData.type = datasource.Fields[Templates.ArticleFeaturedCard.Fields.type] != null ? datasource.Fields[Templates.ArticleFeaturedCard.Fields.type].Value : "";
                var multilist = Utils.GetMultiListValueItem(datasource, Templates.ArticleFeaturedCard.Fields.widgetItems);
                MultilistField galleryMultilistField = datasource.Fields[Templates.ArticleFeaturedCard.Fields.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        ArticleFeaturedCardItems ItemsListData = new ArticleFeaturedCardItems();
                        ItemsListData.id = galleryItem.Fields[Templates.ArticleFeaturedCard.Fields.widgetItem.id].Value;
                        ItemsListData.title = galleryItem.Fields[Templates.ArticleFeaturedCard.Fields.widgetItem.title].Value;
                        ItemsListData.ctaText = galleryItem.Fields[Templates.ArticleFeaturedCard.Fields.widgetItem.ctaText].Value;

                        ArticleCaption articleCaption = new ArticleCaption();
                        articleCaption.CaptionSource = galleryItem.Fields[Templates.ArticleFeaturedCard.Fields.widgetItem.CaptionSource].Value;
                        Sitecore.Data.Fields.DateField dateTimeField = galleryItem.Fields[Templates.ArticleFeaturedCard.Fields.widgetItem.CaptionDate];

                        if (!string.IsNullOrEmpty(dateTimeField.Value))
                        {
                            string dateTimeString = dateTimeField.Value;
                            DateTime dateTimeStruct = Sitecore.DateUtil.IsoDateToDateTime(dateTimeString);
                            articleCaption.CaptionDate = String.Format("{0:d MMM yyyy}", dateTimeStruct);
                        }
                        ItemsListData.Caption = articleCaption;
                        ItemsListData.ctaLink = Utils.GetLinkURL(galleryItem?.Fields[Templates.ArticleFeaturedCard.Fields.widgetItem.ctaLink]);
                        ItemsListData.imageSrc = Utils.GetImageURLByFieldId(galleryItem, Templates.ArticleFeaturedCard.Fields.widgetItem.imageSrc);
                        widgetItemsListData.Add(ItemsListData);
                    }
                }
                articleFeaturedCardModelData.widgetItems = widgetItemsListData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return articleFeaturedCardModelData;
        }


        public Faq GetFaq(Rendering rendering)
        {
            Faq aboutFaqData = new Faq();
            List<FaqWidgetItems> widgetItemsData = new List<FaqWidgetItems>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetFaq : Datasource is empty", this);
                return aboutFaqData;
            }
            try
            {
                aboutFaqData.id = datasource.Fields[Templates.Faq.Feilds.id] != null ? datasource.Fields[Templates.Faq.Feilds.id].Value : "";
                aboutFaqData.type = datasource.Fields[Templates.Faq.Feilds.type] != null ? datasource.Fields[Templates.Faq.Feilds.type].Value : "";
                aboutFaqData.title = datasource.Fields[Templates.Faq.Feilds.title] != null ? datasource.Fields[Templates.Faq.Feilds.title].Value : "";
                aboutFaqData.subTitle = datasource.Fields[Templates.Faq.Feilds.subTitle] != null ? datasource.Fields[Templates.Faq.Feilds.subTitle].Value : "";
                MultilistField galleryMultilistField = datasource.Fields[Templates.Faq.Feilds.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        FaqWidgetItems FaqWidgetItemsData = new FaqWidgetItems();
                        FaqWidgetItemsData.id = galleryItem.Fields[Templates.Faq.Feilds.widgetItem.id].Value;
                        FaqWidgetItemsData.title = galleryItem.Fields[Templates.Faq.Feilds.widgetItem.title].Value;
                        FaqWidgetItemsData.subTitle = galleryItem.Fields[Templates.Faq.Feilds.widgetItem.subTitle].Value;
                        widgetItemsData.Add(FaqWidgetItemsData);
                    }
                }
                aboutFaqData.faqWidgetItems = widgetItemsData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                //throw ex;
            }
            return aboutFaqData;
        }

        public LegalNavbar GetLegalNavbar(Rendering rendering)
        {
            LegalNavbar legalNavbarData = new LegalNavbar();
            List<LegalNavbarwidgetItems> widgetItemsData = new List<LegalNavbarwidgetItems>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetLegalNavbar : Datasource is empty", this);
                return legalNavbarData;
            }
            try
            {
                legalNavbarData.id = datasource.Fields[Templates.LegalNavbar.Feilds.id] != null ? datasource.Fields[Templates.LegalNavbar.Feilds.id].Value : "";
                legalNavbarData.type = datasource.Fields[Templates.LegalNavbar.Feilds.type] != null ? datasource.Fields[Templates.LegalNavbar.Feilds.type].Value : "";
                legalNavbarData.title = datasource.Fields[Templates.LegalNavbar.Feilds.title] != null ? datasource.Fields[Templates.LegalNavbar.Feilds.title].Value : "";
                legalNavbarData.imageSrc = datasource.Fields[Templates.LegalNavbar.Feilds.imageSrc] != null ? datasource.Fields[Templates.LegalNavbar.Feilds.imageSrc].Value : "";
                MultilistField galleryMultilistField = datasource.Fields[Templates.LegalNavbar.Feilds.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        LegalNavbarwidgetItems LegalNavbarwidgetItemsData = new LegalNavbarwidgetItems();
                        LegalNavbarwidgetItemsData.id = galleryItem.Fields[Templates.LegalNavbar.Feilds.widgetItem.id].Value;
                        LegalNavbarwidgetItemsData.title = galleryItem.Fields[Templates.LegalNavbar.Feilds.widgetItem.title].Value;
                        widgetItemsData.Add(LegalNavbarwidgetItemsData);
                    }
                }
                legalNavbarData.legalNavbarwidgetItems = widgetItemsData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return legalNavbarData;
        }

        public AddVehicle GetAddVehicle(Rendering rendering)
        {
            AddVehicle addVehicleData = new AddVehicle();
            List<AddVehicleWidgetItems> widgetItemsData = new List<AddVehicleWidgetItems>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetAddVehicle : Datasource is empty", this);
                return addVehicleData;
            }
            try
            {
                addVehicleData.id = datasource.Fields[Templates.AddVehicle.Feilds.id] != null ? datasource.Fields[Templates.AddVehicle.Feilds.id].Value : "";
                addVehicleData.type = datasource.Fields[Templates.AddVehicle.Feilds.type] != null ? datasource.Fields[Templates.AddVehicle.Feilds.type].Value : "";
                addVehicleData.title = datasource.Fields[Templates.AddVehicle.Feilds.title] != null ? datasource.Fields[Templates.AddVehicle.Feilds.title].Value : "";
                addVehicleData.subTitle = datasource.Fields[Templates.AddVehicle.Feilds.subTitle] != null ? datasource.Fields[Templates.AddVehicle.Feilds.subTitle].Value : "";
                var multilist = Utils.GetMultiListValueItem(datasource, Templates.AddVehicle.Feilds.widgetItems);
                MultilistField galleryMultilistField = datasource.Fields[Templates.AddVehicle.Feilds.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        AddVehicleWidgetItems AddVehicleWidgetItemsData = new AddVehicleWidgetItems();
                        AddVehicleWidgetItemsData.id = galleryItem.Fields[Templates.AddVehicle.Feilds.widgetItem.id].Value;
                        AddVehicleWidgetItemsData.title = galleryItem.Fields[Templates.AddVehicle.Feilds.widgetItem.title].Value;
                        AddVehicleWidgetItemsData.imageSrc = datasource.Fields[Templates.AddVehicle.Feilds.type] != null ? datasource.Fields[Templates.AddVehicle.Feilds.imageSrc].Value : "";
                        widgetItemsData.Add(AddVehicleWidgetItemsData);
                    }
                }
                addVehicleData.addVehicleWidgetItems = widgetItemsData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return addVehicleData;
        }

        public AddVehicleForm GetAddVehicleForm(Rendering rendering)
        {
            AddVehicleForm addVehicleFormData = new AddVehicleForm();
            List<AddVehicleFormWidgetItems> widgetItemsData = new List<AddVehicleFormWidgetItems>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetAddVehicleForm : Datasource is empty", this);
                return addVehicleFormData;
            }
            try
            {
                addVehicleFormData.id = datasource.Fields[Templates.AddVehicleForm.Feilds.id] != null ? datasource.Fields[Templates.AddVehicleForm.Feilds.id].Value : "";
                addVehicleFormData.type = datasource.Fields[Templates.AddVehicleForm.Feilds.type] != null ? datasource.Fields[Templates.AddVehicleForm.Feilds.type].Value : "";
                addVehicleFormData.title = datasource.Fields[Templates.AddVehicleForm.Feilds.title] != null ? datasource.Fields[Templates.AddVehicleForm.Feilds.title].Value : "";
                addVehicleFormData.ctaText = datasource.Fields[Templates.AddVehicleForm.Feilds.ctaText] != null ? datasource.Fields[Templates.AddVehicleForm.Feilds.ctaText].Value : "";
                var multilist = Utils.GetMultiListValueItem(datasource, Templates.AddVehicleForm.Feilds.widgetItems);
                MultilistField galleryMultilistField = datasource.Fields[Templates.AddVehicleForm.Feilds.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        AddVehicleFormWidgetItems AddVehicleFormWidgetItemsData = new AddVehicleFormWidgetItems();
                        AddVehicleFormWidgetItemsData.id = galleryItem.Fields[Templates.AddVehicleForm.Feilds.widgetItem.id].Value;
                        AddVehicleFormWidgetItemsData.type = galleryItem.Fields[Templates.AddVehicleForm.Feilds.widgetItem.title].Value;
                        AddVehicleFormWidgetItemsData.formLabel = galleryItem.Fields[Templates.AddVehicleForm.Feilds.widgetItem.subTitle].Value;
                        widgetItemsData.Add(AddVehicleFormWidgetItemsData);
                    }
                }
                addVehicleFormData.addVehicleFormWidgetItems = widgetItemsData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return addVehicleFormData;
        }

        public FaqContactUs GetFaqContactUs(Rendering rendering)
        {
            FaqContactUs faqContactUsData = new FaqContactUs();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetFaqContactUs : Datasource is empty", this);
                return faqContactUsData;
            }
            try
            {
                faqContactUsData.id = datasource.Fields[Templates.FAQContactUs.Feilds.id] != null ? datasource.Fields[Templates.FAQContactUs.Feilds.id].Value : "";
                faqContactUsData.type = datasource.Fields[Templates.FAQContactUs.Feilds.type] != null ? datasource.Fields[Templates.FAQContactUs.Feilds.type].Value : "";
                faqContactUsData.title = datasource.Fields[Templates.FAQContactUs.Feilds.title] != null ? datasource.Fields[Templates.FAQContactUs.Feilds.title].Value : "";
                faqContactUsData.subTitle = datasource.Fields[Templates.FAQContactUs.Feilds.subTitle] != null ? datasource.Fields[Templates.FAQContactUs.Feilds.subTitle].Value : "";
                faqContactUsData.imageSrc = Utils.GetImageURLByFieldId(datasource, Templates.ArticleVideoGalleryCarousel.Fields.widgetItem.imageSrc);
                faqContactUsData.ctaText = datasource.Fields[Templates.FAQContactUs.Feilds.ctaText] != null ? datasource.Fields[Templates.FAQContactUs.Feilds.ctaText].Value : "";
                faqContactUsData.ctaLink = Utils.GetLinkURL(datasource?.Fields[Templates.ArticleFeaturedFilters.Fields.widgetItem.ctaLink]);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return faqContactUsData;
        }

        public LegalTermsAndCondition GetLegalTermsAndCondition(Rendering rendering)
        {
            LegalTermsAndCondition legalTermsAndConditionData = new LegalTermsAndCondition();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetLegalTermsAndCondition : Datasource is empty", this);
                return legalTermsAndConditionData;
            }
            try
            {
                legalTermsAndConditionData.id = datasource.Fields[Templates.LegalTermsAndCondition.Feilds.id] != null ? datasource.Fields[Templates.LegalTermsAndCondition.Feilds.id].Value : "";
                legalTermsAndConditionData.type = datasource.Fields[Templates.LegalTermsAndCondition.Feilds.type] != null ? datasource.Fields[Templates.LegalTermsAndCondition.Feilds.type].Value : "";
                legalTermsAndConditionData.title = datasource.Fields[Templates.LegalTermsAndCondition.Feilds.title] != null ? datasource.Fields[Templates.LegalTermsAndCondition.Feilds.title].Value : "";
                legalTermsAndConditionData.text = datasource.Fields[Templates.LegalTermsAndCondition.Feilds.text] != null ? datasource.Fields[Templates.LegalTermsAndCondition.Feilds.text].Value : "";
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return legalTermsAndConditionData;
        }

        public LegalPrivacyPolicy GetLegalPrivacyPolicy(Rendering rendering)
        {
            LegalPrivacyPolicy legalPrivacyPolicyData = new LegalPrivacyPolicy();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetLegalPrivacyPolicy : Datasource is empty", this);
                return legalPrivacyPolicyData;
            }
            try
            {
                legalPrivacyPolicyData.id = datasource.Fields[Templates.LegalPrivacyPolicy.Feilds.id] != null ? datasource.Fields[Templates.LegalPrivacyPolicy.Feilds.id].Value : "";
                legalPrivacyPolicyData.type = datasource.Fields[Templates.LegalPrivacyPolicy.Feilds.type] != null ? datasource.Fields[Templates.LegalPrivacyPolicy.Feilds.type].Value : "";
                legalPrivacyPolicyData.title = datasource.Fields[Templates.LegalPrivacyPolicy.Feilds.title] != null ? datasource.Fields[Templates.LegalPrivacyPolicy.Feilds.title].Value : "";
                legalPrivacyPolicyData.text = datasource.Fields[Templates.LegalPrivacyPolicy.Feilds.text] != null ? datasource.Fields[Templates.LegalPrivacyPolicy.Feilds.text].Value : "";
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return legalPrivacyPolicyData;
        }

        public ArticleDetailsDiscoverMore ArticleDetailsDiscoverMore(Rendering rendering)
        {
            ArticleDetailsDiscoverMore articleDetailsDiscoverMoreData = new ArticleDetailsDiscoverMore();
            List<ArticleDetailsDiscoverMoreCardItems> widgetItemsListData = new List<ArticleDetailsDiscoverMoreCardItems>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("ArticleDetailsDiscoverMore : Datasource is empty", this);
                return articleDetailsDiscoverMoreData;
            }
            try
            {
                articleDetailsDiscoverMoreData.id = datasource.Fields[Templates.ArticleDetailsDiscoverMore.Fields.id] != null ? datasource.Fields[Templates.ArticleDetailsDiscoverMore.Fields.id].Value : "";
                articleDetailsDiscoverMoreData.type = datasource.Fields[Templates.ArticleDetailsDiscoverMore.Fields.type] != null ? datasource.Fields[Templates.ArticleDetailsDiscoverMore.Fields.type].Value : "";
                var multilist = Utils.GetMultiListValueItem(datasource, Templates.ArticleDetailsDiscoverMore.Fields.widgetItems);
                MultilistField galleryMultilistField = datasource.Fields[Templates.ArticleDetailsDiscoverMore.Fields.widgetItems];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        ArticleDetailsDiscoverMoreCardItems ItemsListData = new ArticleDetailsDiscoverMoreCardItems();
                        ItemsListData.id = galleryItem.Fields[Templates.ArticleDetailsDiscoverMore.Fields.widgetItem.id].Value;
                        ItemsListData.imageSrc = Utils.GetImageURLByFieldId(galleryItem, Templates.ArticleDetailsDiscoverMore.Fields.widgetItem.imageSrc);
                        ItemsListData.title = galleryItem.Fields[Templates.ArticleDetailsDiscoverMore.Fields.widgetItem.title].Value;
                        ArticleDetailsDiscoverMoreCaption articleCaption = new ArticleDetailsDiscoverMoreCaption();
                        articleCaption.CaptionSource = galleryItem.Fields[Templates.ArticleDetailsDiscoverMore.Fields.widgetItem.CaptionSource].Value;
                        Sitecore.Data.Fields.DateField dateTimeField = galleryItem.Fields[Templates.ArticleDetailsDiscoverMore.Fields.widgetItem.CaptionDate];

                        if (!string.IsNullOrEmpty(dateTimeField.Value))
                        {
                            string dateTimeString = dateTimeField.Value;
                            DateTime dateTimeStruct = Sitecore.DateUtil.IsoDateToDateTime(dateTimeString);
                            articleCaption.CaptionDate = String.Format("{0:d MMM yyyy}", dateTimeStruct);
                        }
                        ItemsListData.Caption = articleCaption;
                        ItemsListData.ctaLink = Utils.GetLinkURL(galleryItem?.Fields[Templates.ArticleDetailsDiscoverMore.Fields.widgetItem.ctaLink]);

                        widgetItemsListData.Add(ItemsListData);
                    }
                }
                articleDetailsDiscoverMoreData.widgetItems = widgetItemsListData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return articleDetailsDiscoverMoreData;
        }

        public ArticleDetails ArticleDetailsSocialMediaLinks(Rendering rendering)
        {
            ArticleDetails articleDetailsSocialMediaLinksData = new ArticleDetails();
            List<ArticleDetailsSocialMediaLinksItems> widgetItemsListData = new List<ArticleDetailsSocialMediaLinksItems>();
            List<Contentwidget> contentwidget = new List<Contentwidget>();
            List<Images> images = new List<Images>();
            Widget widget = new Widget();
            Contentwidget contentwidgetwidget = new Contentwidget();
            SocialMediaLinks socialMediaLinks = new SocialMediaLinks();
            Content content = new Content();



            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;


            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("ArticleDetailsDiscoverMore : Datasource is empty", this);
                return articleDetailsSocialMediaLinksData;
            }
            try
            {
                articleDetailsSocialMediaLinksData.id = datasource.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.id] != null ? datasource.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.id].Value : "";
                articleDetailsSocialMediaLinksData.type = datasource.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.type] != null ? datasource.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.type].Value : "";
                articleDetailsSocialMediaLinksData.title = datasource.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.title] != null ? datasource.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.title].Value : "";
                articleDetailsSocialMediaLinksData.author = datasource.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.author] != null ? datasource.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.author].Value : "";
                articleDetailsSocialMediaLinksData.readTime = datasource.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.readTime] != null ? datasource.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.readTime].Value : "";
                Sitecore.Data.Fields.DateField dateTimeField = datasource.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.date];
                if (!string.IsNullOrEmpty(dateTimeField.Value))
                {
                    string dateTimeString = dateTimeField.Value;
                    DateTime dateTimeStruct = Sitecore.DateUtil.IsoDateToDateTime(dateTimeString);
                    articleDetailsSocialMediaLinksData.date = String.Format("{0:d MMM yyyy}", dateTimeStruct);
                }
                articleDetailsSocialMediaLinksData.date = articleDetailsSocialMediaLinksData.date;

                var objitem = datasource.Axes.GetDescendants().Where(c => c.TemplateID == ID.Parse(Templates.ArticleDetailsTemplates.ArticleDetailsSocialMediaLinksDataID)).FirstOrDefault();
                var objcontentitem = datasource.Axes.GetDescendants().Where(c => c.TemplateID == ID.Parse(Templates.ArticleDetailsTemplates.ArticleDetailsSocialMediaLinksData)).FirstOrDefault();

                if (objitem != null)
                {
                    widget.id = objitem.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.Widget.id] != null ? objitem.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.Widget.id].Value : "";

                    MultilistField galleryMultilistField = objitem.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.Widget.widgetItems];

                    if (galleryMultilistField.Count > 0)
                    {
                        foreach (Item galleryItem in galleryMultilistField.GetItems())
                        {
                            ArticleDetailsSocialMediaLinksItems ItemsListData = new ArticleDetailsSocialMediaLinksItems();
                            ItemsListData.id = galleryItem.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.widgetItem.id].Value;
                            ItemsListData.ctaText = galleryItem.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.widgetItem.ctaText].Value;
                            ItemsListData.imageSrc = Utils.GetImageURLByFieldId(galleryItem, Templates.ArticleDetailsSocialMediaLinks.Fields.widgetItem.imageSrc);
                            ItemsListData.ctaLink = Utils.GetLinkURL(galleryItem?.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.widgetItem.ctaLink]);

                            widgetItemsListData.Add(ItemsListData);
                        }
                    }
                }

                if (objcontentitem != null)
                {
                    contentwidgetwidget.id = objcontentitem.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.WidgetContent.id] != null ? objcontentitem.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.WidgetContent.id].Value : "";
                    contentwidgetwidget.text = objcontentitem.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.WidgetContent.text] != null ? objcontentitem.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.WidgetContent.text].Value : "";

                    MultilistField galleryMultilistField = objcontentitem.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.WidgetContent.widgetItems];

                    if (galleryMultilistField.Count > 0)
                    {
                        foreach (Item galleryItem in galleryMultilistField.GetItems())
                        {
                            Images ItemsListData = new Images();
                            ItemsListData.id = galleryItem.Fields[Templates.ArticleDetailsSocialMediaLinks.Fields.widgetItemcontent.id].Value;
                            ItemsListData.src = Utils.GetImageURLByFieldId(galleryItem, Templates.ArticleDetailsSocialMediaLinks.Fields.widgetItemcontent.imageSrc);

                            images.Add(ItemsListData);
                        }
                    }
                }
                articleDetailsSocialMediaLinksData.SocialMediaLinks.widget.articleDetailsSocialMediaLinksItems = widgetItemsListData;
                articleDetailsSocialMediaLinksData.SocialMediaLinks.widget.id = widget.id;
                articleDetailsSocialMediaLinksData.Content.widget.images = images;
                articleDetailsSocialMediaLinksData.Content.widget.text = contentwidgetwidget.text;
                articleDetailsSocialMediaLinksData.Content.widget.id = contentwidgetwidget.id;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return articleDetailsSocialMediaLinksData;
        }

    }
}