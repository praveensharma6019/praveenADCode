 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Adani.SuperApp.Airport.Feature.DutyFreeProductImport.Platform.Models;
using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using Adani.SuperApp.Airport.Foundation.DataAPI.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Command
{
    public class PranaamPackageImport
    {
        LogRepository _logRepository = new LogRepository();
        List<string> ApiItem = new List<string>();
        public void Execute(Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {
            _logRepository.Info("My Sitecore scheduled task is being run!");
            ReadProductJson();
        }
        private void ReadProductJson()
        {
            APIResponse aPIResponse = new APIResponse();
            _logRepository.Info("get Products form ProductImportAPI");

            Database contextDB = Sitecore.Context.ContentDatabase;
            //_logRepository.Info("Gets the context database->" + contextDB.ToString());
            Item parentItem = contextDB.GetItem(Constants.PranaamAirportDS);

            if (parentItem.Children.Count > 0 && parentItem.Children != null)
            {
                foreach(Item cityAirport in parentItem.Children)
                {
                    var cityCode = !string.IsNullOrEmpty(cityAirport.Fields[Constants.AirportCode].Value) ? cityAirport.Fields[Constants.AirportCode].Value : string.Empty;
                    if(cityCode == Constants.Ahmedabad)
                    {
                        if(cityAirport.Children.Count>0 && cityAirport.Children != null)
                        {
                            foreach(Item serviceType in cityAirport.Children)
                            {
                                var serviceName = !string.IsNullOrEmpty(serviceType.Fields[Constants.ServiceName].Value) ? serviceType.Fields[Constants.ServiceName].Value : string.Empty;
                                var serviceId = !string.IsNullOrEmpty(serviceType.Fields[Constants.ServiceId].Value) ? serviceType.Fields[Constants.ServiceId].Value : string.Empty;
                                var serviceActiveStatus = !string.IsNullOrEmpty(serviceType.Fields[Constants.IsActive].Value) ? serviceType.Fields[Constants.IsActive].Value : string.Empty;
                                if (serviceName == Constants.Arrival && serviceId == "4" && serviceActiveStatus == "1")
                                {
                                    if(serviceType.Children.Count>0 && serviceType.Children != null)
                                    {
                                        foreach(Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.Domestic && travelSectorId == "3" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                    ServiceTypeId = "4",
                                                    ServiceType = "Arrival",
                                                    TravelSector = "Domestic",
                                                    OriginAirport = "DEL",
                                                    DestinationAirport = "AMD",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.International && travelSectorId == "4" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                    ServiceTypeId = "4",
                                                    ServiceType = "Arrival",
                                                    TravelSector = "International",
                                                    OriginAirport = "DXB",
                                                    DestinationAirport = "AMD",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for Arrival, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }

                                if (serviceName == Constants.Departure && serviceId == "1" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.Domestic && travelSectorId == "3" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                    ServiceTypeId = "1",
                                                    ServiceType = "Departure",
                                                    TravelSector = "Domestic",
                                                    OriginAirport = "AMD",
                                                    DestinationAirport = "DEL",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.International && travelSectorId == "4" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                    ServiceTypeId = "1",
                                                    ServiceType = "Departure",
                                                    TravelSector = "International",
                                                    OriginAirport = "AMD",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for Departure, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }

                                if (serviceName == Constants.Transit && serviceId == "2" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.DomesticToDomestic && travelSectorId == "6" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "Domestic to Domestic",
                                                    OriginAirport = "DEL",
                                                    DestinationAirport = "AMD",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.DomesticToInternational && travelSectorId == "1" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "Domestic to International",
                                                    OriginAirport = "AMD",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.InternationalToDomestic && travelSectorId == "5" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "International to Domestic",
                                                    OriginAirport = "DXB",
                                                    DestinationAirport = "AMD",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.InternationalToInternational && travelSectorId == "2" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "International to International",
                                                    OriginAirport = "DXB",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for Transit, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }

                                if (serviceName == Constants.RoundTrip && serviceId == "3" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.Domestic && travelSectorId == "3" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                    ServiceTypeId = "3",
                                                    ServiceType = "round trip",
                                                    TravelSector = "Domestic",
                                                    OriginAirport = "AMD",
                                                    DestinationAirport = "DEL",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.International && travelSectorId == "4" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                    ServiceTypeId = "3",
                                                    ServiceType = "round trip",
                                                    TravelSector = "International",
                                                    OriginAirport = "AMD",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for RoundTrip, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (cityCode == Constants.Guwahati)
                    {
                        if (cityAirport.Children.Count > 0 && cityAirport.Children != null)
                        {
                            foreach (Item serviceType in cityAirport.Children)
                            {
                                var serviceName = !string.IsNullOrEmpty(serviceType.Fields[Constants.ServiceName].Value) ? serviceType.Fields[Constants.ServiceName].Value : string.Empty;
                                var serviceId = !string.IsNullOrEmpty(serviceType.Fields[Constants.ServiceId].Value) ? serviceType.Fields[Constants.ServiceId].Value : string.Empty;
                                var serviceActiveStatus = !string.IsNullOrEmpty(serviceType.Fields[Constants.IsActive].Value) ? serviceType.Fields[Constants.IsActive].Value : string.Empty;
                                if (serviceName == Constants.Arrival && serviceId == "4" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.Domestic && travelSectorId == "3" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                    ServiceTypeId = "4",
                                                    ServiceType = "Arrival",
                                                    TravelSector = "Domestic",
                                                    OriginAirport = "DEL",
                                                    DestinationAirport = "GAU",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.International && travelSectorId == "4" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                    ServiceTypeId = "4",
                                                    ServiceType = "Arrival",
                                                    TravelSector = "International",
                                                    OriginAirport = "DXB",
                                                    DestinationAirport = "GAU",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for Arrival, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }

                                if (serviceName == Constants.Departure && serviceId == "1" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.Domestic && travelSectorId == "3" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                    ServiceTypeId = "1",
                                                    ServiceType = "Departure",
                                                    TravelSector = "Domestic",
                                                    OriginAirport = "GAU",
                                                    DestinationAirport = "DEL",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.International && travelSectorId == "4" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                    ServiceTypeId = "1",
                                                    ServiceType = "Departure",
                                                    TravelSector = "International",
                                                    OriginAirport = "GAU",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for Departure, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }

                                if (serviceName == Constants.Transit && serviceId == "2" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.DomesticToDomestic && travelSectorId == "6" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "Domestic to Domestic",
                                                    OriginAirport = "DEL",
                                                    DestinationAirport = "GAU",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.DomesticToInternational && travelSectorId == "1" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "Domestic to International",
                                                    OriginAirport = "GAU",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.InternationalToDomestic && travelSectorId == "5" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "International to Domestic",
                                                    OriginAirport = "DXB",
                                                    DestinationAirport = "GAU",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.InternationalToInternational && travelSectorId == "2" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "International to International",
                                                    OriginAirport = "DXB",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for Transit, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }

                                if (serviceName == Constants.RoundTrip && serviceId == "3" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.Domestic && travelSectorId == "3" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "3",
                                                    ServiceType = "round trip",
                                                    TravelSector = "Domestic",
                                                    OriginAirport = "GAU",
                                                    DestinationAirport = "DEL",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.International && travelSectorId == "4" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "3",
                                                    ServiceType = "round trip",
                                                    TravelSector = "International",
                                                    OriginAirport = "GAU",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for RoundTrip, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (cityCode == Constants.Jaipur)
                    {
                        if (cityAirport.Children.Count > 0 && cityAirport.Children != null)
                        {
                            foreach (Item serviceType in cityAirport.Children)
                            {
                                var serviceName = !string.IsNullOrEmpty(serviceType.Fields[Constants.ServiceName].Value) ? serviceType.Fields[Constants.ServiceName].Value : string.Empty;
                                var serviceId = !string.IsNullOrEmpty(serviceType.Fields[Constants.ServiceId].Value) ? serviceType.Fields[Constants.ServiceId].Value : string.Empty;
                                var serviceActiveStatus = !string.IsNullOrEmpty(serviceType.Fields[Constants.IsActive].Value) ? serviceType.Fields[Constants.IsActive].Value : string.Empty;
                                if (serviceName == Constants.Arrival && serviceId == "4" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.Domestic && travelSectorId == "3" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "4",
                                                    ServiceType = "Arrival",
                                                    TravelSector = "Domestic",
                                                    OriginAirport = "DEL",
                                                    DestinationAirport = "JAI",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.International && travelSectorId == "4" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "4",
                                                    ServiceType = "Arrival",
                                                    TravelSector = "International",
                                                    OriginAirport = "DXB",
                                                    DestinationAirport = "JAI",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for Arrival, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }

                                if (serviceName == Constants.Departure && serviceId == "1" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.Domestic && travelSectorId == "3" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "1",
                                                    ServiceType = "Departure",
                                                    TravelSector = "Domestic",
                                                    OriginAirport = "JAI",
                                                    DestinationAirport = "DEL",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.International && travelSectorId == "4" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "1",
                                                    ServiceType = "Departure",
                                                    TravelSector = "International",
                                                    OriginAirport = "JAI",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for Departure, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }

                                if (serviceName == Constants.Transit && serviceId == "2" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.DomesticToDomestic && travelSectorId == "6" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "Domestic to Domestic",
                                                    OriginAirport = "DEL",
                                                    DestinationAirport = "JAI",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.DomesticToInternational && travelSectorId == "1" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "Domestic to International",
                                                    OriginAirport = "JAI",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.InternationalToDomestic && travelSectorId == "5" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "International to Domestic",
                                                    OriginAirport = "DXB",
                                                    DestinationAirport = "JAI",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.InternationalToInternational && travelSectorId == "2" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "International to International",
                                                    OriginAirport = "DXB",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for Transit, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }

                                if (serviceName == Constants.RoundTrip && serviceId == "3" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.Domestic && travelSectorId == "3" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "3",
                                                    ServiceType = "round trip",
                                                    TravelSector = "Domestic",
                                                    OriginAirport = "JAI",
                                                    DestinationAirport = "DEL",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.International && travelSectorId == "4" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "3",
                                                    ServiceType = "round trip",
                                                    TravelSector = "International",
                                                    OriginAirport = "JAI",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for RoundTrip, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (cityCode == Constants.Lucknow)
                    {
                        if (cityAirport.Children.Count > 0 && cityAirport.Children != null)
                        {
                            foreach (Item serviceType in cityAirport.Children)
                            {
                                var serviceName = !string.IsNullOrEmpty(serviceType.Fields[Constants.ServiceName].Value) ? serviceType.Fields[Constants.ServiceName].Value : string.Empty;
                                var serviceId = !string.IsNullOrEmpty(serviceType.Fields[Constants.ServiceId].Value) ? serviceType.Fields[Constants.ServiceId].Value : string.Empty;
                                var serviceActiveStatus = !string.IsNullOrEmpty(serviceType.Fields[Constants.IsActive].Value) ? serviceType.Fields[Constants.IsActive].Value : string.Empty;
                                if (serviceName == Constants.Arrival && serviceId == "4" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.Domestic && travelSectorId == "3" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "4",
                                                    ServiceType = "Arrival",
                                                    TravelSector = "Domestic",
                                                    OriginAirport = "DEL",
                                                    DestinationAirport = "LKO",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.International && travelSectorId == "4" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "4",
                                                    ServiceType = "Arrival",
                                                    TravelSector = "International",
                                                    OriginAirport = "DXB",
                                                    DestinationAirport = "LKO",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for Arrival, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }

                                if (serviceName == Constants.Departure && serviceId == "1" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.Domestic && travelSectorId == "3" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "1",
                                                    ServiceType = "Departure",
                                                    TravelSector = "Domestic",
                                                    OriginAirport = "LKO",
                                                    DestinationAirport = "DEL",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.International && travelSectorId == "4" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "1",
                                                    ServiceType = "Departure",
                                                    TravelSector = "International",
                                                    OriginAirport = "LKO",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for Departure, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }

                                if (serviceName == Constants.Transit && serviceId == "2" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.DomesticToDomestic && travelSectorId == "6" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "Domestic to Domestic",
                                                    OriginAirport = "DEL",
                                                    DestinationAirport = "LKO",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.DomesticToInternational && travelSectorId == "1" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "Domestic to International",
                                                    OriginAirport = "LKO",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.InternationalToDomestic && travelSectorId == "5" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "International to Domestic",
                                                    OriginAirport = "DXB",
                                                    DestinationAirport = "LKO",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.InternationalToInternational && travelSectorId == "2" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "International to International",
                                                    OriginAirport = "DXB",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for Transit, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }

                                if (serviceName == Constants.RoundTrip && serviceId == "3" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.Domestic && travelSectorId == "3" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "3",
                                                    ServiceType = "round trip",
                                                    TravelSector = "Domestic",
                                                    OriginAirport = "LKO",
                                                    DestinationAirport = "DEL",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.International && travelSectorId == "4" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "3",
                                                    ServiceType = "round trip",
                                                    TravelSector = "International",
                                                    OriginAirport = "LKO",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for RoundTrip, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (cityCode == Constants.Mangalore)
                    {
                        if (cityAirport.Children.Count > 0 && cityAirport.Children != null)
                        {
                            foreach (Item serviceType in cityAirport.Children)
                            {
                                var serviceName = !string.IsNullOrEmpty(serviceType.Fields[Constants.ServiceName].Value) ? serviceType.Fields[Constants.ServiceName].Value : string.Empty;
                                var serviceId = !string.IsNullOrEmpty(serviceType.Fields[Constants.ServiceId].Value) ? serviceType.Fields[Constants.ServiceId].Value : string.Empty;
                                var serviceActiveStatus = !string.IsNullOrEmpty(serviceType.Fields[Constants.IsActive].Value) ? serviceType.Fields[Constants.IsActive].Value : string.Empty;
                                if (serviceName == Constants.Arrival && serviceId == "4" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.Domestic && travelSectorId == "3" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "4",
                                                    ServiceType = "Arrival",
                                                    TravelSector = "Domestic",
                                                    OriginAirport = "DEL",
                                                    DestinationAirport = "IXE",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.International && travelSectorId == "4" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "4",
                                                    ServiceType = "Arrival",
                                                    TravelSector = "International",
                                                    OriginAirport = "DXB",
                                                    DestinationAirport = "IXE",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for Arrival, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }

                                if (serviceName == Constants.Departure && serviceId == "1" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.Domestic && travelSectorId == "3" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "1",
                                                    ServiceType = "Departure",
                                                    TravelSector = "Domestic",
                                                    OriginAirport = "IXE",
                                                    DestinationAirport = "DEL",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.International && travelSectorId == "4" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "1",
                                                    ServiceType = "Departure",
                                                    TravelSector = "International",
                                                    OriginAirport = "IXE",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for Departure, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }

                                if (serviceName == Constants.Transit && serviceId == "2" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.DomesticToDomestic && travelSectorId == "6" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "Domestic to Domestic",
                                                    OriginAirport = "DEL",
                                                    DestinationAirport = "IXE",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.DomesticToInternational && travelSectorId == "1" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "Domestic to International",
                                                    OriginAirport = "IXE",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.InternationalToDomestic && travelSectorId == "5" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "International to Domestic",
                                                    OriginAirport = "DXB",
                                                    DestinationAirport = "IXE",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.InternationalToInternational && travelSectorId == "2" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "International to International",
                                                    OriginAirport = "DXB",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for Transit, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }

                                if (serviceName == Constants.RoundTrip && serviceId == "3" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.Domestic && travelSectorId == "3" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "3",
                                                    ServiceType = "round trip",
                                                    TravelSector = "Domestic",
                                                    OriginAirport = "IXE",
                                                    DestinationAirport = "DEL",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.International && travelSectorId == "4" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "3",
                                                    ServiceType = "round trip",
                                                    TravelSector = "International",
                                                    OriginAirport = "IXE",
                                                    DestinationAirport = "DEL",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for RoundTrip, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (cityCode == Constants.Trivandrum)
                    {
                        if (cityAirport.Children.Count > 0 && cityAirport.Children != null)
                        {
                            foreach (Item serviceType in cityAirport.Children)
                            {
                                var serviceName = !string.IsNullOrEmpty(serviceType.Fields[Constants.ServiceName].Value) ? serviceType.Fields[Constants.ServiceName].Value : string.Empty;
                                var serviceId = !string.IsNullOrEmpty(serviceType.Fields[Constants.ServiceId].Value) ? serviceType.Fields[Constants.ServiceId].Value : string.Empty;
                                var serviceActiveStatus = !string.IsNullOrEmpty(serviceType.Fields[Constants.IsActive].Value) ? serviceType.Fields[Constants.IsActive].Value : string.Empty;
                                if (serviceName == Constants.Arrival && serviceId == "4" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.Domestic && travelSectorId == "3" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "4",
                                                    ServiceType = "Arrival",
                                                    TravelSector = "Domestic",
                                                    OriginAirport = "DEL",
                                                    DestinationAirport = "TRV",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.International && travelSectorId == "4" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "4",
                                                    ServiceType = "Arrival",
                                                    TravelSector = "International",
                                                    OriginAirport = "DXB",
                                                    DestinationAirport = "TRV",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for Arrival, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }

                                if (serviceName == Constants.Departure && serviceId == "1" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.Domestic && travelSectorId == "3" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "1",
                                                    ServiceType = "Departure",
                                                    TravelSector = "Domestic",
                                                    OriginAirport = "TRV",
                                                    DestinationAirport = "DEL",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.International && travelSectorId == "4" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "1",
                                                    ServiceType = "Departure",
                                                    TravelSector = "International",
                                                    OriginAirport = "TRV",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for Departure, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }

                                if (serviceName == Constants.Transit && serviceId == "2" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.DomesticToDomestic && travelSectorId == "6" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "Domestic to Domestic",
                                                    OriginAirport = "DEL",
                                                    DestinationAirport = "TRV",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.DomesticToInternational && travelSectorId == "1" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "Domestic to International",
                                                    OriginAirport = "TRV",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.InternationalToDomestic && travelSectorId == "5" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "International to Domestic",
                                                    OriginAirport = "DXB",
                                                    DestinationAirport = "TRV",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.InternationalToInternational && travelSectorId == "2" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "2",
                                                    ServiceType = "Transit",
                                                    TravelSector = "International to International",
                                                    OriginAirport = "DXB",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for Transit, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }

                                if (serviceName == Constants.RoundTrip && serviceId == "3" && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == Constants.Domestic && travelSectorId == "3" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "3",
                                                    ServiceType = "round trip",
                                                    TravelSector = "Domestic",
                                                    OriginAirport = "TRV",
                                                    DestinationAirport = "DEL",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else if (travelSectorName == Constants.International && travelSectorId == "4" && travelSectorActiveStatus == "1")
                                            {
                                                string ServiceTime = DateTime.Now.AddDays(5).ToString("dd-MM-yyyy");
                                                Root _obj = new Root
                                                {
                                                     
                                                    ServiceTypeId = "3",
                                                    ServiceType = "round trip",
                                                    TravelSector = "International",
                                                    OriginAirport = "TRV",
                                                    DestinationAirport = "DXB",
                                                    AdultCount = 1,
                                                    ChildCount = 1,
                                                    InfantCount = 1,
                                                    ServiceTime = "21:40:00",
                                                    ServiceDate = ServiceTime
                                                };
                                                var response = aPIResponse.GetAPIResponse("POST", Sitecore.Configuration.Settings.GetSetting("PranaamPackages"), CreateRequestHeaders(), null, _obj);
                                                SchedulerPackages aPIresult = new SchedulerPackages();
                                                aPIresult = JsonConvert.DeserializeObject<SchedulerPackages>(response);
                                                if (aPIresult.data != null)
                                                {
                                                    CreatePackage(aPIresult.data.packageDetails);
                                                    CreateCityPackage(aPIresult.data.packageDetails, serviceName, travelSectorName, cityCode);
                                                }
                                                else
                                                {
                                                    _logRepository.Info("Null API response, City ->" + cityCode.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _logRepository.Info("No Package for RoundTrip, City ->" + cityCode.ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }                    
                    else
                    {
                        _logRepository.Info("No Airport available");
                    }                   
                }
            }
        }
        public Dictionary<string, string> CreateRequestHeaders()
        {
            Random _random = new Random();
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Accept", "application/json");
            headers.Add("agentId", "86b17a29-860a-488f-a163-03beb3b5d778");
            headers.Add("channelId", "test");
            headers.Add("traceId", "d9a3d255-491b-40bd-899c-421631ba9f33");
            return headers;
        }
        private void CreatePackage(List<PackageDetail> jsonObject)
        {
            Database contextDB = Sitecore.Context.ContentDatabase;
            _logRepository.Info("Gets the context database->" + contextDB.ToString());
            Item PranaamAirportsDS = contextDB.GetItem(Constants.PranaamAirportDS);
            Item PranaamPackagesFolderDS = contextDB.GetItem(Constants.PranaamPackagesFolder);
            Item PranaamPackagesDS = contextDB.GetItem(Constants.PranaamPackages);
            _logRepository.Info("Pranaam Gets the Parent Item under which product need to be created");
            var packageTemplate = contextDB.GetTemplate(Constants.PranaamPackageCard);

            try
            {
                _logRepository.Info("Checks for the null of Parent folder and the template");
                if (PranaamAirportsDS != null && packageTemplate != null && PranaamPackagesFolderDS != null)
                {
                    _logRepository.Info("Checks passed for the Parent folder and the template");
                    using (new Sitecore.SecurityModel.SecurityDisabler())
                    {
                        _logRepository.Info("Disabeling Sitecore Security for content creation");
                        foreach (var product in jsonObject)
                        {
                            ApiItem.Add(product.packageId.ToString());
                            Item pranaamPackageItem = null;
                            bool isExist = false;
                            try
                            {
                                if (PranaamPackagesDS != null)
                                {
                                    _logRepository.Info("Checks if the item already exists");                                  
                                    pranaamPackageItem = GetExistingItemBasedOnLanguage("/sitecore/content/AirportHome/Datasource/Adani/PranaamFolder/PranaamPackagesFolder/PranaamPackages/" + product.packageId);
                                    
                                    if (pranaamPackageItem == null)
                                    {
                                        pranaamPackageItem = PranaamPackagesDS.Add(product.packageId.ToString(), packageTemplate);
                                    }
                                    else
                                    {
                                        isExist = true;
                                    }
                                    _logRepository.Info("Item Id of the item to be added");
                                    try
                                    {
                                        if (pranaamPackageItem != null)
                                        {
                                            if (isExist)
                                            {
                                                pranaamPackageItem = pranaamPackageItem.Versions.AddVersion();
                                            }
                                            pranaamPackageItem.Editing.BeginEdit();

                                            pranaamPackageItem.Appearance.DisplayName = product.name + "_" + product.packageId;
                                            pranaamPackageItem.Fields[Templates.PranaamPackages.Fields.Id].Value = !string.IsNullOrEmpty(product.packageId.ToString()) ? product.packageId.ToString() : string.Empty;                                            
                                            pranaamPackageItem[Templates.PranaamPackages.Fields.Title] = !string.IsNullOrEmpty(product.name) ? product.name : String.Empty;                                            
                                            pranaamPackageItem.Fields[Templates.PranaamPackages.Fields.Description].Value = !string.IsNullOrEmpty(product.shortDesc) ? product.shortDesc : string.Empty;                                            
                                            pranaamPackageItem.Fields[Templates.PranaamPackages.Fields.NewPrice].Value = !string.IsNullOrEmpty(product.priceToPay.ToString()) ? product.priceToPay.ToString() : string.Empty;

                                            if (product.name == "Elite")
                                            {
                                                pranaamPackageItem.Fields[Templates.PranaamPackages.Fields.IsRecommended].Value = "1";
                                            }

                                            if (product.packageService != null && product.packageService.Count > 0)
                                            {
                                                List<string> listOfjsonIds = new List<string>();
                                                foreach (PackageService packageservice in product.packageService)
                                                {
                                                    Item serviceItem = GetExistingItem("/sitecore/content/AirportHome/Datasource/Adani/PranaamFolder/PackageServicesFolder/" + Sanitize(packageservice.addOnServiceName));
                                                    serviceItem = (serviceItem == null) ? CreateServices(ref contextDB, packageservice, product) : serviceItem;
                                                    listOfjsonIds.Add(packageservice.addOnServiceId.ToString());
                                                }
                                                
                                                MultilistField offerField = new MultilistField(pranaamPackageItem.Fields[Templates.PranaamPackages.Fields.ServicesList]);
                                                foreach(Sitecore.Data.Items.Item item in ((Sitecore.Data.Fields.MultilistField)pranaamPackageItem.Fields[Templates.PranaamPackages.Fields.ServicesList]).GetItems())
                                                {
                                                    if (!(listOfjsonIds.Contains(item.Fields[Templates.PackageServicesData.Fields.Value].Value.ToString())))
                                                    {
                                                        offerField.Remove(item.ID.ToString());
                                                    }
                                                }
                                            }

                                            if (product.packageAddOn != null && product.packageAddOn.Count > 0)
                                            {
                                                List<string> listOfaddOnIds = new List<string>();
                                                foreach (PackageAddOn packageaddOns in product.packageAddOn)
                                                {
                                                    Item serviceItem = GetExistingItem("/sitecore/content/AirportHome/Datasource/Adani/PranaamFolder/AddOnServicesFolder/ListOfServicesFolder/" + Sanitize(packageaddOns.addOnServiceName));
                                                    serviceItem = (serviceItem == null) ? CreateAddOns(ref contextDB, packageaddOns, product) : serviceItem;
                                                    listOfaddOnIds.Add(packageaddOns.addOnServiceId.ToString());
                                                }

                                                MultilistField addOnField = new MultilistField(pranaamPackageItem.Fields[Templates.PranaamPackages.Fields.AddOns]);
                                                foreach (Sitecore.Data.Items.Item item in ((Sitecore.Data.Fields.MultilistField)pranaamPackageItem.Fields[Templates.PranaamPackages.Fields.AddOns]).GetItems())
                                                {
                                                    if (!(listOfaddOnIds.Contains(item.Fields[Templates.PackageServicesData.Fields.Value].Value.ToString())))
                                                    {
                                                        addOnField.Remove(item.ID.ToString());
                                                    }
                                                }
                                            }
                                            
                                            pranaamPackageItem.Editing.EndEdit();
                                            _logRepository.Info("Packages Imported Successfully");
                                            PublishProduct(pranaamPackageItem);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logRepository.Error("Item creation failed due to -> " + ex.Message);
                                        pranaamPackageItem.Editing.CancelEdit();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logRepository.Error("Item creation failed due to -> " + ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error("Pranaam CreateProduct method failed due to -> " + ex.Message);
            }
            DeleteItems();
        }
        private void CreateCityPackage(List<PackageDetail> jsonObject, string ServiceName, string TravelSectorName, string CityCode)
        {
            Database contextDB = Sitecore.Context.ContentDatabase;
            //_logRepository.Info("Gets the context database->" + contextDB.ToString());
            Item parentItem = contextDB.GetItem(Constants.PranaamAirportDS);
            Item datasource = null;
            var packageTemplate = contextDB.GetTemplate(Constants.PranaamPackageCard);
            string ServiceID = string.Empty;
            string TravelSectorID = string.Empty;

            /// <summary>
            /// Datasource
            /// </summary>
            #region

            /// <summary>
            /// Ahmedabad Datasource
            /// </summary>
            #region
            if (CityCode.ToLower() == Constants.Ahmedabad.ToLower() && ServiceName.ToLower() == Constants.Arrival.ToLower() && TravelSectorName.ToLower() == Constants.Domestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.AMDArrivalDomestic);
            }              
            else if(CityCode.ToLower() == Constants.Ahmedabad.ToLower() && ServiceName.ToLower() == Constants.Arrival.ToLower() && TravelSectorName.ToLower() == Constants.International.ToLower())
            {
                datasource = contextDB.GetItem(Constants.AMDArrivalInternational);
            }
            else if (CityCode.ToLower() == Constants.Ahmedabad.ToLower() && ServiceName.ToLower() == Constants.Departure.ToLower() && TravelSectorName.ToLower() == Constants.Domestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.AMDDepartureDomestic);
            }
            else if (CityCode.ToLower() == Constants.Ahmedabad.ToLower() && ServiceName.ToLower() == Constants.Departure.ToLower() && TravelSectorName.ToLower() == Constants.International.ToLower())
            {
                datasource = contextDB.GetItem(Constants.AMDDepartureInternational);
            }
            else if (CityCode.ToLower() == Constants.Ahmedabad.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.DomesticToDomestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.AMDTransitDD);
            }
            else if (CityCode.ToLower() == Constants.Ahmedabad.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.DomesticToInternational.ToLower())
            {
                datasource = contextDB.GetItem(Constants.AMDTransitDI);
            }
            else if (CityCode.ToLower() == Constants.Ahmedabad.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.InternationalToDomestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.AMDTransitID);
            }
            else if (CityCode.ToLower() == Constants.Ahmedabad.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.InternationalToInternational.ToLower())
            {
                datasource = contextDB.GetItem(Constants.AMDTransitII);
            }
            else if (CityCode.ToLower() == Constants.Ahmedabad.ToLower() && ServiceName.ToLower() == Constants.RoundTrip.ToLower() && TravelSectorName.ToLower() == Constants.International.ToLower())
            {
                datasource = contextDB.GetItem(Constants.AMDRoundTripInternational);
            }
            else if (CityCode.ToLower() == Constants.Ahmedabad.ToLower() && ServiceName.ToLower() == Constants.RoundTrip.ToLower() && TravelSectorName.ToLower() == Constants.Domestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.AMDRoundTripDomestic);
            }
            #endregion

            /// <summary>
            /// Lucknow Datasource
            /// </summary>
            #region
            else if (CityCode.ToLower() == Constants.Lucknow.ToLower() && ServiceName.ToLower() == Constants.Arrival.ToLower() && TravelSectorName.ToLower() == Constants.Domestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.LKOArrivalDomestic);
            }
            else if (CityCode.ToLower() == Constants.Lucknow.ToLower() && ServiceName.ToLower() == Constants.Arrival.ToLower() && TravelSectorName.ToLower() == Constants.International.ToLower())
            {
                datasource = contextDB.GetItem(Constants.LKOArrivalInternational);
            }
            else if (CityCode.ToLower() == Constants.Lucknow.ToLower() && ServiceName.ToLower() == Constants.Departure.ToLower() && TravelSectorName.ToLower() == Constants.Domestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.LKODepartureDomestic);
            }
            else if (CityCode.ToLower() == Constants.Lucknow.ToLower() && ServiceName.ToLower() == Constants.Departure.ToLower() && TravelSectorName.ToLower() == Constants.International.ToLower())
            {
                datasource = contextDB.GetItem(Constants.LKODepartureInternational);
            }
            else if (CityCode.ToLower() == Constants.Lucknow.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.DomesticToDomestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.LKOTransitDD);
            }
            else if (CityCode.ToLower() == Constants.Lucknow.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.DomesticToInternational.ToLower())
            {
                datasource = contextDB.GetItem(Constants.LKOTransitDI);
            }
            else if (CityCode.ToLower() == Constants.Lucknow.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.InternationalToDomestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.LKOTransitID);
            }
            else if (CityCode.ToLower() == Constants.Lucknow.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.InternationalToInternational.ToLower())
            {
                datasource = contextDB.GetItem(Constants.LKOTransitII);
            }
            else if (CityCode.ToLower() == Constants.Lucknow.ToLower() && ServiceName.ToLower() == Constants.RoundTrip.ToLower() && TravelSectorName.ToLower() == Constants.International.ToLower())
            {
                datasource = contextDB.GetItem(Constants.LKORoundTripInternational);
            }
            else if (CityCode.ToLower() == Constants.Lucknow.ToLower() && ServiceName.ToLower() == Constants.RoundTrip.ToLower() && TravelSectorName.ToLower() == Constants.Domestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.LKORoundTripDomestic);
            }
            #endregion

            /// <summary>
            /// Guwahati Datasource
            /// </summary>
            #region
            else if (CityCode.ToLower() == Constants.Guwahati.ToLower() && ServiceName.ToLower() == Constants.Arrival.ToLower() && TravelSectorName.ToLower() == Constants.Domestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.GAUArrivalDomestic);
            }
            else if (CityCode.ToLower() == Constants.Guwahati.ToLower() && ServiceName.ToLower() == Constants.Arrival.ToLower() && TravelSectorName.ToLower() == Constants.International.ToLower())
            {
                datasource = contextDB.GetItem(Constants.GAUArrivalInternational);
            }
            else if (CityCode.ToLower() == Constants.Guwahati.ToLower() && ServiceName.ToLower() == Constants.Departure.ToLower() && TravelSectorName.ToLower() == Constants.Domestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.GAUDepartureDomestic);
            }
            else if (CityCode.ToLower() == Constants.Guwahati.ToLower() && ServiceName.ToLower() == Constants.Departure.ToLower() && TravelSectorName.ToLower() == Constants.International.ToLower())
            {
                datasource = contextDB.GetItem(Constants.GAUDepartureInternational);
            }
            else if (CityCode.ToLower() == Constants.Guwahati.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.DomesticToDomestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.GAUTransitDD);
            }
            else if (CityCode.ToLower() == Constants.Guwahati.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.DomesticToInternational.ToLower())
            {
                datasource = contextDB.GetItem(Constants.GAUTransitDI);
            }
            else if (CityCode.ToLower() == Constants.Guwahati.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.InternationalToDomestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.GAUTransitID);
            }
            else if (CityCode.ToLower() == Constants.Guwahati.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.InternationalToInternational.ToLower())
            {
                datasource = contextDB.GetItem(Constants.GAUTransitII);
            }
            else if (CityCode.ToLower() == Constants.Guwahati.ToLower() && ServiceName.ToLower() == Constants.RoundTrip.ToLower() && TravelSectorName.ToLower() == Constants.International.ToLower())
            {
                datasource = contextDB.GetItem(Constants.GAURoundTripInternational);
            }
            else if (CityCode.ToLower() == Constants.Guwahati.ToLower() && ServiceName.ToLower() == Constants.RoundTrip.ToLower() && TravelSectorName.ToLower() == Constants.Domestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.GAURoundTripDomestic);
            }
            #endregion

            /// <summary>
            /// Jaipur Datasource
            /// </summary>
            #region
            else if (CityCode.ToLower() == Constants.Jaipur.ToLower() && ServiceName.ToLower() == Constants.Arrival.ToLower() && TravelSectorName.ToLower() == Constants.Domestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.JAIArrivalDomestic);
            }
            else if (CityCode.ToLower() == Constants.Jaipur.ToLower() && ServiceName.ToLower() == Constants.Arrival.ToLower() && TravelSectorName.ToLower() == Constants.International.ToLower())
            {
                datasource = contextDB.GetItem(Constants.JAIArrivalInternational);
            }
            else if (CityCode.ToLower() == Constants.Jaipur.ToLower() && ServiceName.ToLower() == Constants.Departure.ToLower() && TravelSectorName.ToLower() == Constants.Domestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.JAIDepartureDomestic);
            }
            else if (CityCode.ToLower() == Constants.Jaipur.ToLower() && ServiceName.ToLower() == Constants.Departure.ToLower() && TravelSectorName.ToLower() == Constants.International.ToLower())
            {
                datasource = contextDB.GetItem(Constants.JAIDepartureInternational);
            }
            else if (CityCode.ToLower() == Constants.Jaipur.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.DomesticToDomestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.JAITransitDD);
            }
            else if (CityCode.ToLower() == Constants.Jaipur.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.DomesticToInternational.ToLower())
            {
                datasource = contextDB.GetItem(Constants.JAITransitDI);
            }
            else if (CityCode.ToLower() == Constants.Jaipur.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.InternationalToDomestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.JAITransitID);
            }
            else if (CityCode.ToLower() == Constants.Jaipur.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.InternationalToInternational.ToLower())
            {
                datasource = contextDB.GetItem(Constants.JAITransitII);
            }
            else if (CityCode.ToLower() == Constants.Jaipur.ToLower() && ServiceName.ToLower() == Constants.RoundTrip.ToLower() && TravelSectorName.ToLower() == Constants.International.ToLower())
            {
                datasource = contextDB.GetItem(Constants.JAIRoundTripInternational);
            }
            else if (CityCode.ToLower() == Constants.Jaipur.ToLower() && ServiceName.ToLower() == Constants.RoundTrip.ToLower() && TravelSectorName.ToLower() == Constants.Domestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.JAIRoundTripDomestic);
            }
            #endregion

            /// <summary>
            /// Mangaluru Datasource
            /// </summary>
            #region
            else if (CityCode.ToLower() == Constants.Mangalore.ToLower() && ServiceName.ToLower() == Constants.Arrival.ToLower() && TravelSectorName.ToLower() == Constants.Domestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.IXEArrivalDomestic);
            }
            else if (CityCode.ToLower() == Constants.Mangalore.ToLower() && ServiceName.ToLower() == Constants.Arrival.ToLower() && TravelSectorName.ToLower() == Constants.International.ToLower())
            {
                datasource = contextDB.GetItem(Constants.IXEArrivalInternational);
            }
            else if (CityCode.ToLower() == Constants.Mangalore.ToLower() && ServiceName.ToLower() == Constants.Departure.ToLower() && TravelSectorName.ToLower() == Constants.Domestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.IXEDepartureDomestic);
            }
            else if (CityCode.ToLower() == Constants.Mangalore.ToLower() && ServiceName.ToLower() == Constants.Departure.ToLower() && TravelSectorName.ToLower() == Constants.International.ToLower())
            {
                datasource = contextDB.GetItem(Constants.IXEDepartureInternational);
            }
            else if (CityCode.ToLower() == Constants.Mangalore.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.DomesticToDomestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.IXETransitDD);
            }
            else if (CityCode.ToLower() == Constants.Mangalore.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.DomesticToInternational.ToLower())
            {
                datasource = contextDB.GetItem(Constants.IXETransitDI);
            }
            else if (CityCode.ToLower() == Constants.Mangalore.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.InternationalToDomestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.IXETransitID);
            }
            else if (CityCode.ToLower() == Constants.Mangalore.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.InternationalToInternational.ToLower())
            {
                datasource = contextDB.GetItem(Constants.IXETransitII);
            }
            else if (CityCode.ToLower() == Constants.Mangalore.ToLower() && ServiceName.ToLower() == Constants.RoundTrip.ToLower() && TravelSectorName.ToLower() == Constants.International.ToLower())
            {
                datasource = contextDB.GetItem(Constants.IXERoundTripInternational);
            }
            else if (CityCode.ToLower() == Constants.Mangalore.ToLower() && ServiceName.ToLower() == Constants.RoundTrip.ToLower() && TravelSectorName.ToLower() == Constants.Domestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.IXERoundTripDomestic);
            }
            #endregion

            /// <summary>
            /// Trivandrum Datasource
            /// </summary>
            #region
            else if (CityCode.ToLower() == Constants.Trivandrum.ToLower() && ServiceName.ToLower() == Constants.Arrival.ToLower() && TravelSectorName.ToLower() == Constants.Domestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.TRVArrivalDomestic);
            }
            else if (CityCode.ToLower() == Constants.Trivandrum.ToLower() && ServiceName.ToLower() == Constants.Arrival.ToLower() && TravelSectorName.ToLower() == Constants.International.ToLower())
            {
                datasource = contextDB.GetItem(Constants.TRVArrivalInternational);
            }
            else if (CityCode.ToLower() == Constants.Trivandrum.ToLower() && ServiceName.ToLower() == Constants.Departure.ToLower() && TravelSectorName.ToLower() == Constants.Domestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.TRVDepartureDomestic);
            }
            else if (CityCode.ToLower() == Constants.Trivandrum.ToLower() && ServiceName.ToLower() == Constants.Departure.ToLower() && TravelSectorName.ToLower() == Constants.International.ToLower())
            {
                datasource = contextDB.GetItem(Constants.TRVDepartureInternational);
            }
            else if (CityCode.ToLower() == Constants.Trivandrum.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.DomesticToDomestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.TRVTransitDD);
            }
            else if (CityCode.ToLower() == Constants.Trivandrum.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.DomesticToInternational.ToLower())
            {
                datasource = contextDB.GetItem(Constants.TRVTransitDI);
            }
            else if (CityCode.ToLower() == Constants.Trivandrum.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.InternationalToDomestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.TRVTransitID);
            }
            else if (CityCode.ToLower() == Constants.Trivandrum.ToLower() && ServiceName.ToLower() == Constants.Transit.ToLower() && TravelSectorName.ToLower() == Constants.InternationalToInternational.ToLower())
            {
                datasource = contextDB.GetItem(Constants.TRVTransitII);
            }
            else if (CityCode.ToLower() == Constants.Trivandrum.ToLower() && ServiceName.ToLower() == Constants.RoundTrip.ToLower() && TravelSectorName.ToLower() == Constants.International.ToLower())
            {
                datasource = contextDB.GetItem(Constants.TRVRoundTripInternational);
            }
            else if (CityCode.ToLower() == Constants.Trivandrum.ToLower() && ServiceName.ToLower() == Constants.RoundTrip.ToLower() && TravelSectorName.ToLower() == Constants.Domestic.ToLower())
            {
                datasource = contextDB.GetItem(Constants.TRVRoundTripDomestic);
            }
            #endregion

            else
            {
                _logRepository.Info("Invalid City Code");
            }
            #endregion

            /// <summary>
            /// Service ID
            /// </summary>
            #region
            if (ServiceName != null)
            {
                if (ServiceName.ToLower() == Constants.Arrival.ToLower())
                {
                    ServiceID = Constants.ArrivalId;
                }
                else if (ServiceName.ToLower() == Constants.Departure.ToLower())
                {
                    ServiceID = Constants.DepartureId;
                }
                else if (ServiceName.ToLower() == Constants.Transit.ToLower())
                {
                    ServiceID = Constants.TransitId;
                }
                else if (ServiceName.ToLower() == Constants.RoundTrip.ToLower())
                {
                    ServiceID = Constants.RoundTripId;
                }
                else
                {
                    _logRepository.Info("Invalid Service Name");
                }
            }
            #endregion

            /// <summary>
            /// Travel Sector ID
            /// </summary> 
            #region
            if (TravelSectorName != null)
            {
                if (TravelSectorName.ToLower() == Constants.Domestic.ToLower())
                {
                    TravelSectorID = Constants.DomesticId;
                }
                else if (TravelSectorName.ToLower() == Constants.International.ToLower())
                {
                    TravelSectorID = Constants.InternationalId;
                }
                else if (TravelSectorName.ToLower() == Constants.DomesticToInternational.ToLower())
                {
                    TravelSectorID = Constants.DomesticToInternationalId;
                }
                else if (TravelSectorName.ToLower() == Constants.InternationalToInternational.ToLower())
                {
                    TravelSectorID = Constants.InternationalToInternationalId;
                }
                else if (TravelSectorName.ToLower() == Constants.InternationalToDomestic.ToLower())
                {
                    TravelSectorID = Constants.InternationalToDomesticId;
                }
                else if (TravelSectorName.ToLower() == Constants.DomesticToDomestic.ToLower())
                {
                    TravelSectorID = Constants.DomesticToDomesticId;
                }
                else
                {
                    _logRepository.Info("Invalid TravelSectorName");
                }
            }
            #endregion

            if (parentItem.Children.Count > 0 && parentItem.Children != null)
            {
                foreach (Item cityAirport in parentItem.Children)
                {
                    var cityCode = !string.IsNullOrEmpty(cityAirport.Fields[Constants.AirportCode].Value) ? cityAirport.Fields[Constants.AirportCode].Value : string.Empty;
                    if (cityCode == CityCode)
                    {
                        if (cityAirport.Children.Count > 0 && cityAirport.Children != null)
                        {
                            foreach (Item serviceType in cityAirport.Children)
                            {
                                var serviceName = !string.IsNullOrEmpty(serviceType.Fields[Constants.ServiceName].Value) ? serviceType.Fields[Constants.ServiceName].Value : string.Empty;
                                var serviceId = !string.IsNullOrEmpty(serviceType.Fields[Constants.ServiceId].Value) ? serviceType.Fields[Constants.ServiceId].Value : string.Empty;
                                var serviceActiveStatus = !string.IsNullOrEmpty(serviceType.Fields[Constants.IsActive].Value) ? serviceType.Fields[Constants.IsActive].Value : string.Empty;
                                if (serviceName.ToLower() == ServiceName.ToLower() && serviceId == ServiceID && serviceActiveStatus == "1")
                                {
                                    if (serviceType.Children.Count > 0 && serviceType.Children != null)
                                    {
                                        foreach (Item travelSector in serviceType.Children)
                                        {
                                            var travelSectorName = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorName].Value) ? travelSector.Fields[Constants.TravelSectorName].Value : string.Empty;
                                            var travelSectorId = !string.IsNullOrEmpty(travelSector.Fields[Constants.TravelSectorId].Value) ? travelSector.Fields[Constants.TravelSectorId].Value : string.Empty;
                                            var travelSectorActiveStatus = !string.IsNullOrEmpty(travelSector.Fields[Constants.IsActive].Value) ? travelSector.Fields[Constants.IsActive].Value : string.Empty;
                                            if (travelSectorName == TravelSectorName && travelSectorId == TravelSectorID && travelSectorActiveStatus == "1")
                                            {
                                                List<string> CityApiItem = new List<string>();
                                                using (new Sitecore.SecurityModel.SecurityDisabler())
                                                {
                                                    _logRepository.Info("Disabeling Sitecore Security for content creation");
                                                    
                                                    foreach (var product in jsonObject)
                                                    {
                                                        CityApiItem.Add(product.packageId.ToString());
                                                        Item PackageItem = null;
                                                        bool isExist = false;
                                                        try
                                                        {
                                                            if (datasource != null)
                                                            {
                                                                _logRepository.Info("Checks if the item already exists");
                                                                PackageItem = GetExistingItemBasedOnLanguage("/sitecore/content/AirportHome/Datasource/Adani/PranaamFolder/PranaamAirports/" + cityAirport.Name + "/" + serviceName + "/" + travelSectorName + "/" + product.packageId);
                                                                if (PackageItem == null)
                                                                {
                                                                    PackageItem = datasource.Add(product.packageId.ToString(), packageTemplate);
                                                                }
                                                                else
                                                                {
                                                                    isExist = true;
                                                                }
                                                                _logRepository.Info("Item Id of the item to be added");
                                                                try
                                                                {
                                                                    if (PackageItem != null)
                                                                    {
                                                                        if (isExist)
                                                                        {
                                                                            PackageItem = PackageItem.Versions.AddVersion();
                                                                        }
                                                                        PackageItem.Editing.BeginEdit();

                                                                        PackageItem.Appearance.DisplayName = product.name + "_" + product.packageId;
                                                                        PackageItem.Fields[Templates.PranaamPackages.Fields.Id].Value = !string.IsNullOrEmpty(product.packageId.ToString()) ? product.packageId.ToString() : string.Empty;
                                                                        PackageItem[Templates.PranaamPackages.Fields.Title] = !string.IsNullOrEmpty(product.name) ? product.name : String.Empty;
                                                                        PackageItem.Fields[Templates.PranaamPackages.Fields.Description].Value = !string.IsNullOrEmpty(product.shortDesc) ? product.shortDesc : string.Empty;
                                                                        PackageItem.Fields[Templates.PranaamPackages.Fields.NewPrice].Value = !string.IsNullOrEmpty(product.priceToPay.ToString()) ? product.priceToPay.ToString() : string.Empty;
                                                                        if (product.name == "Elite")
                                                                        {
                                                                            PackageItem.Fields[Templates.PranaamPackages.Fields.IsRecommended].Value = "1";
                                                                        }
                                                                        
                                                                        if (product.packageService != null && product.packageService.Count > 0)
                                                                        {
                                                                            List<string> listOfjsonIds = new List<string>();
                                                                            foreach (PackageService packageservice in product.packageService)
                                                                            {
                                                                                Item serviceItem = GetExistingItem("/sitecore/content/AirportHome/Datasource/Adani/PranaamFolder/PackageServicesFolder/" + Sanitize(packageservice.addOnServiceName));
                                                                                serviceItem = (serviceItem == null) ? CreateServices(ref contextDB, packageservice, product) : serviceItem;
                                                                                listOfjsonIds.Add(packageservice.addOnServiceId.ToString());
                                                                            }
                                                                            MultilistField packageField = new MultilistField(PackageItem.Fields[Templates.PranaamPackages.Fields.ServicesList]);
                                                                            foreach (Sitecore.Data.Items.Item item in ((Sitecore.Data.Fields.MultilistField)PackageItem.Fields[Templates.PranaamPackages.Fields.ServicesList]).GetItems())
                                                                            {
                                                                                if (!(listOfjsonIds.Contains(item.Fields[Templates.PackageServicesData.Fields.Value].Value.ToString())))
                                                                                {
                                                                                    packageField.Remove(item.ID.ToString());
                                                                                }
                                                                            }
                                                                        }

                                                                        if (product.packageAddOn != null && product.packageAddOn.Count > 0)
                                                                        {
                                                                            List<string> listOfaddOnIds = new List<string>();
                                                                            foreach (PackageAddOn packageaddOns in product.packageAddOn)
                                                                            {
                                                                                Item serviceItem = GetExistingItem("/sitecore/content/AirportHome/Datasource/Adani/PranaamFolder/AddOnServicesFolder/ListOfServicesFolder/" + Sanitize(packageaddOns.addOnServiceName));
                                                                                serviceItem = (serviceItem == null) ? CreateAddOns(ref contextDB, packageaddOns, product) : serviceItem;
                                                                                listOfaddOnIds.Add(packageaddOns.addOnServiceId.ToString());
                                                                            }

                                                                            MultilistField addOnField = new MultilistField(PackageItem.Fields[Templates.PranaamPackages.Fields.AddOns]);
                                                                            foreach (Sitecore.Data.Items.Item item in ((Sitecore.Data.Fields.MultilistField)PackageItem.Fields[Templates.PranaamPackages.Fields.AddOns]).GetItems())
                                                                            {
                                                                                if (!(listOfaddOnIds.Contains(item.Fields[Templates.PackageServicesData.Fields.Value].Value.ToString())))
                                                                                {
                                                                                    addOnField.Remove(item.ID.ToString());
                                                                                }
                                                                            }
                                                                        }

                                                                        PackageItem.Editing.EndEdit();
                                                                        _logRepository.Info("Packages Imported Successfully");
                                                                        PublishProduct(PackageItem);
                                                                    }
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    _logRepository.Error("Item creation failed due to -> " + ex.Message);
                                                                    PackageItem.Editing.CancelEdit();
                                                                }
                                                            }

                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            _logRepository.Error("Item creation failed due to -> " + ex.Message);
                                                        }

                                                        List<Item> cityPackage = travelSector.GetChildren().ToList();
                                                        foreach (Item item2 in cityPackage)
                                                        {
                                                            if (!(CityApiItem.Contains(item2.Name)))
                                                            {
                                                                item2.Delete();
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
                }
            }
        }
        private void DeleteItems()
        {
            Database contextDB = Sitecore.Context.ContentDatabase;
            Item PranaamPackagesDS = contextDB.GetItem(Constants.PranaamPackages);
            IEnumerable<Item> PackageItems = PranaamPackagesDS.GetChildren();
            //Item pranaamPackageItem = null;
            foreach (var item in PackageItems)
            {
                if (!(ApiItem.Contains(item.Name)))
                {
                    item.Delete();
                }
            }
        }
        private string Sanitize(string name)
        {
            return Regex.Replace(name, "[^0-9A-Za-z ]+", "");           
        }
        private Sitecore.Data.Items.Item GetExistingItemBasedOnLanguage(string ItemPath)
        {
            return Sitecore.Context.ContentDatabase.GetItem(ItemPath, Sitecore.Context.Language);
        }
        private Sitecore.Data.Items.Item GetExistingItem(string ItemPath)
        {
            return Sitecore.Context.ContentDatabase.GetItem(ItemPath);
        }
        internal Item CreateServices(ref Database contextDB, PackageService service, PackageDetail product)
        {
            Item ServiceItem = null;

            Item ServiceParentItem = contextDB.GetItem(Constants.PackageServicesParent);
            var ServicesTemplate = contextDB.GetTemplate(Constants.PackageServicesTemplate);  
            try
            {
                if (ServiceParentItem != null && ServicesTemplate != null)
                {
                    ServiceItem = ServiceParentItem.Add(Sanitize(service.addOnServiceName), ServicesTemplate);
                    using (new EditContext(ServiceItem))
                    {
                        ServiceItem[Templates.PackageServicesData.Fields.OfferingsName] = !string.IsNullOrEmpty(service.addOnServiceName) ? service.addOnServiceName : string.Empty;
                        ServiceItem[Templates.PackageServicesData.Fields.OfferingsDescription] = !string.IsNullOrEmpty(service.addOnServiceDescription) ? service.addOnServiceDescription : string.Empty;
                        ServiceItem[Templates.PackageServicesData.Fields.Value] = !string.IsNullOrEmpty(service.addOnServiceId.ToString()) ? service.addOnServiceId.ToString() : string.Empty;                        
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error("Create Package Service method failed due to -> " + ex.Message);
                _logRepository.Error("Service name error -> " + service.addOnServiceName);

            }

            return ServiceItem;
        }

        internal Item CreateAddOns(ref Database contextDB, PackageAddOn addons, PackageDetail product)
        {
            Item AddOnItem = null;

            Item AddOnsParentItem = contextDB.GetItem(Constants.PackageAddOnsParent);
            var AddOnsTemplate = contextDB.GetTemplate(Constants.PackageAddOnsTemplate);
            try
            {
                if (AddOnsParentItem != null && AddOnsTemplate != null)
                {
                    AddOnItem = AddOnsParentItem.Add(Sanitize(addons.addOnServiceName), AddOnsTemplate);
                    using (new EditContext(AddOnItem))
                    {
                        AddOnItem[Templates.AddOnService.Fields.Title] = !string.IsNullOrEmpty(addons.addOnServiceName) ? addons.addOnServiceName : string.Empty;
                        AddOnItem[Templates.AddOnService.Fields.Description] = !string.IsNullOrEmpty(addons.addOnServiceDescription) ? addons.addOnServiceDescription : string.Empty;
                        AddOnItem[Templates.AddOnService.Fields.Id] = !string.IsNullOrEmpty(addons.addOnServiceId.ToString()) ? addons.addOnServiceId.ToString() : string.Empty;
                        AddOnItem[Templates.AddOnService.Fields.NewPrice] = !string.IsNullOrEmpty(addons.price.ToString()) ? addons.price.ToString() : string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error("Create Package Service method failed due to -> " + ex.Message);
                _logRepository.Error("Service name error -> " + addons.addOnServiceName);

            }

            return AddOnItem;
        }
        private void PublishProduct(Sitecore.Data.Items.Item item)
        {
            _logRepository.Info("Package Publish Started ");
            try
            {
                Sitecore.Data.Database master = Sitecore.Configuration.Factory.GetDatabase("master");
                _logRepository.Info("Gets the master/Source database ");
                Sitecore.Data.Database web = Sitecore.Configuration.Factory.GetDatabase("web");
                _logRepository.Info("Gets the Web/Target database ");
                Sitecore.Publishing.PublishOptions publishOptions = new Sitecore.Publishing.PublishOptions(master,
                                        web,
                                        Sitecore.Publishing.PublishMode.SingleItem,
                                        item.Language,
                                        System.DateTime.Now);
                _logRepository.Info("Publish started -> " + System.DateTime.Now.ToString());
                Sitecore.Publishing.Publisher publisher = new Sitecore.Publishing.Publisher(publishOptions);
                _logRepository.Info("Set publish Options -> ");
                publisher.Options.RootItem = item;
                _logRepository.Info("Set Root Item ");
                publisher.Options.Deep = true;

                publisher.Publish();
                _logRepository.Info("Item Published ");
            }
            catch (Exception ex)
            {
                _logRepository.Error("Item Published Failed due to -> " + ex.Message);
            }

        }
        public class Root
        {
            public int AdultCount { get; set; }
            public string ServiceTypeId { get; set; }
            public string ServiceType { get; set; }
            public string TravelSector { get; set; }
            public string OriginAirport { get; set; }
            public string DestinationAirport { get; set; }
            public int ChildCount { get; set; }
            public int InfantCount { get; set; }
            public string ServiceTime { get; set; }
            public string ServiceDate { get; set; }
        }

    }
}