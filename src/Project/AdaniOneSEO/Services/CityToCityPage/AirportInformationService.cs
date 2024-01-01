using Glass.Mapper.Sc;
using Project.AdaniOneSEO.Website.Helpers;
using Project.AdaniOneSEO.Website.Models.CityToCityPagev2;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AdaniOneSEO.Website.Services.CityToCityPage
{
    public class AirportInformationService : IAirportInformationService
    {
        private readonly ISitecoreService _sitecoreService;

        public AirportInformationService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public AirportInformationModel GetAirportInformationModel(Rendering rendering)
        {
            AirportInformationModel data = new AirportInformationModel();
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;
                
                data.Title = Utils.GetValue(datasource, BaseTemplates.CityToCityDetails.AirportInformationFields.Title);
                data.ToCity = Utils.GetValue(datasource, BaseTemplates.CityToCityDetails.AirportInformationFields.ToCity);
                data.ToCityDescription = Utils.GetValue(datasource, BaseTemplates.CityToCityDetails.AirportInformationFields.ToCityDescription);
                data.FromCity = Utils.GetValue(datasource, BaseTemplates.CityToCityDetails.AirportInformationFields.FromCity);
                data.FromCityDescription = Utils.GetValue(datasource, BaseTemplates.CityToCityDetails.AirportInformationFields.FromCityDescription);               
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return data;
        }
    }
}