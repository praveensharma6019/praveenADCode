using Project.AdaniInternationalSchool.Website.Helpers;
using Project.AdaniInternationalSchool.Website.LearningPage;
using Project.AdaniInternationalSchool.Website.Models;
using Project.AdaniInternationalSchool.Website.Templates;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using DateTime = System.DateTime;

namespace Project.AdaniInternationalSchool.Website.Services.LearningPage
{
    public class AISLearningPageServices : IAISLearningPageServices
    {
        public LearningStoryList GetLearningStoryList(Rendering rendering)
        {
            LearningStoryList learningStoryListData = new LearningStoryList();
            List<LearningStoryListDataModel> learningStoryListDataModelData = new List<LearningStoryListDataModel>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                learningStoryListData.Heading = Utils.GetValue(datasource, BaseTemplates.HeadingTemplate.HeadingFieldId);
                learningStoryListData.LinkText = Utils.GetValue(datasource, BaseTemplates.CtaTemplate.CtaTextFieldId);
                learningStoryListData.Link = Utils.GetLinkURL(datasource?.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]);
                learningStoryListData.GtmData = new GtmDataModel();
                learningStoryListData.GtmData.Event = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmEventFieldId);
                learningStoryListData.GtmData.Category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmCategoryFieldId);
                learningStoryListData.GtmData.Sub_category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId);
                learningStoryListData.GtmData.Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item);
                learningStoryListData.GtmData.Label = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmLabelFieldId);

                foreach (Item galleryItem in datasource.Children)
                {
                    LearningStoryListDataModel ItemsListData = new LearningStoryListDataModel();
                    ItemsListData.EventTxt = Utils.GetValue(galleryItem, BaseTemplates.HeadingTemplate.HeadingFieldId);
                    ItemsListData.UpcomingEvent = Utils.GetBoleanValue(galleryItem, BaseTemplates.TextFirstTemplate.TextFirstFieldId);
                    ItemsListData.StoryHeading = Utils.GetValue(galleryItem, BaseTemplates.TitleTemplate.TitleFieldId);
                    var linkItem = ServiceHelper.GetLinkItem(galleryItem);
                    ItemsListData.Link = linkItem.Url;
                    ItemsListData.Target = linkItem.Target;

                    ItemsListData.ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                    ItemsListData.ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);
                    ItemsListData.GtmData = new GtmDataModel();
                    ItemsListData.GtmData.Event = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmEventFieldId);
                    ItemsListData.GtmData.Category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId);
                    ItemsListData.GtmData.Sub_category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId);
                    ItemsListData.GtmData.Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item);
                    ItemsListData.GtmData.Label = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmLabelFieldId);
                    Sitecore.Data.Fields.DateField dateTimeField = galleryItem.Fields[BaseTemplates.DateTemplate.DateFieldId];

                    if (!string.IsNullOrEmpty(dateTimeField.Value))
                    {
                        string dateTimeString = dateTimeField.Value;
                        DateTime dateTimeStruct = Sitecore.DateUtil.IsoDateToDateTime(dateTimeString);
                        ItemsListData.EventDate = String.Format("{0:d MMM yyyy}", dateTimeStruct);
                    }
                    learningStoryListDataModelData.Add(ItemsListData);
                }
                learningStoryListData.StoryList = learningStoryListDataModelData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return learningStoryListData;
        }

        public BaseContentModel<FindOutMoreDataModel> GetFindOutMore(Rendering rendering)
        {
            var findOutMoreData = new BaseContentModel<FindOutMoreDataModel>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                findOutMoreData.Description = Utils.GetValue(datasource, BaseTemplates.DescriptionTemplate.DescriptionFieldId);
                findOutMoreData.Heading = Utils.GetValue(datasource, BaseTemplates.HeadingTemplate.HeadingFieldId);


                foreach (Item galleryItem in datasource.Children)
                {
                    FindOutMoreDataModel FindOutMoreDataModelData = new FindOutMoreDataModel();
                    FindOutMoreDataModelData.Theme = Utils.GetValue(galleryItem, BaseTemplates.ThemeTemplate.ThemeFieldId);
                    FindOutMoreDataModelData.Heading = Utils.GetValue(galleryItem, BaseTemplates.HeadingTemplate.HeadingFieldId);
                    FindOutMoreDataModelData.Description = Utils.GetValue(galleryItem, BaseTemplates.DescriptionTemplate.DescriptionFieldId);
                    FindOutMoreDataModelData.ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                    FindOutMoreDataModelData.ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
                    FindOutMoreDataModelData.ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);
                    FindOutMoreDataModelData.ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);
                    FindOutMoreDataModelData.Url = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]);
                    FindOutMoreDataModelData.Label = Utils.GetValue(galleryItem, BaseTemplates.CtaTemplate.CtaTextFieldId);
                    FindOutMoreDataModelData.Target = Utils.GetValue(galleryItem, BaseTemplates.LinkTemplate.LinkTextFieldId);
                    FindOutMoreDataModelData.GtmData = new GtmDataModel();
                    FindOutMoreDataModelData.GtmData.Event = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmEventFieldId);
                    FindOutMoreDataModelData.GtmData.Category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId);
                    FindOutMoreDataModelData.GtmData.Sub_category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId);
                    FindOutMoreDataModelData.GtmData.Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item);
                    FindOutMoreDataModelData.GtmData.Label = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmLabelFieldId);
                    findOutMoreData.Data.Add(FindOutMoreDataModelData);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return findOutMoreData;
        }

        public SteamCard GetSteamCard(Rendering rendering)
        {
            SteamCard steamCardData = new SteamCard();
            List<SteamCardDataModel> steamCardDataModelData = new List<SteamCardDataModel>();
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                steamCardData.Heading = Utils.GetValue(datasource, BaseTemplates.HeadingTemplate.HeadingFieldId);
                steamCardData.SubHeading = Utils.GetValue(datasource, BaseTemplates.SubHeadingTemplate.SubHeadingFieldId);
                steamCardData.Description = Utils.GetValue(datasource, BaseTemplates.DescriptionTemplate.DescriptionFieldId);

                foreach (Item galleryItem in datasource.Children)
                {
                    var steamCardDataModel = new SteamCardDataModel
                    {
                        Label = Utils.GetValue(galleryItem, BaseTemplates.LabelTemplate.LabelFieldId),
                        Theme = Utils.GetValue(galleryItem, BaseTemplates.ThemeTemplate.ThemeFieldId),
                        ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                        ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                        ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId),
                        ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId)
                    };
                    steamCardDataModelData.Add(steamCardDataModel);
                }
                steamCardData.LearningStoryListData = steamCardDataModelData;
            }

            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return steamCardData;
        }
        public FacilitiesModel GetFacilities(Rendering rendering)
        {
            var facilitiesData = new FacilitiesModel();
            List<GalleryItemModel> galleryItemModel = new List<GalleryItemModel>();
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                facilitiesData.Heading = Utils.GetValue(datasource, BaseTemplates.HeadingTemplate.HeadingFieldId);
                facilitiesData.Description = Utils.GetValue(datasource, BaseTemplates.DescriptionTemplate.DescriptionFieldId);
                facilitiesData.SectionID = Utils.GetValue(datasource, BaseTemplates.SectionIdTemplate.SectionIdFieldId);
                facilitiesData.Theme = Utils.GetValue(datasource, BaseTemplates.ThemeTemplate.ThemeFieldId);
                facilitiesData.Variant = Utils.GetValue(datasource, BaseTemplates.VariantTemplate.VariantFieldId);


                foreach (Item galleryItem in datasource.Children)
                {
                    var facilitiesDataModelModel = new GalleryItemModel
                    {
                        ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                        ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, FacilitiesTemplate.Fields.SteamCardDataModel.imageSourceMobile),
                        ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, FacilitiesTemplate.Fields.SteamCardDataModel.imageSourceTablet),
                        ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId),
                        Heading = Utils.GetValue(galleryItem, BaseTemplates.HeadingTemplate.HeadingFieldId),
                        Inpression = Utils.GetValue(galleryItem, BaseTemplates.SubHeadingTemplate.SubHeadingFieldId)
                    };
                    galleryItemModel.Add(facilitiesDataModelModel);
                }
                facilitiesData.Gallery = galleryItemModel;
            }

            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return facilitiesData;
        }

        public SectionCards<CommunicationCardDataModel> GetCommunicationCard(Rendering rendering)
        {
            var communicationCard = new SectionCards<CommunicationCardDataModel>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                communicationCard.Variant = Utils.GetValue(datasource, BaseTemplates.VariantTemplate.VariantFieldId);
                communicationCard.SectionID = Utils.GetValue(datasource, BaseTemplates.SectionIdTemplate.SectionIdFieldId);

                if (datasource.Children.Count > 0)
                {
                    foreach (Item galleryItem in datasource.Children)
                    {
                        var ItemsListData = new CommunicationCardDataModel
                        {
                            Theme = Utils.GetValue(galleryItem, BaseTemplates.ThemeTemplate.ThemeFieldId),
                            TextFirst = Utils.GetBoleanValue(galleryItem, BaseTemplates.TextFirstTemplate.TextFirstFieldId),
                            CardType = galleryItem.Fields[BaseTemplates.CardTypeTemplate.CardTypeFieldId].Value,
                            MediaType = galleryItem.Fields[BaseTemplates.MediaTypeTemplate.MediaTypeFieldId].Value,
                            SubHeading = Utils.GetValue(galleryItem, BaseTemplates.SubHeadingTemplate.SubHeadingFieldId),
                            Heading = Utils.GetValue(galleryItem, BaseTemplates.HeadingTemplate.HeadingFieldId),
                            Description = Utils.GetValue(galleryItem, BaseTemplates.DescriptionTemplate.DescriptionFieldId),
                            Link = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),
                            LinkText = Utils.GetValue(galleryItem, BaseTemplates.LinkTemplate.LinkTextFieldId),
                            ImageAlt = galleryItem.Fields[BaseTemplates.ImageSourceTemplate.ImageAltFieldId].Value,
                            ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                            ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                            ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId)
                        };
                        ItemsListData.GtmData = new GtmDataModel
                        {
                            Event = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                            Category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                            Sub_category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                            Label = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                            Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                        };

                        communicationCard.Data.Add(ItemsListData);
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return communicationCard;
        }

        public SectionCards<CarouselDataModel> GetTechnologyInnovation(Rendering rendering)
        {
            var technologyInnovationCarousel = new SectionCards<CarouselDataModel>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                technologyInnovationCarousel.Variant = Utils.GetValue(datasource, BaseTemplates.VariantTemplate.VariantFieldId); ;
                technologyInnovationCarousel.SectionID = Utils.GetValue(datasource, BaseTemplates.SectionIdTemplate.SectionIdFieldId);


                foreach (Item galleryItem in datasource.Children)
                {
                    var ItemsListData = new CarouselDataModel
                    {
                        Theme = galleryItem.Fields[BaseTemplates.ThemeTemplate.ThemeFieldId].Value,
                        TextFirst = Utils.GetBoleanValue(galleryItem, BaseTemplates.TextFirstTemplate.TextFirstFieldId),
                        CardType = galleryItem.Fields[BaseTemplates.CardTypeTemplate.CardTypeFieldId].Value,
                        MediaType = galleryItem.Fields[BaseTemplates.MediaTypeTemplate.MediaTypeFieldId].Value,
                        SubHeading = Utils.GetValue(galleryItem, BaseTemplates.SubHeadingTemplate.SubHeadingFieldId),
                        Heading = Utils.GetValue(galleryItem, BaseTemplates.HeadingTemplate.HeadingFieldId),
                        Description = Utils.GetValue(galleryItem, BaseTemplates.DescriptionTemplate.DescriptionFieldId),
                        ImageText = Utils.GetValue(galleryItem, BaseTemplates.TextTemplate.TextFieldId),
                        Link = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),
                        LinkText = Utils.GetValue(galleryItem, BaseTemplates.LinkTemplate.LinkTextFieldId),
                        ImageAlt = galleryItem.Fields[BaseTemplates.ImageSourceTemplate.ImageAltFieldId].Value,
                        ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                        ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                        ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId)
                    };
                    ItemsListData.GtmData = new GtmDataModel
                    {
                        Event = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Label = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };
                    technologyInnovationCarousel.Data.Add(ItemsListData);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return technologyInnovationCarousel;
        }
    }
}