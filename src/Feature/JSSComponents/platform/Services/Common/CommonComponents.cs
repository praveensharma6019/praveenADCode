using Adani.SuperApp.Realty.Feature.JSSComponents.Platform.Helpers;
using Adani.SuperApp.Realty.Feature.JSSComponents.Platform.Models;
using Adani.SuperApp.Realty.Feature.JSSComponents.Platform.Templates;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.JSSComponents.Platform.Services.Common
{
    public class CommonComponents : ICommonComponents
    {
        public HomeMainBanner GetHomeMainBanner(Rendering rendering)
        {
            HomeMainBanner bannercarouselData = new HomeMainBanner();
            List<BannerGallery> bannerGalleryData = new List<BannerGallery>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {

                bannercarouselData.Heading = Utils.GetValue(datasource, BaseTemplates.TitleTemplate.TitleFieldId);
                bannercarouselData.SubHeading = Utils.GetValue(datasource, BaseTemplates.SubHeadingTemplate.SubHeading2FieldId);
                bannercarouselData.Link = Utils.GetLinkURL(datasource.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]);
                bannercarouselData.LinkText = Utils.GetValue(datasource, BaseTemplates.CtaTemplate.CtaTextFieldId);
                bannercarouselData.GtmData = new GtmDataModel
                {
                    Event = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmEventFieldId),
                    Category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                    Banner_category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmBannerCategoryFieldId),
                    Sub_category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                    Title = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                    Index = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmIndexFieldId),
                    Label = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                    Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                };

                foreach (Item galleryItem in datasource.Children)
                {
                    var itemsData = new BannerGallery
                    {
                        MediaType = Utils.GetValue(galleryItem, BaseTemplates.MediaTypeTemplate.MediaTypeFieldId),
                        ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId),
                        IsOverlayRequired = Utils.GetBoleanValue(galleryItem, HomeBannerCarouselTemplate.CarouselItem.Fields.isOverlayRequired),
                        AutoPlay = Utils.GetBoleanValue(galleryItem, BaseTemplates.AutoPlayTemplate.AutoPlayFieldId),
                        ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                        ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                        ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId),

                        VideoSource = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceFieldId]),
                        VideoSourceMobile = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceMobileFieldId]),
                        VideoSourceTablet = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceTabletFieldId]),
                        VideoSourceOGG = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.VideoSourceOggTemplate.VideoSourceOggFieldId]),
                        VideoSourceMobileOGG = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.VideoSourceOggTemplate.VideoSourceOggMobileFieldId]),
                        VideoSourceTabletOGG = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.VideoSourceOggTemplate.VideoSourceOggTabletFieldId]),

                    };
                    itemsData.GtmData = new GtmDataModel
                    {
                        Event = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Banner_category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmBannerCategoryFieldId),
                        Sub_category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Title = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                        Index = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmIndexFieldId),
                        Label = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };
                    bannerGalleryData.Add(itemsData);
                }
                bannercarouselData.Data = bannerGalleryData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
            }
            return bannercarouselData;
        }
        public GalleryContentModel<LifeAtSchoolGalleryItem> GetLifeAtSchool(Rendering rendering)
        {
            var lifeAtSchoolData = new GalleryContentModel<LifeAtSchoolGalleryItem>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;
            try
            {
                lifeAtSchoolData.Heading = Utils.GetValue(datasource, BaseTemplates.TitleTemplate.TitleFieldId);
                lifeAtSchoolData.SubHeading = Utils.GetValue(datasource, BaseTemplates.SubHeadingTemplate.SubHeading2FieldId);
                lifeAtSchoolData.Description = Utils.GetValue(datasource, BaseTemplates.DescriptionTemplate.DescriptionFieldId);
                lifeAtSchoolData.SectionID = Utils.GetValue(datasource, BaseTemplates.SectionIDTemplate.SectionIDFieldId);
                lifeAtSchoolData.Theme = Utils.GetValue(datasource, BaseTemplates.ThemeTemplate.ThemeFieldId);
                lifeAtSchoolData.Link = Utils.GetLinkURL(datasource.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]);
                lifeAtSchoolData.LinkText = Utils.GetValue(datasource, BaseTemplates.CtaTemplate.CtaTextFieldId);
                lifeAtSchoolData.GtmData = new GtmDataModel
                {
                    Event = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmEventFieldId),
                    Category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                    Sub_category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                    Label = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                    Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                };
                lifeAtSchoolData.Gallery = new List<LifeAtSchoolGalleryItem>();

                foreach (Item galleryItem in datasource.Children)
                {
                    LinkField lf = galleryItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId];

                    var ItemsData = new LifeAtSchoolGalleryItem
                    {
                        Heading = Utils.GetValue(galleryItem, BaseTemplates.TitleTemplate.TitleFieldId),
                        Description = Utils.GetValue(galleryItem, BaseTemplates.DescriptionTemplate.DescriptionFieldId),
                        Target = lf.Target,
                        Link = Utils.GetLinkURL(lf),
                        ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                        ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                        ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId),
                        ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId)
                    };
                    ItemsData.GtmData = new GtmDataModel
                    {
                        Event = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Label = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };
                    lifeAtSchoolData.Gallery.Add(ItemsData);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
            }
            return lifeAtSchoolData;
        }
        public VideoBanner GetVideoBanner(Rendering rendering)
        {
            VideoBanner videoBannerData = new VideoBanner();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
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
                videoBannerData.WelcomeText = Utils.GetValue(datasource, BaseTemplates.TextTemplate.TextFieldId);
                videoBannerData.PlayText = Utils.GetValue(datasource, BaseTemplates.LinkTemplate.LinkTextFieldId);
                videoBannerData.PosterImage = Utils.GetImageURLByFieldId(datasource, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                videoBannerData.seoDescription = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.SeoDescriptionFieldId);
                videoBannerData.seoName = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.SeoNameFieldId);
                videoBannerData.Autoplay = Utils.GetBoleanValue(datasource, BaseTemplates.AutoPlayTemplate.AutoPlayFieldId);

                var gtmDataSource = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmDataFieldId);
                var gtmDataSourceItem = Sitecore.Context.Database.GetItem(gtmDataSource);

                videoBannerData.GtmData = new GtmDataModel();
                videoBannerData.GtmData.Event = Utils.GetValue(gtmDataSourceItem, BaseTemplates.GTMTemplate.GtmEventFieldId);
                videoBannerData.GtmData.Category = Utils.GetValue(gtmDataSourceItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId);
                videoBannerData.GtmData.Sub_category = Utils.GetValue(gtmDataSourceItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId);
                videoBannerData.GtmData.Label = Utils.GetValue(gtmDataSourceItem, BaseTemplates.GTMTemplate.GtmLabelFieldId);
                videoBannerData.GtmData.Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item);

                var gtmVideoStartSource = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmVideoStartFieldId);
                var gtmVideoStartSourceItem = Sitecore.Context.Database.GetItem(gtmVideoStartSource);

                videoBannerData.GtmVideoStart = new GtmVideoStartModel();
                videoBannerData.GtmVideoStart.Event = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmEventFieldId);
                videoBannerData.GtmVideoStart.Category = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId);
                videoBannerData.GtmVideoStart.Sub_category = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId);
                videoBannerData.GtmVideoStart.Title = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmTitleFieldId);
                videoBannerData.GtmVideoStart.Label = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmLabelFieldId);
                videoBannerData.GtmVideoStart.Video_duration = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmVideodurationFieldId);
                videoBannerData.GtmVideoStart.Video_action = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmVideoactionFieldId);
                videoBannerData.GtmVideoStart.Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item);

                var gtmVideoProgressSource = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmVideoStartFieldId);
                var gtmVideoProgressSourceItem = Sitecore.Context.Database.GetItem(gtmVideoProgressSource);

                videoBannerData.GtmVideoProgress = new GtmVideoProgressModel();
                videoBannerData.GtmVideoProgress.Event = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmEventFieldId);
                videoBannerData.GtmVideoProgress.Category = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId);
                videoBannerData.GtmVideoProgress.Sub_category = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId);
                videoBannerData.GtmVideoProgress.Title = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmTitleFieldId);
                videoBannerData.GtmVideoProgress.Label = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmLabelFieldId);
                videoBannerData.GtmVideoProgress.Video_duration = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmVideodurationFieldId);
                videoBannerData.GtmVideoProgress.Video_percent = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmVideopercentFieldId);
                videoBannerData.GtmVideoProgress.Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item);


                var gtmVideoCompleteSource = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmVideoStartFieldId);
                var gtmVideoCompleteSourceItem = Sitecore.Context.Database.GetItem(gtmVideoCompleteSource);

                videoBannerData.GtmVideoComplete = new GtmVideoStartModel();
                videoBannerData.GtmVideoComplete.Event = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.GtmEventFieldId);
                videoBannerData.GtmVideoComplete.Category = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId);
                videoBannerData.GtmVideoComplete.Sub_category = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId);
                videoBannerData.GtmVideoComplete.Title = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.GtmTitleFieldId);
                videoBannerData.GtmVideoComplete.Label = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.GtmLabelFieldId);
                videoBannerData.GtmVideoComplete.Video_duration = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.GtmVideodurationFieldId);
                videoBannerData.GtmVideoComplete.Video_action = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.GtmVideoactionFieldId);
                videoBannerData.GtmVideoComplete.Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item);


                DateField dateTimeField = datasource.Fields[BaseTemplates.GTMTemplate.UploadDateFieldId];
                if (!string.IsNullOrEmpty(dateTimeField.Value))
                {
                    string dateTimeString = dateTimeField.Value;
                    DateTime dateTimeStruct = Sitecore.DateUtil.IsoDateToDateTime(dateTimeString);
                    videoBannerData.uploadDate = System.Convert.ToString(dateTimeStruct); ;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return videoBannerData;
        }
        public MainBanner GetMainBanner(Rendering rendering)
        {
            MainBanner mainBannerData = new MainBanner();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                mainBannerData.ImageSource = Utils.GetImageURLByFieldId(datasource, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                mainBannerData.ImageSourceMobile = Utils.GetImageURLByFieldId(datasource, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
                mainBannerData.ImageSourceTablet = Utils.GetImageURLByFieldId(datasource, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);
                mainBannerData.ImageAlt = Utils.GetValue(datasource, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);
                mainBannerData.SubHeading = Utils.GetValue(datasource, BaseTemplates.SubHeadingTemplate.SubHeadingFieldId);
                mainBannerData.Heading = Utils.GetValue(datasource, BaseTemplates.HeadingTemplate.HeadingFieldId);
                mainBannerData.Link = Utils.GetLinkURL(datasource.Fields[BaseTemplates.LinkTemplate.LinkFieldId]);
                mainBannerData.LinkText = Utils.GetValue(datasource, BaseTemplates.LinkTemplate.LinkTextFieldId);
                mainBannerData.GtmData = new GtmDataModel();
                mainBannerData.GtmData.Event = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmEventFieldId);
                mainBannerData.GtmData.Category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmCategoryFieldId);
                mainBannerData.GtmData.Banner_category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmBannerCategoryFieldId);
                mainBannerData.GtmData.Title = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId);
                mainBannerData.GtmData.Label = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmLabelFieldId);
                mainBannerData.GtmData.Index = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmIndexFieldId);
                mainBannerData.GtmData.Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return mainBannerData;
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
                homeCurriculumData.SubHeading = Utils.GetValue(datasource, BaseTemplates.SubHeadingTemplate.SubHeading2FieldId);
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
                homeFAQData.SectionHeading = Utils.GetValue(datasource, BaseTemplates.TitleTemplate.TitleFieldId);
                homeFAQData.Target = Utils.GetValue(datasource, BaseTemplates.TargetTemplate.TargetFieldId);
                homeFAQData.ViewAllLink = Utils.GetLinkURL(datasource.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]);
                homeFAQData.ViewAllLabel = Utils.GetValue(datasource, BaseTemplates.CtaTemplate.CtaTextFieldId);

                homeFAQData.GtmData = new GtmDataModel
                {
                    Event = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmEventFieldId),
                    Category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                    Sub_category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                    Label = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                    Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                };

                foreach (Item galleryItem in datasource.Children)
                {
                    FAQDataItem itemsData = new FAQDataItem();
                    itemsData.Title = galleryItem.Fields[HomeFAQTemplate.Fields.question].Value;
                    itemsData.Body = galleryItem.Fields[HomeFAQTemplate.Fields.answer].Value;
                    itemsData.CategoryID = galleryItem.Fields[HomeFAQTemplate.Fields.categoryID].Value;
                    itemsData.QuestionID = galleryItem.Fields[HomeFAQTemplate.Fields.questionID].Value;
                    itemsData.CategoryHeading = galleryItem.Fields[HomeFAQTemplate.Fields.categoryID].Value;
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
    }
}