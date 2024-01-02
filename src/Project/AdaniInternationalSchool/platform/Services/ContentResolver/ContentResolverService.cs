using Project.AdaniInternationalSchool.Website.Helpers;
using Project.AdaniInternationalSchool.Website.Models;
using Project.AdaniInternationalSchool.Website.Templates;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Services.ContentResolver
{
    public class ContentResolverService : IContentResolverService
    {
        public WhyUsCardModel GetWhyUsCardModel(Rendering rendering)
        {
            WhyUsCardModel whyUsCardModel = new WhyUsCardModel();
            try
            {
                //Get the datasource for the item
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                whyUsCardModel.Variant = datasource.Fields[BaseTemplates.VariantTemplate.VariantFieldId].Value;
                whyUsCardModel.Heading = datasource.Fields[BaseTemplates.TitleTemplate.TitleFieldId].Value;
                whyUsCardModel.Description = datasource.Fields[BaseTemplates.DescriptionTemplate.DescriptionFieldId].Value;

                whyUsCardModel.Data = new List<WhyUsCardDataModel>();
                MultilistField whyUsCardData = datasource.Fields[WhyUsCardTemplate.WhyUsCardDataFieldId];

                foreach (Item whyUsCardDataItem in whyUsCardData.GetItems())
                {
                    WhyUsCardDataModel whyUsCardDataModel = new WhyUsCardDataModel();
                    whyUsCardDataModel.Description = whyUsCardDataItem.Fields[BaseTemplates.DescriptionTemplate.DescriptionFieldId].Value;
                    whyUsCardDataModel.Theme = whyUsCardDataItem.Fields[BaseTemplates.ThemeTemplate.ThemeFieldId].Value;
                    whyUsCardDataModel.ImageSource = Utils.GetImageURLByFieldId(whyUsCardDataItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                    whyUsCardDataModel.ImageSourceMobile = Utils.GetImageURLByFieldId(whyUsCardDataItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
                    whyUsCardDataModel.ImageSourceTablet = Utils.GetImageURLByFieldId(whyUsCardDataItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);
                    whyUsCardDataModel.ImageAlt = whyUsCardDataItem.Fields[BaseTemplates.ImageSourceTemplate.ImageAltFieldId].Value;

                    whyUsCardModel.Data.Add(whyUsCardDataModel);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return whyUsCardModel;
        }

        public SectionCards<ExtendedContentModel> GetInquireNowCardModel(Rendering rendering)
        {
            var inquireNowModel = new SectionCards<ExtendedContentModel>();
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                inquireNowModel.SectionID = datasource.Fields[BaseTemplates.SectionIdTemplate.SectionIdFieldId].Value;
                inquireNowModel.Variant = datasource.Fields[BaseTemplates.VariantTemplate.VariantFieldId].Value;

                MultilistField inquireNowData = datasource.Fields[InquireNowTemplate.DataFieldId];

                foreach (Item inquireNowDataItem in inquireNowData.GetItems())
                {
                    inquireNowModel.Data.Add(GetInquireNowDataModel(inquireNowDataItem));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return inquireNowModel;
        }
        public SectionCards<VisitOurSchoolDataItemModel> GetVisitOurSchoolCardModel(Rendering rendering)
        {
            var visitOurSchool = new SectionCards<VisitOurSchoolDataItemModel>();
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                visitOurSchool.SectionID = datasource.Fields[BaseTemplates.SectionIdTemplate.SectionIdFieldId].Value;
                visitOurSchool.Variant = datasource.Fields[BaseTemplates.VariantTemplate.VariantFieldId].Value;

                MultilistField visitOurSchoolData = datasource.Fields[VisitOurSchoolTemplate.DataFieldId];

                foreach (Item visitOurSchoolDataItem in visitOurSchoolData.GetItems())
                {
                    ExtendedContentModel inquireNowDataModel = GetInquireNowDataModel(visitOurSchoolDataItem);
                    VisitOurSchoolDataItemModel visitOurSchoolDataModel = new VisitOurSchoolDataItemModel();
                    visitOurSchoolDataModel.Theme = inquireNowDataModel.Theme;
                    visitOurSchoolDataModel.TextFirst = inquireNowDataModel.TextFirst;
                    visitOurSchoolDataModel.CardType = inquireNowDataModel.CardType;
                    visitOurSchoolDataModel.MediaType = inquireNowDataModel.MediaType;
                    visitOurSchoolDataModel.SubHeading = inquireNowDataModel.SubHeading;
                    visitOurSchoolDataModel.Heading = inquireNowDataModel.Heading;
                    visitOurSchoolDataModel.Description = inquireNowDataModel.Description;
                    visitOurSchoolDataModel.Link = inquireNowDataModel.Link;
                    visitOurSchoolDataModel.LinkText = inquireNowDataModel.LinkText;
                    visitOurSchoolDataModel.ImageSource = inquireNowDataModel.ImageSource;
                    visitOurSchoolDataModel.ImageSourceMobile = inquireNowDataModel.ImageSourceMobile;
                    visitOurSchoolDataModel.ImageSourceTablet = inquireNowDataModel.ImageSourceTablet;
                    visitOurSchoolDataModel.ImageAlt = inquireNowDataModel.ImageAlt;
                    visitOurSchoolDataModel.VideoSource = inquireNowDataModel.VideoSource;
                    visitOurSchoolDataModel.VideoSourceMobile = inquireNowDataModel.VideoSourceMobile;
                    visitOurSchoolDataModel.VideoSourceTablet = inquireNowDataModel.VideoSourceTablet;
                    visitOurSchoolDataModel.DefaultVideoSource = inquireNowDataModel.DefaultVideoSource;
                    visitOurSchoolDataModel.DefaultVideoSourceMobile = inquireNowDataModel.DefaultVideoSourceMobile;
                    visitOurSchoolDataModel.DefaultVideoSourceTablet = inquireNowDataModel.DefaultVideoSourceTablet;
                    visitOurSchoolDataModel.DefaultVideoSourceOgg = inquireNowDataModel.DefaultVideoSourceOgg;
                    visitOurSchoolDataModel.DefaultVideoSourceOggMobile = inquireNowDataModel.DefaultVideoSourceOggMobile;
                    visitOurSchoolDataModel.DefaultVideoSourceOggTablet = inquireNowDataModel.DefaultVideoSourceOggTablet;
                    visitOurSchoolDataModel.MapSource = inquireNowDataModel.MapSource;
                    visitOurSchoolDataModel.LocationData = new List<LocationDetails>();

                    MultilistField location = visitOurSchoolDataItem.Fields[VisitOurSchoolDataTemplate.LocationDataFieldId];

                    foreach (Item locationDataItem in location.GetItems())
                    {
                        LocationDetails locationDetails = new LocationDetails();
                        locationDetails.Detail = locationDataItem.Fields[LocationDataTemplate.DetailFieldId].Value;
                        locationDetails.Label = locationDataItem.Fields[LocationDataTemplate.LabelFieldId].Value;
                        locationDetails.Link = locationDataItem.Fields[LocationDataTemplate.LinkFieldId].Value;
                        visitOurSchoolDataModel.LocationData.Add(locationDetails);
                    }

                    visitOurSchool.Data.Add(visitOurSchoolDataModel);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return visitOurSchool;
        }

        private static ExtendedContentModel GetInquireNowDataModel(Item inquireNowDataItem)
        {
            ExtendedContentModel inquireNowDataModel = new ExtendedContentModel();
            inquireNowDataModel.Theme = inquireNowDataItem.Fields[BaseTemplates.ThemeTemplate.ThemeFieldId].Value;
            inquireNowDataModel.TextFirst = Utils.GetBoleanValue(inquireNowDataItem, InquireNowDataTemplate.TextFirstFieldId);
            inquireNowDataModel.CardType = inquireNowDataItem.Fields[InquireNowDataTemplate.CardTypeFieldId].Value;
            inquireNowDataModel.MediaType = inquireNowDataItem.Fields[InquireNowDataTemplate.MediaTypeFieldId].Value;
            inquireNowDataModel.SubHeading = inquireNowDataItem.Fields[BaseTemplates.SubHeadingTemplate.SubHeadingFieldId].Value;
            inquireNowDataModel.Heading = inquireNowDataItem.Fields[BaseTemplates.TitleTemplate.TitleFieldId].Value;
            inquireNowDataModel.Description = inquireNowDataItem.Fields[BaseTemplates.DescriptionTemplate.DescriptionFieldId].Value;
            inquireNowDataModel.Link = Utils.GetLinkURL(inquireNowDataItem?.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]);
            inquireNowDataModel.LinkText = inquireNowDataItem.Fields[BaseTemplates.CtaTemplate.CtaTextFieldId].Value;
            inquireNowDataModel.ImageSource = Utils.GetImageURLByFieldId(inquireNowDataItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
            inquireNowDataModel.ImageSourceMobile = Utils.GetImageURLByFieldId(inquireNowDataItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
            inquireNowDataModel.ImageSourceTablet = Utils.GetImageURLByFieldId(inquireNowDataItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);
            inquireNowDataModel.ImageAlt = inquireNowDataItem.Fields[BaseTemplates.ImageSourceTemplate.ImageAltFieldId].Value;
            inquireNowDataModel.VideoSource = Utils.GetLinkURL(inquireNowDataItem?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceFieldId]);
            inquireNowDataModel.VideoSourceMobile = Utils.GetLinkURL(inquireNowDataItem?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceMobileFieldId]);
            inquireNowDataModel.VideoSourceTablet = Utils.GetLinkURL(inquireNowDataItem?.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceTabletFieldId]);
            inquireNowDataModel.DefaultVideoSource = Utils.GetLinkURL(inquireNowDataItem?.Fields[BaseTemplates.DefaultVideoSourceTemplate.DefaultVideoSourceFieldId]);
            inquireNowDataModel.DefaultVideoSourceMobile = Utils.GetLinkURL(inquireNowDataItem?.Fields[BaseTemplates.DefaultVideoSourceTemplate.DefaultVideoSourceMobileFieldId]);
            inquireNowDataModel.DefaultVideoSourceTablet = Utils.GetLinkURL(inquireNowDataItem?.Fields[BaseTemplates.DefaultVideoSourceTemplate.DefaultVideoSourceTabletFieldId]);
            inquireNowDataModel.DefaultVideoSourceOgg = Utils.GetLinkURL(inquireNowDataItem?.Fields[BaseTemplates.DefaultVideoSourceOggTemplate.DefaultVideoSourceOggFieldId]);
            inquireNowDataModel.DefaultVideoSourceOggMobile = Utils.GetLinkURL(inquireNowDataItem?.Fields[BaseTemplates.DefaultVideoSourceOggTemplate.DefaultVideoSourceOggMobileFieldId]);
            inquireNowDataModel.DefaultVideoSourceOggTablet = Utils.GetLinkURL(inquireNowDataItem?.Fields[BaseTemplates.DefaultVideoSourceOggTemplate.DefaultVideoSourceOggTabletFieldId]);
            inquireNowDataModel.MapSource = inquireNowDataItem.Fields[InquireNowDataTemplate.MapSourceFieldId].Value;
            return inquireNowDataModel;
        }
    }
}