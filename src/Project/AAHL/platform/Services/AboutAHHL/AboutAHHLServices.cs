using Glass.Mapper.Sc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models.AboutAHHL;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AAHL.Website.Services.AboutAHHL
{
    public class AboutAHHLServices : IAboutAHHLServices
    {
        //readonly Item _contextItem;
        private readonly ISitecoreService _sitecoreService;
        public AboutAHHLServices(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public OurAirportModel GetOurAiport(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ourAirportdata = _sitecoreService.GetItem<OurAirportModel>(datasource);
                return ourAirportdata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public OurAirportTabModel GetOurAiportTab(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ourAirportTabdata = _sitecoreService.GetItem<OurAirportTabModel>(datasource);
                return ourAirportTabdata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}