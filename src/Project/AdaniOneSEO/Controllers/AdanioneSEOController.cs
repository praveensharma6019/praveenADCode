using Newtonsoft.Json;
using Project.AdaniOneSEO.Website.Models;
using Project.AdaniOneSEO.Website.Models.FlightsToDestination;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Mvc;

namespace Project.AdaniOneSEO.Website.Controllers
{
    public class AdanioneSEOController : Controller
    {
        // GET: Adanione
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllFlights()
        {
            var departingCityItem = Sitecore.Context.Database.GetItem(ID.Parse("{81C7C032-8AEA-4233-8EA7-4B74A853FBFB}"));

            var arrivalCityDetails = new ArrivalCityDetails();
            arrivalCityDetails.CityName = departingCityItem.Fields[ID.Parse("{FAE42E25-C7C7-4D86-9A42-2974DBF58B19}")].Value;
            arrivalCityDetails.CityCode = departingCityItem.Fields[ID.Parse("{69D2E525-50F0-4C37-8C71-D1E149ACFD58}")].Value;

            Field fromCityListField = departingCityItem.Fields["FromCityList"];

            var flightApiResponse = new List<CheapestFlightDetailsModel>();

            if (fromCityListField != null && fromCityListField.Type == "Treelist")
            {
                MultilistField multiListField = (MultilistField)fromCityListField;
                if (multiListField.TargetIDs.Any())
                {
                    foreach (var targetId in multiListField.TargetIDs)
                    {
                        Item linkedItem = departingCityItem.Database.GetItem(targetId);
                        if (linkedItem != null)
                        {
                            string fromCity_CityCode = linkedItem["CityCode"];

                            DateTime today = DateTime.Now;
                            DateTime tomorrow = today.AddDays(1);
                            string tomorrowAsString = tomorrow.ToString("dd-MM-yyyy");

                            string apiUrl = "https://flightbooking-dev.adanione.cloud/api/Flights/InternalfareCalendar?from={0}&to={1}&originccode=IN&destccode=IN&adt=1&deptdate={2}&returndate={3}";
                            //string apiUrl = "https://aksdev.adanione.cloud/api/flightbookingv2/api/Flights/InternalfareCalendar?from={0}&to={1}&originccode=IN&destccode=IN&adt=1&deptdate={2}&returndate={3}";
                            string actualApiUrl = string.Format(apiUrl, fromCity_CityCode, arrivalCityDetails.CityCode, tomorrowAsString, tomorrowAsString);

                            var client = new HttpClient();
                            client.DefaultRequestHeaders.Add("traceId", "test");
                            client.DefaultRequestHeaders.Add("channelId", "Web");
                            HttpResponseMessage response = client.GetAsync(actualApiUrl).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                string content = response.Content.ReadAsStringAsync().Result;
                                CheapestFlightDetailsModel result = JsonConvert.DeserializeObject<CheapestFlightDetailsModel>(content);
                                if (result != null && result.data != null)
                                {
                                    flightApiResponse.Add(result);
                                }
                            }
                        }
                    }
                }
            }
            return Json(flightApiResponse, JsonRequestBehavior.AllowGet);
        }

        public IEnumerable<ToCityDetailsResponseModel> GetAllFlightsToCity(DateTime date, Item cityItem)
        {
            var searchApiResponse = new List<ToCityDetailsResponseModel>();
            if (cityItem != null)
            {
                var flightSearchAPIRequestModel = new FlightSearchAPIRequestModel();

                var passengerDetails = new PassengerDetails();
                passengerDetails.type = "ADT";
                passengerDetails.count = 1;

                flightSearchAPIRequestModel.passengers = new List<PassengerDetails>
                {
                    passengerDetails
                };

                var processingInfo = new ProcessingInfo();
                processingInfo.countryCode = "IN";
                processingInfo.isSpecialFare = false;
                processingInfo.sectorInd = "D";
                processingInfo.tripType = "0";
                processingInfo.fareType = "STUDENT";

                flightSearchAPIRequestModel.processingInfo = processingInfo;

                var travelPreferences = new TravelPreferences();

                var farePref = new FarePref();
                farePref.fareDisplayCurrency = "INR";

                var cabinPref = new CabinPref();
                cabinPref.cabin = "Economy";

                travelPreferences.farePref = farePref;
                travelPreferences.cabinPref = new List<CabinPref> { cabinPref };

                flightSearchAPIRequestModel.travelPreferences = travelPreferences;

                var originDestinationInformation = new OriginDestinationInformation();

                originDestinationInformation.rph = 1;

                var departureDateTime = new DepartureDateTime();
                string dateAsString = date.ToString("dd-MM-yyyy");
                departureDateTime.windowAfter = dateAsString;
                departureDateTime.windowBefore = dateAsString;

                originDestinationInformation.departureDateTime = departureDateTime;

                Field toCityDroptreeField = cityItem.Fields["ToCity"];

                if (toCityDroptreeField != null && toCityDroptreeField.TypeKey == "droptree")
                {
                    ID toCityDropTreeSelectedItemId = ((ReferenceField)toCityDroptreeField).TargetID;

                    Item toCityDropTreeSelectedItem = GetItemById(toCityDropTreeSelectedItemId.ToString());

                    if (toCityDropTreeSelectedItem != null)
                    {
                        if (toCityDropTreeSelectedItem.TemplateID == ID.Parse("{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}"))
                        {
                            Field fromCityDroptreeField = cityItem.Fields["FromCity"];

                            if (fromCityDroptreeField != null && fromCityDroptreeField.TypeKey == "droptree")
                            {
                                ID fromCityDropTreeSelectedItemId = ((ReferenceField)fromCityDroptreeField).TargetID;

                                Item fromCityDropTreeSelectedItem = GetItemById(fromCityDropTreeSelectedItemId.ToString());

                                if (fromCityDropTreeSelectedItem != null && fromCityDropTreeSelectedItem.TemplateID == ID.Parse("{5C682FD6-B029-479D-8A9F-42EF51097F60}"))
                                {
                                    var originLocationDetails = new OriginLocationDetails();

                                    originLocationDetails.radius = string.Empty;
                                    originLocationDetails.location = fromCityDropTreeSelectedItem.Fields["CityName"].Value;
                                    originLocationDetails.countryCode = "IN";
                                    originLocationDetails.locationCode = fromCityDropTreeSelectedItem.Fields["CityCode"].Value;

                                    originDestinationInformation.originLocation = originLocationDetails;

                                    if (toCityDropTreeSelectedItem.Children.Any())
                                    {
                                        foreach (Item item in toCityDropTreeSelectedItem.Children)
                                        {
                                            if (item != null && item.ID != fromCityDropTreeSelectedItem.ID)
                                            {
                                                var destinationLocationDetails = new DestinationLocationDetails();

                                                destinationLocationDetails.radius = string.Empty;
                                                destinationLocationDetails.location = item.Fields["CityName"].Value;
                                                destinationLocationDetails.countryCode = "IN";
                                                destinationLocationDetails.locationCode = item.Fields["CityCode"].Value;

                                                originDestinationInformation.destinationLocation = destinationLocationDetails;

                                                flightSearchAPIRequestModel.originDestinationInformation = new List<OriginDestinationInformation> { originDestinationInformation };

                                                Item SEOFlightSearchApiUrlItem = GetItemById("{614295D1-60D4-4D94-A46C-6C7097840923}");
                                                if (SEOFlightSearchApiUrlItem != null)
                                                {
                                                    string flightSearchApiUrl = SEOFlightSearchApiUrlItem.Fields["APIEndpoint"].Value;

                                                    if (!string.IsNullOrEmpty(flightSearchApiUrl))
                                                    {
                                                        var client = new HttpClient();
                                                        client.Timeout = TimeSpan.FromMinutes(10);

                                                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                        client.DefaultRequestHeaders.Add("traceId", "60u0VZxl5ai7jKJ7laMqZqKHrUlFIufT");
                                                        client.DefaultRequestHeaders.Add("agentId", "8c41ea48-a1e9-4d33-a6e6-1c54f38fab0e");
                                                        client.DefaultRequestHeaders.Add("channelId", "web");
                                                        client.DefaultRequestHeaders.Add("Cookie", "_abck=F10B7D75E210B94D41116372588A8B29~-1~YAAQB/QwFy5otXyKAQAAULDamApfSnb28l7FBpGh3/ZcabWMFCs+rmLzNlGocJtLh+5rcz5TKfzM4wwAEm8jjIVOkew+4cYJu9OD9Iiz9QtGdy0NiHZeGAJefRCsAMjsWh+F4q8z1mydKpOlE8lPTmwDRVs2sbHH2d9R2GdU7Z5z/3rzARTzSXk5ogOjo5syALBOMbU5RFul5PxP3UNNhA0oBSDbvFUm8dKSn1ffx7sY34WN1afhVopRQQiNbbDsLoiQagTPcbJByCoAiTaH+i/V+8XXY42E/tfOuNX94qWzk+zeFuY1KquPo0M4XERjD4fkzrBjEebIF3ivRanTN+dnto3oulV0PPDhcq31TeT4e4eMivm5LsFq5gW+xl5l4nVnIP5oTnzLhWMS089Wo6gOtp/v0NLPBiLg~-1~-1~-1");

                                                        var serlisedObj = JsonConvert.SerializeObject(flightSearchAPIRequestModel);
                                                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, flightSearchApiUrl);
                                                        request.Content = new StringContent(serlisedObj,
                                                                            Encoding.UTF8,
                                                                            "application/json");
                                                        var response = client.SendAsync(request).Result;

                                                        if (response.IsSuccessStatusCode)
                                                        {
                                                            string content = response.Content.ReadAsStringAsync().Result;
                                                            FlightSearchAPIResponseModel result = JsonConvert.DeserializeObject<FlightSearchAPIResponseModel>(content);
                                                            if (result != null && result.data != null && result.data.pricedItineraries.Count > 0)
                                                            {
                                                                var toCityDetailsResponseModel = new ToCityDetailsResponseModel();
                                                                var distinctFlightToCity = result.data.pricedItineraries;
                                                                var directFlightToCity = distinctFlightToCity.Select(i => i.airItinerary).Where(j => j.originDestinationOption[0].technicalStops.noOfStops == 0).FirstOrDefault();

                                                                if (directFlightToCity != null)
                                                                {
                                                                    var directFlightItineraryPricingInfo = distinctFlightToCity.Select(i => i).Where(j => j.airItinerary == directFlightToCity).FirstOrDefault();

                                                                    toCityDetailsResponseModel.airlineDetails = directFlightToCity.originDestinationOption[0].flightSegment[0].marketingAirline;
                                                                    toCityDetailsResponseModel.depatureAirportDetails = directFlightToCity.originDestinationOption[0].flightSegment[0].departureAirport;
                                                                    toCityDetailsResponseModel.departureTime = directFlightToCity.originDestinationOption[0].flightSegment[0].departureTime;
                                                                    toCityDetailsResponseModel.arrivalAirportDetails = directFlightToCity.originDestinationOption[0].flightSegment[0].arrivalAirport;
                                                                    toCityDetailsResponseModel.arrivalTime = directFlightToCity.originDestinationOption[0].flightSegment[0].arrivalTime;
                                                                    toCityDetailsResponseModel.duration = directFlightToCity.originDestinationOption[0].flightSegment[0].duration;
                                                                    toCityDetailsResponseModel.price = directFlightItineraryPricingInfo.airItineraryPricingInfo.totalFare.amount;

                                                                    searchApiResponse.Add(toCityDetailsResponseModel);
                                                                }

                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Field fromCityDroptreeField = cityItem.Fields["FromCity"];

                            if (fromCityDroptreeField != null && fromCityDroptreeField.TypeKey == "droptree")
                            {
                                ID fromCityDropTreeSelectedItemId = ((ReferenceField)fromCityDroptreeField).TargetID;

                                Item fromCityDropTreeSelectedItem = GetItemById(fromCityDropTreeSelectedItemId.ToString());

                                if (fromCityDropTreeSelectedItem != null && fromCityDropTreeSelectedItem.TemplateID == ID.Parse("{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}"))
                                {
                                    var destinationLocationDetails = new DestinationLocationDetails();

                                    destinationLocationDetails.radius = string.Empty;
                                    destinationLocationDetails.location = toCityDropTreeSelectedItem.Fields["CityName"].Value;
                                    destinationLocationDetails.countryCode = "IN";
                                    destinationLocationDetails.locationCode = toCityDropTreeSelectedItem.Fields["CityCode"].Value;

                                    originDestinationInformation.destinationLocation = destinationLocationDetails;

                                    if (fromCityDropTreeSelectedItem.Children.Any())
                                    {
                                        foreach (Item item in fromCityDropTreeSelectedItem.Children)
                                        {
                                            if (item != null && item.ID != toCityDropTreeSelectedItem.ID)
                                            {
                                                var originLocationDetails = new OriginLocationDetails();

                                                originLocationDetails.radius = string.Empty;
                                                originLocationDetails.location = item.Fields["CityName"].Value;
                                                originLocationDetails.countryCode = "IN";
                                                originLocationDetails.locationCode = item.Fields["CityCode"].Value;

                                                originDestinationInformation.originLocation = originLocationDetails;

                                                flightSearchAPIRequestModel.originDestinationInformation = new List<OriginDestinationInformation> { originDestinationInformation };

                                                Item SEOFlightSearchApiUrlItem = GetItemById("{614295D1-60D4-4D94-A46C-6C7097840923}");
                                                if (SEOFlightSearchApiUrlItem != null)
                                                {
                                                    string flightSearchApiUrl = SEOFlightSearchApiUrlItem.Fields["APIEndpoint"].Value;

                                                    if (!string.IsNullOrEmpty(flightSearchApiUrl))
                                                    {
                                                        var client = new HttpClient();
                                                        client.Timeout = TimeSpan.FromMinutes(10);

                                                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                        client.DefaultRequestHeaders.Add("traceId", "60u0VZxl5ai7jKJ7laMqZqKHrUlFIufT");
                                                        client.DefaultRequestHeaders.Add("agentId", "8c41ea48-a1e9-4d33-a6e6-1c54f38fab0e");
                                                        client.DefaultRequestHeaders.Add("channelId", "web");
                                                        client.DefaultRequestHeaders.Add("Cookie", "_abck=F10B7D75E210B94D41116372588A8B29~-1~YAAQB/QwFy5otXyKAQAAULDamApfSnb28l7FBpGh3/ZcabWMFCs+rmLzNlGocJtLh+5rcz5TKfzM4wwAEm8jjIVOkew+4cYJu9OD9Iiz9QtGdy0NiHZeGAJefRCsAMjsWh+F4q8z1mydKpOlE8lPTmwDRVs2sbHH2d9R2GdU7Z5z/3rzARTzSXk5ogOjo5syALBOMbU5RFul5PxP3UNNhA0oBSDbvFUm8dKSn1ffx7sY34WN1afhVopRQQiNbbDsLoiQagTPcbJByCoAiTaH+i/V+8XXY42E/tfOuNX94qWzk+zeFuY1KquPo0M4XERjD4fkzrBjEebIF3ivRanTN+dnto3oulV0PPDhcq31TeT4e4eMivm5LsFq5gW+xl5l4nVnIP5oTnzLhWMS089Wo6gOtp/v0NLPBiLg~-1~-1~-1");

                                                        var serlisedObj = JsonConvert.SerializeObject(flightSearchAPIRequestModel);
                                                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, flightSearchApiUrl);
                                                        request.Content = new StringContent(serlisedObj,
                                                                            Encoding.UTF8,
                                                                            "application/json");
                                                        var response = client.SendAsync(request).Result;

                                                        if (response.IsSuccessStatusCode)
                                                        {
                                                            string content = response.Content.ReadAsStringAsync().Result;
                                                            FlightSearchAPIResponseModel result = JsonConvert.DeserializeObject<FlightSearchAPIResponseModel>(content);
                                                            if (result != null && result.data != null && result.data.pricedItineraries.Count > 0)
                                                            {
                                                                var toCityDetailsResponseModel = new ToCityDetailsResponseModel();
                                                                var distinctFlightToCity = result.data.pricedItineraries;
                                                                var directFlightToCity = distinctFlightToCity.Select(i => i.airItinerary).Where(j => j.originDestinationOption[0].technicalStops.noOfStops == 0).FirstOrDefault();
                                                                if (directFlightToCity != null)
                                                                {
                                                                    var directFlightItineraryPricingInfo = distinctFlightToCity.Select(i => i).Where(j => j.airItinerary == directFlightToCity).FirstOrDefault();

                                                                    toCityDetailsResponseModel.airlineDetails = directFlightToCity.originDestinationOption[0].flightSegment[0].marketingAirline;
                                                                    toCityDetailsResponseModel.depatureAirportDetails = directFlightToCity.originDestinationOption[0].flightSegment[0].departureAirport;
                                                                    toCityDetailsResponseModel.departureTime = directFlightToCity.originDestinationOption[0].flightSegment[0].departureTime;
                                                                    toCityDetailsResponseModel.arrivalAirportDetails = directFlightToCity.originDestinationOption[0].flightSegment[0].arrivalAirport;
                                                                    toCityDetailsResponseModel.arrivalTime = directFlightToCity.originDestinationOption[0].flightSegment[0].arrivalTime;
                                                                    toCityDetailsResponseModel.duration = directFlightToCity.originDestinationOption[0].flightSegment[0].duration;
                                                                    toCityDetailsResponseModel.price = directFlightItineraryPricingInfo.airItineraryPricingInfo.totalFare.amount;

                                                                    searchApiResponse.Add(toCityDetailsResponseModel);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        return searchApiResponse.OrderBy(i => i.price).Take(15);
                    }
                }
            }
            return searchApiResponse;
        }

        public ActionResult CreateSitecoreItem(string date)
        {
            DateTime flightcriteriaDate = DateTime.Now;
            if (string.IsNullOrEmpty(date))
            {
                flightcriteriaDate = flightcriteriaDate.AddDays(5);
            }
            else
            {
                flightcriteriaDate = DateTime.Parse(date).AddDays(5);
            }

            bool success = false;
            string message = string.Empty;
            var messageList = new List<string>();

            List<int> days = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            foreach (int day in days)
            {
                string folderItemName = string.Empty;

                string domesticFlightsFolderItemID = "{3FDC2D71-8830-489C-86CE-53800018E124}";
                Item domesticFlightsFolderItem = GetItemById(domesticFlightsFolderItemID);

                var appRouteItems = domesticFlightsFolderItem.Children.Where(i => i.TemplateID == ID.Parse("{1C5E12C1-CB4C-4284-9E98-AD75F473346F}"));

                if (appRouteItems.Any())
                {
                    foreach (Item mainItem in appRouteItems)
                    {
                        var finalRendering = mainItem.Visualization.GetRenderings(Sitecore.Context.Device, false);
                        var filterOptionRendering = finalRendering.Where(i => i.RenderingItem.ID == ID.Parse("{13CD6063-29CF-48C9-976F-6E7E5E5704A0}")).FirstOrDefault();
                        if (filterOptionRendering != null)
                        {
                            var cityItemId = filterOptionRendering.Settings.DataSource;
                            Item cityItem = GetItemById(cityItemId);
                            if (cityItem != null)
                            {
                                folderItemName = flightcriteriaDate.AddDays(day).ToString("dd-MM-yyyy");

                                string flightDetailsFolderTemplateId = "{1C5BFDE8-CAF4-461C-891C-F1272C33E928}";
                                string filterOptionsResponseListTemplateId = "{BBCDAA1D-37B4-4753-9B24-B85520F0E350}";

                                if (cityItem.Children.Any(i => i.Name == folderItemName))
                                {
                                    success = false;
                                    message = "New Item:" + folderItemName + " is already defined on this level for " + mainItem.Name;
                                    messageList.Add(message);
                                }
                                else
                                {
                                    using (new SecurityDisabler())
                                    {
                                        Item dateFolderItem = cityItem.Add(folderItemName, new TemplateID(new ID(flightDetailsFolderTemplateId)));

                                        if (dateFolderItem != null)
                                        {
                                            Log.Info($"Created item: {dateFolderItem.Paths.FullPath}", dateFolderItem);
                                            dateFolderItem.Editing.BeginEdit();

                                            dateFolderItem.Fields["Date"].Value = Sitecore.DateUtil.ToIsoDate(flightcriteriaDate.AddDays(day));

                                            dateFolderItem.Editing.EndEdit();

                                            success = true;
                                            message = "New Item created:" + dateFolderItem.Name + " under " + mainItem.Name;
                                            messageList.Add(message);

                                            IEnumerable<ToCityDetailsResponseModel> apiResponse = new List<ToCityDetailsResponseModel>();

                                            apiResponse = GetAllFlightsToCity(flightcriteriaDate.AddDays(day), cityItem);

                                            if (mainItem.Name.Contains("from"))
                                            {
                                                foreach (var item in apiResponse)
                                                {
                                                    string toCityItemByDate = string.Empty;

                                                    if (dateFolderItem.Children.Any(i => i.Name == item.arrivalAirportDetails.city))
                                                    {
                                                        toCityItemByDate = item.arrivalAirportDetails.city + "-" + item.arrivalAirportDetails.locationCode;
                                                    }
                                                    else
                                                    {
                                                        toCityItemByDate = item.arrivalAirportDetails.city;
                                                    }
                                                    Item filterOptionsResponseItem = dateFolderItem.Add(toCityItemByDate, new TemplateID(new ID(filterOptionsResponseListTemplateId)));

                                                    if (filterOptionsResponseItem != null)
                                                    {
                                                        filterOptionsResponseItem.Editing.BeginEdit();
                                                        filterOptionsResponseItem.Fields["code"].Value = item.airlineDetails.code;
                                                        filterOptionsResponseItem.Fields["flightNumber"].Value = item.airlineDetails.flightNumber;
                                                        filterOptionsResponseItem.Fields["name"].Value = item.airlineDetails.name;

                                                        filterOptionsResponseItem.Fields["arrivalLocationCode"].Value = item.arrivalAirportDetails.locationCode;
                                                        filterOptionsResponseItem.Fields["arrivalTerminal"].Value = item.arrivalAirportDetails.terminal;
                                                        filterOptionsResponseItem.Fields["arrivalName"].Value = item.arrivalAirportDetails.name;
                                                        filterOptionsResponseItem.Fields["arrivalCity"].Value = item.arrivalAirportDetails.city;
                                                        filterOptionsResponseItem.Fields["arrivalCountry"].Value = item.arrivalAirportDetails.country;
                                                        filterOptionsResponseItem.Fields["arrivalCountryCode"].Value = item.arrivalAirportDetails.countryCode;

                                                        filterOptionsResponseItem.Fields["depatureLocationCode"].Value = item.depatureAirportDetails.locationCode;
                                                        filterOptionsResponseItem.Fields["depatureTerminal"].Value = item.depatureAirportDetails.terminal;
                                                        filterOptionsResponseItem.Fields["depatureName"].Value = item.depatureAirportDetails.name;
                                                        filterOptionsResponseItem.Fields["depatureCity"].Value = item.depatureAirportDetails.city;
                                                        filterOptionsResponseItem.Fields["depatureCountry"].Value = item.depatureAirportDetails.country;
                                                        filterOptionsResponseItem.Fields["depatureCountryCode"].Value = item.depatureAirportDetails.countryCode;

                                                        filterOptionsResponseItem.Fields["arrivalTime"].Value = item.arrivalTime;
                                                        filterOptionsResponseItem.Fields["departureTime"].Value = item.departureTime;

                                                        TimeSpan timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(item.duration));
                                                        string formattedTime = $"{(int)timeSpan.TotalHours} h {timeSpan.Minutes} m";
                                                        filterOptionsResponseItem.Fields["duration"].Value = formattedTime;

                                                        filterOptionsResponseItem.Fields["price"].Value = item.price.ToString();

                                                        filterOptionsResponseItem.Editing.EndEdit();
                                                        Log.Info($"Created item: {filterOptionsResponseItem.Paths.FullPath}", filterOptionsResponseItem);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                foreach (var item in apiResponse)
                                                {
                                                    string toCityItemByDate = string.Empty;

                                                    if (dateFolderItem.Children.Any(i => i.Name == item.depatureAirportDetails.city))
                                                    {
                                                        toCityItemByDate = item.depatureAirportDetails.city + "-" + item.depatureAirportDetails.locationCode;
                                                    }
                                                    else
                                                    {
                                                        toCityItemByDate = item.depatureAirportDetails.city;
                                                    }
                                                    Item filterOptionsResponseItem = dateFolderItem.Add(toCityItemByDate, new TemplateID(new ID(filterOptionsResponseListTemplateId)));

                                                    if (filterOptionsResponseItem != null)
                                                    {
                                                        filterOptionsResponseItem.Editing.BeginEdit();
                                                        filterOptionsResponseItem.Fields["code"].Value = item.airlineDetails.code;
                                                        filterOptionsResponseItem.Fields["flightNumber"].Value = item.airlineDetails.flightNumber;
                                                        filterOptionsResponseItem.Fields["name"].Value = item.airlineDetails.name;

                                                        filterOptionsResponseItem.Fields["arrivalLocationCode"].Value = item.arrivalAirportDetails.locationCode;
                                                        filterOptionsResponseItem.Fields["arrivalTerminal"].Value = item.arrivalAirportDetails.terminal;
                                                        filterOptionsResponseItem.Fields["arrivalName"].Value = item.arrivalAirportDetails.name;
                                                        filterOptionsResponseItem.Fields["arrivalCity"].Value = item.arrivalAirportDetails.city;
                                                        filterOptionsResponseItem.Fields["arrivalCountry"].Value = item.arrivalAirportDetails.country;
                                                        filterOptionsResponseItem.Fields["arrivalCountryCode"].Value = item.arrivalAirportDetails.countryCode;

                                                        filterOptionsResponseItem.Fields["depatureLocationCode"].Value = item.depatureAirportDetails.locationCode;
                                                        filterOptionsResponseItem.Fields["depatureTerminal"].Value = item.depatureAirportDetails.terminal;
                                                        filterOptionsResponseItem.Fields["depatureName"].Value = item.depatureAirportDetails.name;
                                                        filterOptionsResponseItem.Fields["depatureCity"].Value = item.depatureAirportDetails.city;
                                                        filterOptionsResponseItem.Fields["depatureCountry"].Value = item.depatureAirportDetails.country;
                                                        filterOptionsResponseItem.Fields["depatureCountryCode"].Value = item.depatureAirportDetails.countryCode;

                                                        filterOptionsResponseItem.Fields["arrivalTime"].Value = item.arrivalTime;
                                                        filterOptionsResponseItem.Fields["departureTime"].Value = item.departureTime;

                                                        TimeSpan timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(item.duration));
                                                        string formattedTime = $"{(int)timeSpan.TotalHours} h {timeSpan.Minutes} m";
                                                        filterOptionsResponseItem.Fields["duration"].Value = formattedTime;

                                                        filterOptionsResponseItem.Fields["price"].Value = item.price.ToString();

                                                        filterOptionsResponseItem.Editing.EndEdit();
                                                        Log.Info($"Created item: {filterOptionsResponseItem.Paths.FullPath}", filterOptionsResponseItem);
                                                    }
                                                }
                                            }

                                            var lowestPriceItem = dateFolderItem.Children.OrderBy(i => Convert.ToDecimal(i.Fields["price"].Value)).FirstOrDefault();

                                            if (lowestPriceItem != null && dateFolderItem.Children.Any())
                                            {
                                                using (new EditContext(lowestPriceItem))
                                                {
                                                    CheckboxField checkboxField = lowestPriceItem.Fields["lowestfair"];

                                                    if (checkboxField != null)
                                                    {
                                                        checkboxField.Checked = true;

                                                        lowestPriceItem.Editing.EndEdit();
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Log.Error("Failed to create item.", dateFolderItem);
                                            success = false;
                                            message = "Failed to create item.";
                                            messageList.Add(message);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Log.Error("Parent item not found.", cityItem);
                                success = false;
                                message = "Parent item not found.";
                                messageList.Add(message);
                            }
                        }
                    }
                }
            }

            return Json(new { Success = success, Message = messageList }, JsonRequestBehavior.AllowGet);
        }

        private static Item GetItemById(string itemId)
        {
            Database masterDb = Sitecore.Configuration.Factory.GetDatabase("master");
            return masterDb.GetItem(new ID(itemId));
        }

        public ActionResult GetFlightToCityByDate(string date, string city)
        {
            List<FlightDetailResponseModel> flightDetailResponse = new List<FlightDetailResponseModel>();
            if (!string.IsNullOrEmpty(date))
            {
                date = DateTime.Parse(date).ToString("yyyy-MM-dd");
            }
            else
            {
                FlightDetailResponseModel responseModel = new FlightDetailResponseModel();
                responseModel.error = "date is required parameter.";
                flightDetailResponse.Add(responseModel);
                return Json(flightDetailResponse, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrEmpty(city))
            {
                int startIndex = city.IndexOf('-') + 1;
                if (startIndex >= 0 && startIndex < city.Length)
                {
                    city = city.Substring(startIndex);
                }
            }
            else
            {
                FlightDetailResponseModel responseModel = new FlightDetailResponseModel();
                responseModel.error = "city is required parameter.";
                flightDetailResponse.Add(responseModel);
                return Json(flightDetailResponse, JsonRequestBehavior.AllowGet);
            }

            try
            {
                string AllCityDataSource = "{3FDC2D71-8830-489C-86CE-53800018E124}";
                Database DB = Sitecore.Configuration.Factory.GetDatabase("web");
                Item AllCityDataSourceContext = DB.GetItem(AllCityDataSource);
                if (AllCityDataSourceContext != null)
                {
                    Item CityItemContext = AllCityDataSourceContext.GetChildren().Where(x => x["pageTitle"].ToLower() == city.ToLower() && x.TemplateID == ID.Parse("{1C5E12C1-CB4C-4284-9E98-AD75F473346F}")).FirstOrDefault();
                    if (CityItemContext != null)
                    {
                        var allRenderingsReferences = CityItemContext.Visualization.GetRenderings(Sitecore.Context.Device, false);
                        if (allRenderingsReferences != null)
                        {
                            var renderingList = allRenderingsReferences.ToList();
                            var rendering = renderingList.Where(x => x.RenderingID == ID.Parse("{13CD6063-29CF-48C9-976F-6E7E5E5704A0}")).FirstOrDefault();
                            if (rendering != null)
                            {
                                Item datasource = Sitecore.Context.Database.GetItem(rendering.Settings.DataSource);
                                if (datasource != null)
                                {
                                    foreach (Item item in datasource.GetChildren())
                                    {
                                        #region formatting item datefield value
                                        string itemDate = "";
                                        string contextItemDate = item.Fields["Date"].Value;
                                        string datePart = contextItemDate.Substring(0, 8);
                                        if (DateTime.TryParseExact(datePart, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime outputDate))
                                        {
                                            itemDate = outputDate.ToString("yyyy-MM-dd");
                                        }
                                        #endregion

                                        if (date == itemDate)
                                        {
                                            if (item.HasChildren)
                                            {
                                                foreach (Item child in item.GetChildren())
                                                {
                                                    FlightDetailResponseModel responseModel = new FlightDetailResponseModel();
                                                    responseModel.price = child.Fields["price"]?.Value;
                                                    responseModel.duration = child.Fields["duration"]?.Value;
                                                    responseModel.minutes = child.Fields["minutes"]?.Value;
                                                    responseModel.arrivalTime = child.Fields["arrivalTime"]?.Value;
                                                    responseModel.departureTime = child.Fields["departureTime"]?.Value;
                                                    responseModel.date = itemDate;

                                                    AirlineDetails airlineDetail = new AirlineDetails();
                                                    airlineDetail.code = child.Fields["code"]?.Value;
                                                    airlineDetail.name = child.Fields["name"]?.Value;
                                                    airlineDetail.flightNumber = child.Fields["flightNumber"]?.Value;

                                                    responseModel.airlineDetails = airlineDetail;

                                                    DepatureAirportDetails depatureAirportDetail = new DepatureAirportDetails();
                                                    depatureAirportDetail.terminal = child.Fields["depatureTerminal"]?.Value;
                                                    depatureAirportDetail.name = child.Fields["depatureName"]?.Value;
                                                    depatureAirportDetail.country = child.Fields["depatureCountry"]?.Value;
                                                    depatureAirportDetail.countryCode = child.Fields["depatureCountryCode"]?.Value;
                                                    depatureAirportDetail.locationCode = child.Fields["depatureLocationCode"]?.Value;
                                                    depatureAirportDetail.city = child.Fields["depatureCity"].Value;

                                                    responseModel.depatureAirportDetails = depatureAirportDetail;

                                                    ArrivalAirportDetails arrivalAirportDetail = new ArrivalAirportDetails();
                                                    arrivalAirportDetail.terminal = child.Fields["arrivalTerminal"]?.Value;
                                                    arrivalAirportDetail.name = child.Fields["arrivalName"]?.Value;
                                                    arrivalAirportDetail.country = child.Fields["arrivalCountry"]?.Value;
                                                    arrivalAirportDetail.countryCode = child.Fields["arrivalCountryCode"]?.Value;
                                                    arrivalAirportDetail.locationCode = child.Fields["arrivalLocationCode"]?.Value;
                                                    arrivalAirportDetail.city = child.Fields["arrivalCity"]?.Value;

                                                    responseModel.arrivalAirportDetails = arrivalAirportDetail;
                                                    flightDetailResponse.Add(responseModel);

                                                }

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    FlightDetailResponseModel responseModel = new FlightDetailResponseModel();
                                    responseModel.error = "Datasource is null";
                                    flightDetailResponse.Add(responseModel);
                                }
                            }
                        }
                        else
                        {
                            FlightDetailResponseModel responseModel = new FlightDetailResponseModel();
                            responseModel.error = "Getting an error while extracting rendering from context item.";
                            flightDetailResponse.Add(responseModel);
                        }
                    }
                    else
                    {
                        FlightDetailResponseModel responseModel = new FlightDetailResponseModel();
                        responseModel.error = "No item found as " + city.ToLower();
                        flightDetailResponse.Add(responseModel);
                    }
                }
                else
                {
                    FlightDetailResponseModel responseModel = new FlightDetailResponseModel();
                    responseModel.error = "Folder with ID {3FDC2D71-8830-489C-86CE-53800018E124} not found.";
                    flightDetailResponse.Add(responseModel);
                }
            }
            catch (Exception ex)
            {
                FlightDetailResponseModel responseModel = new FlightDetailResponseModel();
                responseModel.error = ex.Message;
                flightDetailResponse.Add(responseModel);
                return Json(flightDetailResponse, JsonRequestBehavior.AllowGet);
            }

            return Json(flightDetailResponse.OrderBy(y => y.price), JsonRequestBehavior.AllowGet);
        }
    }
}
