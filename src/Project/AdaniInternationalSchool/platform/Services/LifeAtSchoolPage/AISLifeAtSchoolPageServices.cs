using Project.AdaniInternationalSchool.Website.AdmissionPage;
using Project.AdaniInternationalSchool.Website.Helpers;
using Project.AdaniInternationalSchool.Website.Models;
using Project.AdaniInternationalSchool.Website.Templates;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Services.LifeAtSchoolPage
{
    public class AISLifeAtSchoolPageServices : IAISLifeAtSchoolPageServices
    {
        public SectionCards<LifeAtSchoolArtsDataModel> GetLifeAtSchoolArts(Rendering rendering)
        {
            var lifeAtSchoolArts = new SectionCards<LifeAtSchoolArtsDataModel>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                lifeAtSchoolArts.Variant = Utils.GetValue(datasource, BaseTemplates.VariantTemplate.VariantFieldId);
                lifeAtSchoolArts.SectionID = Utils.GetValue(datasource, BaseTemplates.SectionIdTemplate.SectionIdFieldId);

                if (datasource.Children.Count > 0)
                {
                    foreach (Item galleryItem in datasource.Children)
                    {
                        var ItemsListData = new LifeAtSchoolArtsDataModel
                        {
                            Theme = Utils.GetValue(galleryItem, BaseTemplates.ThemeTemplate.ThemeFieldId),
                            TextFirst = Utils.GetBoleanValue(galleryItem, BaseTemplates.TextFirstTemplate.TextFirstFieldId),
                            CardType = Utils.GetValue(galleryItem, BaseTemplates.CardTypeTemplate.CardTypeFieldId),
                            MediaType = Utils.GetValue(galleryItem, BaseTemplates.MediaTypeTemplate.MediaTypeFieldId),
                            SubHeading = Utils.GetValue(galleryItem, BaseTemplates.SubHeadingTemplate.SubHeadingFieldId),
                            Heading = Utils.GetValue(galleryItem, BaseTemplates.HeadingTemplate.HeadingFieldId),
                            Description = Utils.GetValue(galleryItem, BaseTemplates.DescriptionTemplate.DescriptionFieldId),
                            MapSource = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.MapSourceTemplate.MapSourceFieldId]),
                            VideoSource = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceFieldId]),
                            VideoSourceMobile = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceMobileFieldId]),
                            VideoSourceTablet = Utils.GetLinkURL(galleryItem?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceTabletFieldId]),
                            ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId),
                            ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                            ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                            ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId),
                            Link = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]),
                            LinkText = Utils.GetValue(galleryItem, BaseTemplates.CtaTemplate.CtaTextFieldId),
                        };
                        ItemsListData.GtmData = new GtmDataModel
                        {
                            Event = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                            Category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                            Sub_category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                            Label = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                            Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                        };
                        lifeAtSchoolArts.Data.Add(ItemsListData);
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return lifeAtSchoolArts;
        }
        public GalleryContentModel<BaseImageContentModel> GetLifeAtSchoolComplementingTheAcademics(Rendering rendering)
        {
            var complementingTheAcademics = new GalleryContentModel<BaseImageContentModel>();
            List<BaseImageContentModel> baseImageContentModel = new List<BaseImageContentModel>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                complementingTheAcademics.Heading = Utils.GetValue(datasource, BaseTemplates.HeadingTemplate.HeadingFieldId);
                complementingTheAcademics.SubHeading = Utils.GetValue(datasource, BaseTemplates.SubHeadingTemplate.SubHeadingFieldId); ;
                complementingTheAcademics.Description = Utils.GetValue(datasource, BaseTemplates.DescriptionTemplate.DescriptionFieldId);
                complementingTheAcademics.SectionID = Utils.GetValue(datasource, BaseTemplates.SubHeadingTemplate.SubHeadingFieldId);
                complementingTheAcademics.Theme = Utils.GetValue(datasource, BaseTemplates.ThemeTemplate.ThemeFieldId);
                complementingTheAcademics.Link = Utils.GetLinkURL(datasource.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]);
                complementingTheAcademics.LinkText = Utils.GetValue(datasource, BaseTemplates.CtaTemplate.CtaTextFieldId);
                complementingTheAcademics.Variant = Utils.GetValue(datasource, BaseTemplates.VariantTemplate.VariantFieldId);
                complementingTheAcademics.GtmData = new GtmDataModel();
                complementingTheAcademics.GtmData.Event = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmEventFieldId);
                complementingTheAcademics.GtmData.Category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmCategoryFieldId);
                complementingTheAcademics.GtmData.Sub_category = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId);
                complementingTheAcademics.GtmData.Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item);
                complementingTheAcademics.GtmData.Label = Utils.GetValue(datasource, BaseTemplates.GTMTemplate.GtmLabelFieldId);
                if (datasource.Children.Count > 0)
                {
                    foreach (Item galleryItem in datasource.Children)
                    {
                        var ItemsListData = new BaseImageContentModel
                        {
                            Heading = Utils.GetValue(galleryItem, BaseTemplates.HeadingTemplate.HeadingFieldId),
                            Description = Utils.GetValue(galleryItem, BaseTemplates.DescriptionTemplate.DescriptionFieldId),
                            ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId),
                            ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                            ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                            ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId)
                        };
                        baseImageContentModel.Add(ItemsListData);
                    }
                    complementingTheAcademics.Gallery = baseImageContentModel;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return complementingTheAcademics;
        }

        public BaseContentModel<LifeAtSchoolFindOutMoreDataModel> GetLifeAtSchoolFindOutMore(Rendering rendering)
        {
            var lifeAtSchoolCTA = new BaseContentModel<LifeAtSchoolFindOutMoreDataModel>();
            List<LifeAtSchoolFindOutMoreDataModel> widgetItemsListData = new List<LifeAtSchoolFindOutMoreDataModel>();

            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            try
            {
                lifeAtSchoolCTA.Heading = Utils.GetValue(datasource, BaseTemplates.HeadingTemplate.HeadingFieldId);
                lifeAtSchoolCTA.Description = Utils.GetValue(datasource, BaseTemplates.DescriptionTemplate.DescriptionFieldId);

                if (datasource.Children.Count > 0)
                {
                    foreach (Item galleryItem in datasource.Children)
                    {
                        LifeAtSchoolFindOutMoreDataModel ItemsListData = new LifeAtSchoolFindOutMoreDataModel();
                        ItemsListData.Heading = Utils.GetValue(galleryItem, BaseTemplates.HeadingTemplate.HeadingFieldId);
                        ItemsListData.Description = Utils.GetValue(galleryItem, BaseTemplates.DescriptionTemplate.DescriptionFieldId);
                        ItemsListData.Theme = Utils.GetValue(galleryItem, BaseTemplates.ThemeTemplate.ThemeFieldId);
                        ItemsListData.Url = Utils.GetLinkURL(galleryItem.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]);
                        ItemsListData.Label = Utils.GetValue(galleryItem, BaseTemplates.CtaTemplate.CtaTextFieldId);
                        ItemsListData.Target = Utils.GetValue(galleryItem, BaseTemplates.TargetTemplate.TargetFieldId);
                        ItemsListData.ImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);
                        ItemsListData.ImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                        ItemsListData.ImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
                        ItemsListData.ImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);
                        ItemsListData.GtmData = new GtmDataModel();
                        ItemsListData.GtmData.Event = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmEventFieldId);
                        ItemsListData.GtmData.Category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId);
                        ItemsListData.GtmData.Sub_category = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId);
                        ItemsListData.GtmData.Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item);
                        ItemsListData.GtmData.Label = Utils.GetValue(galleryItem, BaseTemplates.GTMTemplate.GtmLabelFieldId);
                        widgetItemsListData.Add(ItemsListData);
                    }
                }
                lifeAtSchoolCTA.Data = widgetItemsListData;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return lifeAtSchoolCTA;
        }
    }
}