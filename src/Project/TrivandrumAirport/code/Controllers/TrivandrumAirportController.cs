using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Sitecore.Diagnostics;
using Sitecore.Exceptions;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.TrivandrumAirport.Website.Model;
using Sitecore.TrivandrumAirport.Website.Models;
using Sitecore.TrivandrumAirport.Website.Services;
using Sitecore.TrivandrumAirport.Website.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Sitecore.TrivandrumAirport.Website.Controllers
{
    public class TrivandrumAirportController : Controller
    {
        string key = DictionaryPhraseRepository.Current.Get("AESEncryptDecryptKey/EncryptionKey", "8080808080808080");
        string iv = DictionaryPhraseRepository.Current.Get("AESEncryptDecryptKey/iv", "8080808080808080");

        #region Contact Us Form
        // GET: Mangalore Airport Contact Form
        public ActionResult Index()
        {
            return View();
        }
        TenderDBDataContext akodb = new TenderDBDataContext();
        [HttpGet]
        public ActionResult TRVFlightDetails(string ArrivalTime)
        {
            DateTime filterdate = DateTime.Now;
            if (ArrivalTime != null || ArrivalTime == "")
            {
                filterdate = DateTime.Parse(ArrivalTime);
            }

            TrivandrumAirportFlightModel TrivandrumAirportmodel = new TrivandrumAirportFlightModel();
            List<TrivandrumFlight> TrivandrumFlights = new List<TrivandrumFlight>();
            Table<TRV_AirportFlight> TrivandrumAirportFlightDetail = this.akodb.TRV_AirportFlights;
            var StatusList = Sitecore.Context.Database.GetItem("{EEADBEDB-DBA5-411A-8284-259BBA75D2EC}");

            IQueryable<TRV_AirportFlight> TrivandrumFlightInformation = TrivandrumAirportFlightDetail.Where(val => val.SpecialAction != "Delete" && val.ServiceType == "J" && val.ScheduledTime != null && val.ScheduledTime.Value.Date == filterdate.Date).OrderBy(val => val.Scheduled);
           
                foreach (TRV_AirportFlight TrivandrumAirportFlightData in TrivandrumFlightInformation)
                {
                    DateTime? schDate = TrivandrumAirportFlightData.ScheduledTime;



                    var schTime = schDate.Value.ToShortTimeString();
                    var schTimeFormat = DateTime.Parse(schTime).ToString("HH:mm");

                    TrivandrumFlight Trivandrumflights = new TrivandrumFlight()

                    {
                        Scheduled = schTimeFormat,
                        FlightNumber = TrivandrumAirportFlightData.FlightNumber.ToString(),
                        ArrivingFrom = TrivandrumAirportFlightData.ArrivingFrom.ToString(),
                        DepartureFrom = TrivandrumAirportFlightData.DepartureFrom.ToString(),
                        Airline = TrivandrumAirportFlightData.Airline,
                        terminal = TrivandrumAirportFlightData.Terminal,
                        Flightkind = TrivandrumAirportFlightData.flightkind,
                        FlightType = TrivandrumAirportFlightData.FlightType,
                    };
                    if (StatusList != null)
                    {
                        if (StatusList.HasChildren)
                        {
                            var CodeValuedetails = StatusList.GetChildren();
                            foreach (var CodeName in CodeValuedetails.ToList())
                            {
                                if (TrivandrumAirportFlightData.Status == CodeName["CodeValue"])
                                {
                                    var remarkdescription = CodeName["RemarkDescription"];
                                    TrivandrumAirportFlightData.Status = remarkdescription;
                                    Trivandrumflights.Status = TrivandrumAirportFlightData.Status;
                                }
                                else
                                {
                                    Trivandrumflights.Status = TrivandrumAirportFlightData.Status;
                                }

                            }
                        }
                    }
                    if (TrivandrumAirportFlightData.EstimationTime != null)
                    {
                        DateTime? estimateDate = TrivandrumAirportFlightData.EstimationTime;
                        var estimate = estimateDate.Value.ToShortTimeString();
                        var estimateFormat = DateTime.Parse(estimate).ToString("HH:mm");
                        Trivandrumflights.EstimationTime = estimateFormat;
                    }

                    TrivandrumFlights.Add(Trivandrumflights);
                }

              


            TrivandrumAirportmodel.TrivandrumAirportList = TrivandrumFlights;


            return base.View(TrivandrumAirportmodel.TrivandrumAirportList.ToList<TrivandrumFlight>());
        }


        [HttpGet]
        public ActionResult TrivandrumFlightDetailsArrivalUpdate(string ArrivalTime, string FlightType)
        {
            DateTime filterdate = DateTime.Now;
            if (ArrivalTime != null || ArrivalTime == "")
            {
                filterdate = DateTime.Parse(ArrivalTime);
            }

            TrivandrumAirportFlightModel TrivandrumAirportmodel = new TrivandrumAirportFlightModel();
            List<TrivandrumFlight> TrivandrumFlights = new List<TrivandrumFlight>();
            Table<TRV_AirportFlight> TrivandrumAirportFlightDetail = this.akodb.TRV_AirportFlights;
            var StatusList = Sitecore.Context.Database.GetItem("{EEADBEDB-DBA5-411A-8284-259BBA75D2EC}");
            IQueryable<TRV_AirportFlight> TrivandrumFlightInformation = TrivandrumAirportFlightDetail.Where(val => val.SpecialAction != "Delete" && val.ServiceType == "J" && val.ScheduledTime != null && val.ScheduledTime.Value.Date == filterdate.Date && val.flightkind == "Arrival" && val.FlightType == FlightType).OrderBy(val => val.Scheduled);

            if (TrivandrumFlightInformation.Count() >= 1)
            {
                foreach (TRV_AirportFlight TrivandrumAirportFlightData in TrivandrumFlightInformation)
                {
                    DateTime? schDate = TrivandrumAirportFlightData.ScheduledTime;
                    var schTime = TrivandrumAirportFlightData.ScheduledTime.Value.ToShortTimeString();
                    var schTimeFormat = DateTime.Parse(schTime).ToString("HH:mm");
                    
                        TrivandrumFlight Trivandrumflights = new TrivandrumFlight()

                        {
                            Scheduled = schTimeFormat,

                            FlightNumber = TrivandrumAirportFlightData.FlightNumber.ToString(),
                            ArrivingFrom = TrivandrumAirportFlightData.ArrivingFrom.ToString(),
                            DepartureFrom = TrivandrumAirportFlightData.DepartureFrom.ToString(),
                            Airline = TrivandrumAirportFlightData.Airline,
                            terminal = TrivandrumAirportFlightData.Terminal,
                            Flightkind = TrivandrumAirportFlightData.flightkind,
                            FlightType = TrivandrumAirportFlightData.FlightType,
                        };

                        if (StatusList != null)
                        {
                            if (StatusList.HasChildren)
                            {
                                var CodeValuedetails = StatusList.GetChildren();
                                foreach (var CodeName in CodeValuedetails.ToList())
                                {
                                    if (TrivandrumAirportFlightData.Status == CodeName["CodeValue"])
                                    {
                                        var remarkdescription = CodeName["RemarkDescription"];
                                        TrivandrumAirportFlightData.Status = remarkdescription;
                                        Trivandrumflights.Status = TrivandrumAirportFlightData.Status;
                                    }
                                    else
                                    {
                                        Trivandrumflights.Status = TrivandrumAirportFlightData.Status;
                                    }

                                }
                            }
                        }

                        if (TrivandrumAirportFlightData.EstimationTime != null)
                        {
                            DateTime? estimateDate = TrivandrumAirportFlightData.EstimationTime;
                            var estimate = estimateDate.Value.ToShortTimeString();
                            var estimateFormat = DateTime.Parse(estimate).ToString("HH:mm");
                            Trivandrumflights.EstimationTime = estimateFormat;
                        }
                        TrivandrumFlights.Add(Trivandrumflights);
                    }

                }

            



            else
            {
                ViewBag.Message = "There is no data to show";
            }
            TrivandrumAirportmodel.TrivandrumAirportList = TrivandrumFlights;
            return base.View("/views/TrivandrumAirport/TrivandrumFlightDetailsArrivalUpdate.cshtml", TrivandrumAirportmodel.TrivandrumAirportList.ToList<TrivandrumFlight>());

        }

        [HttpGet]
        public ActionResult TrivandrumFlightDetailsUpdateDeparture(string DepartureFlight, string FlightType)
        {
            DateTime filterdate = DateTime.Now;
            if (DepartureFlight != null || DepartureFlight == "")
            {
                filterdate = DateTime.Parse(DepartureFlight);
            }
            TrivandrumAirportFlightModel TrivandrumAirportmodel = new TrivandrumAirportFlightModel();
            List<TrivandrumFlight> TrivandrumFlights = new List<TrivandrumFlight>();
            Table<TRV_AirportFlight> TrivandrumAirportFlightDetail = this.akodb.TRV_AirportFlights;
            var StatusList = Sitecore.Context.Database.GetItem("{EEADBEDB-DBA5-411A-8284-259BBA75D2EC}");
            IQueryable<TRV_AirportFlight> TrivandrumFlightInformation = TrivandrumAirportFlightDetail.Where(val => val.SpecialAction != "Delete" && val.ServiceType == "J" && val.ScheduledTime != null && val.ScheduledTime.Value.Date == filterdate.Date && val.flightkind == "Departure" && val.FlightType == FlightType).OrderBy(val => val.Scheduled);
            if (TrivandrumFlightInformation.Count() >= 1)
            {
                foreach (TRV_AirportFlight TrivandrumAirportFlightData in TrivandrumFlightInformation)
                {
                    DateTime? schDate = TrivandrumAirportFlightData.ScheduledTime;
                  var schTime = schDate.Value.ToShortTimeString();
                        var schTimeFormat = DateTime.Parse(schTime).ToString("HH:mm");

                        TrivandrumFlight Trivandrumflights = new TrivandrumFlight()

                        {

                            Scheduled = schTimeFormat,

                            FlightNumber = TrivandrumAirportFlightData.FlightNumber.ToString(),
                            ArrivingFrom = TrivandrumAirportFlightData.ArrivingFrom.ToString(),
                            DepartureFrom = TrivandrumAirportFlightData.DepartureFrom.ToString(),
                            Airline = TrivandrumAirportFlightData.Airline,
                            terminal = TrivandrumAirportFlightData.Terminal,
                            Flightkind = TrivandrumAirportFlightData.flightkind,
                            FlightType = TrivandrumAirportFlightData.FlightType,

                        };
                        if (StatusList != null)
                        {
                            if (StatusList.HasChildren)
                            {
                                var CodeValuedetails = StatusList.GetChildren();
                                foreach (var CodeName in CodeValuedetails.ToList())
                                {
                                    if (TrivandrumAirportFlightData.Status == CodeName["CodeValue"])
                                    {
                                        var remarkdescription = CodeName["RemarkDescription"];
                                        TrivandrumAirportFlightData.Status = remarkdescription;
                                        Trivandrumflights.Status = TrivandrumAirportFlightData.Status;
                                    }
                                    else
                                    {
                                        Trivandrumflights.Status = TrivandrumAirportFlightData.Status;
                                    }

                                }
                            }
                        }
                        if (TrivandrumAirportFlightData.EstimationTime != null)
                        {
                            DateTime? estimateDate = TrivandrumAirportFlightData.EstimationTime;
                            var estimate = estimateDate.Value.ToShortTimeString();
                            var estimateFormat = DateTime.Parse(estimate).ToString("HH:mm");
                            Trivandrumflights.EstimationTime = estimateFormat;
                        }
                        TrivandrumFlights.Add(Trivandrumflights);
                    }
                }
            
            else
            {
                ViewBag.Message = "There is no data to show";
            }




            TrivandrumAirportmodel.TrivandrumAirportList = TrivandrumFlights;



            return base.View("/views/TrivandrumAirport/TrivandrumFlightDetailsUpdateDeparture.cshtml", TrivandrumAirportmodel.TrivandrumAirportList.ToList<TrivandrumFlight>());
        }


        [HttpGet]
        public ActionResult TrivandrumFlighSearchDetailsArival(string Search, string ArrivalFlights, string FlightType)
        {
            DateTime filterdate = DateTime.Now;
            if (ArrivalFlights != null || ArrivalFlights == "")
            {
                filterdate = DateTime.Parse(ArrivalFlights);
            }
            var StatusList = Sitecore.Context.Database.GetItem("{EEADBEDB-DBA5-411A-8284-259BBA75D2EC}");
            TrivandrumAirportFlightModel TrivandrumAirportmodel = new TrivandrumAirportFlightModel();
            List<TrivandrumFlight> TrivandrumFlights = new List<TrivandrumFlight>();
            Table<TRV_AirportFlight> TrivandrumAirportFlightDetail = this.akodb.TRV_AirportFlights;
            IQueryable<TRV_AirportFlight> TrivandrumFlightInformation = TrivandrumAirportFlightDetail.Where(val => val.SpecialAction != "Delete" && val.ServiceType == "J" && val.ScheduledTime != null && val.ScheduledTime.Value.Date == filterdate.Date && val.flightkind == "Arrival" && (val.Airline.StartsWith(Search) || val.FlightNumber.StartsWith(Search))).OrderBy(val => val.Scheduled);

            //IQueryable<TRV_AirportFlight> TrivandrumFlightInformation = TrivandrumAirportFlightDetail.Where(val => val.SpecialAction != "Delete" && val.ServiceType == "J" && val.ScheduledTime != null && val.ScheduledTime.Value.Date == filterdate.Date && val.flightkind == "Arrival" && val.Airline.StartsWith(Search) || val.FlightNumber.StartsWith(Search)).OrderBy(val => val.Scheduled);
            if (TrivandrumFlightInformation.Count() >= 1)
            {
                foreach (TRV_AirportFlight TrivandrumAirportFlightData in TrivandrumFlightInformation)
                {

                    if (TrivandrumAirportFlightData.FlightType != FlightType)
                    {
                        continue;
                    }
                    DateTime? schDate = TrivandrumAirportFlightData.ScheduledTime;
                    // schDate.Value.ToShortDateString();



                    var schTime = schDate.Value.ToShortTimeString();
                    var schTimeFormat = DateTime.Parse(schTime).ToString("HH:mm");

                    TrivandrumFlight Trivandrumflights = new TrivandrumFlight()

                    {
                        Scheduled = schTimeFormat,

                        FlightNumber = TrivandrumAirportFlightData.FlightNumber.ToString(),
                        ArrivingFrom = TrivandrumAirportFlightData.ArrivingFrom.ToString(),
                        DepartureFrom = TrivandrumAirportFlightData.DepartureFrom.ToString(),
                        Airline = TrivandrumAirportFlightData.Airline,
                        terminal = TrivandrumAirportFlightData.Terminal,
                        Flightkind = TrivandrumAirportFlightData.flightkind,
                        FlightType = TrivandrumAirportFlightData.FlightType,
                    };
                    if (StatusList != null)
                    {
                        if (StatusList.HasChildren)
                        {
                            var CodeValuedetails = StatusList.GetChildren();
                            foreach (var CodeName in CodeValuedetails.ToList())
                            {
                                if (TrivandrumAirportFlightData.Status == CodeName["CodeValue"])
                                {
                                    var remarkdescription = CodeName["RemarkDescription"];
                                    TrivandrumAirportFlightData.Status = remarkdescription;
                                    Trivandrumflights.Status = TrivandrumAirportFlightData.Status;
                                }
                                else
                                {
                                    Trivandrumflights.Status = TrivandrumAirportFlightData.Status;
                                }

                            }
                        }
                    }
                    if (TrivandrumAirportFlightData.EstimationTime != null)
                    {
                        DateTime? estimateDate = TrivandrumAirportFlightData.EstimationTime;
                        var estimate = estimateDate.Value.ToShortTimeString();
                        var estimateFormat = DateTime.Parse(estimate).ToString("HH:mm");
                        Trivandrumflights.EstimationTime = estimateFormat;
                    }
                    TrivandrumFlights.Add(Trivandrumflights);


                }

            }
        
    
     else
            {
                ViewBag.Message = "There is no data to show";
            }

          TrivandrumAirportmodel.TrivandrumAirportList = TrivandrumFlights;
            return base.View("/views/TrivandrumAirport/TrivandrumFlightDetailsArrivalUpdate.cshtml", TrivandrumAirportmodel.TrivandrumAirportList.ToList<TrivandrumFlight>());


        }


        [HttpGet]
        public ActionResult TrivandrumFlighSearchDetailsDeparture(string Search, string DepartureFlights, string FlightType)
        {
            DateTime filterdate = DateTime.Now;
            if (DepartureFlights != null || DepartureFlights == "")
            {
                filterdate = DateTime.Parse(DepartureFlights);
            }

            TrivandrumAirportFlightModel TrivandrumAirportmodel = new TrivandrumAirportFlightModel();
            List<TrivandrumFlight> TrivandrumFlights = new List<TrivandrumFlight>();
            Table<TRV_AirportFlight> TrivandrumAirportFlightDetail = this.akodb.TRV_AirportFlights;
            var StatusList = Sitecore.Context.Database.GetItem("{EEADBEDB-DBA5-411A-8284-259BBA75D2EC}");
            IQueryable<TRV_AirportFlight> TrivandrumFlightInformation = TrivandrumAirportFlightDetail.Where(val => val.SpecialAction != "Delete" && val.ServiceType == "J" && val.ScheduledTime != null && val.ScheduledTime.Value.Date == filterdate.Date && val.flightkind == "Departure" && (val.Airline.StartsWith(Search) || val.FlightNumber.StartsWith(Search))).OrderBy(val => val.Scheduled);
            //IQueryable<TRV_AirportFlight> TrivandrumFlightInformation = TrivandrumAirportFlightDetail.Where(val => val.SpecialAction != "Delete" && val.ServiceType == "J" && val.ScheduledTime != null && val.ScheduledTime.Value.Date == filterdate.Date && val.flightkind == "Departure" && val.Airline.StartsWith(Search) || val.FlightNumber.StartsWith(Search)).OrderBy(val => val.Scheduled);
            if (TrivandrumFlightInformation.Count() >= 1)
            {
                foreach (TRV_AirportFlight TrivandrumAirportFlightData in TrivandrumFlightInformation)
            {
                    var da = TrivandrumAirportFlightData.ScheduledTime.Value.Date;
                    var fd = filterdate.Date;
                    if(da==fd)
                    {
                        var a = "Hello";
                    }
                    if (TrivandrumAirportFlightData.FlightType != FlightType)
                    {
                        continue;
                    }
                    DateTime? schDate = TrivandrumAirportFlightData.ScheduledTime;
                
                        var schTime = schDate.Value.ToShortTimeString();
                        var schTimeFormat = DateTime.Parse(schTime).ToString("HH:mm");
                        TrivandrumFlight Trivandrumflights = new TrivandrumFlight()

                        {
                            Scheduled = schTimeFormat,

                            FlightNumber = TrivandrumAirportFlightData.FlightNumber.ToString(),
                            ArrivingFrom = TrivandrumAirportFlightData.ArrivingFrom.ToString(),
                            DepartureFrom = TrivandrumAirportFlightData.DepartureFrom.ToString(),
                            Airline = TrivandrumAirportFlightData.Airline,
                            terminal = TrivandrumAirportFlightData.Terminal,
                            Flightkind = TrivandrumAirportFlightData.flightkind,
                            FlightType = TrivandrumAirportFlightData.FlightType,
                        };
                        if (StatusList != null)
                        {
                            if (StatusList.HasChildren)
                            {
                                var CodeValuedetails = StatusList.GetChildren();
                                foreach (var CodeName in CodeValuedetails.ToList())
                                {
                                    if (TrivandrumAirportFlightData.Status == CodeName["CodeValue"])
                                    {
                                        var remarkdescription = CodeName["RemarkDescription"];
                                        TrivandrumAirportFlightData.Status = remarkdescription;
                                        Trivandrumflights.Status = TrivandrumAirportFlightData.Status;
                                    }
                                    else
                                    {
                                        Trivandrumflights.Status = TrivandrumAirportFlightData.Status;
                                    }

                                }
                            }
                        }
                        if (TrivandrumAirportFlightData.EstimationTime != null)
                        {
                            DateTime? estimateDate = TrivandrumAirportFlightData.EstimationTime;
                            var estimate = estimateDate.Value.ToShortTimeString();
                            var estimateFormat = DateTime.Parse(estimate).ToString("HH:mm");
                            Trivandrumflights.EstimationTime = estimateFormat;
                        }
                        TrivandrumFlights.Add(Trivandrumflights);

                    }
                }
            else
            {
                ViewBag.Message = "There is no data to show";
            }

            TrivandrumAirportmodel.TrivandrumAirportList = TrivandrumFlights;
            return base.View("/views/TrivandrumAirport/TrivandrumFlightDetailsUpdateDeparture.cshtml", TrivandrumAirportmodel.TrivandrumAirportList.ToList<TrivandrumFlight>());


        }


        private static DateTime? ConvertDate(string Time)
        {

            DateTime? utcdate = null;
            try
            {
                if (Time != "0001-01-01T00:00:00Z")
                {
                    // DateTime? nullableDate = DateTimeOffset.Parse(Time).UtcDateTime;
                    DateTime? nullableDate = DateTimeOffset.Parse(Time).DateTime;
                    DateTime date = nullableDate.HasValue ? nullableDate.Value : default(DateTime);
                    utcdate = TimeZoneInfo.ConvertTimeFromUtc(date, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                }


            }
            catch (System.Exception e)
            {
                // Note: When using ASP.NET Core Web Apps, to output to streaming logs, use ILogger rather than System.Diagnostics
                Console.WriteLine($"Exception occurred while getting time: {e.Message}");
                //return null;         // or throw e; if you want to bubble the exception up to the caller
            }
            return utcdate;
        }


        [HttpPost]
        public ActionResult FLIGHTS()
        {

            string Details = "";
            var result = new { status = "1" };
            Log.Error("Validating API", "Start");
            string authHeader = HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authHeader))
            {
                result = new { status = "4" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }


            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            clsEncryptAES objcrypt = new clsEncryptAES("@TRV2021");

            //string userName_ = objcrypt.EncryptText("TRA");
            //string Password_ = objcrypt.EncryptText("TRA@123");

            string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
            string usernamePassword = encoding.GetString(System.Convert.FromBase64String(encodedUsernamePassword));
            string[] seperatorIndex = usernamePassword.Split(':');
            string username = objcrypt.DecryptText(seperatorIndex[0]);
            string password = objcrypt.DecryptText(seperatorIndex[1]);
            // if (username != "TRA" || password != "TRA@123")
            //  string username = "TRA";
            //  string password = "TRA@123";

             //string username = seperatorIndex[0];
            //string password = seperatorIndex[1];

            if (username != "TRA"|| password != "TRA@123")
            {
                result = new { status = "3" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            try
            {
                    StreamReader bodyStream = new StreamReader(System.Web.HttpContext.Current.Request.InputStream);
                    bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                    Details = bodyStream.ReadToEnd();
                }
                catch (Exception ex)
                {
                    result = new { status = "0" };
                    Console.WriteLine(ex);
                }

                try
                {
                    dynamic obj = JsonConvert.DeserializeObject<Object>(Details);
                    dynamic FlightsOBJ = obj["Flights"];
                    dynamic OBJ = FlightsOBJ[0];

                    if (OBJ != null)
                    {
                        string fligthNumber = "";
                        string Airline = "";
                        string Departurefrom = "";
                        DateTime? Schedule = null;

                        DateTime? ScheduledTime = null;
                        DateTime? EstimationTime = null;
                        DateTime? ActualTime = null;
                        string ArrivinfFrom = "";
                        string ServiceType = "";
                        string Status = "";
                        string Terminal = "";
                        string OperationalStatus = "";
                        string SpecialAction = "";
                        string flightkind = "";

                        string flightType = "";

                        if (OBJ["flightId"] != null)
                        {

                            if (OBJ["flightId"]["flightNumber"] != null)
                            {
                                fligthNumber = OBJ["flightId"]["flightNumber"].Value;
                            }
                            if (OBJ["flightId"]["iataAirline"] != null)
                            {
                                Airline = OBJ["flightId"]["iataAirline"].Value;
                            }
                            if (OBJ["flightId"]["iatalocalairport"] != null)
                            {
                                Departurefrom = OBJ["flightId"]["iatalocalairport"].Value;
                            }
                            if (OBJ["flightId"]["schedule"] != null)
                            {
                                Schedule = OBJ["flightId"]["schedule"].Value;
                                ScheduledTime = OBJ["flightId"]["schedule"].Value;
                            }
                            if (OBJ["flightId"]["flightkind"] != null)
                            {
                                flightkind = OBJ["flightId"]["flightkind"].Value;
                            }

                        }

                        if (OBJ["flightEvents"] != null)
                        {

                            if (OBJ["flightEvents"]["estimated"] != null)
                            {
                                EstimationTime = OBJ["flightEvents"]["estimated"].Value;
                            }
                            if (OBJ["flightEvents"]["actual"] != null)
                            {
                                ActualTime = OBJ["flightEvents"]["actual"].Value;
                            }
                        }
                        if (OBJ["iataroute"] != null)
                        {
                            ArrivinfFrom = OBJ["iataroute"][0].Value;

                        }
                        if (OBJ["qualifier"] != null)
                        {
                            ServiceType = OBJ["qualifier"].Value;

                        }
                        if (OBJ["terminal"] != null)
                        {
                            Terminal = OBJ["terminal"].Value;

                        }
                        if (OBJ["status"] != null)
                        {
                            Status = OBJ["status"].Value;

                        }
                        if (OBJ["remark"] != null)
                        {
                            OperationalStatus = OBJ["remark"].Value;

                        }
                        if (OBJ["specialAction"] != null)
                        {

                            SpecialAction = OBJ["specialAction"].Value;
                        }
                        if (OBJ["domesticintcode"] != null)
                        {
                            if (OBJ["domesticintcode"] == "I")
                            {
                                flightType = "International";
                            }
                            if (OBJ["domesticintcode"] == "D")
                            {
                                flightType = "Domestic";
                            }
                        }
                        //save and update records
                        try
                        {


                            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                            {

                                //if (dbcontext.TRV_AirportFlights.Where(t => t.Scheduled.Value == DateTime.Parse(Schedule.ToString()) && t.FlightNumber.ToString() == fligthNumber).Any())
                                if (dbcontext.TRV_AirportFlights.Where(t => t.OriginDate.Value == DateTime.Parse(Schedule.Value.Date.ToString()) && t.FlightNumber.ToString() == fligthNumber && t.Airline.ToString() == Airline).Any())
                                {

                                    //TRV_AirportFlight flight = dbcontext.TRV_AirportFlights.Where(t => t.Scheduled.Value == DateTime.Parse(Schedule.ToString()) && t.FlightNumber.ToString() == fligthNumber).FirstOrDefault();
                                    TRV_AirportFlight flight = dbcontext.TRV_AirportFlights.Where(t => t.OriginDate.Value == DateTime.Parse(Schedule.Value.Date.ToString()) && t.FlightNumber.ToString() == fligthNumber && t.Airline.ToString() == Airline).FirstOrDefault();

                                    if (Schedule.ToString() != "")
                                    {
                                        flight.Scheduled = DateTime.Parse(Schedule.ToString());
                                    }
                                    flight.FlightNumber = fligthNumber;
                                    flight.ArrivingFrom = ArrivinfFrom;
                                    flight.OriginDate = Schedule.Value.Date;
                                    flight.DepartureFrom = Departurefrom;
                                    if (ScheduledTime.ToString() != "")
                                    {
                                        flight.Scheduled = ConvertDate(ScheduledTime.ToString());
                                    }
                                    flight.Airline = Airline;
                                    flight.Terminal = Terminal;
                                    flight.Status = Status;
                                    flight.OperationalStatus = OperationalStatus;
                                    flight.ServiceType = ServiceType;
                                    flight.SpecialAction = SpecialAction;
                                    flight.flightkind = flightkind;
                                    flight.FlightType = flightType;

                                    if (EstimationTime.ToString() != "")
                                    {
                                        flight.EstimationTime = ConvertDate(EstimationTime.ToString());
                                    }
                                    if (ScheduledTime.ToString() != "")
                                    {
                                        flight.ScheduledTime = ConvertDate(ScheduledTime.ToString());
                                    }
                                    if (ActualTime.ToString() != "")
                                    {
                                        flight.ActualTime = ConvertDate(ActualTime.ToString());
                                    }
                                    flight.UpdatedOn = System.DateTime.Now;

                                    try
                                    {
                                        dbcontext.SubmitChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        result = new { status = "2" };
                                        Console.WriteLine(ex);
                                    }

                                }

                                else
                                {
                                    TRV_AirportFlight r1 = new TRV_AirportFlight();

                                    if (Schedule.ToString() != "" && Schedule.ToString() != "")
                                        r1.Scheduled = DateTime.Parse(Schedule.ToString());
                                    r1.FlightNumber = fligthNumber;

                                    r1.ArrivingFrom = ArrivinfFrom;
                                    r1.DepartureFrom = Departurefrom;
                                    if (ScheduledTime.ToString() != "" && ScheduledTime.ToString() != "")
                                    {
                                        r1.Scheduled = ConvertDate(ScheduledTime.ToString());
                                    }
                                    r1.Airline = Airline;
                                    r1.Terminal = Terminal;
                                    r1.Status = Status;
                                    r1.OriginDate = Schedule.Value.Date;
                                    r1.OperationalStatus = OperationalStatus;
                                    r1.ServiceType = ServiceType;
                                    r1.flightkind = flightkind;
                                    r1.FlightType = flightType;
                                    if (EstimationTime.ToString() != "" && EstimationTime.ToString() != "")
                                    {
                                        r1.EstimationTime = ConvertDate(EstimationTime.ToString());
                                    }
                                    if (ScheduledTime.ToString() != "" && ScheduledTime.ToString() != "")
                                    {
                                        r1.ScheduledTime = ConvertDate(ScheduledTime.ToString());
                                    }
                                    if (ActualTime.ToString() != "" && ActualTime.ToString() != "")
                                    {
                                        r1.ActualTime = ConvertDate(ActualTime.ToString());
                                    }
                                    r1.OperationalStatus = OperationalStatus;

                                    r1.SpecialAction = SpecialAction;
                                    r1.CreatedOn = System.DateTime.Now;
                                    r1.UpdatedOn = System.DateTime.Now;



                                    #region Insert to DB
                                    ////
                                    dbcontext.TRV_AirportFlights.InsertOnSubmit(r1);
                                    // rdb.SubmitChanges();
                                    try
                                    {
                                        dbcontext.SubmitChanges();
                                        //  Session["validate"] = Request.Cookies["SIDCC"].Value;
                                        //  Log.Info("Form SportsLine Contact Form data saved into db successfully: ", this);


                                    }
                                    catch (Exception ex)
                                    {
                                        result = new { status = "2" };
                                        Console.WriteLine(ex);
                                    }

                                    #endregion
                                }

                            }
                        }
                        catch (System.Exception ex)
                        {
                            throw ex;
                        }
                    }


                }
                catch (Exception ex)
                {
                    result = new { status = "0" };
                    Console.WriteLine(ex);
                }

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult GetSearchResult(string itemId)
        {

            var homeItem = Sitecore.Context.Database.GetItem("/sitecore/content/Adani/Home/thiruvananthapuram airport");
            var item = homeItem.Axes.GetDescendants().Where(p => p.Name.ToLower().Contains(itemId)).ToList();

            var list = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < item.Count(); i++)
            {
                list.Add(new KeyValuePair<string, string>(item[i].Name, item[i].Paths.ContentPath));
            }

            return Json(list);
        }

        public bool IsReCaptchValid(string reResponse)
        {
            var result = false;
            // var captchaResponse = Request.Form["g-recaptcha-response"];
            var captchaResponse = reResponse;
            //   string secretKey = DictionaryPhraseRepository.Current.Get("/CaptachaKey/SecretKey", "6Ldv1uEUAAAAABJojxOHBvrT3B1U8iNBosijSg79");
            string secretKey = DictionaryPhraseRepository.Current.Get("/CaptachaKey/SecretKey", "6Lcql9QZAAAAAOSIsZ-9gNRiZaVPrDmSrbKSe3y2");
            //var secretKey = ConfigurationManager.AppSettings["SecretKey"];
            // var secretKey = "6LdkC64UAAAAAJiii15Up9xETgsLuPQnQ1BKZft8";
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    result = (isSuccess) ? true : false;
                }
            }
            return result;
        }

        [HttpPost]
        public ActionResult InsertContact(AdaniAirportsContactUsForm m)
        {
            bool validationStatus = false;
            var result = new { status = "1" };
            Log.Error("Validating Adani Airport ContactForm to stop auto script ", "Start");

            try
            {
                validationStatus = IsReCaptchValid(m.reResponse);

            }
            catch (Exception ex)
            {
                result = new { status = "2" };
                Log.Error("Failed to validate auto script : " + ex.ToString(), "Failed");
            }

            if (validationStatus == true)
            {
                Log.Error("InsertAdaniAirportContact", "Start");
                var getEmailTo = "";

                try
                {
                    TenderDBDataContext rdb = new TenderDBDataContext();
                    TRV_AdaniAirportsContactForm r1 = new TRV_AdaniAirportsContactForm();

                    r1.Name = m.Name;
                    r1.Email = m.Email;
                    r1.Mobile = m.Mobile;
                    r1.MessageType = m.MessageType;
                    r1.Message = m.Message;
                    r1.FormType = m.FormType;
                    r1.PageInfo = m.PageInfo;
                    r1.FormSubmitOn = m.FormSubmitOn;



                    #region Insert to DB
                    rdb.TRV_AdaniAirportsContactForms.InsertOnSubmit(r1);
                    rdb.SubmitChanges();
                    //  Session["validate"] = Request.Cookies["SIDCC"].Value;
                }
                catch (Exception ex)
                {
                    result = new { status = "0" };
                    Console.WriteLine(ex);
                }

                try
                {
                    var msgTpye = Sitecore.Context.Database.GetItem("{7D0B4CAF-EE05-45A3-94D5-AB459C235B8B}");
                    var getfilteredItem = msgTpye.Children.Where(x => x.Fields["SubjectName"].Value.ToLower() == m.MessageType.ToLower());

                    foreach (var itemData in getfilteredItem.ToList())
                    {
                        getEmailTo = itemData.Fields["EmailTo"].Value;
                    }
                }
                catch (Exception ex)
                {
                    result = new { status = "1" };
                    Log.Error("Failed to get subject specific Email", ex.ToString());
                }

                try
                {
                    var mail = GetContactUSMailTemplate();
                    mail.To.Add(new MailAddress(getEmailTo));
                    mail.Body = mail.Body.Replace("#Url#", HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host);
                    mail.Body = mail.Body.Replace("#Name#", m.Name);
                    mail.Body = mail.Body.Replace("#EmailId#", m.Email);
                    mail.Body = mail.Body.Replace("#Subject#", m.MessageType);
                    mail.Body = mail.Body.Replace("#Contact#", m.Mobile);
                    mail.Body = mail.Body.Replace("#Message#", m.Message);
                    mail.Subject = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailSubject", "");

                    //string emailText = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailText", "");
                    //string message = "";
                    //message = "Hello<br><br>" + emailText + "<br><br>";
                    //message = message + "<br>Name: " + m.Name + "<br>Email-Id: " + m.Email + "<br>Subject of Message: " + m.MessageType + "<br>Contact Number: " + m.Mobile + "<br>Message: " + m.Message + "<br><br>Thanks";
                    ////string to = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailTo", "");
                    //string from = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailFrom", "");
                    //string emailSubject = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailSubject", "");
                    //bool results = sendEmail(getEmailTo, emailSubject, message, from);


                    //var mail = this.GetTenderMailTemplate();
                    //mail.To.Add(email);
                    //mail.Body = mail.Body.Replace("#url#", HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host);
                    //mail.Body = mail.Body.Replace("#UserName#", Username);
                    //mail.Body = mail.Body.Replace("#Pwd#", pwd);
                    //mail.Body = mail.Body.Replace("#TenderName#", Description);
                    //mail.Body = mail.Body.Replace("#NitNo#", NitNo);
                    //mail.Subject = mail.Subject.Replace("#Subject#", NitNo);
                    MainUtil.SendMail(mail);
                }
                catch (Exception ex)
                {
                    result = new { status = "1" };
                    Log.Error("Failed to sent Email", ex.ToString());
                }
            }
            else
            {
                result = new { status = "2" };

            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult BookAirportAssistance(AirportAssistance m)
        {
            String[] TypeOfSectors = { "Departure", "Arrival", "Transit" };
            String[] TravelSectors = { "International", "Domestic" };
            bool isValid = true;
            bool validationStatus = false;
            var result = new { status = "1" };
            Log.Error("Validating Adani Airport BookAirportAssistance to stop auto script ", "Start");

            try
            {
                validationStatus = IsReCaptchValid(m.reResponse);

            }
            catch (Exception ex)
            {
                result = new { status = "2" };
                Log.Error("Failed to validate auto script : " + ex.ToString(), "Failed");
            }

           if (validationStatus == true)
            {
                Log.Error("BookAirportAssistance", "Start");


                /* try
                 {
                     using (TenderDBDataContext objcontext = new TenderDBDataContext())
                     {
                         if (objcontext.TRV_AirportAssistances.Where(x => x.ContactNumber.ToString() == m.ContactNumber.ToString()).Any())
                         {
                             result = new { status = "3" };
                             return Json(result, JsonRequestBehavior.AllowGet);
                         }
                     }
                 }
                 catch (Exception ex)
                 {
                     result = new { status = "2" };
                     Log.Error("Failed to validate auto script : " + ex.ToString(), "Failed");
                 }
                 */
                if (m.Name.Length < 8 || (!Regex.IsMatch(m.Name, (@"(Mr|Ms).[a-zA-Z ]*$"))))
                {
                    this.ModelState.AddModelError(nameof(m.Name), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter a valid Name"));
                    //return this.View(model);
                    isValid = false;
                    result = new { status = "8" };
                }
                if (string.IsNullOrEmpty(m.Name))
                {
                    this.ModelState.AddModelError(nameof(m.Name), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter  Name"));
                    //return this.View(model);
                    isValid = false;
                    result = new { status = "4" };
                }
                if (string.IsNullOrEmpty(m.ContactNumber))
                {
                    this.ModelState.AddModelError(nameof(m.Name), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter  Contact Number"));
                    //return this.View(model);
                    isValid = false;
                    result = new { status = "4" };
                }
                if ((!Regex.IsMatch(m.ContactNumber, (@"(^\d{1,3}[- ]??\d{10}$)|(^\+\d{1,3}[- ]??\d{10}$)"))))
                {
                    this.ModelState.AddModelError(nameof(m.ContactNumber), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter valid Phone Number"));
                    //return this.View(model);
                    isValid = false;
                    result = new { status = "5" };
                }
                if (string.IsNullOrEmpty(m.EmailAddress))
                {
                    this.ModelState.AddModelError(nameof(m.Name), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter  Email Address"));
                    //return this.View(model);
                    isValid = false;
                    result = new { status = "4" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!Regex.IsMatch(m.EmailAddress, (@"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[com]{2,9})$")))
                {
                    this.ModelState.AddModelError(nameof(m.EmailAddress), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter valid Email Address"));
                    //return this.View(model);
                    isValid = false;
                    result = new { status = "6" };
                }
                if (string.IsNullOrEmpty(m.TravelDate))
                {
                    this.ModelState.AddModelError(nameof(m.TravelDate), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter Travel Date"));
                    //return this.View(model);
                    isValid = false;
                    result = new { status = "4" };
                }
                if (!Regex.IsMatch(m.TravelDate, (@"(([1-9]|(0|1)[0-9]|2[0-9]|3[0-1])\/([1-9]|0[1-9]|1[0-2])\/((19|20)\d\d))$")))
                {
                    this.ModelState.AddModelError(nameof(m.TravelDate), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter valid travel date"));
                    //return this.View(model);
                    isValid = false;
                    result = new { status = "7" };
                }
                if (string.IsNullOrEmpty(m.TypeOfService))
                {
                    this.ModelState.AddModelError(nameof(m.TypeOfService), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter Type Of Service"));
                    //return this.View(model);
                    isValid = false;
                    result = new { status = "4" };
                }
                if (!TypeOfSectors.Contains(m.TypeOfService))
                {
                    this.ModelState.AddModelError(nameof(m.TypeOfService), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter Type Of Service"));
                    //return this.View(model);
                    isValid = false;
                    result = new { status = "9" };
                }
                if (string.IsNullOrEmpty(m.TravelSector))
                {
                    this.ModelState.AddModelError(nameof(m.TravelSector), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter Travel Sector"));
                    //return this.View(model);
                    isValid = false;
                    result = new { status = "4" };
                }
                if (!TravelSectors.Contains(m.TravelSector))
                {
                    this.ModelState.AddModelError(nameof(m.TypeOfService), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter Type Of Service"));
                    //return this.View(model);
                    isValid = false;
                    result = new { status = "10" };
                }
                if (isValid == false)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        TenderDBDataContext rdb = new TenderDBDataContext();
                        TRV_AirportAssistance r1 = new TRV_AirportAssistance();

                        r1.Name = m.Name;
                        r1.ContactNumber = m.ContactNumber;
                        r1.AlternateContactNumber = m.AlternateContactNumber;
                        r1.EmailAddress = m.EmailAddress;
                        r1.TravelDate = m.TravelDate;
                        r1.TypeOfService = m.TypeOfService;
                        r1.FormSubmitDate = DateTime.Now.ToString();
                        //new 
                        r1.TravelSector = m.TravelSector;
                        #region Insert to DB
                        rdb.TRV_AirportAssistances.InsertOnSubmit(r1);
                        rdb.SubmitChanges();
                        //  Session["validate"] = Request.Cookies["SIDCC"].Value;
                    }
                    catch (Exception ex)
                    {
                        result = new { status = "0" };
                        Console.WriteLine(ex);
                    }
               

                    SendMeetandGreetEmail_Adani(m);
                    SendMeetandGreetEmail_customer(m);
                    SendMeetandGreetSMS(m);
                }
                else
                {
                    result = new { status = "4" };
                }

            }
            else
             {
                result = new { status = "2" };

             }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private void SendMeetandGreetEmail_Adani(AirportAssistance m)
        {
            MailMessage mail = null;
            var getEmailTo = "";

            try
            {
                var msgTpye = Sitecore.Context.Database.GetItem("{A7542702-9DCD-441B-BDBD-2DA3DC0E9E34}");
                getEmailTo = msgTpye.Fields["EmailTo"].Value;


                var settingsItem = Context.Database.GetItem("{FE582E25-40D8-4A0D-A159-1B1D2EC61B74}");
                var mailTemplateItem = settingsItem;
                var fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }

                var body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
                var subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];

                mail = new MailMessage
                {
                    From = new MailAddress(fromMail.Value),
                    Body = body.Value,
                    Subject = subject.Value,
                    IsBodyHtml = true
                };
            }
            catch (Exception ex)
            {
               // result = new { status = "1" };
                Log.Error("Failed to get subject specific Email", ex.ToString());
            }


            try
            {
                mail.To.Add(getEmailTo);
                mail.Body = mail.Body.Replace("#Name#", m.Name);
                mail.Body = mail.Body.Replace("#ContactNumber#", m.ContactNumber);
                mail.Body = mail.Body.Replace("#EmailAddress#", m.EmailAddress);
                mail.Body = mail.Body.Replace("#TravelDate#", m.TravelDate);
                mail.Body = mail.Body.Replace("#TypeOfService#", m.TypeOfService);
                mail.Subject = mail.Subject;
                MainUtil.SendMail(mail);
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email to the " + getEmailTo + " - Error - " + ex.Message + "", ex, this);
            }
        }

        private void SendMeetandGreetEmail_customer(AirportAssistance m)
        {
            MailMessage mail = null;
            var getEmailTo = m.EmailAddress;
            try
            {
                var settingsItem = Context.Database.GetItem("{83B2908B-C701-468E-8A9D-526075FC1B88}");
                var mailTemplateItem = settingsItem;
                var fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }

                var body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
                var subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];

                mail = new MailMessage
                {
                    From = new MailAddress(fromMail.Value),
                    Body = body.Value,
                    Subject = subject.Value,
                    IsBodyHtml = true
                };
            }
            catch (Exception ex)
            {
               // result = new { status = "1" };
                Log.Error("Failed to get subject specific Email", ex.ToString());
            }

            try
            {
                mail.To.Add(getEmailTo);
                mail.Body = mail.Body.Replace("#Name#", m.Name);
                mail.Subject = mail.Subject;
                MainUtil.SendMail(mail);
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email to the " + getEmailTo + " - Error - " + ex.Message + "", ex, this);
            }
        }

        private void SendMeetandGreetSMS(AirportAssistance m)
        {

            #region Api call to send SMS of OTP
            try
            {


                var client = new RestClient(DictionaryPhraseRepository.Current.Get("/SMS/URL", ""));
                string username = DictionaryPhraseRepository.Current.Get("/SMS/UserName", "");
                string password = DictionaryPhraseRepository.Current.Get("/SMS/Password", "");
                string from = DictionaryPhraseRepository.Current.Get("/SMS/from", "");


                var msg = "Dear " + m.Name + ", Thank you for placing your enquiry. Our Customer Service Team will reach out to you within 24 hours. ";
                byte[] concatenated = System.Text.ASCIIEncoding.ASCII.GetBytes(username + ":" + password);
                string header = System.Convert.ToBase64String(concatenated);
                var request = new RestRequest(Method.POST);

                request.AddHeader("accept", "application/json");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", "Basic " + header);
                request.AddParameter("application/json", "{\"from\":\"" + from + "\", \"to\":\"" + m.ContactNumber + "\",\"text\":\"" + msg + "\"}", ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);


            }
            catch (Exception ex)
            {
                Log.Error($"{0}", ex, this);
            }
            #endregion
        }

        public MailMessage GetContactUSMailTemplate()
        {
            var settingsItem = Context.Database.GetItem(Templates.MailTemplate.ContactUs);
            var mailTemplateItem = settingsItem;
            var fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];

            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }

            var body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
            var subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];

            return new MailMessage
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }

        public bool sendEmail(string to, string subject, string body, string from)
        {
            bool status = false;
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                var ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf);
                //mail.From = new MailAddress(Sitecore.Configuration.Settings.MailServerUserName);
                mail.From = new MailAddress(from);
                //System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(ms, ct);
                // attachment.ContentDisposition.FileName = fileName;
                // mail.Attachments.Add(attachment);
                MainUtil.SendMail(mail);
                status = true;
                return status;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message, "sendEmail - ");
                Log.Error(ex.Message, "sendEmail - ");
                Log.Error(ex.InnerException.ToString(), "sendEmail - ");
                return status;
            }
        }

        #endregion

        #region Tender Listing and User Login/Logout Registration

        /// <summary>
        /// Tender Listing for all opened/closed/Corrigendum
        /// </summary>
        /// <returns>Lsit of Tenders</returns>
        public ActionResult TenderListing()
        {
            string TenderType = "";
            if (Request.QueryString["type"] != null)
            {
                TenderType = Request.QueryString["type"].ToString();
            }

            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                List<TRV_TenderList> tenderdata;
                if (!string.IsNullOrEmpty(TenderType))
                    tenderdata = dbcontext.TRV_TenderLists.Where(x => x.TenderType == TenderType).ToList();
                else
                    tenderdata = dbcontext.TRV_TenderLists.ToList();

                List<TenderDetails> ObjTender = new List<TenderDetails>();
                foreach (var data in tenderdata)
                {
                    TenderDetails ObjTd = new TenderDetails();
                    ObjTd.Id = data.Id;
                    ObjTd.NITPRNo = data.NITNo;
                    ObjTd.Business = data.Business;
                    ObjTd.Description = data.Description;
                    ObjTd.Adv_Date = data.Adv_Date;
                    ObjTd.Bid_Submision_ClosingDate = data.Closing_Date.ToString();
                    ObjTd.Estimated_Cost = !string.IsNullOrEmpty(data.Estimated_Cost) ? data.Estimated_Cost : "-";
                    ObjTd.Cost_of_EMD = !string.IsNullOrEmpty(data.Cost_of_EMD) ? data.Cost_of_EMD : "-";
                    ObjTd.Location = data.Location;
                    //ObjTd.FileName = TNDoc.FileName;
                    ObjTd.isCorrigendumPresent = OpenTenderDocPAth(data.Id, data.NITNo);
                    ObjTd.CreatedDate = data.Created_Date;
                    ObjTd.ModifiedDate = data.Modified_Date;
                    ObjTd.Status = data.Staus;
                    ObjTd.ClosingDate = data.Closing_Date;
                    ObjTd.TenderType = data.TenderType;
                    ObjTd.SupportEmailAddress = data.SupportEmailAddress;
                    ObjTender.Add(ObjTd);
                }
                List<TRV_Corrigendum> Corr = dbcontext.TRV_Corrigendums.Where(x => x.Status == true).ToList();
                List<CorrigendumDetails> Status = new List<CorrigendumDetails>();
                foreach (var item in Corr)
                {
                    CorrigendumDetails details = new CorrigendumDetails();
                    details.Title = item.Title;
                    details.Date = (DateTime)item.Date;
                    details.NITPRNo = NITRNoFunction(item.Id);
                    details.TenderDocument = TenderDocFunction(item.Id);
                    Status.Add(details);
                }

                TenderStatus objtendersdata = new TenderStatus();
                objtendersdata.OpenTender = ObjTender;
                objtendersdata.CorrigendumTender = Status;

                return View(objtendersdata);
            }
        }

        /// <summary>
        /// User/Bidder Registration
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult UserRegistration(UserDetails model)
        {
            var listPage = Sitecore.Context.Database.GetItem(Templates.Tender.TenderList);

            bool validationStatus = false;
            Log.Error("Validating Adani Airport UserRegistration to stop auto script ", "Start");
            try
            {
                var captchaResponse = Request.Form["g-recaptcha-response"];
                validationStatus = IsReCaptchValid(captchaResponse);
                //TempData["Error"] = "Please validate captcha to continue!";
            }
            catch (Exception ex)
            {
                Log.Error("Failed to validate captcha in UserRegistration : " + ex.ToString(), "Failed");
            }
            if (validationStatus)
            {
                if (ModelState.IsValid)
                {
                    UtilityFunction ut = new UtilityFunction();
                    try
                    {
                        var userid = ut.GenerateRandomUserId();
                        var password = ut.GenerateRandomPassword();
                        using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                        {
                            var user = dbcontext.TRV_Registrations.Where(x => x.UniqueId == model.UniqueId && x.TenderId == model.TenderID).FirstOrDefault();
                            if (user == null)
                            {
                                TRV_Registration ObjReg = new TRV_Registration();
                                Guid obj = Guid.NewGuid();
                                ObjReg.Id = obj;
                                ObjReg.UserId = userid;
                                ObjReg.Password = password;
                                ObjReg.Name = model.Name;
                                ObjReg.CompanyName = model.Company;
                                ObjReg.Fax_No = model.FaxNo;
                                ObjReg.TenderId = model.TenderID;
                                ObjReg.Email = model.Email;
                                ObjReg.Mobile = model.MobileNo;
                                ObjReg.Name = model.Name;
                                ObjReg.UserType = "Visitor";
                                ObjReg.UniqueId = model.UniqueId;
                                ObjReg.AllowLogin = true;
                                ObjReg.IsFinalSubmit = false;
                                ObjReg.status = true;
                                ObjReg.Created_Date = System.DateTime.Now;
                                ObjReg.CreatedBy = model.Email;
                                //ObjReg.IsPQVerified = false;
                                dbcontext.TRV_Registrations.InsertOnSubmit(ObjReg);
                                dbcontext.SubmitChanges();
                            }
                            else
                            {
                                TempData["Error"] = "You have already registered.";
                            }
                        }

                        if (TempData["Error"] == null)
                        {
                            string RedirectUrl = string.Empty;
                            var tenderLoginitem = Sitecore.Context.Database.GetItem(Templates.Tender.TenderLogin);
                            if (tenderLoginitem != null)
                            {
                                string baseurl = tenderLoginitem.Url();
                                RedirectUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + baseurl;
                            }
                            try
                            {
                                if (!string.IsNullOrEmpty(RedirectUrl))
                                {
                                    using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                                    {
                                        var tender = dbcontext.TRV_TenderLists.Where(x => x.Id == model.TenderID).FirstOrDefault();
                                        //Note : Send Mail to User 
                                        TenderService Tn = new TenderService();
                                        Tn.SendUserRegistrationEmail(model.Email, RedirectUrl, userid, password, tender.NITNo, tender.Description);
                                        TempData["Success"] = "You have successfully registered.Please check you mail box to continue.";
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Log.Error($"Email Sending Error at User Registration:" + e.Message, e, this);
                                TempData["Success"] = "You have successfully registered. Some issue in Email sending!";
                            }
                        }

                    }
                    catch (MembershipCreateUserException ex)
                    {
                        Log.Error($"Can't create user with", ex, this);
                        TempData["Error"] = "Please try Again later.";
                        this.ModelState.AddModelError("Not able to create User", ex.Message);
                    }
                }
                else
                {
                    TempData["Error"] = "Please provide valid information for registration and try again!";
                }
            }
            else
            {
                TempData["Error"] = "Please validate captcha to continue!";
            }
            return Redirect(listPage.Url());
        }

        /// <summary>
        /// Login page for Bidder/User
        /// Login page for Admin as well
        /// </summary>
        /// <returns></returns>
        public ActionResult TenderLogin()
        {
            Session.Abandon();
            Response.Cookies.Add(new System.Web.HttpCookie("ASP.NET_SessionId", ""));
            return View(new Login());
        }

        /// <summary>
        /// Login for Tender User and Admin
        /// Check Details
        /// Download Documents
        /// Upload Documents
        /// Change Password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TenderLogin(Login model)
        {
            bool validationStatus = true;
            try
            {
                validationStatus = this.IsReCaptchValid(model.reResponse);
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Log.Info(string.Concat("Failed to validate auto script : ", ex.ToString()), "Failed");
            }
            
            if (!validationStatus)
            {
                ModelState.AddModelError(nameof(model.reResponse), DictionaryPhraseRepository.Current.Get("/Mangalore/Controller Messages/Captcha Error", "Captcha is Invalid, Please try again"));
                return this.View(model);
            }
            else
            {
                try
                {
                    var item = Context.Database.GetItem(Templates.Tender.TenderDetails);

                    //To do
                    var adminTenderListing = Context.Database.GetItem(Templates.Tender.AdminTenderListing);

                    if (!ModelState.IsValid)
                    {
                        return this.View(model);
                    }
                    using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                    {
                        var registerUser = dbcontext.TRV_Registrations.Where(x => x.UserId == model.LoginName && x.Password == model.Password && x.status == true).FirstOrDefault();
                        if (registerUser != null && registerUser.UserType != "SuperAdmin" && registerUser.UserType != "Admin")
                        {
                            DateTime tenderClosingDate = dbcontext.TRV_TenderLists.Where(t => t.Id == registerUser.TenderId).FirstOrDefault().Closing_Date.Value;
                            if (tenderClosingDate < DateTime.Now && registerUser.UserType.ToLower() == "visitor")
                            {
                                ModelState.AddModelError(nameof(model.Password), DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Login tender Error", "This Tender is closed, you won't be able to login now."));
                                return this.View(model);
                            }
                            else if (registerUser.UserType.ToLower() == "visitor" && registerUser.AllowLogin == false)
                            {
                                ModelState.AddModelError(nameof(model.Password), DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Login tender Error", "You won't be able to login now."));
                                return this.View(model);
                            }
                        }
                        if (registerUser != null)
                        {
                            TenderUserSession.TenderUserSessionContext = new TenderLoginModel
                            {
                                userId = model.LoginName,
                                TenderId = registerUser.TenderId.ToString(),
                                UserType = registerUser.UserType.ToString()
                            };
                        }
                        else
                        {
                            var loginhistories = dbcontext.TRV_LoginHistories.Where(x => x.Userid == model.LoginName && x.CreatedDate.GetValueOrDefault().Date == DateTime.Now.Date).Count();
                            if (loginhistories <= 5)
                            {
                                TRV_LoginHistory entity = new TRV_LoginHistory()
                                {
                                    Userid = model.LoginName,
                                    CreatedDate = DateTime.Now.Date
                                };
                                dbcontext.TRV_LoginHistories.InsertOnSubmit(entity);
                                dbcontext.SubmitChanges();
                            }
                            else
                            {
                                ModelState.AddModelError(nameof(model.Password), DictionaryPhraseRepository.Current.Get("/MangaloreAirport/Tender/Login Limit", "You have exceeded the Login Limit for Today"));
                                return this.View(model);
                            }
                            ModelState.AddModelError(nameof(model.Password), DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Login User Error", "User and password is not valid. Please enter valid credential."));
                            return this.View(model);
                        }

                        if (TenderUserSession.TenderUserSessionContext.UserType == "SuperAdmin" || TenderUserSession.TenderUserSessionContext.UserType == "EnvelopeAdmin" || TenderUserSession.TenderUserSessionContext.UserType == "Admin")
                        {
                            var url = adminTenderListing.Url();
                            return this.Redirect(url);
                        }
                        else
                        {
                            var url = item.Url();
                            return this.Redirect(url);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Error at TenderLogin Post:" + ex.Message, this);
                    ModelState.AddModelError(nameof(model.Password), DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Login Technical Error", "There is technical problem. Please try after sometime."));
                    return this.View(model);
                }
            }
        }

        public bool OpenTenderDocPAth(Guid tenderid, string NITPRNo)
        {
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {

                bool IsCorrigendumPresent = dbcontext.TRV_CorrigendumTenderMappings.Any(x => x.TenderId == tenderid && x.TRV_Corrigendum.Status == true);
                if (IsCorrigendumPresent)
                {
                    return true;
                }
            }
            return false;
        }

        [HttpPost] //Tender Logout
        public ActionResult Logout()
        {
            var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
            if (TenderUserSession.TenderUserSessionContext != null && !string.IsNullOrEmpty(TenderUserSession.TenderUserSessionContext.userId))
            {
                using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                {
                    var result = (from user in dbcontext.TRV_Registrations where user.UserId == TenderUserSession.TenderUserSessionContext.userId select user).Single();
                    result.Modified_Date = System.DateTime.Now;
                    result.ModifiedBy = TenderUserSession.TenderUserSessionContext.userId;
                    dbcontext.SubmitChanges();
                }
            }
            this.Session["TenderUserLogin"] = null;
            TenderUserSession.TenderUserSessionContext = null;
            return this.Redirect(item.Url());
        }

        private string IsUploadingInSequesnce(string envelopeType, string userid)
        {
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                if (envelopeType == "2")
                {
                    if (!dbcontext.TRV_UserTenderDocuments.Any(x => x.UserId == userid && x.EnvelopeType == "1"))
                    {
                        return DictionaryPhraseRepository.Current.Get("/Accounts/Tender/Upload in Envelope II before I", "Please upload document in Envelope I prior to uploading any document in Envelope II.");
                    }
                    else
                    {
                        return "success";
                    }
                }
                else if (envelopeType == "3")
                {
                    if (!dbcontext.TRV_UserTenderDocuments.Any(x => x.UserId == userid && x.EnvelopeType == "1") || !dbcontext.TRV_UserTenderDocuments.Any(x => x.UserId == userid && x.EnvelopeType == "2"))
                    {
                        return DictionaryPhraseRepository.Current.Get("/Accounts/Tender/Upload in Envelope III before II", "Please upload document in Envelope I and II prior to uploading any document in Envelope III.");
                    }
                    else
                    {
                        return "success";
                    }
                }

            }
            return "";
        }

        private bool CheckExtension(string fileName)
        {
            //'jpg', 'jpeg', 'dwg', 'pdf', 'doc', 'docx', 'xls', 'xlsx'
            string fileExtension = System.IO.Path.GetExtension(fileName);
            if (fileExtension == ".jpg" ||
                fileExtension == ".jpeg" ||
                fileExtension == ".pdf" ||
                fileExtension == ".doc" ||
                fileExtension == ".docx" ||
                fileExtension == ".xls" ||
                fileExtension == ".xlsx"
                )
                return true;
            else
                return false;
        }

        public FileResult DownloadUserFile(Guid id)
        {
            if (TenderUserSession.TenderUserSessionContext != null)
            {

                if (TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin" && TenderUserSession.TenderUserSessionContext.UserType != "EnvelopeAdmin" && TenderUserSession.TenderUserSessionContext.UserType != "Admin"
                    && TenderUserSession.TenderUserSessionContext.UserType != "Visitor")
                {
                    return null;
                }

            }
            else
            {
                return null;
            }

            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                var fileToDownload = dbcontext.TRV_UserTenderDocuments.Where(i => i.Id == id).FirstOrDefault();
                return File(fileToDownload.DocData.ToArray(), fileToDownload.ContentType, fileToDownload.FileName);
                //if (fileToDownload.UserId == TenderUserSession.TenderUserSessionContext.userId)
                //{
                //    return File(fileToDownload.DocData.ToArray(), fileToDownload.ContentType, fileToDownload.FileName);
                //}
                //else
                //    return null;
            }
        }

        public FileResult DownloadFile(Guid id)
        {
            if (TenderUserSession.TenderUserSessionContext != null)
            {

                if (TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin" && TenderUserSession.TenderUserSessionContext.UserType != "EnvelopeAdmin" && TenderUserSession.TenderUserSessionContext.UserType != "Admin"
                    && TenderUserSession.TenderUserSessionContext.UserType != "Visitor")
                {
                    return null;
                }

            }
            else
            {
                return null;
            }

            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                var fileToDownload = dbcontext.TRV_TenderDocuments.Where(i => i.Id == id).FirstOrDefault();
                return File(fileToDownload.DocData.ToArray(), fileToDownload.ContentType, fileToDownload.FileName);
            }
        }

        public FileResult DownloadCorrigendumFile(Guid id)
        {
            if (TenderUserSession.TenderUserSessionContext != null)
            {

                if (TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin" && TenderUserSession.TenderUserSessionContext.UserType != "EnvelopeAdmin" && TenderUserSession.TenderUserSessionContext.UserType != "Admin"
                    && TenderUserSession.TenderUserSessionContext.UserType != "Visitor")
                {
                    return null;
                }

            }
            else
            {
                return null;
            }
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                var fileToDownload = dbcontext.TRV_CorrigendumDocuments.Where(i => i.Id == id).FirstOrDefault();
                return File(fileToDownload.DocData.ToArray(), fileToDownload.ContentType, fileToDownload.FileName);
            }
        }




        /// <summary>
        /// After Bidder/User login 
        /// Display Tender Details 
        /// Allow user to View/Docwnload/Upload documents
        /// Change Password
        /// </summary>
        /// <returns></returns>
        public ActionResult TenderDetails()
        {
            Tender Tender = new Tender();
            try
            {
                if (Session["TenderUserLogin"] == null)
                {
                    var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                    return this.Redirect(item.Url());
                }
                if (TenderUserSession.TenderUserSessionContext != null && !string.IsNullOrEmpty(TenderUserSession.TenderUserSessionContext.TenderId))
                {
                    string tenderId = TenderUserSession.TenderUserSessionContext.TenderId;
                    string UserId = TenderUserSession.TenderUserSessionContext.userId;
                    using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                    {
                        var tenderdetails = dbcontext.TRV_TenderLists.Where(x => x.Id == new System.Guid(tenderId)).FirstOrDefault();
                        var userdocument = dbcontext.TRV_UserTenderDocuments.Where(x => x.UserId == UserId).ToList();
                        var userDetail = dbcontext.TRV_Registrations.Where(x => x.UserId == UserId).FirstOrDefault();
                        Tender.UserName = userDetail.Name;
                        Tender.Email = userDetail.Email;
                        Tender.CompanyName = userDetail.CompanyName;
                        Tender.Mobile = userDetail.Mobile;
                        Tender.NITNo = tenderdetails.NITNo;
                        Tender.IsPQVerified = userDetail.IsPQVerified == null ? false : userDetail.IsPQVerified;
                        Tender.TenderName = tenderdetails.Description;
                        Tender.TenderId = tenderId;
                        Tender.AdvDate = tenderdetails.Adv_Date;
                        Tender.ClosingDate = tenderdetails.Closing_Date;
                        Tender.tenderdocument = tenderdetails.TRV_TenderDocuments.Where(t => t.IsPQDoc != true).ToList();
                        Tender.tenderPQdocument = tenderdetails.TRV_TenderDocuments.Where(t => t.IsPQDoc == true).ToList();
                        Tender.userUpladdocument = userdocument;
                        Tender.PQDocumentStatus = userdocument != null && userdocument.Where(d => d.IsPQDoc == true || d.EnvelopeType == "1").Count() > 0 ? "Uploaded" : "Pending";
                        Tender.DocumentStatus = userdocument != null && userdocument.Where(d => d.IsPQDoc == false || d.EnvelopeType == "2").Count() > 0 ? "Uploaded" : "Pending";
                        Tender.IsPQVerified = userDetail.IsPQVerified == null ? false : userDetail.IsPQVerified;
                        Tender.FinalSubmit = userDetail.IsFinalSubmit == null ? false : userDetail.IsFinalSubmit;
                        Tender.PQApprovalRequired = tenderdetails.PQApprovalRequired == null ? false : tenderdetails.PQApprovalRequired;
                    }
                }
                else
                {
                    var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                    return this.Redirect(item.Url());
                }
            }
            catch (System.Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at TenderDetails Get:" + ex.Message, this);
                ViewBag.errormsg = "There is There is technical problem. Please try after sometime.";
            }

            return View(Tender);
        }

        /// <summary>
        /// After Bidder/User login 
        /// Display Tender Details 
        /// Allow user to View/Docwnload/Upload documents
        /// Change Password
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult TenderDetails(Tender Model, string saveDoc = null, string uploadDoc = null, string changePassword = null)
        {
            ViewBag.MessageBoxTitle = "";
            ViewBag.MessageBoxButtonText = "";
            try
            {
                bool envelope1DocUploaded = false;
                bool envelope2DocUploaded = false;
                bool envelope3DocUploaded = false;

                if (!string.IsNullOrEmpty(uploadDoc) || !string.IsNullOrEmpty(saveDoc))
                {
                    if (Model != null)
                    {

                        bool isupload = false;
                        string fileExistError = string.Empty;
                        if (Model.Envelope11 != null || Model.Envelope12 != null || Model.Envelope13 != null || Model.Envelope14 != null || Model.Envelope15 != null
                            || Model.Envelope21 != null || Model.Envelope22 != null || Model.Envelope23 != null || Model.Envelope24 != null || Model.Envelope25 != null || Model.Envelope26 != null || Model.Envelope27 != null || Model.Envelope28 != null || Model.Envelope29 != null || Model.Envelope30 != null || Model.Envelope3 != null)
                        {
                            if (Model.Envelope11 != null || Model.Envelope12 != null || Model.Envelope13 != null || Model.Envelope14 != null || Model.Envelope15 != null)
                            {
                                envelope1DocUploaded = true;
                            }
                            if (Model.Envelope21 != null || Model.Envelope22 != null || Model.Envelope23 != null || Model.Envelope24 != null || Model.Envelope25 != null || Model.Envelope26 != null || Model.Envelope27 != null || Model.Envelope28 != null || Model.Envelope29 != null || Model.Envelope30 != null)
                            {
                                envelope2DocUploaded = true;
                            }
                            if (Model.Envelope3 != null)
                            {
                                envelope3DocUploaded = true;
                            }

                            if (envelope3DocUploaded && (!envelope2DocUploaded || !envelope1DocUploaded))
                            {
                                string result = IsUploadingInSequesnce("3", TenderUserSession.TenderUserSessionContext.userId);
                                if (result != "success")
                                {
                                    ViewBag.Message = result;
                                    return View(GetTenderDetails());
                                }
                            }
                            if (envelope2DocUploaded && !envelope1DocUploaded)
                            {
                                if (Model.PQApprovalRequired == true)
                                {
                                    string result = IsUploadingInSequesnce("2", TenderUserSession.TenderUserSessionContext.userId);
                                    if (result != "success")
                                    {
                                        ViewBag.Message = result;
                                        return View(GetTenderDetails());
                                    }
                                }
                            }

                            if (!CheckFileSize(Model))
                            {
                                ViewBag.Message = "Please upload file maximum up to 10 MB";
                                ViewBag.MessageBoxTitle = "Message";
                                ViewBag.MessageBoxButtonText = "Ok";
                                return View(GetTenderDetails());
                            }

                            if (Model.Envelope11 != null && Model.Envelope11.ContentLength > 0)
                            {
                                if (CheckExtension(Model.Envelope11.FileName))
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope11, "1", TenderUserSession.TenderUserSessionContext.userId);
                                    isupload = true;
                                    envelope1DocUploaded = true;
                                    Model.Envelope11 = null;
                                }
                                //else
                                //{
                                //    ViewBag.Message = "File extension not supported!";
                                //    //return View(GetTenderDetails());
                                //}
                            }
                            if (Model.Envelope12 != null && Model.Envelope12.ContentLength > 0)
                            {
                                if (CheckExtension(Model.Envelope12.FileName))
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope12, "1", TenderUserSession.TenderUserSessionContext.userId);
                                    isupload = true;
                                    envelope1DocUploaded = true;
                                    Model.Envelope12 = null;
                                }
                            }
                            if (Model.Envelope13 != null && Model.Envelope13.ContentLength > 0)
                            {
                                if (CheckExtension(Model.Envelope13.FileName))
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope13, "1", TenderUserSession.TenderUserSessionContext.userId);
                                    isupload = true;
                                    envelope1DocUploaded = true;
                                    Model.Envelope13 = null;
                                }
                            }
                            if (Model.Envelope14 != null && Model.Envelope14.ContentLength > 0)
                            {
                                if (CheckExtension(Model.Envelope14.FileName))
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope14, "1", TenderUserSession.TenderUserSessionContext.userId);
                                    isupload = true;
                                    envelope1DocUploaded = true;
                                    Model.Envelope14 = null;
                                }
                            }
                            if (Model.Envelope15 != null && Model.Envelope15.ContentLength > 0)
                            {
                                if (CheckExtension(Model.Envelope15.FileName))
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope15, "1", TenderUserSession.TenderUserSessionContext.userId);
                                    isupload = true;
                                    envelope1DocUploaded = true;
                                    Model.Envelope15 = null;
                                }
                            }

                            if (Model.Envelope21 != null && Model.Envelope21.ContentLength > 0)
                            {
                                if (CheckExtension(Model.Envelope21.FileName))
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope21, "2", TenderUserSession.TenderUserSessionContext.userId);
                                    isupload = true;
                                    envelope2DocUploaded = true;
                                    Model.Envelope21 = null;
                                }
                            }
                            if (Model.Envelope22 != null && Model.Envelope22.ContentLength > 0)
                            {
                                if (CheckExtension(Model.Envelope22.FileName))
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope22, "2", TenderUserSession.TenderUserSessionContext.userId);
                                    isupload = true;
                                    envelope2DocUploaded = true;
                                    Model.Envelope22 = null;
                                }
                            }
                            if (Model.Envelope23 != null && Model.Envelope23.ContentLength > 0)
                            {
                                if (CheckExtension(Model.Envelope23.FileName))
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope23, "2", TenderUserSession.TenderUserSessionContext.userId);
                                    isupload = true;
                                    envelope2DocUploaded = true;
                                    Model.Envelope23 = null;
                                }
                            }
                            if (Model.Envelope24 != null && Model.Envelope24.ContentLength > 0)
                            {
                                if (CheckExtension(Model.Envelope24.FileName))
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope24, "2", TenderUserSession.TenderUserSessionContext.userId);
                                    isupload = true;
                                    envelope2DocUploaded = true;
                                    Model.Envelope24 = null;
                                }
                            }
                            if (Model.Envelope25 != null && Model.Envelope25.ContentLength > 0)
                            {
                                if (CheckExtension(Model.Envelope25.FileName))
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope25, "2", TenderUserSession.TenderUserSessionContext.userId);
                                    isupload = true;
                                    envelope2DocUploaded = true;
                                    Model.Envelope25 = null;
                                }
                            }
                        }
                        else
                        {
                            bool uploaderr = true;
                            if (Model.userUpladdocument != null)
                            {
                                if (Model.IsPQVerified == true || Model.PQApprovalRequired == false)
                                {
                                    if (Model.userUpladdocument.Where(x => x.EnvelopeType == "2").Count() > 0)
                                    { uploaderr = false; isupload = true; }
                                }
                                else
                                {
                                    if (Model.userUpladdocument.Where(x => x.EnvelopeType == "1").Count() > 0)
                                    { uploaderr = false; isupload = true; }
                                }
                            }
                            if (uploaderr)
                            {
                                ViewBag.Message = "Please upload any file in envelope prior to submit!";
                                ViewBag.MessageBoxTitle = "Message";
                                ViewBag.MessageBoxButtonText = "Ok";
                                return View(GetTenderDetails());
                            }
                        }


                        if (isupload)
                        {
                            if (!string.IsNullOrEmpty(fileExistError))
                            {
                                ViewBag.Message += fileExistError + " With the Same Name Already Present in Database. Other Files are Uploaded";
                                ViewBag.MessageBoxTitle = "Message";
                                ViewBag.MessageBoxButtonText = "Ok";
                            }
                            else
                            {
                                if (envelope1DocUploaded == true)
                                {
                                    bool IsPQVerified = false;
                                    if (Model.IsPQVerified != null)
                                    {
                                        IsPQVerified = (bool)Model.IsPQVerified;
                                    }

                                    if (!string.IsNullOrEmpty(uploadDoc) && IsPQVerified != true)
                                        ViewBag.Message += "Documents uploaded successfully for PQ verification. Please wait for approval.";
                                    else
                                        ViewBag.Message += "Documents uploaded successfully.";

                                    ViewBag.MessageBoxTitle = "Thank You";
                                    ViewBag.MessageBoxButtonText = "OK";
                                }
                                else if (envelope2DocUploaded == true)
                                {
                                    ViewBag.Message += "Documents uploaded successfully";
                                    ViewBag.MessageBoxTitle = "Thank You";
                                    ViewBag.MessageBoxButtonText = "OK";
                                }
                                else
                                {
                                    ViewBag.Message += "Documents uploaded Successfully";
                                    ViewBag.MessageBoxTitle = "Thank You";
                                    ViewBag.MessageBoxButtonText = "Ok";
                                }
                            }
                        }
                        else
                        {
                            ViewBag.Message = "There is problem for uploading file. Please try again.";
                            ViewBag.MessageBoxTitle = "";
                            ViewBag.MessageBoxButtonText = "";
                        }

                        if (!string.IsNullOrEmpty(uploadDoc))
                        {
                            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                            {
                                var user = dbcontext.TRV_Registrations.Where(x => x.UserId == TenderUserSession.TenderUserSessionContext.userId).FirstOrDefault();
                                if (user != null)
                                {
                                    user.IsFinalSubmit = true;
                                    user.Modified_Date = System.DateTime.Now;
                                    user.ModifiedBy = TenderUserSession.TenderUserSessionContext.userId;
                                    dbcontext.SubmitChanges();

                                    // List<Registration> UserEmailList = dbcontext.TRV_Registrations.Where(x => x.UserType.ToLower() == "admin" || x.UserType.ToLower() == "superadmin" || (x.UserType.ToLower() == "envelopeadmin" && x.TenderId.ToString() == Model.TenderId.ToString())).ToList();
                                    List<TRV_Registration> UserEmailList;

                                    bool IsPQVerified = false;
                                    if (Model.PQApprovalRequired == true)
                                    {
                                        if (user.IsPQVerified != null)
                                        {
                                            IsPQVerified = (bool)user.IsPQVerified;
                                        }
                                    }

                                    if (Model.PQApprovalRequired == true && IsPQVerified == false && Model.PQEnvelopeReviewStatus == false)
                                    {
                                        UserEmailList = dbcontext.TRV_Registrations.Where(x => (x.UserType.ToLower() == "envelopeadmin" && x.TenderId.ToString() == Model.TenderId.ToString())).ToList();
                                    }
                                    else
                                    {
                                        UserEmailList = dbcontext.TRV_Registrations.Where(x => x.UserType.ToLower() == "admin" || x.UserType.ToLower() == "superadmin" || (x.UserType.ToLower() == "envelopeadmin" && x.TenderId.ToString() == Model.TenderId.ToString())).ToList();
                                    }

                                    TenderService Tn = new TenderService();
                                    Tn.SendForPQApprovalEmail(UserEmailList, IsPQVerified, user.Name, user.Mobile, user.UserId, Model.TenderName, Model.NITNo);
                                }
                            }
                        }
                        else
                        {
                            Model.FinalSubmit = false;
                        }

                    }
                }
                if (!string.IsNullOrEmpty(changePassword))
                {
                    if (!ModelState.IsValid)
                    {
                        return View(GetTenderDetails());
                    }
                    if (string.IsNullOrEmpty(Model.OldPassword))
                    {
                        ModelState.AddModelError(nameof(Model.OldPassword), DictionaryPhraseRepository.Current.Get("/Accounts/Tender/Required old password", "Please enter old password"));
                        return View(GetTenderDetails());
                    }
                    if (string.IsNullOrEmpty(Model.Password))
                    {
                        ModelState.AddModelError(nameof(Model.Password), DictionaryPhraseRepository.Current.Get("/Accounts/Tender/Required new password", "Please enter New password"));
                        return View(GetTenderDetails());
                    }
                    using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                    {
                        var user = dbcontext.TRV_Registrations.Where(x => x.UserId == TenderUserSession.TenderUserSessionContext.userId && x.Password == Model.OldPassword).FirstOrDefault();
                        if (user != null)
                        {
                            user.Password = Model.Password;
                            user.Modified_Date = System.DateTime.Now;
                            user.ModifiedBy = TenderUserSession.TenderUserSessionContext.userId;
                            dbcontext.SubmitChanges();
                            //ViewBag.Message = "Your password is updated Successfully.";
                            //return View(GetTenderDetails());
                            TempData["Success"] = "Your password is updated Successfully, please login to continue";
                            var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                            this.Session["TenderUserLogin"] = null;
                            TenderUserSession.TenderUserSessionContext = null;
                            return this.Redirect(item.Url());
                        }
                        else
                        {
                            ModelState.AddModelError(nameof(Model.OldPassword), DictionaryPhraseRepository.Current.Get("/Accounts/Tender/Invalid old password", "Invalid Old Password"));
                            return View(GetTenderDetails());
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at TenderDetails Post:" + ex.Message, this);

                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());

            }


            return View(GetTenderDetails());
        }

        private bool CheckFileSize(Tender Model)
        {
            Stream strm;
            decimal size;

            if (Model.Envelope11 != null && Model.Envelope11.ContentLength > 0)
            {
                strm = Model.Envelope11.InputStream;
                size = Math.Round(((decimal)strm.Length / (decimal)1024), 2);

                if (size > 10000)
                {
                    return false;
                }
            }
            if (Model.Envelope12 != null && Model.Envelope12.ContentLength > 0)
            {
                strm = Model.Envelope12.InputStream;
                size = Math.Round(((decimal)strm.Length / (decimal)1024), 2);

                if (size > 10000)
                {
                    return false;
                }
            }
            if (Model.Envelope13 != null && Model.Envelope13.ContentLength > 0)
            {
                strm = Model.Envelope13.InputStream;
                size = Math.Round(((decimal)strm.Length / (decimal)1024), 2);

                if (size > 10000)
                {
                    return false;
                }
            }
            if (Model.Envelope14 != null && Model.Envelope14.ContentLength > 0)
            {
                strm = Model.Envelope14.InputStream;
                size = Math.Round(((decimal)strm.Length / (decimal)1024), 2);

                if (size > 10000)
                {
                    return false;
                }
            }
            if (Model.Envelope15 != null && Model.Envelope15.ContentLength > 0)
            {
                strm = Model.Envelope15.InputStream;
                size = Math.Round(((decimal)strm.Length / (decimal)1024), 2);

                if (size > 10000)
                {
                    return false;
                }
            }

            if (Model.Envelope21 != null && Model.Envelope21.ContentLength > 0)
            {
                strm = Model.Envelope21.InputStream;
                size = Math.Round(((decimal)strm.Length / (decimal)1024), 2);

                if (size > 10000)
                {
                    return false;
                }
            }
            if (Model.Envelope22 != null && Model.Envelope22.ContentLength > 0)
            {
                strm = Model.Envelope22.InputStream;
                size = Math.Round(((decimal)strm.Length / (decimal)1024), 2);

                if (size > 10000)
                {
                    return false;
                }
            }
            if (Model.Envelope23 != null && Model.Envelope23.ContentLength > 0)
            {
                strm = Model.Envelope23.InputStream;
                size = Math.Round(((decimal)strm.Length / (decimal)1024), 2);

                if (size > 10000)
                {
                    return false;
                }
            }
            if (Model.Envelope24 != null && Model.Envelope24.ContentLength > 0)
            {
                strm = Model.Envelope24.InputStream;
                size = Math.Round(((decimal)strm.Length / (decimal)1024), 2);

                if (size > 10000)
                {
                    return false;
                }
            }
            if (Model.Envelope25 != null && Model.Envelope25.ContentLength > 0)
            {
                strm = Model.Envelope25.InputStream;
                size = Math.Round(((decimal)strm.Length / (decimal)1024), 2);

                if (size > 10000)
                {
                    return false;
                }
            }
            return true;

        }

        public string saveUserTenderFile(HttpPostedFileBase file, string envelopeType, string userid)
        {
            if (file != null && file.ContentLength > 0)
            {
                // extract only the filename
                //var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var fileExt = Path.GetExtension(file.FileName);

                var userFile = Path.GetFileName(file.FileName);

                using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                {
                    if (dbcontext.TRV_UserTenderDocuments.Any(x => x.UserId == userid && x.EnvelopeType == envelopeType && x.FileName == file.FileName))
                    {
                        string returnError = userFile + " , ";
                        return returnError;
                    }
                    else
                    {
                        byte[] bytes;
                        if (fileExt == ".jpg" || fileExt == "jpeg")
                        {
                            ExifReader ExifReaderobj = new ExifReader();
                            bytes = ExifReaderobj.SetUpMetadataOnImage(file.InputStream, file.FileName);
                            //using (BinaryReader br = new BinaryReader(file.InputStream))
                            //{
                            //    bytes = br.ReadBytes(file.ContentLength);
                            //}
                        }
                        else
                        {



                            using (BinaryReader br = new BinaryReader(file.InputStream))
                            {
                                bytes = br.ReadBytes(file.ContentLength);
                            }
                        }

                        TRV_UserTenderDocument dbdoc = new TRV_UserTenderDocument();
                        dbdoc.Id = Guid.NewGuid();
                        dbdoc.UserId = userid;
                        dbdoc.FileName = file.FileName;
                        dbdoc.EnvelopeType = envelopeType;
                        dbdoc.ContentType = file.ContentType;
                        dbdoc.DocData = bytes;
                        dbdoc.IsPQDoc = envelopeType == "1" ? true : false;
                        dbdoc.Created_Date = System.DateTime.Now;
                        dbdoc.CreatedBy = userid;
                        dbcontext.TRV_UserTenderDocuments.InsertOnSubmit(dbdoc);
                        dbcontext.SubmitChanges();


                    }

                }
            }
            return string.Empty;
        }

        public Tender GetTenderDetails()
        {
            Tender tender = new Tender();
            try
            {
                string tenderId = TenderUserSession.TenderUserSessionContext.TenderId;
                string UserId = TenderUserSession.TenderUserSessionContext.userId;
                using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                {
                    var tenderdetails = dbcontext.TRV_TenderLists.Where(x => x.Id == new System.Guid(tenderId)).FirstOrDefault();
                    var userdocument = dbcontext.TRV_UserTenderDocuments.Where(x => x.UserId == UserId).ToList();
                    var userDetail = dbcontext.TRV_Registrations.Where(x => x.UserId == UserId).FirstOrDefault();

                    tender.UserName = userDetail.Name;
                    tender.Email = userDetail.Email;
                    tender.CompanyName = userDetail.CompanyName;
                    tender.Mobile = userDetail.Mobile;
                    //tender.UserName = tenderdetails.Registrations.Where(x => x.TenderId == new System.Guid(tenderId) && x.UserId == UserId).FirstOrDefault().Name;
                    tender.NITNo = tenderdetails.NITNo;
                    tender.TenderName = tenderdetails.Description;
                    tender.TenderId = tenderId;
                    tender.AdvDate = tenderdetails.Adv_Date;
                    tender.ClosingDate = tenderdetails.Closing_Date;
                    //tender.tenderdocument = tenderdetails.TRV_TenderDocuments.ToList();
                    tender.userUpladdocument = userdocument;
                    tender.PQDocumentStatus = userdocument != null && userdocument.Where(d => d.IsPQDoc == true || d.EnvelopeType == "1").Count() > 0 ? "Uploaded" : "Pending";
                    tender.DocumentStatus = userdocument != null && userdocument.Where(d => d.IsPQDoc == false || d.EnvelopeType == "2").Count() > 0 ? "Uploaded" : "Pending";
                    tender.IsPQVerified = userDetail.IsPQVerified == null ? false : userDetail.IsPQVerified;
                    tender.tenderdocument = tenderdetails.TRV_TenderDocuments.Where(t => t.IsPQDoc != true).ToList();
                    tender.tenderPQdocument = tenderdetails.TRV_TenderDocuments.Where(t => t.IsPQDoc == true).ToList();
                    tender.FinalSubmit = userDetail.IsFinalSubmit == null ? false : userDetail.IsFinalSubmit;
                    tender.PQApprovalRequired = tenderdetails.PQApprovalRequired == null ? false : tenderdetails.PQApprovalRequired;
                    tender.TenderType = tenderdetails.TenderType;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetTenderDetails Get:" + ex.Message, this);

            }
            return tender;
        }


        public ActionResult GetTenderDocument(Guid Id)
        {
            try
            {
                using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                {
                    List<TRV_TenderDocument> TD = dbcontext.TRV_TenderDocuments.Where(x => x.TenderId == Id).ToList();
                    return PartialView("GetTenderDocument", TD);
                }
            }
            catch (MembershipCreateUserException ex)
            {
                Log.Error($"Can't create user with", ex, this);
                this.ModelState.AddModelError("GetTenderDocument", ex.Message);
            }
            return RedirectToAction("TenderListing");

        }

        //Method: Delete Documents for Bidder
        //Used for Delete document of tender
        public ActionResult TenderDetailDocumentDelete(Guid id)
        {
            TrivandrumAirportRepository ElecRepo = new TrivandrumAirportRepository();
            var result = ElecRepo.DeleteUserTenderDocument(id, TenderUserSession.TenderUserSessionContext.userId, TenderUserSession.TenderUserSessionContext.TenderId);
            if (result == 0)
            {
                ViewBag.Message = "Request successful!";
            }
            else if (result == 1)
            {
                ViewBag.Message = "Not authorized!";
            }
            else
            {
                ViewBag.Message = "Request not accepted!";
            }
            var item = Context.Database.GetItem(Templates.Tender.TenderDetails);

            return Redirect(item.Url());
        }



        #endregion

        #region Admin Section 

        /// <summary>
        /// Show List of all Tender to Admin
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminTenderListing()
        {
            List<TenderDetails> ObjTender = new List<TenderDetails>();
            try
            {
                if (Session["TenderUserLogin"] == null)
                {
                    var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                    return this.Redirect(item.Url());
                }
                string UserId = TenderUserSession.TenderUserSessionContext.userId;
                string UserType = TenderUserSession.TenderUserSessionContext.UserType;
                ViewBag.UserType = UserType;
                List<TRV_TenderList> tenderdata = new List<TRV_TenderList>();
                using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                {
                    TRV_TenderList objtender = new TRV_TenderList();
                    if (UserType == "SuperAdmin")
                    {
                        tenderdata = dbcontext.TRV_TenderLists.OrderBy(x => x.Closing_Date).ToList();
                    }
                    else if (UserType == "Admin")
                    {
                        tenderdata = (from tnderlist in dbcontext.TRV_TenderLists orderby tnderlist.Closing_Date ascending where tnderlist.CreatedBy == UserId select tnderlist).ToList();
                    }
                    else // Envelope Admin
                    {
                        tenderdata = (from tdruser in dbcontext.TRV_UserTenderMappings join tnderlist in dbcontext.TRV_TenderLists on tdruser.TenderId equals tnderlist.Id orderby tnderlist.Closing_Date ascending where tdruser.UserId == UserId select tnderlist).ToList();
                    }

                    foreach (var data in tenderdata)
                    {
                        TenderDetails ObjTd = new TenderDetails();
                        ObjTd.Id = data.Id;
                        ObjTd.NITPRNo = data.NITNo;
                        ObjTd.Business = data.Business;
                        ObjTd.Description = data.Description;
                        ObjTd.Cost_of_EMD = data.Cost_of_EMD;
                        ObjTd.Estimated_Cost = data.Estimated_Cost;
                        ObjTd.Adv_Date = data.Adv_Date;
                        ObjTd.Bid_Submision_ClosingDate = data.Closing_Date.ToString();
                        ObjTd.CreatedDate = data.Created_Date;
                        ObjTd.ModifiedDate = data.Modified_Date;
                        ObjTd.Status = data.Staus;
                        ObjTd.ClosingDate = data.Closing_Date;
                        ObjTender.Add(ObjTd);
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at AdminTenderListing Get:" + ex.Message, this);
            }
            return View(ObjTender);
        }

        /// <summary>
        /// Show List of all Tender to Admin
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AdminTenderListing(string Status)
        {
            List<TenderDetails> ObjTender = new List<TenderDetails>();
            try
            {
                if (Session["TenderUserLogin"] == null)
                {
                    var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                    return this.Redirect(item.Url());
                }
                string UserId = TenderUserSession.TenderUserSessionContext.userId;
                string UserType = TenderUserSession.TenderUserSessionContext.UserType;
                ViewBag.UserType = UserType;
                List<TRV_TenderList> tenderdata = new List<TRV_TenderList>();
                using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                {
                    TRV_TenderList objtender = new TRV_TenderList();
                    if (UserType == "SuperAdmin")
                    {
                        tenderdata = dbcontext.TRV_TenderLists.OrderByDescending(x => x.Created_Date).ToList();
                    }
                    else // Envelope Admin
                    {
                        tenderdata = (from tdruser in dbcontext.TRV_UserTenderMappings join tnderlist in dbcontext.TRV_TenderLists on tdruser.TenderId equals tnderlist.Id where tdruser.UserId == UserId select tnderlist).ToList();
                    }

                    foreach (var data in tenderdata)
                    {
                        TenderDetails ObjTd = new TenderDetails();
                        ObjTd.Id = data.Id;
                        ObjTd.NITPRNo = data.NITNo;
                        ObjTd.Business = data.Business;
                        ObjTd.Description = data.Description;
                        ObjTd.Adv_Date = data.Adv_Date;
                        ObjTd.Bid_Submision_ClosingDate = data.Closing_Date.ToString();
                        ObjTd.Cost_of_EMD = data.Cost_of_EMD;
                        ObjTd.Estimated_Cost = data.Estimated_Cost;
                        ObjTd.CreatedDate = data.Created_Date;
                        ObjTd.ModifiedDate = data.Modified_Date;
                        ObjTd.Status = data.Staus.ToLower();
                        ObjTd.ClosingDate = data.Closing_Date;
                        ObjTender.Add(ObjTd);
                    }
                }

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at AdminTenderListing Get:" + ex.Message, this);
            }
            var selectedData = ObjTender;
            if (Status != "All")
            {
                selectedData = ObjTender.Where(x => x.Status == Status.ToLower()).ToList();
            }

            return View(selectedData);
        }

        /// <summary>
        /// To retrieve and display list of Users/Bidder for a Tender
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminTenderUserListing()
        {
            var datalist = new List<RegistedUserHasDocument>();
            try
            {
                if (Session["TenderUserLogin"] == null)
                {
                    var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                    return this.Redirect(item.Url());
                }
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    Guid tenderId = new Guid(Request.QueryString["id"].ToString());
                    using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                    {
                        datalist = dbcontext.TRV_Registrations.Where(x => x.TenderId == tenderId && x.status == true && x.UserType.ToLower() == "visitor").Select(s => new RegistedUserHasDocument
                        {
                            Name = s.Name,
                            Company = s.CompanyName,
                            Email = s.Email,
                            Mobile = s.Mobile,
                            UserId = s.UserId,
                            Description = s.TRV_TenderList.Description,
                            HasDocumentUploaded = dbcontext.TRV_UserTenderDocuments.Where(x => x.UserId == s.UserId && (x.IsPQDoc == false || x.EnvelopeType == "2")).Any() ? "Yes" : "No",
                            HasPQDocumentUploaded = dbcontext.TRV_UserTenderDocuments.Where(x => x.UserId == s.UserId && (x.IsPQDoc == true || x.EnvelopeType == "2")).Any() ? "Yes" : "No",
                            TenderId = s.TRV_TenderList.Id.ToString(),
                            PQApprovalStatus = (s.IsPQVerified == null) ? "None" : (s.IsPQVerified == true ? "Approved" : "Rejected")
                        }).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at AdminTenderUserListing Get:" + ex.Message, this);
            }
            return View(datalist);
        }

        public ActionResult PQVerificationApprove(string uid, Guid tenderid)
        {
            var item = Context.Database.GetItem(Templates.Tender.AdminTenderDetail);
            var items = Context.Database.GetItem(Templates.Tender.TenderLogin);
            try
            {
                if (Session["TenderUserLogin"] != null || (TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin" || TenderUserSession.TenderUserSessionContext.UserType != "Admin" || TenderUserSession.TenderUserSessionContext.UserType != "EnvelopeAdmin"))
                {
                    bool adminAproval = false;
                    string UserType = TenderUserSession.TenderUserSessionContext.UserType;

                    if (UserType == "EnvelopeAdmin")
                    {
                        string UserId = TenderUserSession.TenderUserSessionContext.userId;

                        using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                        {
                            var tenderDetail = dbcontext.TRV_TenderLists.Where(x => x.Id == tenderid).FirstOrDefault();
                            if (tenderDetail.PQApprovalRequired == true)
                            {
                                var usertoapprove = dbcontext.TRV_Registrations.Where(i => i.UserId == uid).FirstOrDefault();
                                if (usertoapprove.PQEnvelopeReviewStatus == false || usertoapprove.PQEnvelopeReviewStatus == null)
                                {
                                    usertoapprove.PQEnvelopeReviewStatus = true;
                                    usertoapprove.PQEnvelopeReviewBy = UserId;
                                    dbcontext.SubmitChanges();
                                    ViewBag.Message = "Envelope admin approval done successfully!";


                                    List<TRV_Registration> UserEmailList = dbcontext.TRV_Registrations.Where(x => x.UserType.ToLower() == "admin" || x.UserType.ToLower() == "superadmin").ToList();

                                    TenderService Tn = new TenderService();
                                    Tn.SendForPQApprovalEmail(UserEmailList, false, usertoapprove.Name, usertoapprove.Mobile, usertoapprove.UserId, tenderDetail.Description, tenderDetail.NITNo);
                                }
                                else
                                { adminAproval = true; }
                            }
                            else
                            { adminAproval = true; }

                        }
                    }
                    else
                    { adminAproval = true; }

                    if (adminAproval)
                    {
                        using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                        {
                            var usertoapprove = dbcontext.TRV_Registrations.Where(i => i.UserId == uid).FirstOrDefault();
                            usertoapprove.IsPQVerified = true;
                            usertoapprove.AllowLogin = true;
                            usertoapprove.IsFinalSubmit = false;
                            dbcontext.SubmitChanges();
                            ViewBag.Message = "Approval done successfully!";

                            string RedirectUrl = string.Empty;
                            var tenderLoginitem = Sitecore.Context.Database.GetItem(Templates.Tender.TenderLogin);
                            if (tenderLoginitem != null)
                            {
                                string baseurl = tenderLoginitem.Url();
                                RedirectUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + baseurl;
                            }
                            try
                            {
                                if (!string.IsNullOrEmpty(RedirectUrl))
                                {
                                    var tenderDetails = dbcontext.TRV_TenderLists.Where(t => t.Id == tenderid).FirstOrDefault();
                                    TenderService Tn = new TenderService();
                                    Tn.SendPQApprovalEmail(usertoapprove.Email, RedirectUrl, tenderDetails.Description, tenderDetails.NITNo);
                                }
                            }
                            catch (Exception e)
                            {
                                ViewBag.Message = "Error in approval Email Sending!";
                                Log.Error("Error in PQ approval Tender and user:" + tenderid + "" + uid + " exception:" + e.Message, this);
                            }
                        }
                    }
                }
                else
                {
                    this.Session["TenderUserLogin"] = null;
                    TenderUserSession.TenderUserSessionContext = null;
                    return this.Redirect(items.Url());


                }
            }
            catch (Exception e)
            {
                ViewBag.Message = "Error in approval!";
                Log.Error("Error in PQ approval Tender and user:" + tenderid + "" + uid + " exception:" + e.Message, this);
            }

            return Redirect(item.Url() + "?id=" + tenderid + "&uid=" + uid);
        }

        public ActionResult PQVerificationReject(string uid, Guid tenderid)
        {
            var item = Context.Database.GetItem(Templates.Tender.AdminTenderDetail);
            var items = Context.Database.GetItem(Templates.Tender.TenderLogin);
            try
            {
                if (Session["TenderUserLogin"] != null || (TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin" || TenderUserSession.TenderUserSessionContext.UserType != "Admin" || TenderUserSession.TenderUserSessionContext.UserType != "EnvelopeAdmin"))
                {
                    using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                    {
                        var usertoapprove = dbcontext.TRV_Registrations.Where(i => i.UserId == uid).FirstOrDefault();
                        usertoapprove.IsPQVerified = false;
                        usertoapprove.AllowLogin = false;
                        dbcontext.SubmitChanges();
                        //send email to do
                        ViewBag.Message = "Rejection done successfully!";

                        string RedirectUrl = string.Empty;
                        var tenderLoginitem = Sitecore.Context.Database.GetItem(Templates.Tender.TenderLogin);
                        if (tenderLoginitem != null)
                        {
                            string baseurl = tenderLoginitem.Url();
                            RedirectUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + baseurl;
                        }
                        try
                        {
                            if (!string.IsNullOrEmpty(RedirectUrl))
                            {
                                var tenderDetails = dbcontext.TRV_TenderLists.Where(t => t.Id == tenderid).FirstOrDefault();
                                TenderService Tn = new TenderService();
                                Tn.SendPQRejectionEmail(usertoapprove.Email, RedirectUrl, tenderDetails.Description, tenderDetails.NITNo);
                            }
                        }
                        catch (Exception e)
                        {
                            ViewBag.Message = "Error in Rejection Email Sending!";
                            Log.Error("Error in PQ Rejection Tender and user:" + tenderid + "" + uid + " exception:" + e.Message, this);
                        }
                    }
                }
                else
                {
                    this.Session["TenderUserLogin"] = null;
                    TenderUserSession.TenderUserSessionContext = null;
                    return this.Redirect(items.Url());


                }
            }
            catch (Exception e)
            {
                ViewBag.Message = "Error in Rejection!";
                Log.Error("Error in PQ Rejection Tender and user:" + tenderid + "" + uid + " exception:" + e.Message, this);
            }

            return Redirect(item.Url() + "?id=" + tenderid + "&uid=" + uid);
        }

        //Method: Admin Tender User Export Data
        //Used for Export Tender Corrigendum Datainto CSV
        public ActionResult AdminExportUserData()
        {
            var ReturnURL = Context.Database.GetItem(Templates.Tender.AdminUserTenderListing);

            var datalist = new List<RegistedUserHasDocument>();

            try
            {
                if (Session["TenderUserLogin"] == null)
                {
                    var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                    return this.Redirect(item.Url());
                }
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    Guid tenderId = new Guid(Request.QueryString["id"]);
                    using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                    {
                        datalist = dbcontext.TRV_Registrations.Where(x => x.TenderId == tenderId && x.status == true && x.UserType.ToLower() == "visitor").Select(s => new RegistedUserHasDocument
                        {
                            Name = s.Name,
                            Company = s.CompanyName,
                            Email = s.Email,
                            Mobile = s.Mobile,
                            UserId = s.UserId,
                            Description = s.TRV_TenderList.Description,
                            HasDocumentUploaded = dbcontext.TRV_UserTenderDocuments.Where(x => x.UserId == s.UserId).Any() ? "Yes" : "No",
                            TenderId = s.TRV_TenderList.Id.ToString()
                        }).ToList();
                        var TenderNITNo = dbcontext.TRV_TenderLists.Where(x => x.Id == tenderId).FirstOrDefault().NITNo;

                        DataTable table = ToDataTable(datalist);
                        DatatableToCSV(table, TenderNITNo);
                    }
                }



            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at AdminTenderUserListing Get:" + ex.Message, this);
            }








            return Redirect(ReturnURL.Url());
        }

        //Method: Admin Tender Details
        //used for Tender Details for Super Admin User
        public ActionResult AdminTenderDetails()
        {
            Tender tender = new Tender();
            try
            {
                if (Session["TenderUserLogin"] == null)
                {
                    var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                    return this.Redirect(item.Url());
                }

                if (!string.IsNullOrEmpty(Request.QueryString["id"]) && !string.IsNullOrEmpty(Request.QueryString["uid"]))
                {
                    string UserId = TenderUserSession.TenderUserSessionContext.userId;
                    string UserType = TenderUserSession.TenderUserSessionContext.UserType;

                    Guid tenderId = new Guid(Request.QueryString["id"].ToString());
                    string tenderuserid = Request.QueryString["uid"].ToString();

                    using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                    {
                        var tenderdetails = dbcontext.TRV_TenderLists.Where(x => x.Id == tenderId).FirstOrDefault();
                        var userdocument = dbcontext.TRV_UserTenderDocuments.Where(x => x.UserId == tenderuserid).ToList();

                        var userdetails = dbcontext.TRV_Registrations.Where(x => x.TenderId == tenderId && x.UserId == tenderuserid).FirstOrDefault();

                        tender.UserName = userdetails.Name;
                        tender.Email = userdetails.Email;
                        tender.CompanyName = userdetails.CompanyName;
                        tender.IsPQVerified = userdetails.IsPQVerified;
                        tender.Mobile = userdetails.Mobile;
                        tender.NITNo = tenderdetails.NITNo;
                        tender.TenderName = tenderdetails.Description;
                        tender.TenderId = Request.QueryString["id"].ToString();
                        tender.userUpladdocument = userdocument;
                        tender.PQApprovalRequired = tenderdetails.PQApprovalRequired == null ? false : tenderdetails.PQApprovalRequired;
                        ViewBag.Envelopetype = UserType == "SuperAdmin" ? "0" : dbcontext.TRV_UserTenderMappings.Where(x => x.UserId == UserId && x.TenderId == tenderId).FirstOrDefault().Envelope;
                        tender.PQEnvelopeReviewStatus = userdetails.PQEnvelopeReviewStatus == true ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at AdminTenderUserListing Get:" + ex.Message, this);
            }
            return View(tender);
        }


        //Method Get: Admin Tender Create
        //used for Create Tender
        public ActionResult AdminTenderCreate()
        {
            if (Session["TenderUserLogin"] == null || (TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin" && TenderUserSession.TenderUserSessionContext.UserType != "Admin"))
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());
            }
            TenderCreateModel model = new TenderCreateModel();

            var envelopeType = Context.Database.GetItem(Templates.Datasource.EnvelopeType);
            var objenvlopeList = envelopeType.GetChildren().ToList().Select(x => new EnvelopName()
            {
                Name = x.Fields["Text"].Value,
                Value = x.Fields["Value"].Value,
                IsChecked = false
            }).ToList();
            model.EnvelopUser1EnvelopNameCheckboxs = objenvlopeList;
            model.EnvelopUser2EnvelopNameCheckboxs = objenvlopeList;
            model.EnvelopUser3EnvelopNameCheckboxs = objenvlopeList;
            model.EnvelopUser4EnvelopNameCheckboxs = objenvlopeList;
            model.EnvelopUser5EnvelopNameCheckboxs = objenvlopeList;

            return View(model);
        }


        //Method Post: Admin Tender Create
        //used for Create Admin Tender 
        [HttpPost]
        public ActionResult AdminTenderCreate(TenderCreateModel obj)
        {
            bool validationStatus = true;
            try
            {
                var captchaResponse = Request.Form["g-recaptcha-response"];
                validationStatus = IsReCaptchValid(captchaResponse);
                // validationStatus = this.IsReCaptchValid(obj.reResponse);
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Log.Info(string.Concat("Failed to validate auto script : ", ex.ToString()), "Failed");
            }
            if (!validationStatus)
            {
                ModelState.AddModelError(nameof(obj.reResponse), DictionaryPhraseRepository.Current.Get("/Mangalore/Controller Messages/Captcha Error", "Captcha is Invalid, Please try again"));
                return this.View(obj);
            }
            else
            {
                try
                {
                    if (Session["TenderUserLogin"] == null || (TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin" && TenderUserSession.TenderUserSessionContext.UserType != "Admin"))
                    {
                        var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                        return this.Redirect(item.Url());
                    }

                    if (ModelState.IsValid)
                    {

                        DateTime dateTime;
                        if (!DateTime.TryParseExact(obj.Adv_Date, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault, out dateTime))
                        {
                            ModelState.AddModelError("Adv_Date", DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Date Invalid formate", "Please Input Date in dd-MM-yyyy HH:mm:ss format"));
                            return View(ReturnObj(obj));
                        }
                        else if (!DateTime.TryParseExact(obj.Closing_Date, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault, out dateTime))
                        {
                            ModelState.AddModelError("Closing_Date", DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Date Invalid formate", "Please Input Date in dd-MM-yyyy HH:mm:ss format"));
                            return View(ReturnObj(obj));
                        }
                        if (obj.Files.Count() > 0 && obj.PQFiles.Count() > 0)
                        {
                            bool err = false;
                            if (obj.Files[0] == null)
                            {
                                ModelState.AddModelError("Files", DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Date Invalid formate", "Please upload tender document"));
                                err = true;
                            }
                            if (obj.PQFiles[0] == null && obj.PQApprovalRequired == true)
                            {
                                ModelState.AddModelError("PQFiles", DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Date Invalid formate", "Please upload PQ document"));
                                err = true;
                            }
                            if (err)
                            {
                                return View(ReturnObj(obj));
                            }
                        }
                        else
                        {
                            bool err = false;
                            if (obj.Files.Count() == 0)
                            {
                                ModelState.AddModelError("Files", DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Date Invalid formate", "Please upload tender document"));
                                err = true;
                            }
                            if (obj.PQFiles.Count() == 0 && obj.PQApprovalRequired == true)
                            {
                                ModelState.AddModelError("PQFiles", DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Date Invalid formate", "Please upload PQ document"));
                                err = true;

                            }
                            if (err)
                            {
                                return View(ReturnObj(obj));
                            }

                        }


                        //Check file size
                        foreach (HttpPostedFileBase file in obj.Files)
                        {
                            if (file != null)
                            {
                                Stream strm = file.InputStream;
                                decimal size = Math.Round(((decimal)strm.Length / (decimal)1024), 2);

                                if (size > 10240)
                                {
                                    ModelState.AddModelError("Files", DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Date Invalid formate", "Tender document size can not exceed then 10 MB"));
                                    return View(ReturnObj(obj));
                                }
                            }
                        }

                        //Check file size
                        foreach (HttpPostedFileBase file in obj.PQFiles)
                        {
                            if (file != null)
                            {
                                Stream strm = file.InputStream;
                                decimal size = Math.Round(((decimal)strm.Length / (decimal)1024), 2);

                                if (size > 10000)
                                {
                                    ModelState.AddModelError("PQFiles", DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Date Invalid formate", "PQ document size can not exceed then 10 MB"));
                                    return View(ReturnObj(obj));
                                }
                            }
                        }


                        bool Envelop1User = false;
                        bool Envelop2User = false;
                        bool Envelop3User = false;
                        bool Envelop4User = false;
                        bool Envelop5User = false;

                        if (string.IsNullOrEmpty(obj.EnvelopUser1email) == false || string.IsNullOrEmpty(obj.EnvelopUser1Mobile) == false || string.IsNullOrEmpty(obj.EnvelopUser1Name) == false || obj.EnvelopUser1EnvelopNameCheckboxs.Where(x => x.IsChecked == true).Count() > 0)
                        {
                            if (string.IsNullOrEmpty(obj.EnvelopUser1email) == false && string.IsNullOrEmpty(obj.EnvelopUser1Mobile) == false && string.IsNullOrEmpty(obj.EnvelopUser1Name) == false
                                && obj.EnvelopUser1EnvelopNameCheckboxs.Where(x => x.IsChecked == true).Count() > 0)
                            {
                                Envelop1User = true;
                            }
                            else
                            {
                                ModelState.AddModelError("EnvelopUser1Mobile", DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Date Invalid formate", "All Fields are mandatory for Envelop user."));
                                return View(ReturnObj(obj));
                            }
                        }
                        if (string.IsNullOrEmpty(obj.EnvelopUser2email) == false || string.IsNullOrEmpty(obj.EnvelopUser2Mobile) == false || string.IsNullOrEmpty(obj.EnvelopUser2Name) == false || obj.EnvelopUser2EnvelopNameCheckboxs.Where(x => x.IsChecked == true).Count() > 0)
                        {
                            if (string.IsNullOrEmpty(obj.EnvelopUser2email) == false && string.IsNullOrEmpty(obj.EnvelopUser2Mobile) == false && string.IsNullOrEmpty(obj.EnvelopUser2Name) == false
                                && obj.EnvelopUser2EnvelopNameCheckboxs.Where(x => x.IsChecked == true).Count() > 0)
                            {
                                Envelop2User = true;
                            }
                            else
                            {
                                ModelState.AddModelError("EnvelopUser2Mobile", DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Date Invalid formate", "All Fields are mandatory for Envelop user."));
                                return View(ReturnObj(obj));
                            }
                        }
                        if (string.IsNullOrEmpty(obj.EnvelopUser3email) == false || string.IsNullOrEmpty(obj.EnvelopUser3Mobile) == false || string.IsNullOrEmpty(obj.EnvelopUser3Name) == false)
                        {
                            if (string.IsNullOrEmpty(obj.EnvelopUser3email) == false && string.IsNullOrEmpty(obj.EnvelopUser3Mobile) == false && string.IsNullOrEmpty(obj.EnvelopUser3Name) == false
                                && obj.EnvelopUser3EnvelopNameCheckboxs.Where(x => x.IsChecked == true).Count() > 0)
                            {
                                Envelop3User = true;
                            }
                            else
                            {
                                ModelState.AddModelError("EnvelopUser3Mobile", DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Date Invalid formate", "All Fields are mandatory for Envelop user."));
                                return View(ReturnObj(obj));
                            }
                        }
                        if (string.IsNullOrEmpty(obj.EnvelopUser4email) == false || string.IsNullOrEmpty(obj.EnvelopUser4Mobile) == false || string.IsNullOrEmpty(obj.EnvelopUser4Name) == false)
                        {
                            if (string.IsNullOrEmpty(obj.EnvelopUser4email) == false && string.IsNullOrEmpty(obj.EnvelopUser4Mobile) == false && string.IsNullOrEmpty(obj.EnvelopUser4Name) == false
                                && obj.EnvelopUser4EnvelopNameCheckboxs.Where(x => x.IsChecked == true).Count() > 0)
                            {
                                Envelop4User = true;
                            }
                            else
                            {
                                ModelState.AddModelError("EnvelopUser4Mobile", DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Date Invalid formate", "All Fields are mandatory for Envelop user."));
                                return View(ReturnObj(obj));
                            }
                        }
                        if (string.IsNullOrEmpty(obj.EnvelopUser5email) == false || string.IsNullOrEmpty(obj.EnvelopUser5Mobile) == false || string.IsNullOrEmpty(obj.EnvelopUser5Name) == false)
                        {
                            if (string.IsNullOrEmpty(obj.EnvelopUser5email) == false && string.IsNullOrEmpty(obj.EnvelopUser5Mobile) == false && string.IsNullOrEmpty(obj.EnvelopUser5Name) == false
                                && obj.EnvelopUser5EnvelopNameCheckboxs.Where(x => x.IsChecked == true).Count() > 0)
                            {
                                Envelop5User = true;
                            }
                            else
                            {
                                ModelState.AddModelError("EnvelopUser5Mobile", DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Date Invalid formate", "All Fields are mandatory for Envelop user."));
                                return View(ReturnObj(obj));
                            }
                        }

                        TrivandrumAirportRepository ElecRepo = new TrivandrumAirportRepository();
                        Guid TempId = ElecRepo.InsertTenderList(obj);


                        foreach (HttpPostedFileBase file in obj.Files)
                        {
                            if (file != null)
                            {
                                if (CheckExtension(file.FileName))
                                {
                                    byte[] bytes;
                                    using (BinaryReader br = new BinaryReader(file.InputStream))
                                    {
                                        bytes = br.ReadBytes(file.ContentLength);
                                    }
                                    obj.ContentType = file.ContentType;
                                    obj.DocData = bytes;
                                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                                    obj.Id = TempId;
                                    obj.FileName = file.FileName;
                                    obj.IsPQDoc = false;
                                    ElecRepo.InsertDocumentList(obj);
                                }
                            }
                        }

                        foreach (HttpPostedFileBase file in obj.PQFiles)
                        {
                            if (file != null)
                            {
                                if (CheckExtension(file.FileName))
                                {
                                    byte[] bytes;
                                    using (BinaryReader br = new BinaryReader(file.InputStream))
                                    {
                                        bytes = br.ReadBytes(file.ContentLength);
                                    }
                                    obj.ContentType = file.ContentType;
                                    obj.DocData = bytes;
                                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                                    obj.Id = TempId;
                                    obj.FileName = file.FileName;
                                    obj.IsPQDoc = true;
                                    ElecRepo.InsertDocumentList(obj);
                                }
                            }
                        }

                        ///////////////Need to test
                        if (Envelop1User)
                        {
                            EnvelopRepository EnvelopeRepo = new EnvelopRepository();
                            EnvelopUserDetails EnpObj = new EnvelopUserDetails();

                            EnpObj.Name = obj.EnvelopUser1Name;
                            EnpObj.Email = obj.EnvelopUser1email;
                            EnpObj.MobileNo = obj.EnvelopUser1Mobile;
                            EnpObj.SelectTenderId = TempId.ToString();
                            EnpObj.EnvelopNameCheckboxs = obj.EnvelopUser1EnvelopNameCheckboxs;

                            var registrationObj = EnvelopeRepo.InsertRegistrationEnveleope(EnpObj);
                            var userId = registrationObj.UserId;
                            var userPassword = registrationObj.Password;


                            string RedirectUrl = string.Empty;
                            var tenderLoginitem = Sitecore.Context.Database.GetItem(Templates.Tender.TenderLogin);
                            if (tenderLoginitem != null)
                            {
                                string baseurl = tenderLoginitem.Url();
                                RedirectUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + baseurl;
                            }

                            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                            {
                                var tender = dbcontext.TRV_TenderLists.Where(x => x.Id == new Guid(EnpObj.SelectTenderId)).FirstOrDefault();
                                if (!string.IsNullOrEmpty(RedirectUrl))
                                {
                                    //Note : Send Mail to User 
                                    TenderService Tn = new TenderService();
                                    Tn.SendEnvelopUserEmail(EnpObj.Email, RedirectUrl, userId, userPassword, tender.NITNo, tender.Description);
                                }
                            }
                        }

                        if (Envelop2User)
                        {
                            EnvelopRepository EnvelopeRepo = new EnvelopRepository();
                            EnvelopUserDetails EnpObj = new EnvelopUserDetails();

                            EnpObj.Name = obj.EnvelopUser2Name;
                            EnpObj.Email = obj.EnvelopUser2email;
                            EnpObj.MobileNo = obj.EnvelopUser2Mobile;
                            EnpObj.SelectTenderId = TempId.ToString();
                            EnpObj.EnvelopNameCheckboxs = obj.EnvelopUser2EnvelopNameCheckboxs;

                            var registrationObj = EnvelopeRepo.InsertRegistrationEnveleope(EnpObj);
                            var userId = registrationObj.UserId;
                            var userPassword = registrationObj.Password;


                            string RedirectUrl = string.Empty;
                            var tenderLoginitem = Sitecore.Context.Database.GetItem(Templates.Tender.TenderLogin);
                            if (tenderLoginitem != null)
                            {
                                string baseurl = tenderLoginitem.Url();
                                RedirectUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + baseurl;
                            }

                            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                            {
                                var tender = dbcontext.TRV_TenderLists.Where(x => x.Id == new Guid(EnpObj.SelectTenderId)).FirstOrDefault();
                                if (!string.IsNullOrEmpty(RedirectUrl))
                                {
                                    //Note : Send Mail to User 
                                    TenderService Tn = new TenderService();
                                    Tn.SendEnvelopUserEmail(EnpObj.Email, RedirectUrl, userId, userPassword, tender.NITNo, tender.Description);
                                }
                            }
                        }
                        if (Envelop3User)
                        {
                            EnvelopRepository EnvelopeRepo = new EnvelopRepository();
                            EnvelopUserDetails EnpObj = new EnvelopUserDetails();

                            EnpObj.Name = obj.EnvelopUser3Name;
                            EnpObj.Email = obj.EnvelopUser3email;
                            EnpObj.MobileNo = obj.EnvelopUser3Mobile;
                            EnpObj.SelectTenderId = TempId.ToString();
                            EnpObj.EnvelopNameCheckboxs = obj.EnvelopUser3EnvelopNameCheckboxs;

                            var registrationObj = EnvelopeRepo.InsertRegistrationEnveleope(EnpObj);
                            var userId = registrationObj.UserId;
                            var userPassword = registrationObj.Password;


                            string RedirectUrl = string.Empty;
                            var tenderLoginitem = Sitecore.Context.Database.GetItem(Templates.Tender.TenderLogin);
                            if (tenderLoginitem != null)
                            {
                                string baseurl = tenderLoginitem.Url();
                                RedirectUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + baseurl;
                            }

                            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                            {
                                var tender = dbcontext.TRV_TenderLists.Where(x => x.Id == new Guid(EnpObj.SelectTenderId)).FirstOrDefault();
                                if (!string.IsNullOrEmpty(RedirectUrl))
                                {
                                    //Note : Send Mail to User 
                                    TenderService Tn = new TenderService();
                                    Tn.SendEnvelopUserEmail(EnpObj.Email, RedirectUrl, userId, userPassword, tender.NITNo, tender.Description);
                                }
                            }
                        }
                        if (Envelop4User)
                        {
                            EnvelopRepository EnvelopeRepo = new EnvelopRepository();
                            EnvelopUserDetails EnpObj = new EnvelopUserDetails();

                            EnpObj.Name = obj.EnvelopUser4Name;
                            EnpObj.Email = obj.EnvelopUser4email;
                            EnpObj.MobileNo = obj.EnvelopUser4Mobile;
                            EnpObj.SelectTenderId = TempId.ToString();
                            EnpObj.EnvelopNameCheckboxs = obj.EnvelopUser4EnvelopNameCheckboxs;

                            var registrationObj = EnvelopeRepo.InsertRegistrationEnveleope(EnpObj);
                            var userId = registrationObj.UserId;
                            var userPassword = registrationObj.Password;


                            string RedirectUrl = string.Empty;
                            var tenderLoginitem = Sitecore.Context.Database.GetItem(Templates.Tender.TenderLogin);
                            if (tenderLoginitem != null)
                            {
                                string baseurl = tenderLoginitem.Url();
                                RedirectUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + baseurl;
                            }

                            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                            {
                                var tender = dbcontext.TRV_TenderLists.Where(x => x.Id == new Guid(EnpObj.SelectTenderId)).FirstOrDefault();
                                if (!string.IsNullOrEmpty(RedirectUrl))
                                {
                                    //Note : Send Mail to User 
                                    TenderService Tn = new TenderService();
                                    Tn.SendEnvelopUserEmail(EnpObj.Email, RedirectUrl, userId, userPassword, tender.NITNo, tender.Description);
                                }
                            }
                        }

                        if (Envelop5User)
                        {
                            EnvelopRepository EnvelopeRepo = new EnvelopRepository();
                            EnvelopUserDetails EnpObj = new EnvelopUserDetails();

                            EnpObj.Name = obj.EnvelopUser5Name;
                            EnpObj.Email = obj.EnvelopUser5email;
                            EnpObj.MobileNo = obj.EnvelopUser5Mobile;
                            EnpObj.SelectTenderId = TempId.ToString();
                            EnpObj.EnvelopNameCheckboxs = obj.EnvelopUser5EnvelopNameCheckboxs;

                            var registrationObj = EnvelopeRepo.InsertRegistrationEnveleope(EnpObj);
                            var userId = registrationObj.UserId;
                            var userPassword = registrationObj.Password;


                            string RedirectUrl = string.Empty;
                            var tenderLoginitem = Sitecore.Context.Database.GetItem(Templates.Tender.TenderLogin);
                            if (tenderLoginitem != null)
                            {
                                string baseurl = tenderLoginitem.Url();
                                RedirectUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + baseurl;
                            }

                            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                            {
                                var tender = dbcontext.TRV_TenderLists.Where(x => x.Id == new Guid(EnpObj.SelectTenderId)).FirstOrDefault();
                                if (!string.IsNullOrEmpty(RedirectUrl))
                                {
                                    //Note : Send Mail to User 
                                    TenderService Tn = new TenderService();
                                    Tn.SendEnvelopUserEmail(EnpObj.Email, RedirectUrl, userId, userPassword, tender.NITNo, tender.Description);
                                }
                            }
                        }
                        ViewBag.SuccessMsg = DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Tender Successful Message", "Tender Uploaded Successfully");
                        ModelState.Clear();
                        return View(ReturnObj(obj));

                    }
                    return View(ReturnObj(obj));
                }
                catch (Exception ex)
                {
                    Log.Error("Error at AdminTenderCreate Get:" + ex.Message, this);
                    ViewBag.ErrorMsg = DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/AdminTender Exception", "Error in uploading Please try again");
                    return View(ReturnObj(obj));
                }
            }
        }

        private TenderCreateModel ReturnObj(TenderCreateModel obj)
        {
            var envelopeType = Context.Database.GetItem(Templates.Datasource.EnvelopeType);
            var objenvlopeList = envelopeType.GetChildren().ToList().Select(x => new EnvelopName()
            {
                Name = x.Fields["Text"].Value,
                Value = x.Fields["Value"].Value,
                IsChecked = false
            }).ToList();
            obj.EnvelopUser1EnvelopNameCheckboxs = objenvlopeList;
            obj.EnvelopUser2EnvelopNameCheckboxs = objenvlopeList;
            obj.EnvelopUser3EnvelopNameCheckboxs = objenvlopeList;
            obj.EnvelopUser4EnvelopNameCheckboxs = objenvlopeList;
            obj.EnvelopUser5EnvelopNameCheckboxs = objenvlopeList;
            return obj;
        }


        //Method Get: Admin Tender Update
        //Used for Update Details of Tender
        public ActionResult AdminTenderUpdate()
        {
            ViewBag.Message = "";
            if (Session["TenderUserLogin"] == null || (TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin" && TenderUserSession.TenderUserSessionContext.UserType != "Admin"))
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());
            }

            string tenderid = Request.QueryString["id"];
            if (string.IsNullOrEmpty(tenderid))
            {
                var item = Context.Database.GetItem(Templates.Tender.AdminTenderListing);
                return this.Redirect(item.Url());
            }

            TrivandrumAirportRepository ElecRepo = new TrivandrumAirportRepository();
            TempData["id"] = tenderid;
            var model = ElecRepo.GetEditTenderList().FirstOrDefault(tid => tid.Id == new Guid(tenderid));
            return View(model);

        }
        //Method Post: Admin Tender Update
        //used for Update Tender
        [HttpPost]
        public ActionResult AdminTenderUpdate(TenderEditModel obj, string Inactivate_tender = null, string Activate_tender = null)
        {
            TrivandrumAirportRepository ElecRepo = new TrivandrumAirportRepository();
            Guid tenderid = obj.Id;
            var data = ElecRepo.GetEditTenderList().FirstOrDefault(tid => tid.Id == tenderid);
            obj.TenderDocuments = data.TenderDocuments;

            try
            {
                var listingitem = Sitecore.Context.Database.GetItem(Templates.Tender.AdminTenderListing);
                DateTime dateTime;
                if (!string.IsNullOrEmpty(Inactivate_tender))
                {
                    Log.Info("Inactivate_tender tender with id:" + obj.Id + " NITNo:" + obj.NITNo + " Location:" + obj.Location, this);
                    ElecRepo.InactivateTender(obj.Id);
                    Log.Info("Inactivate_tender tender successful with id:" + obj.Id + " NITNo:" + obj.NITNo + " Location:" + obj.Location, this);
                    return Redirect(listingitem.Url());
                }
                else if (!string.IsNullOrEmpty(Activate_tender))
                {
                    Log.Info("Activate_tender tender with id:" + obj.Id + " NITNo:" + obj.NITNo + " Location:" + obj.Location, this);
                    ElecRepo.ActivateTender(obj.Id);
                    Log.Info("Activate_tender tender successful with id:" + obj.Id + " NITNo:" + obj.NITNo + " Location:" + obj.Location, this);
                    return Redirect(listingitem.Url());
                }
                else if (ModelState.IsValid)
                {

                    //string avd_date = obj.Adv_Date.ToString("dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                    //string close_date = obj.Closing_Date.ToString("dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);

                    if (!DateTime.TryParseExact(obj.Adv_Date, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        ModelState.AddModelError("Adv_Date", DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Date Invalid formate", "Please Input Date in dd-MM-yyyy HH:mm:ss format"));
                        return View(obj);
                    }
                    else if (!DateTime.TryParseExact(obj.Closing_Date, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        ModelState.AddModelError("Closing_Date", DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Date Invalid formate", "Please Input Date in dd-MM-yyyy HH:mm:ss format"));
                        return View(obj);
                    }
                    else
                    {
                        ElecRepo.UpdateTenderList(obj);
                        ViewBag.Message = DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Tender Successful Message", "Tender Uploaded Successfully");
                        return Redirect(listingitem.Url());
                    }
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                Log.Error("Error at AdminTenderUpdate Get:" + ex.Message, this);
                ViewBag.EditError = DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/TenderUpdate Exception", "Error in uploading Please try again");
                return View(obj);
            }
        }

        //Admin Tender Deletefile from super admin role
        public ActionResult DeleteFile(Guid id, string DocumentPath)
        {
            var updateitem = Sitecore.Context.Database.GetItem(Templates.Tender.AdminTenderUpdate);
            try
            {
                TrivandrumAirportRepository ElecRepo = new TrivandrumAirportRepository();
                ElecRepo.DeleteTenderDocument(id, DocumentPath);

            }
            catch (Exception ex)
            {
                Log.Error("Error at DeleteFile Get:" + ex.Message, this);
            }
            return Redirect(updateitem.Url() + "?id=" + TempData["id"]);
        }

        #endregion

        #region Corrigendum

        public ActionResult TenderCorrigendumList()
        {

            Guid TenderId;
            try
            {
                TenderId = new Guid(Request.QueryString["id"]);
            }
            catch
            {
                TenderId = Guid.Empty;
            }

            if (TenderId == null)
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderDetails);
                return this.Redirect(item.Url());
            }
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                //var CorrigendumIdList = dbcontext.TRV_CorrigendumTenderMappings.Where(x => x.TenderId == TenderId).Select(x => x.Corrigendum).ToList();
                var TenderDetail = dbcontext.TRV_TenderLists.Where(x => x.Id == TenderId).FirstOrDefault();
                ViewBag.TenderDetail = TenderDetail.NITNo + " - " + TenderDetail.Business;

                List<TRV_Corrigendum> Corr = dbcontext.TRV_CorrigendumTenderMappings.Where(x => x.TenderId == TenderId && x.TRV_Corrigendum.Status == true).Select(x => x.TRV_Corrigendum).ToList();
                List<CorrigendumDetails> objCorrigendumDetails = new List<CorrigendumDetails>();
                foreach (var item in Corr)
                {
                    CorrigendumDetails details = new CorrigendumDetails();
                    details.Title = item.Title;
                    details.Date = (DateTime)item.Date;
                    details.TenderDocument = TenderDocFunction(item.Id);
                    objCorrigendumDetails.Add(details);
                }
                return View(objCorrigendumDetails);
            }
        }
        public List<TenderDetails> NITRNoFunction(Guid corrigenudmID)
        {
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                List<TenderDetails> list = new List<TenderDetails>();
                var data = dbcontext.TRV_CorrigendumTenderMappings.Where(x => x.CorrigendumId == corrigenudmID).Select(x => x.TenderId).ToList();
                foreach (var item in data)
                {
                    var tendersNITList = dbcontext.TRV_TenderLists.Where(x => x.Id == item).Select(s => new TenderDetails() { NITPRNo = s.NITNo }).ToList();
                    list.AddRange(tendersNITList);
                }
                return list;
            }
        }
        public List<TenderDetails> TenderDocFunction(Guid corrigenudmID)
        {
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                List<TenderDetails> list = new List<TenderDetails>();
                var TenderDocList = dbcontext.TRV_CorrigendumDocuments.Where(x => x.CorrigendumId == corrigenudmID).Select(x => new TenderDetails() { Id = x.Id, FileName = x.FileName }).ToList();
                list.AddRange(TenderDocList);
                return list;
            }
        }

        //Method: Corrigendum List
        //Used for Listing corrigendum
        public ActionResult CorrigendumListing()
        {
            if (Session["TenderUserLogin"] == null || (TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin" && TenderUserSession.TenderUserSessionContext.UserType != "Admin"))
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());
            }
            CorrigendumRepository CorriRepo = new CorrigendumRepository();
            //var model = CorriRepo.GetCorrigendumList().Where(x => x.Status == true).ToList();
            var model = CorriRepo.GetCorrigendumList(TenderUserSession.TenderUserSessionContext.userId, TenderUserSession.TenderUserSessionContext.UserType).ToList();
            return View(model);

        }

        //Method Get: Corrigendum Create
        //Used for Create Corrigendum
        public ActionResult CorrigendumCreate()
        {
            if (Session["TenderUserLogin"] == null || (TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin" && TenderUserSession.TenderUserSessionContext.UserType != "Admin"))
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());
            }
            CorrigendumRepository CorriRepo = new CorrigendumRepository();
            ModelState.Clear();
            CorrigendumModel model = new CorrigendumModel();
            model.CheckBoxes = CorriRepo.GetTenderList(TenderUserSession.TenderUserSessionContext.userId, TenderUserSession.TenderUserSessionContext.UserType).Where(x => x.Status != null && x.Status.ToLower() == "open").ToList();
            return View(model);
        }

        //Method Post: Corrigendum Create
        //Used for Create corrigendum
        [HttpPost]
        public ActionResult CorrigendumCreate(CorrigendumModel obj)
        {
            try
            {
                CorrigendumRepository CorriRepo = new CorrigendumRepository();
                if (ModelState.IsValid)
                {
                    var selectedRecords = obj.CheckBoxes.Where(x => x.IsChecked == true).ToList();
                    if (selectedRecords.Count <= 0)
                    {
                        ViewBag.selectedRecordNull = DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Tender Selection Validation", "Please Select Tender for Corrigendum List");
                        return View(obj);
                    }

                    //string Date = System.Convert.ToString(obj.Date);
                    //string date = obj.Date.ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    DateTime dateTime;
                    if (!DateTime.TryParseExact(obj.Date, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        ModelState.AddModelError("Date", DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Date Invalid formate", "Please Input Date in dd-MM-yyyy HH:mm:ss format"));
                        return View(obj);
                    }
                    else
                    {
                        Guid corri_id = CorriRepo.InsertCorrigendum(obj);
                        foreach (HttpPostedFileBase file in obj.Files)
                        {
                            if (file != null)
                            {
                                if (CheckExtension(file.FileName))
                                {

                                    byte[] bytes;
                                    using (BinaryReader br = new BinaryReader(file.InputStream))
                                    {
                                        bytes = br.ReadBytes(file.ContentLength);
                                    }

                                    obj.ContentType = file.ContentType;
                                    obj.DocData = bytes;

                                    var fileName = file.FileName;//Path.GetFileNameWithoutExtension(file.FileName);
                                    obj.Id = corri_id;
                                    obj.FileName = fileName;
                                    obj.IsPQDoc = false;
                                    CorriRepo.InsertCorrigendumDocument(obj);

                                    /*  var fileExt = Path.GetExtension(file.FileName);
                                        string filenamewithtimestamp = fileName.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + fileExt;
                                        string path = Server.MapPath("~/Tender/Uploadedfile/Corrigendum/");
                                        var filepath = "/Tender/Uploadedfile/Corrigendum/" + filenamewithtimestamp;
                                        if (!Directory.Exists(path))
                                        {
                                            Directory.CreateDirectory(path);
                                        }
                                        file.SaveAs(Path.Combine(path + filenamewithtimestamp));
                                        obj.Id = corri_id;
                                        obj.FileName = fileName;
                                        obj.DocumentPath = filepath;
                                        CorriRepo.InsertCorrigendumDocument(obj);*/

                                }
                            }
                        }
                        Log.Info("CorrigendumCreate created succesfully", this);
                        var tenderLoginitem = Sitecore.Context.Database.GetItem(Templates.Tender.TenderLogin);
                        string baseurl = string.Empty;
                        if (tenderLoginitem != null)
                        {
                            baseurl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + tenderLoginitem.Url();
                        }
                        foreach (var ls in selectedRecords)
                        {
                            var TenderId = ls.Id;
                            CorriRepo.InsertCorrigendumTenderMapping(TenderId, corri_id);
                            //send email
                            try
                            {
                                Log.Info("CorrigendumCreate created succesfully: send email to bidder started", this);
                                using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                                {
                                    var tender = dbcontext.TRV_TenderLists.Where(x => x.Id == TenderId).FirstOrDefault();
                                    var tenderBidders = dbcontext.TRV_Registrations.Where(x => x.TenderId == TenderId && x.UserType.ToLower() == "visitor").Select(s => s.Email).Distinct().ToList();

                                    Log.Info("CorrigendumCreate created succesfully: send email total distinct bidders-" + tenderBidders.Count, this);
                                    foreach (var bidder in tenderBidders)
                                    {
                                        TenderService Tn = new TenderService();

                                        var date = DateTime.ParseExact(obj.Date, "dd-MM-yyyy HH:mm:ss", new System.Globalization.CultureInfo("en-US"));
                                        Tn.SendCorrigendumCreateEmail(bidder, tender.NITNo, tender.Description, obj.Title, date.ToString("dd-MM-yyyy"), baseurl);
                                        Log.Info("CorrigendumCreate created succesfully: mail sent to " + bidder, this);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Log.Error("Error at CorrigendumCreate Send email:" + e.Message, this);
                            }
                        }
                    }
                    ViewBag.SuccessMsg = DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Corrigendum Success Msg", "Corrigendum Uploaded Successfully");
                    ModelState.Clear();
                    CorrigendumModel model = new CorrigendumModel();
                    model.CheckBoxes = CorriRepo.GetTenderList(TenderUserSession.TenderUserSessionContext.userId, TenderUserSession.TenderUserSessionContext.UserType).Where(x => x.Status.ToLower() == "open").ToList();
                    return View(model);
                }
                else
                {
                    return View(obj);
                }

            }
            catch (Exception ex)
            {
                Log.Error("Error at CorrigendumCreate Post:" + ex.Message, this);
                ViewBag.ErrorMsg = DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Corrigendum exception msg", "Error in Insert Please try again");
                return View(obj);
            }
        }

        //Method Get:Corrigendum Update
        //Used for Corrigendum update
        public ActionResult CorrigenudumUpdate()
        {
            if (Session["TenderUserLogin"] == null || (TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin" && TenderUserSession.TenderUserSessionContext.UserType != "Admin"))
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());
            }
            CorrigendumRepository CorriRepo = new CorrigendumRepository();
            TempData["id"] = Request.QueryString["id"];
            string corrigendumId = Request.QueryString["id"];
            if (string.IsNullOrEmpty(corrigendumId))
            {
                var item = Context.Database.GetItem(Templates.Tender.CorrigendumTenderListing);
                return this.Redirect(item.Url());
            }
            var model = CorriRepo.GetEditCorrigendumList(new Guid(corrigendumId));
            return View(model);
        }

        //Method Post: Corrigendum Update
        //Used for update corrigendum Details Update
        [HttpPost]
        public ActionResult CorrigenudumUpdate(CorrigedumEditModel obj, string Inactivate_Corrigendum = null, string Activate_Corrigendum = null)
        {
            CorrigendumRepository CorriRepo = new CorrigendumRepository();
            Guid CorriId = obj.Id;
            var data = CorriRepo.GetEditCorrigendumList(CorriId);
            obj.CorrigendumDocument = data.CorrigendumDocument;
            try
            {
                var listingitem = Sitecore.Context.Database.GetItem(Templates.Tender.CorrigendumTenderListing);
                DateTime dateTime;
                if (!string.IsNullOrEmpty(Inactivate_Corrigendum))
                {
                    Log.Info("InactivateCorrigendum with id:" + obj.Id + " Title:" + obj.Title, this);
                    CorriRepo.InactivateCorrigendum(obj.Id);
                    Log.Info("InactivateCorrigendum successful with id:" + obj.Id + " Title:" + obj.Title, this);
                    return Redirect(listingitem.Url());
                }
                else if (!string.IsNullOrEmpty(Activate_Corrigendum))
                {
                    Log.Info("Activate_Corrigendum with id:" + obj.Id + " Title:" + obj.Title, this);
                    CorriRepo.ActivateCorrigendum(obj.Id);
                    Log.Info("Activate_Corrigendum successful with id:" + obj.Id + " Title:" + obj.Title, this);
                    return Redirect(listingitem.Url());
                }
                else if (ModelState.IsValid)
                {

                    var selectedRecords = obj.TenderList.Where(x => x.IsChecked == true).ToList();
                    //string date = obj.Date.Value.ToString("dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                    if (!DateTime.TryParseExact(obj.Date, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        ModelState.AddModelError("Date", DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Date Invalid formate", "Please Input Date in dd-MM-yyyy HH:mm:ss format"));
                        return View(obj);
                    }
                    else if (selectedRecords.Count <= 0)
                    {
                        ViewBag.selectedRecordNull = DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Tender Selection Validation", "Please Select Tender List");
                        return View(obj);
                    }
                    else
                    {

                        //Update Corrigendum 
                        CorriRepo.UpdateCorrrigendum(obj);

                        foreach (HttpPostedFileBase file in obj.Files)
                        {
                            if (file != null)
                            {
                                if (CheckExtension(file.FileName))
                                {

                                    byte[] bytes;
                                    using (BinaryReader br = new BinaryReader(file.InputStream))
                                    {
                                        bytes = br.ReadBytes(file.ContentLength);
                                    }

                                    obj.ContentType = file.ContentType;
                                    obj.DocData = bytes;
                                    obj.IsPQDoc = false;


                                    var fileName = file.FileName;// Path.GetFileNameWithoutExtension(file.FileName);
                                    /* var fileExt = Path.GetExtension(file.FileName);
                                     string filenamewithtimestamp = fileName.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + fileExt;
                                     string path = Server.MapPath("~/Tender/Uploadedfile/Corrigendum/");
                                     var filepath = "/Tender/Uploadedfile/Corrigendum/" + filenamewithtimestamp;
                                     if (!Directory.Exists(path))
                                     {
                                         Directory.CreateDirectory(path);
                                     }
                                     file.SaveAs(Path.Combine(path + filenamewithtimestamp));*/
                                    obj.Id = obj.Id;
                                    obj.FileName = fileName;
                                    //  obj.DocumentPath = filepath;
                                    CorriRepo.UpdateCorrigendumDocument(obj);
                                }
                            }
                        }
                        //Update CorrigendumTenderMapping
                        CorriRepo.UpdateCorrigendumMapping(obj, selectedRecords);
                        return Redirect(listingitem.Url());
                    }
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                Log.Error("Error at CorrigenudumUpdate Post:" + ex.Message, this);
                ViewBag.EditError = DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Corrigendum exception msg", "Error in Insert Please try again");
                return View(obj);
            }
        }

        //Delete corrigendum uploaded file
        public ActionResult DeleteCorrigendumFile(Guid id, string DocumentPath)
        {
            var updateitem = Sitecore.Context.Database.GetItem(Templates.Tender.CorrigendumTenderUpdate);
            var item = Context.Database.GetItem(Templates.Tender.AdminTenderDetail);

            var items = Context.Database.GetItem(Templates.Tender.TenderLogin);
            try
            {
                if (Session["TenderUserLogin"] != null || (TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin" || TenderUserSession.TenderUserSessionContext.UserType != "Admin" || TenderUserSession.TenderUserSessionContext.UserType != "EnvelopeAdmin"))
                {
                    CorrigendumRepository CorriRepo = new CorrigendumRepository();
                    CorriRepo.DeleteCorrigendumDocument(id, DocumentPath);
                }
                else
                {
                    this.Session["TenderUserLogin"] = null;
                    TenderUserSession.TenderUserSessionContext = null;
                    return this.Redirect(items.Url());

                }
            }
            catch (Exception ex)
            {
                Log.Error("Error at DeleteCorrigendumFile Post:" + ex.Message, this);
                //ViewBag.FileDeleteError = "There is Error in File upload";
                //Guid Edit_Id = (Guid)TempData["id"];
                //return RedirectToAction("EditCorrigendum", "TrivandrumAirport", new { @id = Edit_Id });
            }
            //  Guid EditId = (Guid)TempData["id"];
            //return RedirectToAction("EditCorrigendum", "TrivandrumAirport", new { @id = EditId });
            return Redirect(updateitem.Url() + "?id=" + TempData["id"]);
        }
        #endregion

        #region Envelop-User
        //Method Get: Admin Envelope Create
        //Used for Get Admin Envelope
        public ActionResult AdminEnvelopeCreate()
        {
            if (Session["TenderUserLogin"] == null || (TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin" && TenderUserSession.TenderUserSessionContext.UserType != "Admin"))
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());
            }
            EnvelopRepository EnvelopeRepo = new EnvelopRepository();
            ModelState.Clear();
            EnvelopUserDetails model = new EnvelopUserDetails();
            model.TenderList = EnvelopeRepo.GetOpenTenderListfordropdown(TenderUserSession.TenderUserSessionContext.userId, TenderUserSession.TenderUserSessionContext.UserType);

            var envelopeType = Context.Database.GetItem(Templates.Datasource.EnvelopeType);
            var objenvlopeList = envelopeType.GetChildren().ToList().Select(x => new EnvelopName()
            {
                Name = x.Fields["Text"].Value,
                Value = x.Fields["Value"].Value,
                IsChecked = false
            }).ToList();
            model.EnvelopNameCheckboxs = objenvlopeList;

            return View(model);
        }

        //Method Post: Admin Envelope Create
        //Used for Create admin Envelope
        [HttpPost]
        public ActionResult AdminEnvelopeCreate(EnvelopUserDetails obj)
        {
            if (Session["TenderUserLogin"] == null || (TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin" && TenderUserSession.TenderUserSessionContext.UserType != "Admin"))
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());
            }
            bool validationStatus = false;
            try
            {
                var captchaResponse = Request.Form["g-recaptcha-response"];
                validationStatus = IsReCaptchValid(captchaResponse);
                //TempData["Error"] = "Please validate captcha to continue!";
            }
            catch (Exception ex)
            {
                Log.Error("Failed to validate captcha in UserRegistration : " + ex.ToString(), "Failed");
            }
            if (validationStatus)
            {
                EnvelopRepository EnvelopeRepo = new EnvelopRepository();
                EnvelopUserDetails ReturnObj = new EnvelopUserDetails();
                ReturnObj.TenderList = EnvelopeRepo.GetOpenTenderListfordropdown(TenderUserSession.TenderUserSessionContext.userId, TenderUserSession.TenderUserSessionContext.UserType);
                //List<EnvelopName> objenvlopeList = new List<EnvelopName>();
                //objenvlopeList.Add(new EnvelopName() { Name = "Envelope 1", Value = "1", IsChecked = false });
                //objenvlopeList.Add(new EnvelopName() { Name = "Envelope 2", Value = "2", IsChecked = false });
                //objenvlopeList.Add(new EnvelopName() { Name = "Envelope 3", Value = "3", IsChecked = false });

                var envelopeType = Context.Database.GetItem(Templates.Datasource.EnvelopeType);
                var objenvlopeList = envelopeType.GetChildren().ToList().Select(x => new EnvelopName()
                {
                    Name = x.Fields["Text"].Value,
                    Value = x.Fields["Value"].Value,
                    IsChecked = false
                }).ToList();

                ReturnObj.EnvelopNameCheckboxs = objenvlopeList;
                try
                {
                    if (ModelState.IsValid)
                    {
                        var SelectedEnvelope = obj.EnvelopNameCheckboxs.Where(x => x.IsChecked == true).ToList();
                        if (obj.SelectTenderId == null)
                        {
                            ViewBag.SelectTenderId = DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Selection Validation msg", "Please Select Tender From List");
                            return View(ReturnObj);
                        }
                        if (SelectedEnvelope.Count <= 0)
                        {
                            ViewBag.SelectedEnvelope = DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Envelop selection msg", "Please Select Minimum 1 Envelope");
                            return View(ReturnObj);
                        }
                        var registrationObj = EnvelopeRepo.InsertRegistrationEnveleope(obj);
                        var userId = registrationObj.UserId;
                        var userPassword = registrationObj.Password;


                        string RedirectUrl = string.Empty;
                        var tenderLoginitem = Sitecore.Context.Database.GetItem(Templates.Tender.TenderLogin);
                        if (tenderLoginitem != null)
                        {
                            string baseurl = tenderLoginitem.Url();
                            RedirectUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + baseurl;
                        }

                        using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                        {
                            var tender = dbcontext.TRV_TenderLists.Where(x => x.Id == new Guid(obj.SelectTenderId)).FirstOrDefault();
                            if (!string.IsNullOrEmpty(RedirectUrl))
                            {
                                //Note : Send Mail to User 
                                TenderService Tn = new TenderService();
                                Tn.SendEnvelopUserEmail(obj.Email, RedirectUrl, userId, userPassword, tender.NITNo, tender.Description);
                            }
                        }
                        ViewBag.SuccessMsg = DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/EnvelopUser Success Msg", "User created successfully and an email has been sent with details.");
                        ModelState.Clear();
                        return View(ReturnObj);
                    }
                    else
                    {
                        return View(ReturnObj);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Error at AdminEnvelopCreate Post:" + ex.Message, this);
                    ViewBag.ErrorMsg = DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/EnvelopUser Exception Msg", "Error in Insert Please try again");
                    return View(ReturnObj);
                }
            }
            else {

                TempData["Error"] = "Please validate captcha to continue!";
                ViewBag.ErrorMsg = "Please validate captcha to continue!";

                EnvelopRepository EnvelopeRepo = new EnvelopRepository();
                ModelState.Clear();
                EnvelopUserDetails model = new EnvelopUserDetails();
                model.TenderList = EnvelopeRepo.GetOpenTenderListfordropdown(TenderUserSession.TenderUserSessionContext.userId, TenderUserSession.TenderUserSessionContext.UserType);

                var envelopeType = Context.Database.GetItem(Templates.Datasource.EnvelopeType);
                var objenvlopeList = envelopeType.GetChildren().ToList().Select(x => new EnvelopName()
                {
                    Name = x.Fields["Text"].Value,
                    Value = x.Fields["Value"].Value,
                    IsChecked = false
                }).ToList();
                model.EnvelopNameCheckboxs = objenvlopeList;

                return View(model);
            }
        }

        //Method: Admin Envelope List
        //Used For Listing Envelope List
        public ActionResult AdminEnvelopeListing()
        {
            if (Session["TenderUserLogin"] == null || (TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin" && TenderUserSession.TenderUserSessionContext.UserType != "Admin"))
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());
            }
            EnvelopRepository EnvelopeRepo = new EnvelopRepository();
            return View(EnvelopeRepo.ListRegisterdEnvelope(TenderUserSession.TenderUserSessionContext.userId, TenderUserSession.TenderUserSessionContext.UserType));

        }
        //Method: Delete Envelope User
        //Used for Disable Envelope User from DB
        [HttpPost]
        public ActionResult DisableEnvelopeUser(string id)
        {
            if (TenderUserSession.TenderUserSessionContext != null)
            {

                if ((TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin" && TenderUserSession.TenderUserSessionContext.UserType != "Admin"))
                {
                    var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                    return this.Redirect(item.Url());
                }
            }
            else
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());
            }
            EnvelopRepository EnvelopeRepo = new EnvelopRepository();
            var envelopelisting = Sitecore.Context.Database.GetItem(Templates.Tender.EnvelopeUserListing);
            try
            {
                EnvelopeRepo.DisableUser(id);
            }
            catch (Exception ex)
            {
                Log.Error("Error at DisableEnvelopeUser Post:" + ex.Message, this);
                ViewBag.DisableUserError = DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/DisableEnvelopeUser Exception Msg", "There is Error in Disable Users");
                return View();
            }
            return Redirect(envelopelisting.Url());
        }


        #endregion

        #region Download Functions
        //Method: To data table
        //Used for Convert List to Datatable
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];

                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        //Method: DatatableToCSV
        //Used for data table to csv and download CSV
        public void DatatableToCSV(DataTable dtDataTable, string TenderNITNo)
        {
            #region Old Method
            //StreamWriter sw = new StreamWriter(strFilePath, false);
            ////headers  
            //for (int i = 0; i < dtDataTable.Columns.Count; i++)
            //{
            //    sw.Write(dtDataTable.Columns[i]);
            //    if (i < dtDataTable.Columns.Count - 1)
            //    {
            //        sw.Write(",");
            //    }
            //}
            //sw.Write(sw.NewLine);
            //foreach (DataRow dr in dtDataTable.Rows)
            //{
            //    for (int i = 0; i < dtDataTable.Columns.Count; i++)
            //    {
            //        if (!System.Convert.IsDBNull(dr[i]))
            //        {
            //            string value = dr[i].ToString();
            //            if (value.Contains(','))
            //            {
            //                value = String.Format("\"{0}\"", value);
            //                sw.Write(value);
            //            }
            //            else
            //            {
            //                sw.Write(dr[i].ToString());
            //            }
            //        }
            //        if (i < dtDataTable.Columns.Count - 1)
            //        {
            //            sw.Write(",");
            //        }
            //    }
            //    sw.Write(sw.NewLine);
            //}
            //sw.Close();
            //DownLoad(strFilePath);
            #endregion

            #region New Method
            string fileName = "Tender_User_" + TenderNITNo;
            Stopwatch stw = new Stopwatch();
            stw.Start();
            StringBuilder sb = new StringBuilder();
            //Column headers  
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sb.Append(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append(Environment.NewLine);
            //Column Values
            foreach (DataRow dr in dtDataTable.Rows)
            {
                sb.AppendLine(string.Join(",", dr.ItemArray));
            }

            stw.Stop();

            //Download File
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".csv");
            Response.Charset = "";
            Response.ContentType = "application/csv";
            Response.Output.Write(sb);
            Response.Flush();
            Response.End();


            #endregion
        }

        #endregion


        //Method Get: Admin Envelope Create
        //Used for Get Admin Envelope
        public ActionResult AdminUserCreate()
        {
            if (Session["TenderUserLogin"] == null || (TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin"))
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());
            }
            AdminUserRepository EnvelopeRepo = new AdminUserRepository();
            ModelState.Clear();
            AdminUserDetails model = new AdminUserDetails();
            model.TenderList = EnvelopeRepo.GetOpenTenderListfordropdown();

            /* var envelopeType = Context.Database.GetItem(Templates.Datasource.EnvelopeType);
             var objenvlopeList = envelopeType.GetChildren().ToList().Select(x => new EnvelopName()
             {
                 Name = x.Fields["Text"].Value,
                 Value = x.Fields["Value"].Value,
                 IsChecked = false
             }).ToList();
             model.EnvelopNameCheckboxs = objenvlopeList;*/

            return View(model);
        }

        //Method Post: Admin Envelope Create
        //Used for Create admin Envelope
        [HttpPost]
        public ActionResult AdminUserCreate(AdminUserDetails obj)
        {
            bool validationStatus = false;
            try
            {
                var captchaResponse = Request.Form["g-recaptcha-response"];
                validationStatus = IsReCaptchValid(captchaResponse);
                //TempData["Error"] = "Please validate captcha to continue!";
            }
            catch (Exception ex)
            {
                Log.Error("Failed to validate captcha in UserRegistration : " + ex.ToString(), "Failed");
            }
            if (validationStatus)
            {
                if (Session["TenderUserLogin"] == null || (TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin" && TenderUserSession.TenderUserSessionContext.UserType != "Admin"))
                {
                    var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                    return this.Redirect(item.Url());
                }

                AdminUserRepository EnvelopeRepo = new AdminUserRepository();
                AdminUserDetails ReturnObj = new AdminUserDetails();
                ReturnObj.TenderList = EnvelopeRepo.GetOpenTenderListfordropdown();
                //List<EnvelopName> objenvlopeList = new List<EnvelopName>();
                //objenvlopeList.Add(new EnvelopName() { Name = "Envelope 1", Value = "1", IsChecked = false });
                //objenvlopeList.Add(new EnvelopName() { Name = "Envelope 2", Value = "2", IsChecked = false });
                //objenvlopeList.Add(new EnvelopName() { Name = "Envelope 3", Value = "3", IsChecked = false });

                /*--- var envelopeType = Context.Database.GetItem(Templates.Datasource.EnvelopeType);
                 var objenvlopeList = envelopeType.GetChildren().ToList().Select(x => new EnvelopName()
                 {
                     Name = x.Fields["Text"].Value,
                     Value = x.Fields["Value"].Value,
                     IsChecked = false
                 }).ToList();

                 ReturnObj.EnvelopNameCheckboxs = objenvlopeList;---*/
                try
                {
                    if (ModelState.IsValid)
                    {
                        obj.SelectTenderId = ReturnObj.TenderList.FirstOrDefault().Value;
                        // var SelectedEnvelope = obj.EnvelopNameCheckboxs.Where(x => x.IsChecked == true).ToList();
                        if (obj.SelectTenderId == null)
                        {
                            ViewBag.SelectTenderId = DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Selection Validation msg", "Please Select Tender From List");
                            return View(ReturnObj);
                        }
                        /*if (SelectedEnvelope.Count <= 0)
                        {
                            ViewBag.SelectedEnvelope = DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/Envelop selection msg", "Please Select Minimum 1 Envelope");
                            return View(ReturnObj);
                        }*/
                        var registrationObj = EnvelopeRepo.InsertRegistrationEnveleope(obj);
                        var userId = registrationObj.UserId;
                        var userPassword = registrationObj.Password;


                        string RedirectUrl = string.Empty;
                        var tenderLoginitem = Sitecore.Context.Database.GetItem(Templates.Tender.TenderLogin);
                        if (tenderLoginitem != null)
                        {
                            string baseurl = tenderLoginitem.Url();
                            RedirectUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + baseurl;
                        }

                        using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                        {
                            var tender = dbcontext.TRV_TenderLists.Where(x => x.Id == new Guid(obj.SelectTenderId)).FirstOrDefault();
                            if (!string.IsNullOrEmpty(RedirectUrl))
                            {
                                //Note : Send Mail to User 
                                TenderService Tn = new TenderService();
                                Tn.SendAdminUserEmail(obj.Email, RedirectUrl, userId, userPassword, tender.NITNo, tender.Description);
                            }
                        }
                        ViewBag.SuccessMsg = DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/EnvelopUser Success Msg", "User created successfully and an email has been sent with details.");
                        ModelState.Clear();
                        return View(ReturnObj);
                    }
                    else
                    {
                        return View(ReturnObj);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Error at AdminEnvelopCreate Post:" + ex.Message, this);
                    ViewBag.ErrorMsg = DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/EnvelopUser Exception Msg", "Error in Insert Please try again");
                    return View(ReturnObj);
                }
            }
            else
            {
                TempData["Error"] = "Please validate captcha to continue!";
                ViewBag.ErrorMsg = "Please validate captcha to continue!";
                AdminUserRepository EnvelopeRepo = new AdminUserRepository();
                AdminUserDetails model = new AdminUserDetails();
                model.TenderList = EnvelopeRepo.GetOpenTenderListfordropdown();
                return View(model);
            }
        }


        //Method: Admin Envelope List
        //Used For Listing Envelope List
        public ActionResult AdminUserListing()
        {
            if (Session["TenderUserLogin"] == null || (TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin"))
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());
            }
            AdminUserRepository EnvelopeRepo = new AdminUserRepository();
            return View(EnvelopeRepo.ListRegisterdEnvelope());

        }
        //Method: Delete Envelope User
        //Used for Disable Envelope User from DB
        [HttpPost]
        public ActionResult DisableAdminUser(string id)
        {
            if (TenderUserSession.TenderUserSessionContext != null)
            {

                if (TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin")
                {
                    var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                    return this.Redirect(item.Url());
                }
            }
            else
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());
            }

            AdminUserRepository EnvelopeRepo = new AdminUserRepository();
            var envelopelisting = Sitecore.Context.Database.GetItem(Templates.Tender.AdminUser);
            try
            {
                EnvelopeRepo.DisableUser(id);
            }
            catch (Exception ex)
            {
                Log.Error("Error at DisableEnvelopeUser Post:" + ex.Message, this);
                ViewBag.DisableUserError = DictionaryPhraseRepository.Current.Get("/TrivandrumAirport/Tender/DisableEnvelopeUser Exception Msg", "There is Error in Disable Users");
                return View();
            }
            return Redirect(envelopelisting.Url());
        }
        [HttpPost]
        public ActionResult AirportSurveyFeedback(AirportSurveyFeedback airportSurvey)
        {
            String[] Experience = { "Not Used", "Excellent", "Good", "Average", "Fair", "Poor" };
            String[] Gender_Group = { "Male", "Female", "Rather not say" };
            String[] Travel_Purpose = { "Business/ Client Meeting", "Site Visit", "Conference/Convention", "Business Others", "Individual", "Friends", "Family", "Event/ Wedding", "Leisure Others" };
            String[] Age_Group = { "<18", "18-45", "46-60", "60<" };
            bool isValid = true;
            bool validationStatus = true;
            var result = new { status = "1" };
            Log.Error("Validating Adani Airport Airport Survey Feedback to stop auto script ", "Start");


            if (validationStatus == true)
            {
                Log.Error("AirportSurveyFeedback", "Start");


                if (!String.IsNullOrEmpty(airportSurvey.FirstName))
                {
                    if (airportSurvey.FirstName.Length < 3 || (!Regex.IsMatch(airportSurvey.FirstName, (@"^[a-zA-Z ]*$"))))
                    {
                        isValid = false;
                        result = new { status = "3" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (!String.IsNullOrEmpty(airportSurvey.LastName))
                {
                    if (airportSurvey.LastName.Length < 3 || (!Regex.IsMatch(airportSurvey.LastName, (@"^[a-zA-Z ]*$"))))
                    {
                        isValid = false;
                        result = new { status = "4" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (!String.IsNullOrEmpty(airportSurvey.Contact))
                {
                    if (Regex.IsMatch(airportSurvey.Contact, (@"^\d+$")))
                    {
                        if (airportSurvey.Contact.Length < 10 || airportSurvey.Contact.Length > 10)
                        {
                            isValid = false;
                            result = new { status = "23" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if (!Regex.IsMatch(airportSurvey.Contact, (@"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[com]{2,9})$")))
                    {

                        isValid = false;
                        result = new { status = "23" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (!Experience.Contains(airportSurvey.FacilitiesServices))
                {
                    isValid = false;
                    result = new { status = "8" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!Experience.Contains(airportSurvey.AirportCleanliness))
                {
                    isValid = false;
                    result = new { status = "9" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!Experience.Contains(airportSurvey.AirportFacilities))
                {
                    isValid = false;
                    result = new { status = "10" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!Experience.Contains(airportSurvey.ShoppingFacilities))
                {
                    isValid = false;
                    result = new { status = "11" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!Experience.Contains(airportSurvey.CourtesyOfStaff))
                {
                    isValid = false;
                    result = new { status = "12" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!Experience.Contains(airportSurvey.WaitingTime))
                {
                    isValid = false;
                    result = new { status = "13" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!Experience.Contains(airportSurvey.ArrivalFacilities))
                {
                    isValid = false;
                    result = new { status = "14" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!Experience.Contains(airportSurvey.TransportationFacilities))
                {
                    isValid = false;
                    result = new { status = "15" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!Experience.Contains(airportSurvey.FlightConnectivity))
                {
                    isValid = false;
                    result = new { status = "16" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!Experience.Contains(airportSurvey.MaintenanceUpkeep))
                {
                    isValid = false;
                    result = new { status = "17" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(airportSurvey.RecommendationScale) || (!Regex.IsMatch(airportSurvey.RecommendationScale, (@"^([0-9]|10)$"))))
                {
                    isValid = false;
                    result = new { status = "18" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(airportSurvey.Suggestion))
                {
                    isValid = false;
                    result = new { status = "19" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!Travel_Purpose.Contains(airportSurvey.TravelPurpose))
                {
                    isValid = false;
                    result = new { status = "20" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!Age_Group.Contains(airportSurvey.AgeGroup))
                {
                    isValid = false;
                    result = new { status = "21" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!Gender_Group.Contains(airportSurvey.Gender))
                {
                    isValid = false;
                    result = new { status = "22" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (isValid == false)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                
                    try
                    {
                        TenderDBDataContext rdb = new TenderDBDataContext();
                        TRV_AirportSurveyFeedback ASF = new TRV_AirportSurveyFeedback();
                        ASF.Id = (rdb.TRV_AirportSurveyFeedbacks.Select(x => (long?)x.Id).Max() ?? 0) + 1;
                        ASF.FirstName = airportSurvey.FirstName;
                        ASF.LastName = airportSurvey.LastName;
                        ASF.DateOfVisit = airportSurvey.DateOfVisit;
                        ASF.DateOfVisit = airportSurvey.DateOfVisit;
                        ASF.FlightDetails = airportSurvey.FlightDetails;
                        ASF.Contact = airportSurvey.Contact;
                        ASF.FacilitiesServices = airportSurvey.FacilitiesServices;
                        ASF.AirportCleanliness = airportSurvey.AirportCleanliness;
                        ASF.AirportFacilities = airportSurvey.AirportFacilities;
                        ASF.ShoppingFacilities = airportSurvey.ShoppingFacilities;
                        ASF.CourtesyOfStaff = airportSurvey.CourtesyOfStaff;
                        ASF.WaitingTime = airportSurvey.WaitingTime;
                        ASF.ArrivalFacilities = airportSurvey.ArrivalFacilities;
                        ASF.TransportationFacilities = airportSurvey.TransportationFacilities;
                        ASF.FlightConnectivity = airportSurvey.FlightConnectivity;
                        ASF.MaintenanceUpkeep = airportSurvey.MaintenanceUpkeep;
                        ASF.RecommendationScale = airportSurvey.RecommendationScale;
                        ASF.Suggestion = airportSurvey.Suggestion;
                        ASF.TravelPurpose = airportSurvey.TravelPurpose;
                        ASF.AgeGroup = airportSurvey.AgeGroup;
                        ASF.Gender = airportSurvey.Gender;
                        ASF.FormSubmitDate = DateTime.Now;

                        #region Insert to DB
                        rdb.TRV_AirportSurveyFeedbacks.InsertOnSubmit(ASF);
                        rdb.SubmitChanges();
                        //Sending Email for successful registration
                        /*Data.Items.Item settingsItem;
                        settingsItem = Context.Database.GetItem(Template.MailConfiguration.AirportSurveyTemplate.airportSurveyEmailTemplateID);


                        var mailTemplateItem = settingsItem;
                        var fromMail = mailTemplateItem.Fields[Template.MailConfiguration.AirportSurveyTemplate.Fields.From];
                        var body = mailTemplateItem.Fields[Template.MailConfiguration.AirportSurveyTemplate.Fields.Body];
                        var subject = mailTemplateItem.Fields[Template.MailConfiguration.AirportSurveyTemplate.Fields.Subject];
                        var mailTo = settingsItem.Fields["To"];


                        string bodyText = body.Value.Replace("[FirstName]", airportSurvey.FirstName);
                        bodyText = bodyText.Replace("[LastName]", airportSurvey.LastName);
                        bodyText = bodyText.Replace("[DateOfVisit]", airportSurvey.DateOfVisit.ToString());
                        bodyText = bodyText.Replace("[FlightDetails]", airportSurvey.FlightDetails);
                        bodyText = bodyText.Replace("[Contact]", airportSurvey.Contact);
                        bodyText = bodyText.Replace("[FacilitiesServices]", airportSurvey.FacilitiesServices);
                        bodyText = bodyText.Replace("[AirportCleanliness]", airportSurvey.AirportCleanliness);
                        bodyText = bodyText.Replace("[AirportFacilities]", airportSurvey.AirportFacilities);
                        bodyText = bodyText.Replace("[ShoppingFacilities]", airportSurvey.ShoppingFacilities);
                        bodyText = bodyText.Replace("[CourtesyOfStaff]", airportSurvey.CourtesyOfStaff);
                        bodyText = bodyText.Replace("[WaitingTime]", airportSurvey.WaitingTime);
                        bodyText = bodyText.Replace("[ArrivalFacilities]", airportSurvey.ArrivalFacilities);
                        bodyText = bodyText.Replace("[TransportationFacilities]", airportSurvey.TransportationFacilities);
                        bodyText = bodyText.Replace("[FlightConnectivity]", airportSurvey.FlightConnectivity);
                        bodyText = bodyText.Replace("[MaintenanceUpkeep]", airportSurvey.MaintenanceUpkeep);
                        bodyText = bodyText.Replace("[RecommendationScale]", airportSurvey.RecommendationScale);
                        bodyText = bodyText.Replace("[Suggestion]", airportSurvey.Suggestion);
                        bodyText = bodyText.Replace("[TravelPurpose]", airportSurvey.TravelPurpose);
                        bodyText = bodyText.Replace("[AgeGroup]", airportSurvey.AgeGroup);
                        bodyText = bodyText.Replace("[Gender]", airportSurvey.Gender);

                        MailMessage mail = new MailMessage
                        {
                            From = new MailAddress(fromMail.Value),
                            Body = bodyText,
                            Subject = subject.Value,
                            IsBodyHtml = true,

                        };

                        mail.To.Add(mailTo.ToString());

                        MainUtil.SendMail(mail);*/
                    }
                    catch (Exception ex)
                    {
                        result = new { status = "0" };
                        Console.WriteLine(ex);
                    }


                    /*SendMeetandGreetEmail_Adani(m);
                    SendMeetandGreetEmail_customer(m);
                    SendMeetandGreetSMS(m);*/
                
               

            }
            else
            {
                result = new { status = "2" };
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Captcha Validation
        public bool IsReCaptchValidV3(string reResponse)
        {
            HttpClient httpClient = new HttpClient();
            string secretKey = DictionaryPhraseRepository.Current.Get("/GoogleCaptcha/SecretKey", "");
            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={reResponse}").Result;

            if (res.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);

            if (JSONdata.success != "true" || JSONdata.score <= 0.5)
            {
                return false;
            }

            return true;
        }
        private bool SelectCategory(int CaseType)
        {
            string casetypecode = CaseType.ToString();
            Sitecore.Data.Database web = Sitecore.Configuration.Factory.GetDatabase("web");
            Sitecore.Data.Items.Item ContactUsCategory = web.GetItem("{8F6F87F2-9636-4CA7-B8F1-348E3A08A013}");
            foreach (Sitecore.Data.Items.Item child in ContactUsCategory.Children)
            {
                string Value = child.Fields["Value"].ToString(); ;
                if (Value == casetypecode)
                {
                    return true;
                }
            }
            return false;
        }
        // Tuple Method For Searching
        //Contact Us Form
        [HttpPost]
        public ActionResult AirportContactUs(TRVAirportContactUs airportContactUs)
        {
            bool isValid = true;
            bool validationStatus = true;
            var result = new { status = "1" };
            Log.Info("Contact Us Form TRV Start",this);

            // Retrieve a specific cookie by name
            string cookieName = "EncryptedClientIP";
            System.Web.HttpCookie cookie = Request.Cookies[cookieName];

            if (cookie != null)
            {
                // Get the value of the cookie
                string cookieValue = cookie.Value;

                if (cookieValue != airportContactUs.IPAddress)
                {
                    result = new { status = "5" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string decryptedIPAddress = Adani.Feature.Common.Helpers.CustomExtension.Decrypt(airportContactUs.IPAddress, key, iv);
                    airportContactUs.IPAddress = decryptedIPAddress;
                }
            }
            else
            {
                result = new { status = "5" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            if (ModelState.IsValid)
            {
                Log.Info("Contact Us Form TRV Validation Start", this);

               var flag = this.IsReCaptchValidV3(airportContactUs.reResponse);
                if (flag == false)
                {
                    result = new { status = "2" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                Log.Error("Airport Contact Us Form", "Start");
                bool categoryIsTrue = SelectCategory(airportContactUs.ContactType);
                if (!categoryIsTrue)
                {
                    result = new { status = "4" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }



                try
                {
                    string securitytoken = DictionaryPhraseRepository.Current.Get("/TRVContactFormCRM/securitytoken", "");
                    string CRMAPI = DictionaryPhraseRepository.Current.Get("/TRVContactFormCRM/API", "");
                    Log.Info("CRM Start  TRV Contact Us form", this);

                    var CRMrequest = new Contact_Us_CRM_Request();
                    CRMrequest.Name = airportContactUs.Fullname;
                    CRMrequest.adl_emailaddress = airportContactUs.Email;
                    CRMrequest.adl_description = airportContactUs.Message;
                    CRMrequest.casetypecode = airportContactUs.ContactType;
                    CRMrequest.adl_mobilenumber = airportContactUs.ContactNo;

                    var jsonString = JsonConvert.SerializeObject(CRMrequest);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(jsonString);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpClient client = new HttpClient();
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    client.DefaultRequestHeaders.Add("securitytoken", securitytoken);
                    HttpResponseMessage crmResponseResult = client.PostAsync(CRMAPI, byteContent).Result;
                    var responseString = crmResponseResult.Content.ReadAsStringAsync();
                    Contact_Us_CRM_Response CRMresponse = JsonConvert.DeserializeObject<Contact_Us_CRM_Response>(responseString.Result.ToString());
                    crmResponseResult.EnsureSuccessStatusCode();

                    Log.Info("CRM Result", CRMresponse);
                    Log.Info("CRM END  TRV Contact Us form", this);


                    Log.Info("Insert data into TRV Contact Us DB ", this);

                    TenderDBDataContext rdb = new TenderDBDataContext();
                    TRV_AirportContactUsForm contactUsForm = new TRV_AirportContactUsForm();
                    contactUsForm.Id = (rdb.TRV_AirportContactUsForms.Select(i => (long?)i.Id).Max() ?? 0) + 1;
                    contactUsForm.FullName = airportContactUs.Fullname;
                    contactUsForm.Email = airportContactUs.Email;
                    contactUsForm.ContactNo = airportContactUs.ContactNo;
                    contactUsForm.ContactType = airportContactUs.ContactType.ToString();
                    contactUsForm.Message = airportContactUs.Message;
                    contactUsForm.CaseNumber = CRMresponse.CaseNumber;
                    contactUsForm.ResMessage = CRMresponse.Message;
                    contactUsForm.FormSubmitDate = DateTime.Now;
                    contactUsForm.IPAddress = airportContactUs.IPAddress;

                    #region Insert to DB
                    rdb.TRV_AirportContactUsForms.InsertOnSubmit(contactUsForm);
                    rdb.SubmitChanges();
                    Log.Info("data inserted in  DB  sucessfully  TRV Contact Us form", this);
                }
                catch (Exception ex)
                {
                    Log.Error("Exception thrown in AirportContactUs:" + ex, this);
                    result = new { status = ex.Message };
                    Console.WriteLine(ex);
                }
            }
            else
            {
                result = new { status = "3" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}