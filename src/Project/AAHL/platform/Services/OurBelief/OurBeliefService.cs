using Glass.Mapper.Sc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models;
using Project.AAHL.Website.Models.OurBelief;
using Project.AAHL.Website.Templates;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Services.OurBelief
{
    public class OurBeliefService : IOurBeliefService
    {
        private readonly ISitecoreService _sitecoreService;

        public OurBeliefService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public OurPurposeModel GetOurPurpose(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<OurPurposeModel>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public OurValuesModel GetOurValues(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<OurValuesModel>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public OurMissionModel GetOurMission(Rendering rendering)
        {
            OurMissionModel ourMissionModelModel = null;
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                ourMissionModelModel = new OurMissionModel();
                ourMissionModelModel.Heading = Utils.GetValue(datasource, BaseTemplate.Fields.Heading);
                ourMissionModelModel.Description = Utils.GetValue(datasource, BaseTemplate.Fields.Description);
                List<ImageListModel> imageItemModels = new List<ImageListModel>();
                List<OurMissionList> ourMissonItemModels = new List<OurMissionList>();
                var ImageListModelList = Sitecore.Context.Database.GetItem(BaseTemplate.OurMissionSection.ImageList);
                var OurMissionList = Sitecore.Context.Database.GetItem(BaseTemplate.OurMissionSection.ItemsList);
                foreach (Item banners in ImageListModelList.Children)
                {
                    ImageListModel ItemsListData = new ImageListModel();
                    ItemsListData.ImagePath = Utils.GetImageURLByFieldId(banners, BaseTemplate.ImageSourceTemplate.ImageSourceFieldId);
                    ItemsListData.MImagePath = Utils.GetImageURLByFieldId(banners, BaseTemplate.ImageSourceTemplate.ImageSourceMobileFieldId);
                    ItemsListData.TImagePath = Utils.GetImageURLByFieldId(banners, BaseTemplate.ImageSourceTemplate.ImageSourceTabletFieldId);
                    ItemsListData.Imgalttext = Utils.GetValue(banners, BaseTemplate.ImageSourceTemplate.ImageAltFieldId);
                    imageItemModels.Add(ItemsListData);
                }
                ourMissionModelModel.ImageItems = imageItemModels;

                foreach (Item banners in OurMissionList.Children)
                {
                    OurMissionList OurMissionListData = new OurMissionList();
                    OurMissionListData.Heading = Utils.GetValue(banners, BaseTemplate.Fields.Heading);
                    OurMissionListData.Description = Utils.GetValue(banners, BaseTemplate.Fields.Description);
                    ourMissonItemModels.Add(OurMissionListData);
                }
                ourMissionModelModel.Items = ourMissonItemModels;
                return ourMissionModelModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ServiceExcellenceModel GetServiceExcellence(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<ServiceExcellenceModel>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}