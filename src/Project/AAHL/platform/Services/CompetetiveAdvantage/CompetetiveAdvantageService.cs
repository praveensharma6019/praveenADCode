using Glass.Mapper.Sc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models.CompetetiveAdvantage;
using Sitecore.Mvc.Presentation;
using System;


namespace Project.AAHL.Website.Services.CompetetiveAdvantage
{
    public class CompetetiveAdvantageService : ICompetetiveAdvantageService
    {

        private readonly ISitecoreService _sitecoreService;

        public CompetetiveAdvantageService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public SectionWithImagesModel GetSectionWithImages(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<SectionWithImagesModel>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}