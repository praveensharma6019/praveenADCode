using Project.AdaniInternationalSchool.Website.ExtensionsHelper;
using Project.AdaniInternationalSchool.Website.Helpers;
using Project.AdaniInternationalSchool.Website.Models;
using Project.AdaniInternationalSchool.Website.Templates;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AdaniInternationalSchool.Website.Services.AboutUs
{
    public class AboutUsService : IAboutUsService
    {
        public PageDescriptionModel GetAboutUsModel(Rendering rendering)
        {
            PageDescriptionModel aboutUsModel = null;

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                aboutUsModel = new PageDescriptionModel();
                aboutUsModel.Heading = Utils.GetValue(datasource, BaseTemplates.HeadingTemplate.HeadingFieldId);
                aboutUsModel.Description = Utils.GetValue(datasource, BaseTemplates.DescriptionTemplate.DescriptionFieldId);


                foreach (Item banners in datasource.Children)
                {
                    BannerModel ItemsListData = new BannerModel();
                    ItemsListData.ImageSource = ExtensionsHelper.ItemExtensions.GetImageURLByFieldId(banners, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                    ItemsListData.ImageSourceTablet = ExtensionsHelper.ItemExtensions.GetImageURLByFieldId(banners, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);
                    ItemsListData.ImageSourceMobile = ExtensionsHelper.ItemExtensions.GetImageURLByFieldId(banners, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
                    ItemsListData.ImageAlt = Utils.GetValue(banners, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);
                    ItemsListData.Link = ExtensionsHelper.ItemExtensions.GetLinkURL(banners, BaseTemplates.LinkTemplate.LinkFieldId.ToString());
                    ItemsListData.Target = ExtensionsHelper.ItemExtensions.GetLinkURLTarget(banners, BaseTemplates.LinkTemplate.LinkFieldId.ToString());
                    ItemsListData.GtmData = new GtmDataModel();
                    ItemsListData.GtmData.Event = Utils.GetValue(banners, BaseTemplates.GTMTemplate.GtmEventFieldId);
                    ItemsListData.GtmData.Category = Utils.GetValue(banners, BaseTemplates.GTMTemplate.GtmCategoryFieldId);
                    ItemsListData.GtmData.Sub_category = Utils.GetValue(banners, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId);
                    ItemsListData.GtmData.Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item);
                    ItemsListData.GtmData.Label = Utils.GetValue(banners, BaseTemplates.GTMTemplate.GtmLabelFieldId);
                    aboutUsModel.Banners.Add(ItemsListData);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return aboutUsModel;
        }
    }
}