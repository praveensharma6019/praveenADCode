using Project.AdaniInternationalSchool.Website.Helpers;
using Project.AdaniInternationalSchool.Website.Models;
using Project.AdaniInternationalSchool.Website.Models.Academics;
using Project.AdaniInternationalSchool.Website.Templates;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.AdaniInternationalSchool.Website.Services.Academics
{
    public class AcademicsService : IAcademicsService
    {
        readonly Item _contextItem;
        public AcademicsService()
        {
            _contextItem = Sitecore.Context.Item;
        }

        public AcademicsModel RenderDetails(Rendering rendering)
        {
            var dsItem = Utils.GetRenderingDatasource(rendering);
            if (dsItem == null) return null;

            try
            {
                return new AcademicsModel
                {
                    SideNav = GetSubNav(dsItem),
                    Details = GetDetails(dsItem)
                };
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return null;
        }

        public List<ImageGalleryItemModel> RenderImageGallery(Rendering rendering)
        {
            var dsItem = Utils.GetRenderingDatasource(rendering);
            if (dsItem == null) return null;

            try
            {
                var list = new List<ImageGalleryItemModel>();
                var index = 0;
                foreach (Item galleryItem in dsItem.Children)
                {
                    var imgGalleyModel = AcademicServiceHelper<ImageGalleryItemModel>.InitModelWithCommonImgFields(galleryItem);
                    imgGalleyModel.Id = ++index;
                    imgGalleyModel.ImageTitle = Utils.GetValue(galleryItem, BaseTemplates.TitleTemplate.TitleFieldId, galleryItem.Name);
                    imgGalleyModel.ThumbImageAlt = Utils.GetValue(galleryItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);
                    imgGalleyModel.ThumbImageSource = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                    imgGalleyModel.ThumbImageSourceMobile = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
                    imgGalleyModel.ThumbImageSourceTablet = Utils.GetImageURLByFieldId(galleryItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);
                    list.Add(imgGalleyModel);
                }

                return list;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return null;
        }

        private List<SideNavItemModel> GetSubNav(Item item)
        {
            var sideNav = item.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "SideNav"));
            if (sideNav != null)
            {
                var list = new List<SideNavItemModel>();

                foreach (Item child in sideNav.Children)
                {
                    var linkItem = ServiceHelper.GetLinkItem(child);
                    var linkItemTag = GTMTagServiceHelpers.GetItemGTMTags(child);
                    list.Add(new SideNavItemModel
                    {
                        Title = linkItem.Label,
                        Link = linkItem.Url,
                        Active = _contextItem.Paths.ContentPath.EndsWith(linkItem.Url, StringComparison.CurrentCultureIgnoreCase),
                        GtmEvent = linkItemTag.GtmEvent,
                        GtmCategory = linkItemTag.GtmCategory,
                        GtmSubCategory = linkItemTag.GtmSubCategory
                    });
                }
                return list;
            }

            return null;
        }

        private List<object> GetDetails(Item item)
        {
            var detailFolder = item.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Details"));

            if (detailFolder != null)
            {
                var list = new List<object>();
                foreach (Item child in detailFolder.Children)
                {
                    switch (child.TemplateName)
                    {
                        case "Overview":
                            list.Add(GetOverview(child));
                            break;
                        case "Pedagogy":
                            list.Add(GetPedagogy(child));
                            break;
                        case "Guide":
                            list.Add(GetGuide(child));
                            break;
                        case "Curriculum":
                            list.Add(GetCurriculum(child));
                            break;
                        default:
                            list.Add(GetOverview(child));
                            break;
                    }
                }

                return list;
            }

            return null;
        }

        private CurriculumModel GetCurriculum(Item item)
        {
            var model = AcademicServiceHelper<CurriculumModel>.InitModelWithCommonFields(item);
            model.Variant = Utils.GetValue(item, BaseTemplates.VariantTemplate.VariantFieldId);
            model.Features = GetFeatures(item);
            model.Images = GetImages(item);
            return model;
        }

        private List<FeatureModel> GetFeatures(Item item)
        {
            var list = new List<FeatureModel>();
            var features = item.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Features"))?.Children;
            if (features != null)
            {
                foreach (Item featureItem in features)
                {
                    var featureModel = AcademicServiceHelper<FeatureModel>.InitModelWithCommonImgFields(featureItem);
                    featureModel.FeatureTitle = Utils.GetValue(featureItem, BaseTemplates.TitleTemplate.TitleFieldId, featureItem.Name);
                    list.Add(featureModel);
                }
            }

            return list;
        }

        private List<ImageModel> GetImages(Item item)
        {
            var list = new List<ImageModel>();
            var images = item.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Images"))?.Children;
            if (images != null)
            {
                foreach (Item imageItem in images)
                {
                    var imgModel = AcademicServiceHelper<ImageModel>.InitModelWithCommonImgFields(imageItem);
                    list.Add(imgModel);
                }
            }

            return list;
        }

        private GuideModel GetGuide(Item item)
        {
            var model = AcademicServiceHelper<GuideModel>.InitModelWithCommonFields(item);
            model.Variant = Utils.GetValue(item, BaseTemplates.VariantTemplate.VariantFieldId);
            model.Features = GetFeatures(item);
            return model;
        }

        private PedagogyModel GetPedagogy(Item item)
        {
            var model = AcademicServiceHelper<PedagogyModel>.InitModelWithCommonFields(item);
            model.Variant = Utils.GetValue(item, BaseTemplates.VariantTemplate.VariantFieldId);
            return model;
        }

        private OverviewModel GetOverview(Item item)
        {
            var model = AcademicServiceHelper<OverviewModel>.InitModelWithCommonFields(item);
            model.ImageSource = Utils.GetImageURLByFieldId(item, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
            model.ImageSourceMobile = Utils.GetImageURLByFieldId(item, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
            model.ImageSourceTablet = Utils.GetImageURLByFieldId(item, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);
            model.ImageAlt = Utils.GetValue(item, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);
            return model;
        }
    }
}