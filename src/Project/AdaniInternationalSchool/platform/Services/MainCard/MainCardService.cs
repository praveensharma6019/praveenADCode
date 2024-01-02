using Project.AdaniInternationalSchool.Website.Helpers;
using Project.AdaniInternationalSchool.Website.Models;
using Project.AdaniInternationalSchool.Website.Templates;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Linq;

namespace Project.AdaniInternationalSchool.Website.Services
{
    public class MainCardService : IMainCardService
    {
        public BaseCards<MainCardItemModel> Render(Rendering rendering)
        {
            var model = new BaseCards<MainCardItemModel>();

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
                        model.Data.Add(new MainCardItemModel
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
                            GtmData = new GtmDataModel()
                            {
                                Event = Utils.GetValue(dataitem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                                Category = Utils.GetValue(dataitem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                                Sub_category = Utils.GetValue(dataitem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                                Label = Utils.GetValue(dataitem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                                Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                            },
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