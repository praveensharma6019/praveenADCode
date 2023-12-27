using Glass.Mapper.Sc;
using Project.AdaniOneSEO.Website.Helpers;
using Project.AdaniOneSEO.Website.Models.FlightsToDestination;
using Sitecore.Mvc.Presentation;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Generic;
using System.IO;
using Sitecore.Data.Masters;
using Sitecore.Data;
using Sitecore.Data.Items;
using System.Linq;
using Sitecore.Data.Fields;
using Sitecore.Pipelines.InsertRenderings.Processors;
using Sitecore.Shell.Framework.Pipelines;
using Sitecore.SecurityModel;
using Sitecore.Search;

namespace Project.AdaniOneSEO.Website.Services.FlightsToDestination.Filter_Options
{
    public class FilterOptions : IFilterOptions
    {
        private readonly ISitecoreService _sitecoreService;
        public FilterOptions(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }
        public FilterOptionsModel GetFilterOptions(Rendering rendering)
        {
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;
            var data = _sitecoreService.GetItem<FilterOptionsModel>(datasource);
            data.CurrentDate = DateTime.Now.ToString("yyyy-MM-dd\'T\'hh:mm:ss");
            data.Date = DateTime.Now.AddDays(data.AddDaysToDate).ToString("yyyy-MM-dd\'T\'hh:mm:ss");
            #region Filter Option Old
            //if (!string.IsNullOrEmpty(data.APIURL))
            //{
            //    try
            //    {
            //        var uri = new Uri(data.APIURL);
            //        var request = (HttpWebRequest)WebRequest.Create(uri);
            //        Sitecore.Diagnostics.Log.Info("GetFilterOptions uri: " + uri,uri);
            //        Sitecore.Diagnostics.Log.Info("GetFilterOptions request: " + request, request);
            //        using (var response = (HttpWebResponse)request.GetResponse())
            //        {
            //            Sitecore.Diagnostics.Log.Info("GetFilterOptions response: " + response, response);
            //            if (response.StatusCode == HttpStatusCode.OK)
            //            {
            //                var responseBody = ReadResponseBody(response).Replace("None", "\"\"");
            //                Sitecore.Diagnostics.Log.Info("GetFilterOptions responseBody: " + responseBody, responseBody);
            //                if (!string.IsNullOrEmpty(responseBody))
            //                {
            //                    dynamic OBJ = JsonConvert.DeserializeObject<Object>(responseBody);
            //                    Sitecore.Diagnostics.Log.Info("GetFilterOptions OBJ: " + OBJ, OBJ);
            //                    if (OBJ != null)
            //                    {
            //                        var OBJCount = OBJ.Count;
            //                        Sitecore.Diagnostics.Log.Info("GetFilterOptions OBJCount: " + OBJCount, OBJCount);
            //                        if (OBJCount > 0)
            //                        {
            //                            List<FlightDetailResponseModel> list = new List<FlightDetailResponseModel>();
            //                            for (int i = 0; i < OBJCount; i++)
            //                            {
            //                                FlightDetailResponseModel responseModel = new FlightDetailResponseModel();
            //                                responseModel.price = OBJ[i]["price"];
            //                                responseModel.duration = OBJ[i]["duration"];
            //                                responseModel.arrivalTime = OBJ[i]["arrivalTime"];
            //                                responseModel.departureTime = OBJ[i]["departureTime"];

            //                                var airlineDetails = OBJ[i]["airlineDetails"];
            //                                if (airlineDetails != null)
            //                                {
            //                                    // var airlineDetailsOBJ = airlineDetails.Count;
            //                                    AirlineDetails airlineDetailsOBJ = JsonConvert.DeserializeObject<AirlineDetails>(OBJ[i]["airlineDetails"].ToString());
            //                                    AirlineDetails airlineDetailModel = new AirlineDetails();
            //                                    if (airlineDetailsOBJ != null)
            //                                    {

            //                                        airlineDetailModel.code = airlineDetails.code;
            //                                        airlineDetailModel.name = airlineDetails.name;
            //                                        airlineDetailModel.flightNumber = airlineDetails.flightNumber;
            //                                    }
            //                                    responseModel.airlineDetails = airlineDetailModel;
            //                                }

            //                                var depatureAirportDetails = OBJ[i]["depatureAirportDetails"];
            //                                if (depatureAirportDetails != null)
            //                                {
            //                                    DepatureAirportDetails depatureAirportDetailsOBJ = JsonConvert.DeserializeObject<DepatureAirportDetails>(OBJ[i]["depatureAirportDetails"].ToString());
            //                                    DepatureAirportDetails depatureAirportDetailsModel = new DepatureAirportDetails();
            //                                    if (depatureAirportDetailsOBJ != null)
            //                                    {
            //                                        depatureAirportDetailsModel.terminal = depatureAirportDetails.terminal;
            //                                        depatureAirportDetailsModel.country = depatureAirportDetails.country;
            //                                        depatureAirportDetailsModel.name = depatureAirportDetails.name;
            //                                        depatureAirportDetailsModel.country = depatureAirportDetails.country;
            //                                        depatureAirportDetailsModel.countryCode = depatureAirportDetails.countryCode;
            //                                        depatureAirportDetailsModel.locationCode = depatureAirportDetails.locationCode;
            //                                        depatureAirportDetailsModel.city = depatureAirportDetails.city;
            //                                    }
            //                                    responseModel.depatureAirportDetails = depatureAirportDetailsModel;
            //                                }


            //                                var arrivalAirportDetails = OBJ[i]["arrivalAirportDetails"];
            //                                if (arrivalAirportDetails != null)
            //                                {
            //                                    ArrivalAirportDetails arrivalAirportDetailsOBJ = JsonConvert.DeserializeObject<ArrivalAirportDetails>(OBJ[i]["arrivalAirportDetails"].ToString());
            //                                    ArrivalAirportDetails arrivalAirportDetailsModel = new ArrivalAirportDetails();
            //                                    if (arrivalAirportDetailsOBJ != null)
            //                                    {

            //                                        arrivalAirportDetailsModel.terminal = arrivalAirportDetails.terminal;
            //                                        arrivalAirportDetailsModel.country = arrivalAirportDetails.country;
            //                                        arrivalAirportDetailsModel.name = arrivalAirportDetails.name;
            //                                        arrivalAirportDetailsModel.country = arrivalAirportDetails.country;
            //                                        arrivalAirportDetailsModel.countryCode = arrivalAirportDetails.countryCode;
            //                                        arrivalAirportDetailsModel.locationCode = arrivalAirportDetails.locationCode;
            //                                        arrivalAirportDetailsModel.city = arrivalAirportDetails.city;
            //                                    }
            //                                    responseModel.arrivalAirportDetails = arrivalAirportDetailsModel;
            //                                }

            //                                list.Add(responseModel);
            //                            }
            //                            data.responseModel = list;
            //                        }
            //                    }


            //                }
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //        throw ex;
            //    }
            //}
            #endregion

            #region Filter Option New
            if (datasource != null && datasource.HasChildren)
            {
                var queryDate = DateTime.Now.ToString("dd-MM-yyyy");
                
                foreach (Item item in datasource.GetChildren())
                {
                    if (item.HasChildren)
                    {
                        List<FlightDetailResponseModel> list = new List<FlightDetailResponseModel>();
                        foreach (Item child in item.GetChildren())
                        {
                            FlightDetailResponseModel responseModel = new FlightDetailResponseModel();
                            responseModel.price = child.Fields["price"]?.Value;


                            responseModel.duration = child.Fields["duration"]?.Value;
                            responseModel.minutes = child.Fields["minutes"]?.Value;
                            responseModel.arrivalTime = child.Fields["arrivalTime"]?.Value;
                            responseModel.departureTime = child.Fields["departureTime"]?.Value;
                            responseModel.date = queryDate;

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
                            list.Add(responseModel);

                        }
                        data.responseModel = list;
                        var result = from s in data.responseModel
                                     orderby s.price
                                     select s;
                        data.responseModel = result;
                    }
                }
            }
            #endregion
            //DateTime dateTime = DateTime.Now;
            //string Datevalue = dateTime.ToString("yyyy-MM-dd hh:mm:ss");
            //data.Date = System.Convert.ToString( dateTime.AddDays(data.AddDaysToDate));
            return data;
        }

        public static string ReadResponseBody(HttpWebResponse response)
        {
            if (response == null)
                throw new ArgumentNullException("response", "Value cannot be null");

            // Then, open up a reader to the response and read the contents to a string
            // and return that to the caller.
            string responseBody = string.Empty;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                }
            }
            return responseBody;
        }

    }
}