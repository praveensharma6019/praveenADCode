using Glass.Mapper.Sc.Web.Mvc;
using Project.AmbujaCement.Website.Helpers;
using Project.AmbujaCement.Website.Models.GetInTouchPage;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AmbujaCement.Website.Services.GetInTouch
{
    public class GetInTouchService : IGetInTouchService
    {

        private readonly IMvcContext _mvcContext;
        public GetInTouchService(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }

        public GetInTouchDetailsModel GetGetInTouchDetailsModel(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var getInTouchDetailsModel = _mvcContext.GetDataSourceItem<GetInTouchDetailsModel>();
                return getInTouchDetailsModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public TwoColumnCardModel GetTwoColumnCardModel(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var twoColumnCardModel = _mvcContext.GetDataSourceItem<TwoColumnCardModel>();
                return twoColumnCardModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}