using Project.AmbujaCement.Website.Helpers;
using Project.AmbujaCement.Website.Models;
using Project.AmbujaCement.Website.Models.Home;
using Project.AmbujaCement.Website.Templates;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Item = Sitecore.Data.Items.Item;
using static Project.AmbujaCement.Website.Templates.CostCalculaterFromTemplate;
using Project.AmbujaCement.Website.Models.About_Page;
using Project.AmbujaCement.Website.Templates.Home;
using Project.AmbujaCement.Website.Models.CostCalculator;

namespace Project.AmbujaCement.Website.Services.Home
{
    public class HomeServices : IHomeServices
    {
        readonly Item _contextItem;
        public HomeServices()
        {
            _contextItem = Sitecore.Context.Item;
        }

        public MainBanner GetMainBanner(Rendering rendering)
        {
            MainBanner mainBanner = new MainBanner();
            List<HomeMainBanner> bannercarousel = new List<HomeMainBanner>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                foreach (Item item in datasource.Children)
                {
                    HomeMainBanner bannercarouselData = new HomeMainBanner();
                    bannercarouselData.Heading = Utils.GetValue(item, BaseTemplates.HeadingTemplate.HeadingFieldId);
                    bannercarouselData.SubHeading = Utils.GetValue(item, BaseTemplates.SubHeadingTemplate.SubHeadingFieldId);
                    bannercarouselData.Link = Utils.GetLinkURL(item.Fields[BaseTemplates.LinkTemplate.LinkFieldId]);
                    bannercarouselData.LinkText = Utils.GetValue(item, BaseTemplates.LinkTemplate.LinkTextFieldId);
                    bannercarouselData.LinkTarget = Utils.GetValue(item, BaseTemplates.LinkTemplate.LinkTargetFieldId);
                    bannercarouselData.MediaType = Utils.GetValue(item, BaseTemplates.MediaTypeTemplate.MediaTypeFieldId);
                    bannercarouselData.VideoSource = Utils.GetLinkURL(item?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceFieldId]);
                    bannercarouselData.VideoSourceMobile = Utils.GetLinkURL(item?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceFieldId]);
                    bannercarouselData.VideoSourceTablet = Utils.GetLinkURL(item?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceFieldId]);
                    bannercarouselData.VideoSourceOGG = Utils.GetLinkURL(item?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceFieldId]);
                    bannercarouselData.VideoSourceMobileOGG = Utils.GetLinkURL(item?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceFieldId]);
                    bannercarouselData.VideoSourceTabletOGG = Utils.GetLinkURL(item?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceFieldId]);
                    bannercarouselData.AutoPlay = Utils.GetBoleanValue(item, BaseTemplates.AutoPlayTemplate.AutoPlayFieldId);
                    bannercarouselData.IsOverlayRequired = Utils.GetBoleanValue(item, BaseTemplates.OverlayRequiredTemplate.IsOverlayRequiredFieldId);
                    bannercarouselData.ImageSource = Utils.GetImageURLByFieldId(item, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                    bannercarouselData.ImageSourceMobile = Utils.GetImageURLByFieldId(item, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
                    bannercarouselData.ImageSourceTablet = Utils.GetImageURLByFieldId(item, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);
                    bannercarouselData.ImageAlt = Utils.GetValue(item, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);
                    bannercarouselData.GtmData = new GtmDataModel
                    {
                        Event = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Category = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Title = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                        Label = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };
                    bannercarousel.Add(bannercarouselData);
                }
                mainBanner.Data = bannercarousel;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
            }
            return mainBanner;
        }
        public VideoBanner GetVideoBanner(Rendering rendering)
        {
            VideoBanner videoBannerData = new VideoBanner();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                videoBannerData.Heading = Utils.GetValue(datasource, BaseTemplates.HeadingTemplate.HeadingFieldId);
                videoBannerData.Autoplay = Utils.GetBoleanValue(datasource, BaseTemplates.AutoPlayTemplate.AutoPlayFieldId);
                videoBannerData.MediaType = Utils.GetValue(datasource, BaseTemplates.MediaTypeTemplate.MediaTypeFieldId);
                videoBannerData.DefaultVideoSource = Utils.GetLinkURL(datasource?.Fields[BaseTemplates.DefaultVideoSourceTemplate.DefaultVideoSourceFieldId]);
                videoBannerData.DefaultVideoSourceMobile = Utils.GetLinkURL(datasource?.Fields[BaseTemplates.DefaultVideoSourceTemplate.DefaultVideoSourceMobileFieldId]);
                videoBannerData.DefaultVideoSourceTablet = Utils.GetLinkURL(datasource?.Fields[BaseTemplates.DefaultVideoSourceTemplate.DefaultVideoSourceTabletFieldId]);
                videoBannerData.DefaultVideoSourceOgg = Utils.GetLinkURL(datasource?.Fields[BaseTemplates.DefaultVideoSourceTemplate.DefaultVideoSourceFieldId]);
                videoBannerData.DefaultVideoSourceMobileOgg = Utils.GetLinkURL(datasource?.Fields[BaseTemplates.DefaultVideoSourceTemplate.DefaultVideoSourceMobileFieldId]);
                videoBannerData.DefaultVideoSourceTabletOgg = Utils.GetLinkURL(datasource?.Fields[BaseTemplates.DefaultVideoSourceTemplate.DefaultVideoSourceMobileFieldId]);
                videoBannerData.VideoSource = Utils.GetLinkURL(datasource?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceFieldId]);
                videoBannerData.VideoSourceMobile = Utils.GetLinkURL(datasource?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceMobileFieldId]);
                videoBannerData.VideoSourceTablet = Utils.GetLinkURL(datasource?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceTabletFieldId]);
                videoBannerData.VideoSourceOgg = Utils.GetLinkURL(datasource?.Fields[BaseTemplates.VideoSourceOggTemplate.VideoSourceOggFieldId]);
                videoBannerData.VideoSourceMobileOgg = Utils.GetLinkURL(datasource?.Fields[BaseTemplates.VideoSourceOggTemplate.VideoSourceOggMobileFieldId]);
                videoBannerData.VideoSourceTabletOgg = Utils.GetLinkURL(datasource?.Fields[BaseTemplates.VideoSourceOggTemplate.VideoSourceOggTabletFieldId]);
                videoBannerData.SectionID = Utils.GetValue(datasource, BaseTemplates.SectionIdTemplate.SectionIdFieldId);
                videoBannerData.PosterImage = Utils.GetImageURLByFieldId(datasource, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                videoBannerData.SeoDescription = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.SeoDescriptionFieldId);
                videoBannerData.SeoName = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.SeoNameFieldId);


                var gtmDataSource = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmDataFieldId);
                var gtmDataSourceItem = Sitecore.Context.Database.GetItem(gtmDataSource);

                videoBannerData.GtmData = new GtmDataModel();
                videoBannerData.GtmData.Event = Utils.GetValue(gtmDataSourceItem, BaseTemplates.GTMTemplate.GtmEventFieldId);
                videoBannerData.GtmData.Category = Utils.GetValue(gtmDataSourceItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId);
                videoBannerData.GtmData.Sub_category = Utils.GetValue(gtmDataSourceItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId);
                videoBannerData.GtmData.Label = Utils.GetValue(gtmDataSourceItem, BaseTemplates.GTMTemplate.GtmLabelFieldId);
                videoBannerData.GtmData.Banner_category = Utils.GetValue(gtmDataSourceItem, BaseTemplates.GTMTemplate.GtmBannerCategoryFieldId);
                videoBannerData.GtmData.Title = Utils.GetValue(gtmDataSourceItem, BaseTemplates.GTMTemplate.GtmTitleFieldId);
                videoBannerData.GtmData.Section_title = Utils.GetValue(gtmDataSourceItem, BaseTemplates.GTMTemplate.GtmSectionTitleFieldId);
                videoBannerData.GtmData.Index = Utils.GetValue(gtmDataSourceItem, BaseTemplates.GTMTemplate.GtmIndexFieldId);
                videoBannerData.GtmData.Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item);
                videoBannerData.GtmData.Click_text = Utils.GetValue(gtmDataSourceItem, BaseTemplates.GTMTemplate.GtmClicktextFieldId);

                var gtmVideoStartSource = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmVideoStartFieldId);
                var gtmVideoStartSourceItem = Sitecore.Context.Database.GetItem(gtmVideoStartSource);

                videoBannerData.GtmVideoStart = new GtmVideoStartModel();
                videoBannerData.GtmVideoStart.Event = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmEventFieldId);
                videoBannerData.GtmVideoStart.Category = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId);
                videoBannerData.GtmVideoStart.Sub_category = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId);
                videoBannerData.GtmVideoStart.Banner_category = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmBannerCategoryFieldId);
                videoBannerData.GtmVideoStart.Title = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmTitleFieldId);
                videoBannerData.GtmVideoStart.Section_title = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmSectionTitleFieldId);
                videoBannerData.GtmVideoStart.Label = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmLabelFieldId);
                videoBannerData.GtmVideoStart.Index = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmIndexFieldId);
                videoBannerData.GtmVideoStart.Click_text = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmClicktextFieldId);
                videoBannerData.GtmVideoStart.Video_duration = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmVideodurationFieldId);
                videoBannerData.GtmVideoStart.Video_action = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmVideoactionFieldId);
                videoBannerData.GtmVideoStart.Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item);

                var gtmVideoProgressSource = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmVideoProgressFieldId);
                var gtmVideoProgressSourceItem = Sitecore.Context.Database.GetItem(gtmVideoProgressSource);

                videoBannerData.GtmVideoProgress = new GtmVideoProgressModel();
                videoBannerData.GtmVideoProgress.Event = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmEventFieldId);
                videoBannerData.GtmVideoProgress.Category = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId);
                videoBannerData.GtmVideoProgress.Sub_category = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId);
                videoBannerData.GtmVideoProgress.Banner_category = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId);
                videoBannerData.GtmVideoProgress.Title = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmTitleFieldId);
                videoBannerData.GtmVideoProgress.Section_title = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmSectionTitleFieldId);
                videoBannerData.GtmVideoProgress.Label = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmLabelFieldId);
                videoBannerData.GtmVideoProgress.Index = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmIndexFieldId);
                videoBannerData.GtmVideoProgress.Click_text = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmClicktextFieldId);
                videoBannerData.GtmVideoProgress.Video_duration = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmVideodurationFieldId);
                videoBannerData.GtmVideoProgress.Video_action = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmVideoactionFieldId);
                videoBannerData.GtmVideoProgress.Video_percent = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmVideopercentFieldId);
                videoBannerData.GtmVideoProgress.Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item);


                var gtmVideoCompleteSource = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmVideoCompleteFieldId);
                var gtmVideoCompleteSourceItem = Sitecore.Context.Database.GetItem(gtmVideoCompleteSource);

                videoBannerData.GtmVideoComplete = new GtmVideoStartModel();
                videoBannerData.GtmVideoComplete.Event = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.GtmEventFieldId);
                videoBannerData.GtmVideoComplete.Category = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId);
                videoBannerData.GtmVideoComplete.Sub_category = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId);
                videoBannerData.GtmVideoComplete.Banner_category = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.GtmBannerCategoryFieldId);
                videoBannerData.GtmVideoComplete.Title = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.GtmTitleFieldId);
                videoBannerData.GtmVideoComplete.Section_title = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.GtmSectionTitleFieldId);
                videoBannerData.GtmVideoComplete.Label = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.GtmLabelFieldId);
                videoBannerData.GtmVideoComplete.Index = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.GtmIndexFieldId);
                videoBannerData.GtmVideoComplete.Click_text = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.GtmClicktextFieldId);
                videoBannerData.GtmVideoComplete.Video_duration = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.GtmVideodurationFieldId);
                videoBannerData.GtmVideoComplete.Video_action = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.GtmVideoactionFieldId);
                videoBannerData.GtmVideoComplete.Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item);


                //DateField dateTimeField = datasource.Fields[BaseTemplates.GTMTemplate.UploadDateFieldId];
                //if (!string.IsNullOrEmpty(dateTimeField.Value))
                //{
                //    string dateTimeString = dateTimeField.Value;
                //    DateTime dateTimeStruct = Sitecore.DateUtil.IsoDateToDateTime(dateTimeString);
                //    videoBannerData.uploadDate = System.Convert.ToString(dateTimeStruct); ;
                //}
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return videoBannerData;
        }
        public HomeCurriculum GetCurriculum(Rendering rendering)
        {
            HomeCurriculum homeCurriculumData = new HomeCurriculum();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetCurriculum : Datasource is empty", this);
                return homeCurriculumData;
            }
            try
            {

                homeCurriculumData.Heading = Utils.GetValue(datasource, BaseTemplates.TitleTemplate.TitleFieldId);
                // homeCurriculumData.SubHeading = Utils.GetValue(datasource, BaseTemplates.SubHeadingTemplate.SubHeading2FieldId);
                homeCurriculumData.Description = Utils.GetValue(datasource, BaseTemplates.DescriptionTemplate.DescriptionFieldId);
                homeCurriculumData.BtnLink = Utils.GetLinkURL(datasource.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]);
                homeCurriculumData.BtnText = Utils.GetValue(datasource, BaseTemplates.CtaTemplate.CtaTextFieldId);
                homeCurriculumData.GtmData = new GtmDataModel
                {
                    Event = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmEventFieldId),
                    Category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                    Sub_category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                    Index = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmIndexFieldId),
                    Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                };

                //gallery
                var gelleryFolder = datasource.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Gallery"));
                homeCurriculumData.Gallery = new List<HomeLearningGalleryItem>();
                foreach (Item galleryItem in gelleryFolder.Children)
                {
                    var ItemsData = new HomeLearningGalleryItem
                    {
                        ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId),
                        Heading = Utils.GetValue(galleryItem, BaseTemplates.TitleTemplate.TitleFieldId),
                        Description = galleryItem.Fields[BaseTemplates.DescriptionTemplate.DescriptionFieldId].Value,
                        Link = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),
                        ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                        ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                        ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId),
                        GtmData = new GtmDataModel
                        {
                            Event = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmEventFieldId),
                            Category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                            Sub_category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                            Index = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmIndexFieldId),
                            Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                        }
                    };
                    homeCurriculumData.Gallery.Add(ItemsData);
                }

                //academicDetails
                var academicDetailsFolder = datasource.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "AcademicDetails"));
                homeCurriculumData.AcademicDetails = new List<HomeLearningGalleryItem>();
                foreach (Item galleryItem in academicDetailsFolder.Children)
                {
                    var ItemsData = new HomeLearningGalleryItem
                    {
                        ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId),
                        Heading = Utils.GetValue(galleryItem, BaseTemplates.TitleTemplate.TitleFieldId),
                        Description = galleryItem.Fields[BaseTemplates.DescriptionTemplate.DescriptionFieldId].Value,
                        Link = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),
                        ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                        ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                        ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId),
                        GtmData = new GtmDataModel
                        {
                            Event = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmEventFieldId),
                            Category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                            Sub_category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                            Index = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmIndexFieldId),
                            Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                        }
                    };
                    homeCurriculumData.AcademicDetails.Add(ItemsData);
                }

                //features
                var featuresFolder = datasource.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Features"));
                homeCurriculumData.Features = new List<FeaturesList>();
                foreach (Item galleryItem in featuresFolder.Children)
                {

                    FeaturesList ItemsData = new FeaturesList();
                    ItemsData.Theme = Utils.GetValue(galleryItem, BaseTemplates.ThemeTemplate.ThemeFieldId);
                    ItemsData.Heading = Utils.GetValue(galleryItem, BaseTemplates.TitleTemplate.TitleFieldId);
                    ItemsData.Description = Utils.GetValue(galleryItem, BaseTemplates.RichTextTemplate.RichTextFieldID);
                    ItemsData.ImageAlt = galleryItem.Fields[BaseTemplates.ImageSourceTemplate.ImageAltFieldId].Value;
                    ItemsData.ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                    ItemsData.ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
                    ItemsData.ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);
                    homeCurriculumData.Features.Add(ItemsData);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
            }
            return homeCurriculumData;
        }
        public HomeFAQ GetHomeFAQ(Rendering rendering)
        {
            HomeFAQ homeFAQData = new HomeFAQ();
            List<FAQDataItem> FAQGalleryData = new List<FAQDataItem>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                homeFAQData.SectionHeading = Utils.GetValue(datasource, HomePageTemplate.Fields.SectionHeading);
                homeFAQData.Link = Utils.GetLinkURL(datasource.Fields[BaseTemplates.LinkTemplate.LinkFieldId]);
                homeFAQData.linkTarget = Utils.GetValue(datasource, BaseTemplates.LinkTemplate.LinkTargetFieldId);
                homeFAQData.LinkText = Utils.GetValue(datasource, BaseTemplates.LinkTemplate.LinkTextFieldId);

                homeFAQData.GtmData = new GtmDataModel
                {
                    Event = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmEventFieldId),
                    Title = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmEventFieldId),
                    Category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                    Sub_category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                    Label = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                    Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                };

                foreach (Item galleryItem in datasource.Children)
                {
                    FAQDataItem itemsData = new FAQDataItem();
                    itemsData.Heading = galleryItem.Fields[BaseTemplates.HeadingTemplate.HeadingFieldId].Value;
                    itemsData.Description = galleryItem.Fields[HomePageTemplate.Fields.Description].Value;
                    itemsData.CategoryID = galleryItem.Fields[HomePageTemplate.Fields.categoryID].Value;
                    itemsData.QuestionID = galleryItem.Fields[HomePageTemplate.Fields.questionID].Value;
                    itemsData.CategoryHeading = galleryItem.Fields[HomePageTemplate.Fields.categoryID].Value;
                    FAQGalleryData.Add(itemsData);
                }
                homeFAQData.Data = FAQGalleryData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
            }
            return homeFAQData;
        }
        public GalleryContentModel<ScaleSlider> GetScaleSlider(Rendering rendering)
        {
            var scalesliderdata = new GalleryContentModel<ScaleSlider>();
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;
            try
            {
                scalesliderdata.SectionID = Utils.GetValue(datasource, BaseTemplates.SectionIDTemplate.SectionIDFieldId);
                scalesliderdata.ImageSource = Utils.GetImageURLByFieldId(datasource, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                scalesliderdata.ImageSourceMobile = Utils.GetImageURLByFieldId(datasource, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
                scalesliderdata.ImageSourceTablet = Utils.GetImageURLByFieldId(datasource, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);
                scalesliderdata.ImageAlt = Utils.GetValue(datasource, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);
                scalesliderdata.CardType = Utils.GetValue(datasource, BaseTemplates.CardTypeTemplate.CardTypeFieldId);
                scalesliderdata.MediaType = Utils.GetValue(datasource, BaseTemplates.MediaTypeTemplate.MediaTypeFieldId);
                scalesliderdata.Link = Utils.GetLinkURL(datasource?.Fields[BaseTemplates.LinkTemplate.LinkFieldId]);
                scalesliderdata.LinkText = Utils.GetValue(datasource, BaseTemplates.LinkTemplate.LinkTextFieldId);
                scalesliderdata.LinkTarget = Utils.GetValue(datasource, BaseTemplates.LinkTemplate.LinkTargetFieldId);
                scalesliderdata.SubHeading = Utils.GetValue(datasource, BaseTemplates.SubHeadingTemplate.SubHeadingFieldId);
                scalesliderdata.Heading = Utils.GetValue(datasource, BaseTemplates.HeadingTemplate.HeadingFieldId);


                scalesliderdata.GtmData = new GtmDataModel
                {
                    Event = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmEventFieldId),
                    Title = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                    Category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                    Sub_category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                    Label = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                    Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                };
                scalesliderdata.Gallery = new List<ScaleSlider>();

                foreach (Item galleryItem in datasource.Children)
                {
                    LinkField lf = galleryItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId];

                    var ItemsData = new ScaleSlider
                    {
                        Heading = Utils.GetValue(galleryItem, BaseTemplates.HeadingTemplate.HeadingFieldId),
                        Description = Utils.GetValue(galleryItem, HomePageTemplate.Fields.Description),
                        LinkTarget = Utils.GetValue(galleryItem, BaseTemplates.LinkTemplate.LinkTargetFieldId),
                        Link = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),
                        ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                        ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                        ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId),
                        ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId)
                    };
                    ItemsData.GtmData = new GtmDataModel
                    {

                        Event = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Title = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                        Category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Label = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };
                    scalesliderdata.Gallery.Add(ItemsData);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
            }
            return scalesliderdata;
        }
        public CardSliderContentModel<CardSlider> GetCardSlider(Rendering rendering)
        {
            var CardTypedata = new CardSliderContentModel<CardSlider>();
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;
            try
            {
                CardTypedata.SectionID = Utils.GetValue(datasource, BaseTemplates.SectionIDTemplate.SectionIDFieldId);
                CardTypedata.Variant = Utils.GetValue(datasource, BaseTemplates.VariantTemplate.VariantFieldId);
                CardTypedata.Theme = Utils.GetValue(datasource, BaseTemplates.ThemeTemplate.ThemeFieldId);
                CardTypedata.ImageSource = Utils.GetImageURLByFieldId(datasource, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                CardTypedata.ImageSourceMobile = Utils.GetImageURLByFieldId(datasource, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
                CardTypedata.ImageSourceTablet = Utils.GetImageURLByFieldId(datasource, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);
                CardTypedata.ImageAlt = Utils.GetValue(datasource, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);
                //CardTypedata.CardType = Utils.GetValue(datasource, BaseTemplates.CardTypeTemplate.CardTypeFieldId);
                //CardTypedata.MediaType = Utils.GetValue(datasource, BaseTemplates.MediaTypeTemplate.MediaTypeFieldId);
                CardTypedata.SubHeading = Utils.GetValue(datasource, BaseTemplates.SubHeadingTemplate.SubHeadingFieldId);
                CardTypedata.Heading = Utils.GetValue(datasource, BaseTemplates.HeadingTemplate.HeadingFieldId);
                CardTypedata.ImageTitle = Utils.GetValue(datasource, BaseTemplates.ImageSourceTemplate.ImageTitleFieldId);

                CardTypedata.Gallery = new List<CardSlider>();

                foreach (Item galleryItem in datasource.Children)
                {
                    LinkField lf = galleryItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId];

                    var ItemsData = new CardSlider
                    {
                        Heading = Utils.GetValue(galleryItem, BaseTemplates.HeadingTemplate.HeadingFieldId),
                        Description = Utils.GetValue(galleryItem, BaseTemplates.DescriptionTemplate.DescriptionFieldId),
                        Date = Utils.GetDate(galleryItem, BaseTemplates.DateTemplate.DateFieldId),
                        LinkTarget = Utils.GetValue(galleryItem, BaseTemplates.LinkTemplate.LinkTargetFieldId),
                        Link = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),
                        ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                        ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                        ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId),
                        ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId)
                    };
                    ItemsData.GtmData = new GtmDataModel
                    {

                        Event = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Title = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                        Category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Label = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };
                    CardTypedata.Gallery.Add(ItemsData);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
            }
            return CardTypedata;
        }
        public CategoriesListModel GetCategories(Rendering rendering)
        {
            List<CategoriesModel> category = new List<CategoriesModel>();
            CategoriesListModel categoriesListModel = new CategoriesListModel();
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                foreach (Item item in datasource.Children)
                {
                    CategoriesModel categoryData = new CategoriesModel();
                    categoryData.ImageSource = Utils.GetImageURLByFieldId(item, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                    categoryData.ImageSourceMobile = Utils.GetImageURLByFieldId(item, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
                    categoryData.ImageSourceTablet = Utils.GetImageURLByFieldId(item, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);
                    categoryData.ImageAlt = Utils.GetValue(item, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);
                    categoryData.ImageTitle = Utils.GetValue(item, HomecategoriesListTemplate.CategoriesList.ImageTitle);
                    categoryData.ProductName = Utils.GetValue(item, HomecategoriesListTemplate.CategoriesList.ProductName);
                    categoryData.ProductCount = Utils.GetValue(item, HomecategoriesListTemplate.CategoriesList.ProductCount);
                    categoryData.Link = Utils.GetLinkURL(item.Fields[BaseTemplates.LinkTemplate.LinkFieldId]);
                    categoryData.LinkTarget = Utils.GetValue(item, BaseTemplates.LinkTemplate.LinkTargetFieldId);
                    categoryData.IsActive = Utils.GetBoleanValue(item, BaseTemplates.IsAbsoluteTemplate.IsAbsoluteFieldId);
                    categoryData.GtmData = new GtmDataModel
                    {
                        Event = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Category = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Title = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                        Label = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };
                    category.Add(categoryData);
                }
                categoriesListModel.CategoriesList = category;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return categoriesListModel;
        }

        public VerticalCarousel GetVerticalCarousel(Rendering rendering)
        {
            List<VerticalCarouselDatum> category = new List<VerticalCarouselDatum>();
            VerticalCarousel categoriesListModel = new VerticalCarousel();
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;
            categoriesListModel.StarImage = Utils.GetImageURLByFieldId(datasource, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
            categoriesListModel.StarImageAlt = Utils.GetValue(datasource, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);
            categoriesListModel.SectionHeading = Utils.GetValue(datasource, BaseTemplates.HeadingTemplate.HeadingFieldId);


            try
            {
                foreach (Item item in datasource.Children)
                {
                    VerticalCarouselDatum categoryData = new VerticalCarouselDatum();
                    categoryData.Id = Utils.GetValue(item, BaseTemplates.SectionIDTemplate.SectionIDFieldId);
                    categoryData.ImageSource = Utils.GetImageURLByFieldId(item, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                    categoryData.ImageSourceMobile = Utils.GetImageURLByFieldId(item, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
                    categoryData.ImageSourceTablet = Utils.GetImageURLByFieldId(item, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);
                    categoryData.ImageAlt = Utils.GetValue(item, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);
                    categoryData.Link = Utils.GetLinkURL(item.Fields[BaseTemplates.LinkTemplate.LinkFieldId]);
                    categoryData.LinkText = Utils.GetValue(item, BaseTemplates.LinkTemplate.LinkTextFieldId);
                    categoryData.LinkTarget = Utils.GetValue(item, BaseTemplates.LinkTemplate.LinkTargetFieldId);
                    categoryData.Description = Utils.GetValue(item, BaseTemplates.DescriptionTemplate.DescriptionFieldId);
                    categoryData.Heading = Utils.GetValue(item, BaseTemplates.HeadingTemplate.HeadingFieldId);
                    categoryData.GtmData = new GtmDataModel
                    {
                        Event = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Category = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Title = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                        Label = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };
                    List<VerticalCarouselDatumFeature> verticalCarouselCategory = new List<VerticalCarouselDatumFeature>();
                    foreach (Item subitem in item.Children)
                    {
                        VerticalCarouselDatumFeature verticalCarouselDatumFeature = new VerticalCarouselDatumFeature();
                        verticalCarouselDatumFeature.ImageSource = Utils.GetImageURLByFieldId(subitem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                        verticalCarouselDatumFeature.ImageAlt = Utils.GetValue(subitem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);
                        verticalCarouselDatumFeature.Heading = Utils.GetValue(subitem, BaseTemplates.HeadingTemplate.HeadingFieldId);
                        verticalCarouselCategory.Add(verticalCarouselDatumFeature);
                    }
                    categoryData.Features = verticalCarouselCategory;
                    category.Add(categoryData);
                }
                categoriesListModel.Data = category;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return categoriesListModel;
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
                seoData.MetaKeywords = Utils.GetValue(datasource, BaseTemplates.SubHeadingTemplate.SubHeadingFieldId);
                seoData.MetaTitle = Utils.GetValue(datasource, BaseTemplates.HeadingTemplate.HeadingFieldId);
                seoData.OgTitle = Utils.GetValue(datasource, SeoDataTemplate.Fields.ogTitle);

                var robotTags = new RobotTag();
                robotTags.index = Utils.GetBoleanValue(datasource, SeoDataTemplate.Fields.robotsIndexTag);
                robotTags.follow = Utils.GetBoleanValue(datasource, SeoDataTemplate.Fields.robotsFollowTag);

                seoData.RobotsTags = robotTags;
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
                    ItemsListData.ContactType = Utils.GetValue(galleryItem, BaseTemplates.SubHeadingTemplate.SubHeadingFieldId);
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

        public Cookies GetCookies(Rendering rendering)
        {
            Cookies cookiesData = new Cookies();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetCookies : Datasource is empty", this);
                return cookiesData;
            }
            try
            {
                cookiesData.Heading = Utils.GetValue(datasource, BaseTemplates.HeadingTemplate.HeadingFieldId);
                cookiesData.Description = Utils.GetValue(datasource, BaseTemplates.DescriptionTemplate.DescriptionFieldId);
                cookiesData.Decline = Utils.GetValue(datasource, CookiesTemplate.Fields.Decline);
                cookiesData.AcceptCookies = Utils.GetValue(datasource, CookiesTemplate.Fields.AcceptCookies);
                cookiesData.GtmData = new GtmDataModel
                {
                    Event = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmEventFieldId),
                    Category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                    Sub_category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                    Label = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                    Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                };

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
            }
            return cookiesData;
        }

        public CostCalculatorData GetCostCalculatorFormData(Rendering rendering)
        {
            CostCalculatorData costCalculatorData = new CostCalculatorData();
            HomeLabels labelsdata = new HomeLabels();

            HomeTextData textData = new HomeTextData();
            List<HomeButtonTab> buttonTabs = new List<HomeButtonTab>();
            List<HomeInputTab> inputTabs = new List<HomeInputTab>();

            int parsedValue;
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;
            var LabelContext = datasource.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "CostCalculatorlabels"));
            var TabContext = datasource.Children.FirstOrDefault(y => Utils.CompareIgnoreCase(y.Name, "CostCalculatorTab"));
            var TextContext = datasource.Children.FirstOrDefault(z => Utils.CompareIgnoreCase(z.Name, "CostCalculatorTextData"));
            try
            {
                #region LabelData
                labelsdata.HeadingLabel = Utils.GetValue(LabelContext, CostCalculaterPageTemplate.CostCalculatorLabelsFields.HomeHeadingLabel);

                costCalculatorData.Labels = labelsdata;
                #endregion
                #region TabData
                List<HomeTabDatum> tabData = new List<HomeTabDatum>();
                foreach (Item tabchild in TabContext.Children)
                {
                    HomeTabDatum tabDataum = new HomeTabDatum();
                    HomeData data = new HomeData();

                    tabDataum.Type = Utils.GetValue(tabchild, CostCalculaterPageTemplate.TabDataDetail.Type);
                    tabDataum.Label = Utils.GetValue(tabchild, CostCalculaterPageTemplate.TabDataDetail.Label);
                    tabDataum.SubTitle = Utils.GetValue(tabchild, CostCalculaterPageTemplate.TabDataDetail.SubTitle);
                    var TooltipContext = tabchild.Children.FirstOrDefault(t => Utils.CompareIgnoreCase(t.Name, "tooltipData"));
                    if (TooltipContext != null)
                    {
                        List<HometooltipDataitems> tooltipData = new List<HometooltipDataitems>();
                        foreach (Item tooltipchild in TooltipContext.Children)
                        {
                            HometooltipDataitems tooltip = new HometooltipDataitems();
                            tooltip.Title = Utils.GetValue(tooltipchild, CostCalculaterPageTemplate.TabDataFields.Title);
                            tooltip.Description = Utils.GetValue(tooltipchild, CostCalculaterPageTemplate.TabDataFields.Description);
                            tooltipData.Add(tooltip);
                        }
                        tabDataum.tooltipData = tooltipData;
                    }


                    var ButtonContext = tabchild.Children.FirstOrDefault(b => Utils.CompareIgnoreCase(b.Name, "buttonTabs"));
                    var InputContext = tabchild.Children.FirstOrDefault(p => Utils.CompareIgnoreCase(p.Name, "inputTabs"));
                    if (ButtonContext != null)
                    {
                        List<HomeButtonTab> ButtonTabs = new List<HomeButtonTab>();
                        foreach (Item child in ButtonContext.Children)
                        {
                            HomeButtonTab buttonTab = new HomeButtonTab();
                            buttonTab.Label = Utils.GetValue(child, CostCalculaterPageTemplate.TabDataButtonTabs.Label);
                            buttonTab.Id = Utils.GetValue(child, CostCalculaterPageTemplate.TabDataButtonTabs.Id);
                            buttonTab.InitiallyChecked = Utils.GetBoleanValue(child, CostCalculaterPageTemplate.TabDataButtonTabs.InitiallyChecked);
                            ButtonTabs.Add(buttonTab);
                        }
                        data.ButtonTabs = ButtonTabs;
                    }


                    if (InputContext != null)
                    {
                        List<HomeInputTab> InputTabs = new List<HomeInputTab>();
                        foreach (Item child in InputContext.Children)
                        {
                            HomeInputTab inputTab = new HomeInputTab();
                            inputTab.Type = Utils.GetValue(child, CostCalculaterPageTemplate.TabDataInputTabsFields.Label);
                            inputTab.Placeholder = Utils.GetValue(child, CostCalculaterPageTemplate.TabDataInputTabsFields.Placeholder);
                            inputTab.FieldName = Utils.GetValue(child, CostCalculaterPageTemplate.TabDataInputTabsFields.FieldName);
                            inputTab.ErrorMessage = Utils.GetValue(child, CostCalculaterPageTemplate.TabDataInputTabsFields.ErrorMessage);
                            //var DropdownContext = child.Children.FirstOrDefault(dr => Utils.CompareIgnoreCase(dr.Name, "dropdown"));
                            if (child.Children != null)
                            {
                                List<HomeoptionsList> optionsLists = new List<HomeoptionsList>();
                                foreach (Item optionschild in child.Children)
                                {
                                    HomeoptionsList optionsList = new HomeoptionsList();
                                    string optionsId = Utils.GetValue(optionschild, CostCalculaterPageTemplate.TabDataInputTabsOptionsFields.Id);
                                    if (optionsId != null && int.TryParse(optionsId, out parsedValue))
                                    { optionsList.Id = parsedValue; }
                                    //optionsList.Id = int.Parse(Utils.GetValue(optionschild, CostCalculaterPageTemplate.TabDataInputTabsOptionsFields.Id));
                                    optionsList.Label = Utils.GetValue(optionschild, CostCalculaterPageTemplate.TabDataInputTabsOptionsFields.Label);
                                    optionsLists.Add(optionsList);
                                }
                                inputTab.options = optionsLists;
                            }
                            InputTabs.Add(inputTab);
                        }
                        data.InputTabs = InputTabs;
                    }

                    var LabContext = tabchild.Children.FirstOrDefault(p => Utils.CompareIgnoreCase(p.Name, "TabDataLabel"));
                    HomeLabels matlabdata = new HomeLabels();
                    matlabdata.HeadingLabel = Utils.GetValue(LabContext, CostCalculaterPageTemplate.TabDataLabelsFields.HomeHeadingLabel);
                    matlabdata.SubmitButtonLabel = Utils.GetValue(LabContext, CostCalculaterPageTemplate.TabDataLabelsFields.HomeSubmitButtonLabel);
                    string activekey = Utils.GetValue(LabContext, CostCalculaterPageTemplate.TabDataLabelsFields.HomeDefaultActiveKey);
                    if (activekey != null && int.TryParse(activekey, out parsedValue))
                    { matlabdata.DefaultActiveKey = parsedValue; }
                    //matlabdata.DefaultActiveKey = int.Parse(Utils.GetValue(LabContext, CostCalculaterPageTemplate.TabDataLabelsFields.DefaultActiveKey));
                    data.Labels = matlabdata;

                    var SubmitButtonLabel = tabchild.Children.FirstOrDefault(sb => Utils.CompareIgnoreCase(sb.Name, "TabDataSubmitButton"));
                    HomeSubmitButton homesubmit = new HomeSubmitButton();
                    homesubmit.Link = Utils.GetLinkURL(SubmitButtonLabel.Fields[BaseTemplates.LinkTemplate.LinkFieldId]);
                    homesubmit.LinkTarget = Utils.GetValue(SubmitButtonLabel, BaseTemplates.LinkTemplate.LinkTargetFieldId);
                    homesubmit.Type = Utils.GetValue(SubmitButtonLabel, BaseTemplates.LinkTemplate.LinkTextFieldId);
                    data.SubmitButton = homesubmit;

                    //data.SubmitButton = submitButton;
                    tabDataum.Data = data;
                    tabData.Add(tabDataum);
                }
                costCalculatorData.TabData = tabData;
                #endregion

                #region TextData
                textData.Heading = Utils.GetValue(TextContext, BaseTemplates.HeadingDetailsTemplate.Heading);
                textData.Description = Utils.GetValue(TextContext, BaseTemplates.HeadingDetailsTemplate.Description);
                textData.ImageSource = Utils.GetImageURLByFieldId(TextContext, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                textData.ImageSourceMobile = Utils.GetImageURLByFieldId(TextContext, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
                textData.ImageSourceTablet = Utils.GetImageURLByFieldId(TextContext, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);
                textData.ImageAlt = Utils.GetValue(TextContext, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);

                costCalculatorData.TextData = textData;
                #endregion
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return costCalculatorData;
        }

        public AboutPageModel GetAboutPage(Rendering rendering)
        {
            AboutPageModel aboutpagedata = new AboutPageModel();
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;
            try
            {
                aboutpagedata.SectionID = Utils.GetValue(datasource, BaseTemplates.SectionIDTemplate.SectionIDFieldId);
                aboutpagedata.Heading = Utils.GetValue(datasource, BaseTemplates.HeadingTemplate.HeadingFieldId);
                aboutpagedata.Content = Utils.GetValue(datasource,AboutPageComponantTemplate.Fields.Content);
                aboutpagedata.MoreContent = Utils.GetValue(datasource, AboutPageComponantTemplate.Fields.MoreContent);
                aboutpagedata.ReadMore = Utils.GetValue(datasource, AboutPageComponantTemplate.Fields.ReadMore);
                aboutpagedata.ReadLess = Utils.GetValue(datasource, AboutPageComponantTemplate.Fields.ReadLess);

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
            }
            return aboutpagedata;
        }

    }
}