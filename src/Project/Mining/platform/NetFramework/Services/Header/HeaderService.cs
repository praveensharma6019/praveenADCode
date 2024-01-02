using Glass.Mapper.Sc;
using Project.Mining.Website.Helpers;
using Project.Mining.Website.Models;
using Project.Mining.Website.Templates;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using static Project.Mining.Website.Models.HeaderModel;

namespace Project.Mining.Website.Services.Header
{
    public class HeaderService : IHeaderService
    {
        public HeaderModel GetHeader(Rendering rendering)
        {
            HeaderModel headerdata = new HeaderModel();
            try
            {

                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                Item NavData = datasource.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Navigation"));
                Item TopBarList = datasource.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Top Bar Navigation"));

                headerdata.HeaderLogo = Utils.GetImageURLByFieldId(datasource, BaseTemplates.HeaderFieldTemplate.HeaderLogoFieldId);
                headerdata.Logo = Utils.GetImageURLByFieldId(datasource, BaseTemplates.HeaderFieldTemplate.LogoFieldId);
                headerdata.MobileLogo = Utils.GetImageURLByFieldId(datasource, BaseTemplates.HeaderFieldTemplate.MobileLogoFieldId);
                headerdata.BuLink = Utils.GetLinkURL(datasource.Fields[BaseTemplates.HeaderFieldTemplate.BuLinkFieldId]);
                headerdata.LinkTarget = Utils.GetValue(datasource, BaseTemplates.HeaderFieldTemplate.LinkTargetFieldId);
                headerdata.IsAbsolute = Utils.GetBoleanValue(datasource, BaseTemplates.HeaderFieldTemplate.IsAbsoluteFieldId);
                headerdata.AddOnClass = Utils.GetValue(datasource, BaseTemplates.HeaderFieldTemplate.AddOnClassFieldId);
                headerdata.BuLogoAltText = Utils.GetValue(datasource, BaseTemplates.HeaderFieldTemplate.BuLogoAltFieldId);
                List<NavDatum> navData = new List<NavDatum>();
                List<TopbarList> topbarData = new List<TopbarList>();

                //Nav Data
                if (NavData == null) return null;
                foreach (Item item in NavData.Children)
                {
                    var navItem = new NavDatum
                    {
                        LinkText = Utils.GetValue(item, BaseTemplates.NavigationFieldTemplate.LinkTextFieldId),
                        Link = Utils.GetLinkURL(item.Fields[BaseTemplates.NavigationFieldTemplate.LinkFieldId]),
                        HeaderCallback = Utils.GetBoleanValue(item, BaseTemplates.NavigationFieldTemplate.HeaderCallbackFieldId),
                        DefaultImage = Utils.GetImageURLByFieldId(item, BaseTemplates.NavigationFieldTemplate.DefaultImageFieldId),
                        Class = Utils.GetValue(item, BaseTemplates.NavigationFieldTemplate.ClassId),
                        GtmData = new GtmData
                        {
                            Event = Utils.GetValue(item, BaseTemplates.GtmFieldTemplate.EventFieldId),
                            Title = Utils.GetValue(item, BaseTemplates.GtmFieldTemplate.TitleFieldId),
                            Category = Utils.GetValue(item, BaseTemplates.GtmFieldTemplate.CategoryFieldId),
                            Label = Utils.GetValue(item, BaseTemplates.GtmFieldTemplate.LabelFieldId),
                            Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                        }

                    };

                    navData.Add(navItem);
                }
                //Top Bar List
                if (TopBarList == null) return null;
                foreach (Item item in TopBarList.Children)
                {

                    var topbarlistdata = new TopbarList
                    {
                        LinkText = Utils.GetValue(item, BaseTemplates.NavigationFieldTemplate.LinkTextFieldId),
                        Link = Utils.GetLinkURL(item.Fields[BaseTemplates.NavigationFieldTemplate.LinkFieldId]),
                        LinkIcon = Utils.GetValue(item, BaseTemplates.TopBarListFieldTemplate.linkIconFieldId),
                        HeaderLeftIcon = Utils.GetValue(item, BaseTemplates.TopBarListFieldTemplate.HeaderLeftIconFieldId),
                        HeaderRightIcon = Utils.GetValue(item, BaseTemplates.TopBarListFieldTemplate.HeaderRightIconFieldId),
                        GtmData = new GtmData
                        {
                            Event = Utils.GetValue(item, BaseTemplates.GtmFieldTemplate.EventFieldId),
                            Title = Utils.GetValue(item, BaseTemplates.GtmFieldTemplate.TitleFieldId),
                            Category = Utils.GetValue(item, BaseTemplates.GtmFieldTemplate.CategoryFieldId),
                            Label = Utils.GetValue(item, BaseTemplates.GtmFieldTemplate.LabelFieldId),
                            Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                        },

                    };

                    topbarData.Add(topbarlistdata);
                }


                headerdata.TopbarList = topbarData;
                headerdata.NavData = navData;
            }

            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return headerdata;
        }
    }
}
