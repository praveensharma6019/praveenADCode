using Project.AdaniInternationalSchool.Website.Helpers;
using Project.AdaniInternationalSchool.Website.Models;
using Project.AdaniInternationalSchool.Website.Templates;
using Sitecore.Data.Items;

internal static class GTMTagServiceHelpers
{
    public static GTMTagModel GetItemGTMTags(Item crItem)
    {
        return new GTMTagModel
        {           
            GtmEvent = Utils.GetValue(crItem, BaseTemplates.GTMTemplate.GtmEventFieldId, crItem.Name),
            GtmCategory = Utils.GetValue(crItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId, crItem.Name),
            GtmSubCategory = Utils.GetValue(crItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId, crItem.Name),
            GtmBannerCategory = Utils.GetValue(crItem, BaseTemplates.GTMTemplate.GtmBannerCategoryFieldId, crItem.Name),
            GtmIndex = Utils.GetValue(crItem, BaseTemplates.GTMTemplate.GtmIndexFieldId, crItem.Name),
            GtmEventSub = Utils.GetValue(crItem, BaseTemplates.GTMTemplate.GtmLabelFieldId, crItem.Name),
            PageType = Utils.GetValue(crItem, BaseTemplates.GTMTemplate.PageTypeFieldId, crItem.Name),
            VideoDuration = Utils.GetValue(crItem, BaseTemplates.GTMTemplate.VideoDurationFieldId, crItem.Name),
            GtmVideoStartEvent = Utils.GetValue(crItem, BaseTemplates.GTMTemplate.GtmVideoStartEventFieldId, crItem.Name),
            GtmVideoCompletedEvent = Utils.GetValue(crItem, BaseTemplates.GTMTemplate.GtmVideoCompletedEventFieldId, crItem.Name),
            GtmVideoProgressEvent = Utils.GetValue(crItem, BaseTemplates.GTMTemplate.GtmVideoProgressEventFieldId, crItem.Name),
            SeoName = Utils.GetValue(crItem, BaseTemplates.GTMTemplate.SeoNameFieldId, crItem.Name),
            SeoDescription = Utils.GetValue(crItem, BaseTemplates.GTMTemplate.SeoDescriptionFieldId, crItem.Name)
        };
    }
}