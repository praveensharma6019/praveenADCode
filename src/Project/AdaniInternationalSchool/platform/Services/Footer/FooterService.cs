using Project.AdaniInternationalSchool.Website.Helpers;
using Project.AdaniInternationalSchool.Website.Models;
using Project.AdaniInternationalSchool.Website.Templates;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Linq;

namespace Project.AdaniInternationalSchool.Website.Services.Footer
{
    public class FooterService : IFooterService
    {
        public FooterModel Render(Rendering rendering)
        {
            FooterModel footerModel = new FooterModel();

            var dsItem = Utils.GetRenderingDatasource(rendering);
            if (dsItem == null) return null;

            try
            {
                PopulateQuicklinks(footerModel, dsItem);
                PopulateFooterlinks(footerModel, dsItem);
                PopulateCopyright(footerModel, dsItem);
                PopulateSchoolInfo(footerModel, dsItem);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return footerModel;
        }

        private void PopulateCopyright(FooterModel footerModel, Item item)
        {
            Item copyRight = item.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Copyright"))?.Children.FirstOrDefault();

            if (copyRight != null)
            {
                var copyRightModel = new CopyRightModel
                {
                    CopyRightText = Utils.GetValue(copyRight, BaseTemplates.TitleTemplate.TitleFieldId, copyRight.Name)
                };

                foreach (Item crItem in copyRight.Children)
                {
                    var linkitem = ServiceHelper.GetLinkItem(crItem);
                    ServiceHelper.FillLinkItemGtmValues(linkitem, crItem);
                    copyRightModel.List.Add(linkitem);
                }

                footerModel.CopyRight = copyRightModel;
            }
        }

        private void PopulateFooterlinks(FooterModel footerModel, Item item)
        {
            Item footerlinksFolder = item.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Footerlinks"));

            if (footerlinksFolder != null)
            {
                foreach (Item flItem in footerlinksFolder.Children)
                {
                    var footerlinksModel = new LinksModel
                    {
                        Title = Utils.GetValue(flItem, BaseTemplates.TitleTemplate.TitleFieldId, flItem.Name)
                    };

                    foreach (Item flItemLink in flItem.Children)
                    {
                        var linkItemModel = ServiceHelper.GetLinkItem(flItemLink);
                        ServiceHelper.FillLinkItemGtmValues(linkItemModel, flItemLink);
                        footerlinksModel.List.Add(linkItemModel);
                    }

                    footerModel.Footerlinks.Add(footerlinksModel);
                }
            }
        }

        private void PopulateQuicklinks(FooterModel footerModel, Item item)
        {
            Item quicklinks = item.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Quicklinks"))?.Children.FirstOrDefault();

            if (quicklinks != null)
            {
                var model = new LinksModel
                {
                    Title = Utils.GetValue(quicklinks, BaseTemplates.TitleTemplate.TitleFieldId, quicklinks.Name)
                };

                foreach (Item qlItem in quicklinks.Children)
                {
                    var linkItemModel = ServiceHelper.GetLinkItem(qlItem);
                    ServiceHelper.FillLinkItemGtmValues(linkItemModel, qlItem);
                    model.List.Add(linkItemModel);
                }

                footerModel.Quicklinks = model;
            }
        }

        private void PopulateSchoolInfo(FooterModel footerModel, Item item)
        {
            Item schoolInfo = item.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "SchoolInfo"))?.Children.FirstOrDefault();

            if (schoolInfo != null)
            {
                SchoolInfoModel schoolInfoModel = new SchoolInfoModel
                {
                    SchoolLogo = Utils.GetImageURLByFieldId(schoolInfo, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                    Alt = Utils.GetValue(schoolInfo, BaseTemplates.ImageSourceTemplate.ImageAltFieldId),
                    GtmData = new GtmDataModel()
                    {
                        Event = Utils.GetValue(schoolInfo, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Category = Utils.GetValue(schoolInfo, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(schoolInfo, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Label = Utils.GetValue(schoolInfo, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    },
                    PageType = Utils.GetValue(schoolInfo, BaseTemplates.GTMTemplate.PageTypeFieldId)
                };

                //social links
                Item schoolSocial = schoolInfo.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Social"));

                foreach (Item crItem in schoolSocial.Children)
                {
                    var lnkItemModel = ServiceHelper.GetLinkItem(crItem);
                    ServiceHelper.FillLinkItemGtmValues(lnkItemModel, crItem);
                    schoolInfoModel.Social.Add(lnkItemModel);

                }

                //contacts
                Item schoolContact = schoolInfo.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Contact"));

                foreach (Item crItem in schoolContact.Children)
                {
                    schoolInfoModel.Contact.Add(new FooterContactModel
                    {
                        Label = Utils.GetValue(crItem, BaseTemplates.TitleTemplate.TitleFieldId, crItem.Name),
                        Detail = Utils.GetValue(crItem, BaseTemplates.DescriptionTemplate.DescriptionFieldId),
                        Url = Utils.GetValue(crItem, FooterContactTemplate.Fields.UrlFieldId),
                        GtmData = new GtmDataModel()
                        {
                            Event = Utils.GetValue(schoolInfo, BaseTemplates.GTMTemplate.GtmEventFieldId),
                            Category = Utils.GetValue(schoolInfo, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                            Sub_category = Utils.GetValue(schoolInfo, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                            Label = Utils.GetValue(schoolInfo, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                            Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                        },
                    });
                }

                footerModel.SchoolInfo = schoolInfoModel;
            }

        }
    }
}