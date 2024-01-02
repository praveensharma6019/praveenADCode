using Project.AdaniInternationalSchool.Website.Models;
using Project.AdaniInternationalSchool.Website.Templates;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using static Project.AdaniInternationalSchool.Website.Templates.FounderCardTemplate.Fields;

namespace Project.AdaniInternationalSchool.Website.Helpers
{
    internal static class ServiceHelper
    {
        public static LinkItemModel GetLinkItem(Item item)
        {
            LinkField lf = item.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId];

            return new LinkItemModel
            {
                Label = Utils.GetValue(item, BaseTemplates.CtaTemplate.CtaTextFieldId, item.Name),
                Target = lf.Target,
                Url = Utils.GetLinkURL(lf)
            };
        }

        public static void FillLinkItemGtmValues(LinkItemModel linkItem, Item scItem)
        {
            linkItem.GtmData = new GtmDataModel()
            {
                Event = Utils.GetValue(scItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                Category = Utils.GetValue(scItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                Sub_category = Utils.GetValue(scItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                Label = Utils.GetValue(scItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item),
            Section_title = Utils.GetValue(scItem, BaseTemplates.GTMTemplate.GtmTitleFieldId)
            };
        }
    }
}