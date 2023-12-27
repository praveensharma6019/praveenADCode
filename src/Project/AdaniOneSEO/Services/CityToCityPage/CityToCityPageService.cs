using Glass.Mapper.Sc;
using Project.AdaniOneSEO.Website.Helpers;
using Project.AdaniOneSEO.Website.Models.CityToCityPagev2;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;

namespace Project.AdaniOneSEO.Website.Services.CityToCityPage
{
    public class CityToCityPageService: ICityToCityPageService
    {

        private readonly ISitecoreService _sitecoreService;

        public CityToCityPageService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public AirportCityDetailsModel GetAirportCityDetailsModel(Rendering rendering)
        {
            AirportCityDetailsModel airportCityDetailsModel = null;

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                airportCityDetailsModel = new AirportCityDetailsModel();
                airportCityDetailsModel.CityName = Utils.GetValue(datasource, BaseTemplates.CityToCityDetails.CityName);
                airportCityDetailsModel.CityImage = Utils.GetImageURLByFieldId(datasource, BaseTemplates.CityToCityDetails.CityImage);
                airportCityDetailsModel.AirportName = Utils.GetValue(datasource, BaseTemplates.CityToCityDetails.AirportName);
                airportCityDetailsModel.AirportDescription = Utils.GetValue(datasource, BaseTemplates.CityToCityDetails.AirportDescription);
                airportCityDetailsModel.AboutAirportHeading = Utils.GetValue(datasource, BaseTemplates.CityToCityDetails.AboutAirportHeading);
                airportCityDetailsModel.AboutAirportDescription = Utils.GetValue(datasource, BaseTemplates.CityToCityDetails.AboutAirportDescription);
                airportCityDetailsModel.PlacesToVisitHeading = Utils.GetValue(datasource, BaseTemplates.CityToCityDetails.PlacesToVisitHeading);

                if (datasource.HasChildren)
                {
                    List<PlacesToVisit> placelist = new List<PlacesToVisit>();
                    foreach (Item child in datasource.Children)
                    {
                        PlacesToVisit ItemsListData = new PlacesToVisit();
                        ItemsListData.PlaceName = Utils.GetValue(child, BaseTemplates.PlaceToVisitDetails.PlaceName);
                        ItemsListData.LocationIcon = Utils.GetValue(child, BaseTemplates.PlaceToVisitDetails.LocationIcon);
                        ItemsListData.PlaceLink = Utils.GetLinkURL(child, BaseTemplates.PlaceToVisitDetails.PlaceLink.ToString());
                        placelist.Add(ItemsListData);
                    }
                    airportCityDetailsModel.PlacesToVisitInCity = placelist;
                }
                airportCityDetailsModel.BestTimeToVisitHeading = Utils.GetValue(datasource, BaseTemplates.CityToCityDetails.BestTimeToVisitHeading);
                airportCityDetailsModel.BestTimeToVisitDescription = Utils.GetValue(datasource, BaseTemplates.CityToCityDetails.BestTimeToVisitDescription);
                airportCityDetailsModel.AirportCityCode = Utils.GetValue(datasource, BaseTemplates.CityToCityDetails.AirportCityCode);
                airportCityDetailsModel.AirportCityAdddress = Utils.GetValue(datasource, BaseTemplates.CityToCityDetails.AirportCityAdddress);
                airportCityDetailsModel.CityType = Utils.GetValue(datasource, BaseTemplates.CityToCityDetails.CityType);

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return airportCityDetailsModel;

        }
    }
}