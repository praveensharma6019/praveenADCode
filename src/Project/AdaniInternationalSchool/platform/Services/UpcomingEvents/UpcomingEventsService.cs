using Project.AdaniInternationalSchool.Website.Helpers;
using Project.AdaniInternationalSchool.Website.Models;
using Project.AdaniInternationalSchool.Website.Models.UpcomingEvents;
using Project.AdaniInternationalSchool.Website.Templates;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Linq;

namespace Project.AdaniInternationalSchool.Website.Services
{
    public class UpcomingEventsService : IUpcomingEventsService
    {
        public BaseHeadingModel<EventItemModel> Render(Rendering rendering)
        {
            var model = new BaseHeadingModel<EventItemModel>();

            var dsItem = Utils.GetRenderingDatasource(rendering);
            if (dsItem == null) return null;

            try
            {
                model.Heading = Utils.GetValue(dsItem, BaseTemplates.TitleTemplate.TitleFieldId, dsItem.Name);
                var dataFolder = dsItem.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Data"));
                if (dataFolder != null)
                {
                    foreach (Item listItem in dataFolder.Children)
                    {
                        model.Data.Add(new EventItemModel
                        {
                            Heading = Utils.GetValue(listItem, BaseTemplates.TitleTemplate.TitleFieldId, listItem.Name),
                            Description = Utils.GetValue(listItem, BaseTemplates.DescriptionTemplate.DescriptionFieldId),
                            ImageAlt = Utils.GetValue(listItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId),
                            ImageSource = Utils.GetImageURLByFieldId(listItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                            ImageSourceTablet = Utils.GetImageURLByFieldId(listItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId),
                            ImageSourceMobile = Utils.GetImageURLByFieldId(listItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                            Date = Utils.GetDate(listItem, BaseTemplates.DateTemplate.DateFieldId),
                            Url = Utils.GetLinkURL(listItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),
                            GtmData = new GtmDataModel()
                            {
                                Event = Utils.GetValue(listItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                                Category = Utils.GetValue(listItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                                Sub_category = Utils.GetValue(listItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                                Label = Utils.GetValue(listItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                                Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item),
                                Click_text = Utils.GetValue(listItem, BaseTemplates.GTMTemplate.GtmClicktextFieldId)
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