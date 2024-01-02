using Glass.Mapper.Sc.Web.Mvc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models.AdvertisingSponsorships;
using Project.AAHL.Website.Models.Common;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AAHL.Website.Services.Common
{
    public class AdvertisingSponsorships : IAdvertisingSponsorships
    {

        private readonly IMvcContext _mvcContext;

        public AdvertisingSponsorships(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }


        public Partner GetPartner(Rendering rendering)
        {

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _mvcContext.GetDataSourceItem<Partner>();
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Advertisement GetAdvertisement(Rendering rendering)
        {

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _mvcContext.GetDataSourceItem<Advertisement>();
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}