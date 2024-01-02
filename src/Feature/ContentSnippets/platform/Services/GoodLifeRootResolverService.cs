using Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Services
{
    public class GoodLifeRootResolverService : IGoodLifeRootResolverService
    {

        private readonly ILogRepository _logRepository;
        public GoodLifeRootResolverService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public GoodLifeData GetGoodLifeData(Rendering rendering)
        {
            GoodLifeData goodLifeDataList = new GoodLifeData();
            try
            {

                goodLifeDataList.data = GetGoodLifeDataItem(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return goodLifeDataList;
        }

        public List<Object> GetGoodLifeDataItem(Rendering rendering)
        {
            List<object> goodLifeDataItemList = new List<object>();

            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                GoodLifeDataItem goodLifedataItem;

                if (datasource.TemplateID == Templates.GoodLifeFolder.TemplateID && datasource.Children.ToList().Count > 0)
                {
                    List<Item> children = datasource.Children.Where(x => x.TemplateID == Templates.GoodLife.TemplateID).ToList();
                    if (children != null && children.Count > 0)
                    {
                        foreach (Item item in children)
                        {

                            goodLifedataItem = new GoodLifeDataItem();

                            goodLifedataItem.ImgSrc = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, Templates.Image.FieldsName.Image) != null ?
                                      Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, Templates.Image.FieldsName.Image) : "";

                            goodLifedataItem.ImgAlt = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, Templates.Image.FieldsName.Image) != null ?
                                      Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, Templates.Image.FieldsName.Image).Fields["Alt"].Value : "";

                            goodLifedataItem.ImgTitle = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, Templates.Image.FieldsName.Image) != null ?
                                      Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, Templates.Image.FieldsName.Image).Fields["TItle"].Value : "";

                            goodLifedataItem.Title = item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";
                            goodLifedataItem.status = item.Fields[Templates.ITitle.statusID].Value != null ? item.Fields[Templates.ITitle.statusID].Value : "";

                            goodLifedataItem.Gallery = GetGalleryImages(item?.Fields[Templates.ITitle.Gallery]);

                            goodLifeDataItemList.Add(goodLifedataItem);
                        }
                    }
                }

            }
            catch (Exception ex)
            {


                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return goodLifeDataItemList;
        }

        private List<GoodLifeGallery> GetGalleryImages(Field field)
        {
            List<GoodLifeGallery> galleryList = new List<GoodLifeGallery>();
            try
            {
                MultilistField mulField = field;
                foreach (Item item in mulField.GetItems())
                {
                    GoodLifeGallery goodLifeGallery = new GoodLifeGallery();
                    goodLifeGallery.ImgSrc = Helper.GetImageURLbyField(item?.Fields[Templates.Image.FieldsID.Image]);
                    goodLifeGallery.ImgAlt = Helper.GetImageAltbyField(item?.Fields[Templates.Image.FieldsID.Image]);
                    goodLifeGallery.Imgtitle = Helper.GetImageDetails(item, Templates.Image.FieldsID.Image.ToString()) != null ?
                                         Helper.GetImageDetails(item, Templates.Image.FieldsID.Image.ToString()).Fields[Templates.ImageFeilds.Fields.AltFieldName].Value : "";
                    galleryList?.Add(goodLifeGallery);
                }
            }
            catch (Exception ex)
            {


                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return galleryList;
        }
    }
}
