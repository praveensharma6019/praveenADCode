using Glass.Mapper.Sc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models.About_Us;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AAHL.Website.Services.Certifications
{
    public class CertificationsService : ICertificationsService
    {
        private readonly ISitecoreService _sitecoreService;

        public CertificationsService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }
        public CertificationsModel GetCertificate(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<CertificationsModel>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}