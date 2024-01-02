using Project.AdaniInternationalSchool.Website.Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using DateTime = System.DateTime;
using Project.AdaniInternationalSchool.Website.Templates;
using Project.AdaniInternationalSchool.Website.Helpers;
using System.Linq;
//using System.Web;
//using Sitecore.Data;
//using Sitecore.Data.Serialization.ObjectModel;
//using System.Web.Helpers;
//using static Project.AdaniInternationalSchool.Website.Templates.BaseTemplates;

namespace Project.AdaniInternationalSchool.Website.Services
{
    public class AdaniInternationalSchoolServices : IAdaniInternationalSchoolServices
    {
        readonly Item _contextItem;
        public AdaniInternationalSchoolServices()
        {
            _contextItem = Sitecore.Context.Item;
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

        public SubNav GetSubNav(Rendering rendering)
        {
            SubNav subNavData = new SubNav();
            List<SubNavItem> subNavItemsData = new List<SubNavItem>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                if (datasource.Children.Count > 0)
                {
                    foreach (Item galleryItem in datasource.Children)
                    {
                        SubNavItem ItemsData = new SubNavItem();
                        ItemsData.Active = Utils.GetBoleanValue(galleryItem, BaseTemplates.ActiveCheckboxTemplate.ActiveFieldID);
                        ItemsData.Target = Utils.GetValue(galleryItem, BaseTemplates.LinkTemplate.LinkTextFieldId);
                        ItemsData.Label = Utils.GetValue(galleryItem, BaseTemplates.LabelTemplate.LabelFieldId);
                        ItemsData.Url = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]);
                        ItemsData.GtmData = new GtmDataModel();
                        ItemsData.GtmData.Event = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmEventFieldId);
                        ItemsData.GtmData.Category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId);
                        ItemsData.GtmData.Sub_category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId);
                        ItemsData.GtmData.Label = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmLabelFieldId);
                        ItemsData.GtmData.Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item);
                        subNavItemsData.Add(ItemsData);
                    }
                }
                subNavData.SubNavItems = subNavItemsData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return subNavData;
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

        public SearchData GetFaqsSearchData(Rendering rendering)
        {
            SearchData searchData = new SearchData();
            List<PopularSuggestion> popularSuggestionData = new List<PopularSuggestion>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                searchData.SearchPlaceholder = Utils.GetValue(datasource, BaseTemplates.HeadingTemplate.HeadingFieldId);
                searchData.PopularSearchKeyword = Utils.GetValue(datasource, BaseTemplates.TitleTemplate.TitleFieldId);
                searchData.SuggestionKeyword = Utils.GetValue(datasource, BaseTemplates.TitleTemplate.SubTitleFieldID);

                if (datasource.Children.Count > 0)
                {
                    foreach (Item galleryItem in datasource.Children)
                    {
                        PopularSuggestion ItemsListData = new PopularSuggestion();
                        ItemsListData.ItemLink = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]);
                        ItemsListData.ItemType = Utils.GetValue(galleryItem, BaseTemplates.CtaTemplate.CtaTextFieldId);
                        ItemsListData.ItemHeading = Utils.GetValue(galleryItem, BaseTemplates.TitleTemplate.TitleFieldId);
                        ItemsListData.CategoryID = Utils.GetValue(galleryItem, BaseTemplates.TitleTemplate.SubTitleFieldID);
                        ItemsListData.GtmData = new GtmDataModel
                        {
                            Event = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                            Category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                            Sub_category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                            Label = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                            Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };
                        popularSuggestionData.Add(ItemsListData);
                    }
                }
                searchData.PopularSuggestions = popularSuggestionData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return searchData;
        }

        public BaseCards<FounderCardDataModel> GetFounderCard(Rendering rendering)
        {
            var founderCard = new BaseCards<FounderCardDataModel>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                founderCard.Variant = Utils.GetValue(datasource, BaseTemplates.VariantTemplate.VariantFieldId);

                if (datasource.Children.Count > 0)
                {
                    foreach (Item galleryItem in datasource.Children)
                    {
                        var founderCardData = new FounderCardDataModel
                        {
                            Theme = galleryItem.Fields[BaseTemplates.ThemeTemplate.ThemeFieldId].Value,
                            TextFirst = Utils.GetBoleanValue(galleryItem, BaseTemplates.TextFirstTemplate.TextFirstFieldId),
                            CardType = galleryItem.Fields[BaseTemplates.CardTypeTemplate.CardTypeFieldId].Value,
                            Heading = Utils.GetValue(galleryItem, BaseTemplates.HeadingTemplate.HeadingFieldId),
                            SubHeading = Utils.GetValue(galleryItem, BaseTemplates.SubHeadingTemplate.SubHeadingFieldId),
                            ImageAlt = galleryItem.Fields[BaseTemplates.ImageSourceTemplate.ImageAltFieldId].Value,
                            MediaType = galleryItem.Fields[BaseTemplates.MediaTypeTemplate.MediaTypeFieldId].Value,
                            Description = galleryItem.Fields[BaseTemplates.DescriptionTemplate.DescriptionFieldId].Value,
                            SubDescription = galleryItem.Fields[BaseTemplates.DescriptionTemplate.SubDescriptionFieldId].Value,
                            Link = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),
                            LinkText = Utils.GetValue(galleryItem, BaseTemplates.LinkTemplate.LinkTextFieldId),
                            ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                            ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                            ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId)
                        };
                        founderCardData.GtmData = new GtmDataModel
                        {
                            Event = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                            Category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                            Sub_category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                            Label = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                            Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };
                        founderCard.Data.Add(founderCardData);
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return founderCard;
        }

        public PoliciesPageLinkSection GetPoliciesPageLinkSection(Rendering rendering)
        {
            PoliciesPageLinkSection policiesPageLinkSection = new PoliciesPageLinkSection();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                policiesPageLinkSection.Heading = Utils.GetValue(datasource, BaseTemplates.HeadingTemplate.HeadingFieldId);
                policiesPageLinkSection.SubHeading = Utils.GetValue(datasource, BaseTemplates.SubHeadingTemplate.SubHeading2FieldId);
                policiesPageLinkSection.Description = Utils.GetValue(datasource, BaseTemplates.DescriptionTemplate.DescriptionFieldId);

                if (datasource.Children.Count > 0)
                {
                    foreach (Item galleryItem in datasource.Children)
                    {
                        LinkSectionLinkItemModel linkSectionLinkItemModel = new LinkSectionLinkItemModel();
                        var linkItem = ServiceHelper.GetLinkItem(galleryItem);
                        linkSectionLinkItemModel.LinkText = linkItem.Label;
                        linkSectionLinkItemModel.Target = linkItem.Target;
                        linkSectionLinkItemModel.Link = linkItem.Url;
                        linkSectionLinkItemModel.GtmData = new GtmDataModel();
                        linkSectionLinkItemModel.GtmData.Event = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmEventFieldId);
                        linkSectionLinkItemModel.GtmData.Category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId);
                        linkSectionLinkItemModel.GtmData.Banner_category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmBannerCategoryFieldId);
                        linkSectionLinkItemModel.GtmData.Title = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId);
                        linkSectionLinkItemModel.GtmData.Label = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmLabelFieldId);
                        linkSectionLinkItemModel.GtmData.Index = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmIndexFieldId);
                        linkSectionLinkItemModel.GtmData.Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item);
                        policiesPageLinkSection.Links.Add(linkSectionLinkItemModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return policiesPageLinkSection;
        }


        public OverviewMethod GetOverviewMethod(Rendering rendering)
        {
            OverviewMethod overviewMethodData = new OverviewMethod();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                overviewMethodData.Description = Utils.GetValue(datasource, BaseTemplates.DescriptionTemplate.DescriptionFieldId);
                overviewMethodData.Heading = Utils.GetValue(datasource, BaseTemplates.TitleTemplate.TitleFieldId);
                overviewMethodData.LinkText = Utils.GetValue(datasource, BaseTemplates.CtaTemplate.CtaTextFieldId);
                overviewMethodData.Link = Utils.GetLinkURL(datasource.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]);

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return overviewMethodData;
        }

        public WhyUsImageBanner GetWhyUsImageBanner(Rendering rendering)
        {
            WhyUsImageBanner whyUsImageBanner = new WhyUsImageBanner();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                whyUsImageBanner.ImageBanner = new ImageModel
                {
                    ImageSource = Utils.GetImageURLByFieldId(datasource, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                    ImageSourceMobile = Utils.GetImageURLByFieldId(datasource, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                    ImageSourceTablet = Utils.GetImageURLByFieldId(datasource, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId),
                    ImageAlt = Utils.GetValue(datasource, BaseTemplates.ImageSourceTemplate.ImageAltFieldId)
                };
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return whyUsImageBanner;
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

        public BaseCards<OurApproachModel> GetOurApproach(Rendering rendering)
        {
            var ourApproachData = new BaseCards<OurApproachModel>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;
            try
            {
                ourApproachData.Variant = Utils.GetValue(datasource, BaseTemplates.VariantTemplate.VariantFieldId);
                foreach (Item galleryItem in datasource.GetChildren())
                {
                    var DataItems = new OurApproachModel
                    {
                        Theme = Utils.GetValue(galleryItem, BaseTemplates.ThemeTemplate.ThemeFieldId),
                        TextFirst = Utils.GetBoleanValue(galleryItem, BaseTemplates.TextFirstTemplate.TextFirstFieldId),
                        CardType = Utils.GetValue(galleryItem, BaseTemplates.CardTypeTemplate.CardTypeFieldId),
                        MediaType = Utils.GetValue(galleryItem, BaseTemplates.MediaTypeTemplate.MediaTypeFieldId),

                        SubHeading = Utils.GetValue(galleryItem, BaseTemplates.SubHeadingTemplate.SubHeadingFieldId),
                        Heading = Utils.GetValue(galleryItem, BaseTemplates.HeadingTemplate.HeadingFieldId),
                        Link = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]),
                        LinkText = Utils.GetValue(galleryItem, BaseTemplates.CtaTemplate.CtaTextFieldId),
                        ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                        ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                        ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId),
                        Description = Utils.GetValue(galleryItem, BaseTemplates.DescriptionTemplate.DescriptionFieldId),
                        ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId),
                    };
                    DataItems.GtmData = new GtmDataModel
                    {
                        Event = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Label = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                };

                    ourApproachData.Data.Add(DataItems);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
            }
            return ourApproachData;
        }

        public OurInfrastructure GetOurInfrastructure(Rendering rendering)
        {
            OurInfrastructure ourInfrastructureData = new OurInfrastructure();
            List<InfrastructureGallery> infraGalleryData = new List<InfrastructureGallery>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;
            try
            {
                ourInfrastructureData.Heading = Utils.GetValue(datasource, BaseTemplates.TitleTemplate.TitleFieldId);
                ourInfrastructureData.SubHeading = Utils.GetValue(datasource, BaseTemplates.SubHeadingTemplate.SubHeading2FieldId);
                ourInfrastructureData.Description = Utils.GetValue(datasource, BaseTemplates.DescriptionTemplate.DescriptionFieldId);
                ourInfrastructureData.BtnLink = Utils.GetLinkURL(datasource.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]);
                ourInfrastructureData.BtnText = Utils.GetValue(datasource, BaseTemplates.CtaTemplate.CtaTextFieldId);
                ourInfrastructureData.GtmData = new GtmDataModel
                {
                    Event = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmEventFieldId),
                    Category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                    Sub_category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                    Label = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                    Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
            };


                foreach (Item galleryItem in datasource.Children)
                {

                    var ItemsData = new InfrastructureGallery
                    {
                        Label = Utils.GetValue(galleryItem, BaseTemplates.TitleTemplate.TitleFieldId),
                        ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId),
                        ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                        ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                        ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId),
                        Link = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),
                    };
                    ItemsData.GtmData = new GtmDataModel
                    {
                        Event = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Label = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                };
                    infraGalleryData.Add(ItemsData);
                }
                ourInfrastructureData.Gallery = infraGalleryData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
            }
            return ourInfrastructureData;
        }


        public GalleryContentModel<StoriesGallery> GetHomeStories(Rendering rendering)
        {
            var homeStoriesData = new GalleryContentModel<StoriesGallery>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;
            try
            {
                homeStoriesData.Heading = Utils.GetValue(datasource, BaseTemplates.TitleTemplate.TitleFieldId);
                homeStoriesData.SubHeading = Utils.GetValue(datasource, BaseTemplates.SubHeadingTemplate.SubHeading2FieldId);
                homeStoriesData.Description = Utils.GetValue(datasource, BaseTemplates.DescriptionTemplate.DescriptionFieldId);
                homeStoriesData.SectionID = Utils.GetValue(datasource, BaseTemplates.SectionIdTemplate.SectionIdFieldId);
                homeStoriesData.Theme = Utils.GetValue(datasource, BaseTemplates.ThemeTemplate.ThemeFieldId);
                homeStoriesData.Variant = Utils.GetValue(datasource, BaseTemplates.VariantTemplate.VariantFieldId);
                homeStoriesData.Link = Utils.GetLinkURL(datasource.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]);
                homeStoriesData.LinkText = Utils.GetValue(datasource, BaseTemplates.CtaTemplate.CtaTextFieldId);

                homeStoriesData.GtmData = new GtmDataModel
                {
                    Event = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmEventFieldId),
                    Category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                    Sub_category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                    Label = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                    Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
            };
                homeStoriesData.Gallery = new List<StoriesGallery>();

                foreach (Item galleryItem in datasource.Children)
                {
                    LinkField lf = galleryItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId];

                    var ItemsData = new StoriesGallery
                    {
                        Description = Utils.GetValue(galleryItem, BaseTemplates.TitleTemplate.TitleFieldId),
                        Heading = Utils.GetValue(galleryItem, BaseTemplates.SubHeadingTemplate.SubHeading2FieldId),
                        Target = lf.Target,
                        Link = Utils.GetLinkURL(lf),
                        ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId),
                        ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                        ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                        ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId)
                    };
                    ItemsData.GtmData = new GtmDataModel
                    {
                        Event = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Label = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                };
                    DateField dateTimeField = galleryItem.Fields[BaseTemplates.DateTemplate.DateFieldId];

                    if (!string.IsNullOrEmpty(dateTimeField.Value))
                    {
                        string dateTimeString = dateTimeField.Value;
                        DateTime dateTimeStruct = Sitecore.DateUtil.IsoDateToDateTime(dateTimeString);
                        ItemsData.Date = dateTimeStruct.ToString("MMM dd, yyyy");
                    }
                    homeStoriesData.Gallery.Add(ItemsData);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
            }
            return homeStoriesData;
        }


        public BaseCards<AdmissionCardGallery> GetAdmissionCard(Rendering rendering)
        {
            var admissionCardData = new BaseCards<AdmissionCardGallery>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetAdmissionCard : Datasource is empty", this);
                return admissionCardData;
            }
            try
            {
                admissionCardData.Variant = Utils.GetValue(datasource, BaseTemplates.VariantTemplate.VariantFieldId);
                foreach (Item galleryItem in datasource.GetChildren())
                {

                    AdmissionCardGallery ItemsData = new AdmissionCardGallery
                    {
                        Theme = Utils.GetValue(galleryItem, BaseTemplates.ThemeTemplate.ThemeFieldId),
                        TextFirst = Utils.GetBoleanValue(galleryItem, BaseTemplates.TextFirstTemplate.TextFirstFieldId),
                        CardType = Utils.GetValue(galleryItem, BaseTemplates.CardTypeTemplate.CardTypeFieldId),
                        MediaType = Utils.GetValue(galleryItem, BaseTemplates.MediaTypeTemplate.MediaTypeFieldId),
                        Heading = Utils.GetValue(galleryItem, BaseTemplates.HeadingTemplate.HeadingFieldId),
                        SubHeading = Utils.GetValue(galleryItem, BaseTemplates.SubHeadingTemplate.SubHeadingFieldId),
                        SubDescription = galleryItem.Fields[BaseTemplates.DescriptionTemplate.DescriptionFieldId].Value,
                        Link = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]),
                        LinkText = Utils.GetValue(galleryItem, BaseTemplates.CtaTemplate.CtaTextFieldId),
                        BackgroundImage = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                        ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId),

                    };
                    ItemsData.GtmData = new GtmDataModel
                    {
                        Event = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Label = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                };

                    admissionCardData.Data.Add(ItemsData);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
            }
            return admissionCardData;
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

        public BaseCards<WelcomeCardItemModel> GetWelcomeCard(Rendering rendering)
        {
            var welcomeCardData = new BaseCards<WelcomeCardItemModel>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                welcomeCardData.Variant = Utils.GetValue(datasource, BaseTemplates.VariantTemplate.VariantFieldId);
                foreach (Item galleryItem in datasource.GetChildren())
                {
                    var itemData = new WelcomeCardItemModel
                    {
                        Theme = Utils.GetValue(galleryItem, BaseTemplates.ThemeTemplate.ThemeFieldId),
                        TextFirst = Utils.GetBoleanValue(galleryItem, BaseTemplates.TextFirstTemplate.TextFirstFieldId),
                        CardType = Utils.GetValue(galleryItem, BaseTemplates.CardTypeTemplate.CardTypeFieldId),
                        MediaType = Utils.GetValue(galleryItem, BaseTemplates.MediaTypeTemplate.MediaTypeFieldId),
                        SubHeading = Utils.GetValue(galleryItem, BaseTemplates.SubHeadingTemplate.SubHeadingFieldId),
                        Description = Utils.GetValue(galleryItem, BaseTemplates.DescriptionTemplate.DescriptionFieldId),
                        Heading = Utils.GetValue(galleryItem, BaseTemplates.TitleTemplate.TitleFieldId),
                        Link = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]),
                        LinkText = Utils.GetValue(galleryItem, BaseTemplates.CtaTemplate.CtaTextFieldId),
                        PlayText = Utils.GetValue(galleryItem, BaseTemplates.TextTemplate.TextFieldId),
                        ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId),
                        PosterImage = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),

                        VideoSource = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceFieldId]),
                        VideoSourceMobile = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceMobileFieldId]),
                        VideoSourceTablet = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceTabletFieldId]),
                        VideoSourceOgg = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.VideoSourceOggTemplate.VideoSourceOggFieldId]),
                        VideoSourceMobileOgg = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.VideoSourceOggTemplate.VideoSourceOggMobileFieldId]),
                        VideoSourceTabletOgg = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.VideoSourceOggTemplate.VideoSourceOggTabletFieldId]),
                        DefaultVideoSourceOgg = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.DefaultVideoSourceOggTemplate.DefaultVideoSourceOggFieldId]),
                        DefaultVideoSourceMobileOgg = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.DefaultVideoSourceOggTemplate.DefaultVideoSourceOggMobileFieldId]),
                        DefaultVideoSourceTabletOgg = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.DefaultVideoSourceOggTemplate.DefaultVideoSourceOggTabletFieldId]),
                        DefaultVideoSource = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.DefaultVideoSourceTemplate.DefaultVideoSourceFieldId]),
                        DefaultVideoSourceMobile = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.DefaultVideoSourceTemplate.DefaultVideoSourceMobileFieldId]),
                        DefaultVideoSourceTablet = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.DefaultVideoSourceTemplate.DefaultVideoSourceTabletFieldId]),
                        autoplay = Utils.GetBoleanValue(galleryItem, BaseTemplates.AutoPlayTemplate.AutoPlayFieldId),
                        SeoName = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.SeoNameFieldId),
                        SeoDescription = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.SeoDescriptionFieldId)

                    };
                    //gtmdata datasource
                    var gtmDataSource = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmDataWelcomeCardFieldId);
                    if (!string.IsNullOrEmpty(gtmDataSource))
                    {
                        var gtmDataSourceItem = Sitecore.Context.Database.GetItem(gtmDataSource);
                        itemData.GtmData = new GtmDataModel
                        {
                            Event = Utils.GetValue(gtmDataSourceItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                            Category = Utils.GetValue(gtmDataSourceItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                            Sub_category = Utils.GetValue(gtmDataSourceItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                            Label = Utils.GetValue(gtmDataSourceItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                            Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };
                    }

                    //gtmvideo start datasource
                    var gtmVideoStartSource = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmVideoStartWelcomeCardFieldId);
                    if (!string.IsNullOrEmpty(gtmVideoStartSource))
                    {
                        var gtmVideoStartSourceItem = Sitecore.Context.Database.GetItem(gtmVideoStartSource);
                        itemData.GtmVideoStart = new GtmVideoStartModel
                        {
                            Event = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                            Category = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                            Sub_category = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                            Title = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.VideoDurationFieldId),
                            Label = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                            Video_duration = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmVideodurationFieldId),
                            Video_action = Utils.GetValue(gtmVideoStartSourceItem, BaseTemplates.GTMTemplate.GtmVideoactionFieldId),
                            Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };
                    }

                    //gtmvideocomplete datasource
                    var gtmVideoCompleteSource = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmVideoCompleteWelcomeCardFieldId);
                    if (!string.IsNullOrEmpty(gtmVideoCompleteSource))
                    {
                        var gtmVideoCompleteSourceItem = Sitecore.Context.Database.GetItem(gtmVideoCompleteSource);
                        itemData.GtmVideoComplete = new GtmVideoStartModel
                        {
                            Event = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                            Category = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                            Sub_category = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                            Title = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.PageTypeFieldId),
                            Label = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                            Video_duration = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.VideoDurationFieldId),
                            Video_action = Utils.GetValue(gtmVideoCompleteSourceItem, BaseTemplates.GTMTemplate.PageTypeFieldId),
                            Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };
                    }

                    //gtmvideoprogress datasource
                    var gtmVideoProgressSource = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmVideoProgressWelcomeCardFieldId);
                    if (!string.IsNullOrEmpty(gtmVideoCompleteSource))
                    {
                        var gtmVideoProgressSourceItem = Sitecore.Context.Database.GetItem(gtmVideoProgressSource);
                        itemData.GtmVideoProgress = new GtmVideoProgressModel
                        {
                            Event = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                            Category = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                            Sub_category = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                            Title = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmVideoStartEventFieldId),//
                            Label = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmVideoCompletedEventFieldId),//
                            Video_duration = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.VideoDurationFieldId),
                            Video_action = Utils.GetValue(gtmVideoProgressSourceItem, BaseTemplates.GTMTemplate.GtmVideoProgressEventFieldId),//
                            Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };
                    }

                    DateField dateTimeField = galleryItem.Fields[BaseTemplates.GTMTemplate.UploadDateFieldId];
                    if (!string.IsNullOrEmpty(dateTimeField.Value))
                    {
                        string dateTimeString = dateTimeField.Value;
                        DateTime dateTimeStruct = Sitecore.DateUtil.IsoDateToDateTime(dateTimeString);
                        itemData.uploadDate = System.Convert.ToString(dateTimeStruct);
                    }
                    welcomeCardData.Data.Add(itemData);
                };
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
            }
            return welcomeCardData;
        }

        public object GetHomeConditions(Rendering rendering)
        {
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                return new { Text = Utils.GetValue(datasource, BaseTemplates.DescriptionTemplate.DescriptionFieldId) };
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return null;
            }
        }

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


        public BaseContentModel<VisionMissionCardItem> GetVisionMissionCard(Rendering rendering)
        {
            var visionMissionData = new BaseContentModel<VisionMissionCardItem>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetVisionMissionCard : Datasource is empty", this);
                return visionMissionData;
            }
            try
            {
                visionMissionData.Heading = Utils.GetValue(datasource, BaseTemplates.TitleTemplate.TitleFieldId);
                visionMissionData.Description = Utils.GetValue(datasource, BaseTemplates.RichTextTemplate.RichTextFieldID);

                foreach (Item galleryItem in datasource.Children)
                {
                    var ItemsData = new VisionMissionCardItem
                    {
                        Theme = Utils.GetValue(galleryItem, BaseTemplates.ThemeTemplate.ThemeFieldId),
                        Target = Utils.GetValue(galleryItem, BaseTemplates.TargetTemplate.TargetFieldId),
                        ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId),
                        Heading = Utils.GetValue(galleryItem, BaseTemplates.TitleTemplate.TitleFieldId),
                        Description = Utils.GetValue(galleryItem, BaseTemplates.DescriptionTemplate.DescriptionFieldId),
                        ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                        ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                        ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId),
                        URL = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),
                        VideoDuration = Utils.GetValue(galleryItem,BaseTemplates.GTMTemplate.VideoDurationFieldId),
                    };
                    ItemsData.GtmData = new GtmDataModel
                    {
                        Event = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Label = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item),
                        Click_text = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmClicktextFieldId)
                    };

                    visionMissionData.Data.Add(ItemsData);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
            }
            return visionMissionData;
        }

        public BaseHeadingModel<CoreValuesGallery> GetCoreValues(Rendering rendering)
        {
            var coreValuesData = new BaseHeadingModel<CoreValuesGallery>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                coreValuesData.Heading = Utils.GetValue(datasource, BaseTemplates.TitleTemplate.TitleFieldId);

                foreach (Item galleryItem in datasource.Children)
                {
                    CoreValuesGallery ItemsData = new CoreValuesGallery
                    {
                        ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId),
                        Heading = Utils.GetValue(galleryItem, BaseTemplates.TitleTemplate.TitleFieldId),
                        Description = Utils.GetValue(galleryItem, BaseTemplates.RichTextTemplate.RichTextFieldID),
                        ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                        ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                        ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId),
                        URL = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),
                        SubHeading = Utils.GetValue(galleryItem, BaseTemplates.SubHeadingTemplate.SubHeadingFieldId),
                        Target = Utils.GetValue(galleryItem, BaseTemplates.TargetTemplate.TargetFieldId),

                    };
                    ItemsData.GtmData = new GtmDataModel
                    {
                        Event = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Label = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };
                    coreValuesData.Data.Add(ItemsData);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return coreValuesData;
        }

        public DescriptionModel GetLegal(Rendering rendering)
        {
            var legalData = new DescriptionModel();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                legalData.Description = Utils.GetValue(datasource, BaseTemplates.RichTextTemplate.RichTextFieldID);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return legalData;
        }

        public BaseContentModel GetOverview(Rendering rendering)
        {
            var overviewData = new BaseContentModel();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                overviewData.Heading = Utils.GetValue(datasource, BaseTemplates.TitleTemplate.TitleFieldId);
                overviewData.Description = Utils.GetValue(datasource, BaseTemplates.RichTextTemplate.RichTextFieldID);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return overviewData;
        }

        public MandatoryPublicDisclosure GetMandatoryPublicDisclosure(Rendering rendering)
        {
            MandatoryPublicDisclosure disclosureData = new MandatoryPublicDisclosure();
            List<MandatoryPublicDisclosureLinkItem> disclosureGalleryData = new List<MandatoryPublicDisclosureLinkItem>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetMandatoryPublicDisclosure : Datasource is empty", this);
                return disclosureData;
            }
            try
            {
                disclosureData.Heading = Utils.GetValue(datasource, BaseTemplates.TitleTemplate.TitleFieldId);
                disclosureData.SubHeading = Utils.GetValue(datasource, BaseTemplates.SubHeadingTemplate.SubHeadingFieldId);
                disclosureData.Description = Utils.GetValue(datasource, BaseTemplates.DescriptionTemplate.DescriptionFieldId);

                foreach (Item galleryItem in datasource.Children)
                {
                    MandatoryPublicDisclosureLinkItem itemsData = new MandatoryPublicDisclosureLinkItem();

                    itemsData.Linktext = Utils.GetValue(galleryItem, BaseTemplates.CtaTemplate.CtaTextFieldId);
                    itemsData.Link = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]);
                    itemsData.Target = Utils.GetValue(galleryItem, BaseTemplates.TargetTemplate.TargetFieldId);

                    itemsData.GtmData = new GtmDataModel
                    {
                        Event = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Label = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };
                    disclosureGalleryData.Add(itemsData);
                }

                disclosureData.Links = disclosureGalleryData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
            }
            return disclosureData;
        }

        public BaseHeadingModel<ProgramGalleryList> GetProgram(Rendering rendering)
        {
            var programData = new BaseHeadingModel<ProgramGalleryList>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                programData.Heading = Utils.GetValue(datasource, BaseTemplates.TitleTemplate.TitleFieldId);

                foreach (Item galleryItem in datasource.Children)
                {
                    ProgramGalleryList itemsData = new ProgramGalleryList();
                    itemsData.Type = Utils.GetValue(galleryItem, BaseTemplates.TypeTemplate.TypeFieldId);
                    itemsData.ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);
                    itemsData.ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                    itemsData.ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
                    itemsData.ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);
                    itemsData.Theme = Utils.GetValue(galleryItem, BaseTemplates.ThemeTemplate.ThemeFieldId);
                    itemsData.Heading = Utils.GetValue(galleryItem, BaseTemplates.TitleTemplate.TitleFieldId);
                    itemsData.Detail = Utils.GetValue(galleryItem, BaseTemplates.DescriptionTemplate.DescriptionFieldId);

                    programData.Data.Add(itemsData);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return programData;
        }

        public BaseHeadingModel<List<HolisticSubListItem>> GetHolistic(Rendering rendering)
        {
            var holisticData = new BaseHeadingModel<List<HolisticSubListItem>>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                holisticData.Heading = Utils.GetValue(datasource, BaseTemplates.TitleTemplate.TitleFieldId);

                var folders = datasource.Children;

                foreach (Item folder in folders)
                {
                    int counter = 1;
                    var holisticSubList = new List<HolisticSubListItem>();

                    foreach (Item galleryItem in folder.Children)
                    {
                        var itemsData = new HolisticSubListItem();
                        itemsData.Columns = counter;
                        itemsData.Theme = Utils.GetValue(galleryItem, BaseTemplates.ThemeTemplate.ThemeFieldId);
                        itemsData.Heading = Utils.GetValue(galleryItem, BaseTemplates.TitleTemplate.TitleFieldId);
                        itemsData.Detail = Utils.GetValue(galleryItem, BaseTemplates.DescriptionTemplate.DescriptionFieldId);
                        itemsData.Type = Utils.GetValue(galleryItem, BaseTemplates.TypeTemplate.TypeFieldId);
                        itemsData.ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);
                        itemsData.ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                        itemsData.ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
                        itemsData.ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);
                        holisticSubList.Add(itemsData);
                        counter++;
                    }
                    holisticData.Data.Add(holisticSubList);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return holisticData;
        }

        private List<SideNavItemModel> GetSubNav(Item item)
        {
            if (item != null)
            {
                var sideNav = item.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Side Navigation"));
                var list = new List<SideNavItemModel>();
                foreach (Item child in sideNav.Children)
                {
                    var linkItem = ServiceHelper.GetLinkItem(child);
                    list.Add(new SideNavItemModel
                    {
                        Title = Utils.GetValue(child, BaseTemplates.CtaTemplate.CtaTextFieldId),
                        Active = _contextItem.Paths.ContentPath.EndsWith(linkItem.Url, StringComparison.CurrentCultureIgnoreCase),
                        Link = Utils.GetLinkURL(child?.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]),
                        GtmEvent = Utils.GetValue(child, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        GtmCategory = Utils.GetValue(child, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        GtmSubCategory = Utils.GetValue(child, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        PageType = Utils.GetValue(child, BaseTemplates.GTMTemplate.PageTypeFieldId)

                    });
                }
                return list;
            }

            return null;
        }

        private TransportRoutes GetRouteDetails(Item item, List<TransportRouteDetails> routeListData)
        {
            var dataSource = item.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Routes"))?.Children.FirstOrDefault();
            TransportRoutes transportRoutesData = new TransportRoutes();
            transportRoutesData.Heading = Utils.GetValue(dataSource, BaseTemplates.TitleTemplate.TitleFieldId);
            transportRoutesData.Details = Utils.GetValue(dataSource, BaseTemplates.DescriptionTemplate.DescriptionFieldId);

            var index = 0;
            var list = routeListData;
            foreach (Item child in dataSource.Children)
            {
                list.Add(new TransportRouteDetails
                {
                    Heading = Utils.GetValue(child, BaseTemplates.TitleTemplate.TitleFieldId),
                    BasePoint = Utils.GetValue(child, TransportBusDetailTemplate.Fields.BasePoint),
                    BasePointName = Utils.GetValue(child, TransportBusDetailTemplate.Fields.BasePointName),
                    Timings = Utils.GetValue(child, TransportBusDetailTemplate.Fields.Timings),
                    ImageAlt = Utils.GetValue(child, BaseTemplates.ImageSourceTemplate.ImageAltFieldId),
                    ImageSource = Utils.GetImageURLByFieldId(child, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                    ImageSourceMobile = Utils.GetImageURLByFieldId(child, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                    ImageSourceTablet = Utils.GetImageURLByFieldId(child, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId)
                });
                index++;
            }
            transportRoutesData.Routes = list;
            return transportRoutesData;
        }

        public TransportModel GetTransportRouteDetails(Rendering rendering)
        {
            TransportModel transportData = new TransportModel();
            List<TransportRouteDetails> routeListData = new List<TransportRouteDetails>();
            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetTransportRouteDetails : Datasource is empty", this);
                return transportData;
            }
            try
            {
                transportData.SideNav = GetSubNav(datasource);
                transportData.RouteDetails = GetRouteDetails(datasource, routeListData);

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
            }
            return transportData;
        }


        public BaseContentModel<AffiliationItem> GetAffiliation(Rendering rendering)
        {
            var affiliationData = new BaseContentModel<AffiliationItem>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetAffiliation : Datasource is empty", this);
                return affiliationData;
            }
            try
            {
                affiliationData.Heading = Utils.GetValue(datasource, BaseTemplates.TitleTemplate.TitleFieldId);
                affiliationData.Description = Utils.GetValue(datasource, BaseTemplates.DescriptionTemplate.DescriptionFieldId);

                foreach (Item galleryItem in datasource.GetChildren())
                {
                    AffiliationItem DataItems = new AffiliationItem
                    {
                        ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                        ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                        ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId),
                        Target = Utils.GetValue(galleryItem, BaseTemplates.TargetTemplate.TargetFieldId),
                        ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId),
                        Url = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),
                    };
                    DataItems.GtmData = new GtmDataModel
                    {
                        Event = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Label = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };
                    affiliationData.Data.Add(DataItems);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
            }
            return affiliationData;
        }

        public LatestStories GetLatestStories(Rendering rendering)
        {
            LatestStories latestStories = new LatestStories();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                latestStories.Heading = Utils.GetValue(datasource, BaseTemplates.HeadingTemplate.HeadingFieldId);
                latestStories.NoData = Utils.GetValue(datasource, BaseTemplates.NoDataTemplate.NoDataFieldId);
                latestStories.ShowDataDesktop = Convert.ToInt32(Utils.GetValue(datasource, BaseTemplates.TitleTemplate.TitleFieldId));
                latestStories.ShowDataMobile = Convert.ToInt32(Utils.GetValue(datasource, BaseTemplates.TitleTemplate.SubTitleFieldID));
                latestStories.StoriesData = new List<StoryItemModel>();

                foreach (Item item in datasource.GetChildren())
                {
                    StoryItemModel cardsData = new StoryItemModel();

                    cardsData.Id = Utils.GetValue(item, BaseTemplates.CtaTemplate.CtaTextFieldId);
                    cardsData.ImageAlt = Utils.GetValue(item, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);
                    cardsData.CardHeading = Utils.GetValue(item, BaseTemplates.HeadingTemplate.HeadingFieldId);
                    cardsData.CardDescription = item.Fields[BaseTemplates.DescriptionTemplate.DescriptionFieldId].Value;
                    cardsData.ImageSource = Utils.GetImageURLByFieldId(item, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                    cardsData.ImageSourceMobile = Utils.GetImageURLByFieldId(item, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
                    cardsData.ImageSourceTablet = Utils.GetImageURLByFieldId(item, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);
                    cardsData.CardLink = Utils.GetLinkURL(item.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]);
                    Sitecore.Data.Fields.DateField dateTimeField = item.Fields[BaseTemplates.DateTemplate.DateFieldId];
                    cardsData.GtmData = new GtmDataModel
                    {
                        Event = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Category = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Label = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };

                    if (!string.IsNullOrEmpty(dateTimeField.Value))
                    {
                        string dateTimeString = dateTimeField.Value;
                        DateTime dateTimeStruct = Sitecore.DateUtil.IsoDateToDateTime(dateTimeString);
                        cardsData.Date = dateTimeStruct;
                        cardsData.CardDate = String.Format("{0:d MMM yyyy}", dateTimeStruct);
                    }

                    cardsData.Filter = new List<Filter>();



                    foreach (Item galleryItem in item.Children)
                    {
                        Filter filter = new Filter();
                        filter.Placeholder = Utils.GetValue(galleryItem, BaseTemplates.HeadingTemplate.HeadingFieldId);
                        filter.Label = Utils.GetValue(galleryItem, BaseTemplates.TitleTemplate.TitleFieldId);
                        filter.Id = Utils.GetValue(galleryItem, BaseTemplates.TitleTemplate.SubTitleFieldID);

                        cardsData.Filter.Add(filter);
                    }
                    latestStories.StoriesData.Add(cardsData);
                }

                latestStories.StoriesData = latestStories.StoriesData.OrderByDescending(x => x.Date).ToList();
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return latestStories;
        }


        public InfrastructureCategory GetInfrastructureCategory(Rendering rendering)
        {
            InfrastructureCategory InfrastructureData = new InfrastructureCategory();
            List<InfrastructureCategoryList> InfrastructureCategoryGalleryData = new List<InfrastructureCategoryList>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetInfrastructureCategory : Datasource is empty", this);
                return InfrastructureData;
            }
            try
            {
                foreach (Item galleryItem in datasource.GetChildren())
                {
                    InfrastructureCategoryList dataItems = new InfrastructureCategoryList();
                    dataItems.ImgTitle = Utils.GetValue(galleryItem, BaseTemplates.TitleTemplate.TitleFieldId);
                    dataItems.ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                    dataItems.ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
                    dataItems.ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);
                    dataItems.Target = Utils.GetValue(galleryItem, BaseTemplates.TargetTemplate.TargetFieldId);
                    dataItems.ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);
                    dataItems.Url = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]);
                    dataItems.GtmData = new GtmDataModel
                    {
                        Event = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Label = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };

                    InfrastructureCategoryGalleryData.Add(dataItems);
                }
                InfrastructureData.Features = InfrastructureCategoryGalleryData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
            }
            return InfrastructureData;
        }


        public InfrastructureDetailPage GetInfrastructureDetail(Rendering rendering)
        {
            InfrastructureDetailPage detailData = new InfrastructureDetailPage();
            InfrastructureDetailStory storyData = new InfrastructureDetailStory();
            InfraOverview overviewData = new InfraOverview();
            List<InfrastructureDetailCarousel> carouselData = new List<InfrastructureDetailCarousel>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetInfrastructureDetail : Datasource is empty", this);
                return detailData;
            }
            try
            {
                detailData.BackLabel = Utils.GetValue(datasource, BaseTemplates.BackLabelTemplate.BackLabelFieldId);
                detailData.GtmData = new GtmDataModel
                {
                    Event = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmEventFieldId),
                    Category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                    Sub_category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                    Label = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                    Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                };
                int count = 1;

                foreach (Item galleryItem in datasource.Children)
                {
                    InfrastructureDetailCarousel dataItems = new InfrastructureDetailCarousel();
                    dataItems.Id = count;
                    dataItems.ImageTitle = Utils.GetValue(galleryItem, BaseTemplates.TitleTemplate.TitleFieldId);
                    dataItems.ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                    dataItems.ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
                    dataItems.ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);
                    dataItems.ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);

                    dataItems.ThumbImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ThumbnailImageTemplate.ThumbnailImageSourceFieldId);
                    dataItems.ThumbImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ThumbnailImageTemplate.ThumbnailImageSourceMobileFieldId);
                    dataItems.ThumbImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ThumbnailImageTemplate.ThumbnailImageSourceTabletFieldId);
                    dataItems.ThumbImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ThumbnailImageTemplate.ThumbnailImageAltFieldId);
                    carouselData.Add(dataItems);
                }
                storyData.CarouselData = carouselData;
                detailData.StoryCarousel = storyData;
                overviewData.Heading = Utils.GetValue(datasource, BaseTemplates.TitleTemplate.TitleFieldId);
                overviewData.Description = Utils.GetValue(datasource, BaseTemplates.DescriptionTemplate.DescriptionFieldId);

                detailData.StoryOverview = overviewData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
            }
            return detailData;
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
                cookiesData.Heading = Utils.GetValue(datasource, BaseTemplates.TitleTemplate.TitleFieldId);
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

        public CareerFormData GetCareerForm(Rendering rendering)
        {
            CareerFormData careerFormRoot = new CareerFormData();

            careerFormRoot.FormFields = new List<FormFieldSection>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetOurApproach : Datasource is empty", this);
                return careerFormRoot;
            }

            try
            {
                careerFormRoot.AntiforgeryToken = Utils.GetAntiForgeryToken();
                careerFormRoot.SectionID = "careerForm";
                careerFormRoot.SectionHeading = "Career Form";
                careerFormRoot.Theme = "aliceBlue";

                var MobileNumber = CareerFormTemplate.Fields.TemplatesSection.MobileNumber;
                var Email = CareerFormTemplate.Fields.TemplatesSection.Email;
                var Organization = CareerFormTemplate.Fields.TemplatesSection.Organization;
                var Experience = CareerFormTemplate.Fields.TemplatesSection.Experience;
                var NameField = CareerFormTemplate.Fields.TemplatesSection.NameField;
                var PositionField = CareerFormTemplate.Fields.TemplatesSection.PositionField;
                var ResumeField = CareerFormTemplate.Fields.TemplatesSection.ResumeField;
                var AgreementField = CareerFormTemplate.Fields.TemplatesSection.AgreementField;
                var SubmitField = CareerFormTemplate.Fields.TemplatesSection.SubmitField;

                var sectionItem = datasource.Children.FirstOrDefault()?.Children.FirstOrDefault();
                if (sectionItem != null)
                {
                    ErrorMessageReCaptchaField errorMessageReCaptchaField = new ErrorMessageReCaptchaField();
                    ErrorMessageCheckField errorMessageCheckField = new ErrorMessageCheckField();
                    var errMsgfieldItem = Sitecore.Context.Database.GetItem(CareerFormTemplate.Fields.TemplatesSection.errMsgfieldItem);
                    var thankYouDatafieldItem = Sitecore.Context.Database.GetItem(CareerFormTemplate.Fields.TemplatesSection.thankYouDatafieldItem);
                    var reCaptchaFieldfieldItem = Sitecore.Context.Database.GetItem(CareerFormTemplate.Fields.TemplatesSection.reCaptchaFieldfieldItem);
                    var formGTMDatafieldItem = Sitecore.Context.Database.GetItem(CareerFormTemplate.Fields.TemplatesSection.formGTMDatafieldItem);
                    var progressDatafieldItem = Sitecore.Context.Database.GetItem(CareerFormTemplate.Fields.TemplatesSection.progressDatafieldItem);
                    var formFailDatafieldItem = Sitecore.Context.Database.GetItem(CareerFormTemplate.Fields.TemplatesSection.formFailDatafieldItem);
                    foreach (Item field in sectionItem.Children)
                    {

                        if (field.ID == MobileNumber || field.ID == NameField || field.ID == Email || field.ID == Organization)
                        {
                            FormFieldSection formFieldSection = new FormFieldSection();
                            formFieldSection.Placeholder = field.Fields[CareerFormTemplate.Fields.FormFieldsSection.PlaceholderText].Value;
                            formFieldSection.MinRequiredLength = Convert.ToInt32(field.Fields[CareerFormTemplate.Fields.FormFieldsSection.minRequiredLength].Value);
                            formFieldSection.MaxAllowedLength = Convert.ToInt32(field.Fields[CareerFormTemplate.Fields.FormFieldsSection.maxAllowedLength].Value);
                            formFieldSection.IsClear = true;
                            formFieldSection.Required = Utils.GetBoleanValue(field, CareerFormTemplate.Fields.FormFieldsSection.Required);
                            formFieldSection.FieldName = field.Fields[CareerFormTemplate.Fields.FormFieldsSection.Title].Value;
                            formFieldSection.FieldType = field.Fields[CareerFormTemplate.Fields.FormFieldsSection.DefaultValue].Value;

                            if (field.ID == NameField)
                            {
                                ErrorMessage errorMessage = new ErrorMessage();
                                errorMessage.MaxLengthErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.maxLengthErrorMessageName].Value;
                                errorMessage.MinLengthErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.minLengthErrorMessageName].Value;
                                errorMessage.RegexErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.regexErrorMessageName].Value;
                                errorMessage.RequiredFieldErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.requiredFieldErrorMessageName].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }
                            if (field.ID == MobileNumber)
                            {
                                ErrorMessage errorMessage = new ErrorMessage();
                                errorMessage.MaxLengthErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.maxLengthErrorMessageMobileNumber].Value;
                                errorMessage.MinLengthErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.minLengthErrorMessageMobileNumber].Value;
                                errorMessage.RegexErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.regexErrorMessageMobileNumber].Value;
                                errorMessage.RequiredFieldErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.requiredFieldErrorMessageMobileNumber].Value;
                                formFieldSection.ErrorMessages = errorMessage;

                            }
                            if (field.ID == Email)
                            {
                                ErrorMessage errorMessage = new ErrorMessage();
                                errorMessage.MaxLengthErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.maxLengthErrorMessageEmail].Value;
                                errorMessage.MinLengthErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.minLengthErrorMessageEmail].Value;
                                errorMessage.RegexErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.regexErrorMessageEmail].Value;
                                errorMessage.RequiredFieldErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.requiredFieldErrorMessageEmail].Value;
                                formFieldSection.ErrorMessages = errorMessage;

                            }
                            if (field.ID == Organization)
                            {
                                ErrorMessage errorMessage = new ErrorMessage();
                                errorMessage.MaxLengthErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.maxLengthErrorMessageCurrentOrganisation].Value;
                                errorMessage.MinLengthErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.minLengthErrorMessageCurrentOrganisation].Value;
                                errorMessage.RegexErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.regexErrorMessageCurrentOrganisation].Value;
                                errorMessage.RequiredFieldErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.requiredFieldErrorMessageCurrentOrganisation].Value;
                                formFieldSection.ErrorMessages = errorMessage;

                            }
                            careerFormRoot.FormFields.Add(formFieldSection);
                        }
                        if (field.ID == PositionField || field.ID == Experience)
                        {
                            FormFieldSection formFieldSection = new FormFieldSection();
                            formFieldSection.Placeholder = field.Fields[CareerFormTemplate.Fields.FormFieldsSection.ValueProviderParameters].Value;
                            if (!string.IsNullOrEmpty(field.Fields[CareerFormTemplate.Fields.FormFieldsSection.minRequiredLength].Value))
                            {
                                formFieldSection.MinRequiredLength = Convert.ToInt32(field.Fields[CareerFormTemplate.Fields.FormFieldsSection.minRequiredLength].Value);
                            }
                            if (!string.IsNullOrEmpty(field.Fields[CareerFormTemplate.Fields.FormFieldsSection.maxAllowedLength].Value))
                            {
                                formFieldSection.MaxAllowedLength = Convert.ToInt32(field.Fields[CareerFormTemplate.Fields.FormFieldsSection.maxAllowedLength].Value);
                            }
                            formFieldSection.IsClear = true;
                            formFieldSection.Required = Utils.GetBoleanValue(field, CareerFormTemplate.Fields.FormFieldsSection.Required);
                            formFieldSection.FieldName = field.Fields[CareerFormTemplate.Fields.FormFieldsSection.Title].Value;
                            formFieldSection.FieldType = field.Fields[CareerFormTemplate.Fields.FormFieldsSection.DefaultSelection].Value;
                            List<FieldOption> FieldOptionlist = new List<FieldOption>();
                            if (field.ID == PositionField)
                            {
                                var positionid = 0;
                                var positionField = field.Children.FirstOrDefault()?.Children.FirstOrDefault();
                                foreach (Item position in positionField.Children)
                                {
                                    if (positionid != 0)
                                    {
                                        FieldOption fieldOption = new FieldOption();
                                        fieldOption.Label = position.Fields[CareerFormTemplate.Fields.FormFieldsSection.fieldOptionslabel].Value;
                                        fieldOption.Id = positionid.ToString();
                                        FieldOptionlist.Add(fieldOption);
                                    }
                                    positionid++;
                                }
                                formFieldSection.FieldOptions = FieldOptionlist;
                            }
                            if (field.ID == Experience)
                            {
                                var experienceid = 0;
                                var ExperienceField = field.Children.FirstOrDefault()?.Children.FirstOrDefault();
                                foreach (Item experienceField in ExperienceField.Children)
                                {
                                    if (experienceid != 0)
                                    {
                                        FieldOption fieldOption = new FieldOption();
                                        fieldOption.Label = experienceField.Fields[CareerFormTemplate.Fields.FormFieldsSection.fieldOptionslabel].Value;
                                        fieldOption.Id = experienceid.ToString();
                                        FieldOptionlist.Add(fieldOption);
                                    }
                                    experienceid++;
                                }
                                formFieldSection.FieldOptions = FieldOptionlist;
                            }
                            ErrorMessage errorMessage = new ErrorMessage();
                            if (field.ID == PositionField)
                            {
                                errorMessage.RequiredFieldErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.requiredFieldErrorMessageInterestedPosition].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }
                            if (field.ID == Experience)
                            {
                                errorMessage.RequiredFieldErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.requiredFieldErrorMessageTotalExperience].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }
                            careerFormRoot.FormFields.Add(formFieldSection);
                        }
                        if (field.ID == ResumeField)
                        {
                            FormFieldSection formFieldSection = new FormFieldSection();
                            formFieldSection.Placeholder = field.Fields[CareerFormTemplate.Fields.FormFieldsSection.AllowedContentTypes].Value;
                            if (!string.IsNullOrEmpty(field.Fields[CareerFormTemplate.Fields.FormFieldsSection.minRequiredLength].Value))
                            {
                                formFieldSection.MinRequiredLength = Convert.ToInt32(field.Fields[CareerFormTemplate.Fields.FormFieldsSection.minRequiredLength].Value);
                            }
                            if (!string.IsNullOrEmpty(field.Fields[CareerFormTemplate.Fields.FormFieldsSection.maxAllowedLength].Value))
                            {
                                formFieldSection.MaxAllowedLength = Convert.ToInt32(field.Fields[CareerFormTemplate.Fields.FormFieldsSection.maxAllowedLength].Value);
                            }
                            formFieldSection.MaxAllowedFileSize = Convert.ToInt32(field.Fields[CareerFormTemplate.Fields.FormFieldsSection.MaxFileSize].Value);
                            formFieldSection.MinRequiredFileSize = 0.001;
                            formFieldSection.IsClear = true;
                            formFieldSection.FieldType = "file";
                            formFieldSection.Required = Utils.GetBoleanValue(field, CareerFormTemplate.Fields.FormFieldsSection.Required);
                            formFieldSection.FieldName = field.Fields[CareerFormTemplate.Fields.FormFieldsSection.Title].Value;
                            ErrorMessage errorMessage = new ErrorMessage();
                            errorMessage.MinFileSizeErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.minFileSizeErrorMessageAttachResume].Value;
                            errorMessage.MaxFileSizeErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.maxFileSizeErrorMessageAttachResume].Value;
                            errorMessage.RegexErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.regexErrorMessageAttachResume].Value;
                            errorMessage.RequiredFieldErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.requiredFieldErrorMessageAttachResume].Value;
                            formFieldSection.FieldName = "AttachResume";
                            formFieldSection.FieldDescription = field.Fields[CareerFormTemplate.Fields.FormFieldsSection.Title].Value;
                            errorMessage.MinFileSizeErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.minFileSizeErrorMessageAttachResume].Value;
                            errorMessage.MaxFileSizeErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.maxFileSizeErrorMessageAttachResume].Value;
                            errorMessage.RegexErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.regexErrorMessageAttachResume].Value;
                            errorMessage.RequiredFieldErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.requiredFieldErrorMessageAttachResume].Value;
                            formFieldSection.ErrorMessages = errorMessage;
                            careerFormRoot.FormFields.Add(formFieldSection);
                        }
                        if (field.ID == AgreementField)
                        {
                            CheckField checkFieldSection = new CheckField();
                            checkFieldSection.Placeholder = field.Fields[CareerFormTemplate.Fields.FormFieldsSection.ValueProviderParameters].Value;
                            checkFieldSection.Required = Utils.GetBoleanValue(field, CareerFormTemplate.Fields.FormFieldsSection.Required);
                            checkFieldSection.Selected = Utils.GetBoleanValue(field, CareerFormTemplate.Fields.FormFieldsSection.CheckboxDefaultValue);
                            checkFieldSection.FieldName = field.Fields[CareerFormTemplate.Fields.FormFieldsSection.Title].Value;
                            checkFieldSection.Url = Utils.GetLinkURL(errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.checkboxFieldUrl]);
                            checkFieldSection.Target = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.checkboxFieldtarget].Value;
                            checkFieldSection.FieldID = "acceptTerms";
                            checkFieldSection.FieldType = "checkbox";
                            errorMessageCheckField.RequiredFieldErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.requiredFieldErrorMessageTermsAndConditions].Value;
                            checkFieldSection.ErrorMessages = errorMessageCheckField;
                            careerFormRoot.CheckboxField = checkFieldSection;
                        }
                        if (field.ID == SubmitField)
                        {
                            SubmitButtonText submitButtonText = new SubmitButtonText();
                            submitButtonText.ButtonText = field.Fields[CareerFormTemplate.Fields.FormFieldsSection.Title].Value;
                            careerFormRoot.SubmitButton = submitButtonText;
                        }
                        if (field != null)
                        {
                            ProgressData progressData = new ProgressData();
                            progressData.Heading = progressDatafieldItem.Fields[CareerFormTemplate.Fields.ThankYouDataSection.heading].Value;
                            progressData.Description = progressDatafieldItem.Fields[CareerFormTemplate.Fields.ThankYouDataSection.description].Value;
                            careerFormRoot.ProgressData = progressData;

                            FormFailData formFailData = new FormFailData();
                            formFailData.Heading = formFailDatafieldItem.Fields[CareerFormTemplate.Fields.ThankYouDataSection.heading].Value;
                            formFailData.Description = formFailDatafieldItem.Fields[CareerFormTemplate.Fields.ThankYouDataSection.description].Value;
                            careerFormRoot.FormFailData = formFailData;

                            ThankYouData thankYouData = new ThankYouData();
                            thankYouData.Heading = thankYouDatafieldItem.Fields[CareerFormTemplate.Fields.ThankYouDataSection.heading].Value;
                            thankYouData.Description = thankYouDatafieldItem.Fields[CareerFormTemplate.Fields.ThankYouDataSection.description].Value;
                            careerFormRoot.ThankYouData = thankYouData;


                            FormGTMData formGTMData = new FormGTMData();
                            formGTMData.SubmitEvent = formGTMDatafieldItem.Fields[CareerFormTemplate.Fields.FormGTMDataSection.gtmEvent].Value;
                            formGTMData.GtmCategory = formGTMDatafieldItem.Fields[CareerFormTemplate.Fields.FormGTMDataSection.gtmCategory].Value;
                            formGTMData.GtmSubCategory = formGTMDatafieldItem.Fields[CareerFormTemplate.Fields.FormGTMDataSection.gtmSubCategory].Value;
                            formGTMData.FailEvent = formGTMDatafieldItem.Fields[CareerFormTemplate.Fields.FormGTMDataSection.gtmEventSub].Value;
                            formGTMData.PageType = formGTMDatafieldItem.Fields[CareerFormTemplate.Fields.FormGTMDataSection.pageType].Value;
                            careerFormRoot.FormGTMData = formGTMData;

                            ReCaptchaField reCaptchaField = new ReCaptchaField();
                            reCaptchaField.FieldType = reCaptchaFieldfieldItem.Fields[CareerFormTemplate.Fields.ReCaptchaFieldSection.title].Value;
                            reCaptchaField.FieldName = reCaptchaFieldfieldItem.Fields[CareerFormTemplate.Fields.ReCaptchaFieldSection.heading].Value;
                            reCaptchaField.FieldID = reCaptchaFieldfieldItem.Fields[CareerFormTemplate.Fields.ReCaptchaFieldSection.subTitle].Value;
                            errorMessageReCaptchaField.RequiredFieldErrorMessage = errMsgfieldItem.Fields[CareerFormTemplate.Fields.ErrorMessagesSection.requiredFieldErrorMessagereCaptcha].Value;
                            reCaptchaField.ErrorMessages = errorMessageReCaptchaField;
                            reCaptchaField.Required = Utils.GetBoleanValue(reCaptchaFieldfieldItem, CareerFormTemplate.Fields.ReCaptchaFieldSection.required);
                            careerFormRoot.ReCaptchaField = reCaptchaField;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
            }
            return careerFormRoot;
        }


        public List<SiteMapXML> GetSiteMapXML(Rendering rendering)
        {
            List<SiteMapXML> siteMapXMLList = new List<SiteMapXML>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetMainBanner : Datasource is empty", this);
                return null;
            }
            try
            {
                foreach (Item item in datasource.Children)
                {

                    var descPriority = Utils.GetValue(item, Templates.SitemapXML.SitemapXMLFeilds.Title);
                    var siteMapXML = new SiteMapXML
                    {
                        DescPriority = Convert.ToDouble(descPriority),
                        Priority = Utils.GetValue(item, Templates.SitemapXML.SitemapXMLFeilds.SubTitle),
                        Url = Utils.GetLinkURL(item?.Fields[Templates.SitemapXML.SitemapXMLFeilds.Link])
                    };
                    DateField dateTimeField = item.Fields[Templates.SitemapXML.SitemapXMLFeilds.Datetime];
                    if (!string.IsNullOrEmpty(dateTimeField.Value))
                    {
                        string dateTimeString = dateTimeField.Value;
                        DateTime dateTimeStruct = Sitecore.DateUtil.IsoDateToDateTime(dateTimeString);
                        siteMapXML.Lastmod = System.Convert.ToString(dateTimeStruct); ;
                    }
                    siteMapXMLList.Add(siteMapXML);
                }

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return siteMapXMLList;
        }

    }
}