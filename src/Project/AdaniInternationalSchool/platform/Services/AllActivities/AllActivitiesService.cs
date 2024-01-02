using Project.AdaniInternationalSchool.Website.Helpers;
using Project.AdaniInternationalSchool.Website.Models;
using Project.AdaniInternationalSchool.Website.Templates;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Linq;

namespace Project.AdaniInternationalSchool.Website.Services
{
    public class AllActivitiesService : IAllActivitiesService
    {
        public BaseCards<AllActivitiesDataModel> Render(Rendering rendering)
        {
            var model = new BaseCards<AllActivitiesDataModel>();

            var dsItem = Utils.GetRenderingDatasource(rendering);
            if (dsItem == null) return null;

            try
            {
                model.Variant = Utils.GetValue(dsItem, AllActivitiesTemplate.Fields.VariantFieldId);
                var dataFolder = dsItem.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Data"));
                if (dataFolder != null)
                {
                    foreach (Item listItem in dataFolder.Children)
                    {
                        model.Data.Add(new AllActivitiesDataModel
                        {
                            Heading = Utils.GetValue(listItem, BaseTemplates.TitleTemplate.TitleFieldId, listItem.Name),
                            SubHeading = Utils.GetValue(listItem, AllActivitiesTemplate.Fields.SubHeadingFieldId),
                            Description = Utils.GetValue(listItem, BaseTemplates.DescriptionTemplate.DescriptionFieldId),
                            ImageAlt = Utils.GetValue(listItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId),
                            ImageSource = Utils.GetImageURLByFieldId(listItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                            ImageSourceTablet = Utils.GetImageURLByFieldId(listItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId),
                            ImageSourceMobile = Utils.GetImageURLByFieldId(listItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                            CardType = Utils.GetValue(listItem, AllActivitiesTemplate.Fields.CardTypeFieldId),
                            MediaType = Utils.GetValue(listItem, AllActivitiesTemplate.Fields.MediaTypeFieldId),
                            TextFirst = Utils.GetBoleanValue(listItem, AllActivitiesTemplate.Fields.TextFirst),
                            Theme = Utils.GetValue(listItem, AllActivitiesTemplate.Fields.ThemeFieldId),
                            Link = Utils.GetLinkURL(listItem?.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]),
                            LinkText = Utils.GetValue(listItem, BaseTemplates.CtaTemplate.CtaTextFieldId),
                            GtmData = new GtmDataModel()
                            {
                                Event = Utils.GetValue(listItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                                Category = Utils.GetValue(listItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                                Sub_category = Utils.GetValue(listItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                                Label = Utils.GetValue(listItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                                Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    }
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