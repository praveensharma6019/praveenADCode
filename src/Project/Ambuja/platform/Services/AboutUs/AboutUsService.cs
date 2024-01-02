using Glass.Mapper.Sc.Web.Mvc;
using Project.AmbujaCement.Website.Helpers;
using Project.AmbujaCement.Website.Models.AboutUsPage;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AmbujaCement.Website.Services.AboutUs
{
    public class AboutUsService : IAboutUsService
    {
        private readonly IMvcContext _mvcContext;
        public AboutUsService(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }

        public AchievementsModel GetAchievementsModel(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var getAchievementsModel = _mvcContext.GetDataSourceItem<AchievementsModel>();
                return getAchievementsModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public MessageCardModel GetMessageCardModel(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var getMessageCardModel = _mvcContext.GetDataSourceItem<MessageCardModel>();
                return getMessageCardModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public VisionMissionCardModel GetVisionMissionCardModel(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var getVisionMissionCardModel = _mvcContext.GetDataSourceItem<VisionMissionCardModel>();
                return getVisionMissionCardModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ICanCardsModel GetICanCardsModel(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var getICanCardsModel = _mvcContext.GetDataSourceItem<ICanCardsModel>();
                return getICanCardsModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}