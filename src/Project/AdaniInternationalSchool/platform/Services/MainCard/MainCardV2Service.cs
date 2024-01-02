using Project.AdaniInternationalSchool.Website.Helpers;
using Project.AdaniInternationalSchool.Website.Models;
using Project.AdaniInternationalSchool.Website.Models.LifeAtSchoolActivities;
using Project.AdaniInternationalSchool.Website.Templates;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Linq;

namespace Project.AdaniInternationalSchool.Website.Services
{
    public class MainCardV2Service : IMainCardV2Service
    {
        public BaseCards<MainCardVideoItemModel> Render(Rendering rendering)
        {
            var model = new BaseCards<MainCardVideoItemModel>();

            var dsItem = Utils.GetRenderingDatasource(rendering);
            if (dsItem == null) return null;

            try
            {
                model.Variant = Utils.GetValue(dsItem, MainCardTemplate.Fields.Variant);
                var data = dsItem.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Data"));

                if (data != null)
                {
                    foreach (Item dataitem in data.Children)
                    {
                        model.Data.Add(new MainCardVideoItemModel
                        {
                            Heading = Utils.GetValue(dataitem, BaseTemplates.HeadingTemplate.HeadingFieldId, dataitem.Name),
                            SubHeading = Utils.GetValue(dataitem, MainCardTemplate.Fields.subHeading),
                            Description = Utils.GetValue(dataitem, BaseTemplates.DescriptionTemplate.DescriptionFieldId),
                            SubDescription = Utils.GetValue(dataitem, MainCardTemplate.Fields.SubDescription),
                            MainDescription = Utils.GetValue(dataitem, MainCardTemplate.Fields.MainDescription),
                            LinkText = Utils.GetValue(dataitem, BaseTemplates.CtaTemplate.CtaTextFieldId),
                            Link = Utils.GetLinkURL(dataitem.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]),
                            CardType = Utils.GetValue(dataitem, MainCardTemplate.Fields.CardType),
                            TextFirst = Utils.GetBoleanValue(dataitem, MainCardTemplate.Fields.TextFirst),
                            Theme = Utils.GetValue(dataitem, MainCardTemplate.Fields.Theme),
                            MediaType = Utils.GetValue(dataitem, MainCardTemplate.Fields.MediaType),
                            MapSource = Utils.GetValue(dataitem, MainCardTemplate.Fields.MapSource),
                            ImageAlt = Utils.GetValue(dataitem, MainCardTemplate.Fields.ImageAlt),
                            ImageSource = Utils.GetImageURLByFieldId(dataitem, MainCardTemplate.Fields.ImageSource),
                            ImageSourceMobile = Utils.GetImageURLByFieldId(dataitem, MainCardTemplate.Fields.ImageSourceMobile),
                            ImageSourceTablet = Utils.GetImageURLByFieldId(dataitem, MainCardTemplate.Fields.ImageSourceTablet),
                            VideoSource = Utils.GetLinkURL(dataitem.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceFieldId]),
                            VideoSourceMobile = Utils.GetLinkURL(dataitem.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceMobileFieldId]),
                            VideoSourceTablet = Utils.GetLinkURL(dataitem.Fields[BaseTemplates.VideoSourceTemplate.VideoSourceTabletFieldId]),
                            VideoSourceOgg = Utils.GetLinkURL(dataitem.Fields[BaseTemplates.VideoSourceOggTemplate.VideoSourceOggFieldId]),
                            VideoSourceMobileOgg = Utils.GetLinkURL(dataitem.Fields[BaseTemplates.VideoSourceOggTemplate.VideoSourceOggMobileFieldId]),
                            VideoSourceTabletOgg = Utils.GetLinkURL(dataitem.Fields[BaseTemplates.VideoSourceOggTemplate.VideoSourceOggTabletFieldId])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return model;
        }
    }
}