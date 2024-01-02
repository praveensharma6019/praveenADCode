using Adani.SuperApp.Airport.Feature.Master.Platform.Constant;
using Adani.SuperApp.Airport.Feature.Master.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Master.Platform.Services
{
    public class MasterList : IMasterList
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;

        /// <summary>
        /// instanciate logRepository method
        /// </summary>
        /// <param name="logRepository"></param>
        public MasterList(ILogRepository logRepository, IHelper helper)
        {

            this._logRepository = logRepository;
            this._helper = helper;
        }

        /// <summary>
        /// implementation of the inherited class
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public MasterListModel GetMasterList(Item dataSource, string queryString)
        {
            MasterListModel masterListModel = new MasterListModel();

            try
            {
                queryString = queryString.ToLower();
                if (queryString.Contains(Constants.Airports.ToLower()) || queryString.Contains(Constants.all.ToLower()))
                {
                    IEnumerable<Item> ChildItems = dataSource.Children.Where(x => x.Name.ToLower() == Constants.Airports.ToLower()).ToList();
                    List<AirportList> airportListData = new List<AirportList>();
                    if (ChildItems != null && ChildItems.Count() >= 0)
                    {
                        foreach (Item collapsechild in ChildItems)
                        {
                            IEnumerable<Item> Airportchild = collapsechild.Children;
                            foreach (Item child in Airportchild)
                            {
                                if (child != null)
                                {
                                    airportListData.Add(GetAirportList(child));
                                }
                            }
                            masterListModel.Airports = airportListData;
                        }
                    }
                }

                if (queryString.Contains(Constants.PranaamAirports.ToLower()) || queryString.Contains(Constants.all.ToLower()))
                {
                    IEnumerable<Item> ChildItems = dataSource.Children.Where(x => x.Name.ToLower() == Constants.PranaamAirports.ToLower()).ToList();
                    List<PranaamAirportList> airportListData = new List<PranaamAirportList>();
                    if (ChildItems != null && ChildItems.Count() >= 0)
                    {
                        foreach (Item collapsechild in ChildItems)
                        {
                            IEnumerable<Item> Airportchild = collapsechild.Children;
                            foreach (Item child in Airportchild)
                            {
                                if (child != null)
                                {
                                    airportListData.Add(GetPranaamAirportList(child));
                                }
                            }
                            masterListModel.PranaamAirports = airportListData;
                        }
                    }
                }


                if (queryString.Contains(Constants.nationality.ToLower()) || queryString.Contains(Constants.all.ToLower()))
                {
                    IEnumerable<Item> ChildItems = dataSource.Children.Where(x => x.Name.ToLower() == Constants.nationalitymaster.ToLower()).ToList();
                    List<NationalityList> nationalityListData = new List<NationalityList>();
                    if (ChildItems != null && ChildItems.Count() >= 0)
                    {
                        foreach (Item collapsechild in ChildItems)
                        {
                            IEnumerable<Item> Nationalitychild = collapsechild.Children;
                            foreach (Item child in Nationalitychild)
                            {
                                if (child != null)
                                {
                                    nationalityListData.Add(GetNationalityData(child));
                                }
                            }
                            masterListModel.Nationality = nationalityListData;
                        }
                    }
                }

                if (queryString.Contains(Constants.country.ToLower()) || queryString.Contains(Constants.all.ToLower()))
                {
                    IEnumerable<Item> ChildItems = dataSource.Children.Where(x => x.Name.ToLower() == Constants.countrymaster.ToLower()).ToList();
                    List<CountryList> countryListData = new List<CountryList>();
                    if (ChildItems != null && ChildItems.Count() >= 0)
                    {
                        foreach (Item collapsechild in ChildItems)
                        {
                            IEnumerable<Item> countrychild = collapsechild.Children;
                            foreach (Item country in countrychild)
                            {
                                if (country != null)
                                {
                                    countryListData.Add(GetCountryData(country));
                                }
                            }
                            masterListModel.Country = countryListData;
                        }
                    }
                }

                if (queryString.Contains(Constants.bookingstatus.ToLower()) || queryString.Contains(Constants.all.ToLower()))
                {
                    IEnumerable<Item> ChildItems = dataSource.Children.Where(x => x.Name.ToLower() == Constants.bookingstatus.ToLower()).ToList();
                    List<MasterDropdown> MasterDropdownListData = new List<MasterDropdown>();
                    if (ChildItems != null && ChildItems.Count() >= 0)
                    {
                        foreach (Item collapsechild in ChildItems)
                        {
                            IEnumerable<Item> countrychild = collapsechild.Children;
                            foreach (Item item in countrychild)
                            {
                                if (item != null)
                                {
                                    MasterDropdownListData.Add(GetDropDownList(item));
                                }
                            }
                            masterListModel.BookingStatus = MasterDropdownListData;
                        }
                    }
                }

                if (queryString.Contains(Constants.flyingclass.ToLower()) || queryString.Contains(Constants.all.ToLower()))
                {
                    IEnumerable<Item> ChildItems = dataSource.Children.Where(x => x.Name.ToLower() == Constants.flyingclass.ToLower()).ToList();
                    List<MasterDropdown> MasterDropdownListData = new List<MasterDropdown>();
                    if (ChildItems != null && ChildItems.Count() >= 0)
                    {
                        foreach (Item collapsechild in ChildItems)
                        {
                            IEnumerable<Item> countrychild = collapsechild.Children;
                            foreach (Item item in countrychild)
                            {
                                if (item != null)
                                {
                                    MasterDropdownListData.Add(GetDropDownList(item));
                                }
                            }
                            masterListModel.FlyingClass = MasterDropdownListData;
                        }
                    }
                }

                if (queryString.Contains(Constants.passengertype.ToLower()) || queryString.Contains(Constants.all.ToLower()))
                {
                    IEnumerable<Item> ChildItems = dataSource.Children.Where(x => x.Name.ToLower() == Constants.passengertype.ToLower()).ToList();
                    List<MasterDropdown> MasterDropdownListData = new List<MasterDropdown>();
                    if (ChildItems != null && ChildItems.Count() >= 0)
                    {
                        foreach (Item collapsechild in ChildItems)
                        {
                            IEnumerable<Item> countrychild = collapsechild.Children;
                            foreach (Item item in countrychild)
                            {
                                if (item != null)
                                {
                                    MasterDropdownListData.Add(GetDropDownList(item));
                                }
                            }
                            masterListModel.PassengerType = MasterDropdownListData;
                        }
                    }
                }

                if (queryString.Contains(Constants.servicetype.ToLower()) || queryString.Contains(Constants.all.ToLower()))
                {
                    IEnumerable<Item> ChildItems = dataSource.Children.Where(x => x.Name.ToLower() == Constants.servicetype.ToLower()).ToList();
                    List<MasterDropdown> MasterDropdownListData = new List<MasterDropdown>();
                    if (ChildItems != null && ChildItems.Count() >= 0)
                    {
                        foreach (Item collapsechild in ChildItems)
                        {
                            IEnumerable<Item> countrychild = collapsechild.Children;
                            foreach (Item item in countrychild)
                            {
                                if (item != null)
                                {
                                    MasterDropdownListData.Add(GetDropDownList(item));
                                }
                            }
                            masterListModel.ServiceType = MasterDropdownListData;
                        }
                    }
                }

                if (queryString.Contains(Constants.state_master.ToLower()) || queryString.Contains(Constants.all.ToLower()))
                {
                    IEnumerable<Item> ChildItems = dataSource.Children.Where(x => x.Name.ToLower() == Constants.state_master.ToLower()).ToList();
                    List<State> MasterDropdownListData = new List<State>();
                    if (ChildItems != null && ChildItems.Count() >= 0)
                    {
                        foreach (Item collapsechild in ChildItems)
                        {
                            IEnumerable<Item> countrychild = collapsechild.Children;
                            foreach (Item item in countrychild)
                            {
                                if (item != null)
                                {
                                    MasterDropdownListData.Add(GetState(item));
                                }
                            }
                            masterListModel.State = MasterDropdownListData;
                        }
                    }
                }

                if (queryString.Contains(Constants.salutation.ToLower()) || queryString.Contains(Constants.all.ToLower()))
                {
                    IEnumerable<Item> ChildItems = dataSource.Children.Where(x => x.Name.ToLower() == Constants.salutation.ToLower()).ToList();
                    List<Salutation> MasterDropdownListData = new List<Salutation>();
                    if (ChildItems != null && ChildItems.Count() >= 0)
                    {
                        foreach (Item collapsechild in ChildItems)
                        {
                            IEnumerable<Item> countrychild = collapsechild.Children;
                            foreach (Item item in countrychild)
                            {
                                if (item != null)
                                {
                                    MasterDropdownListData.Add(GetSalutation(item));
                                }
                            }
                            masterListModel.Salutation = MasterDropdownListData;
                        }
                    }
                }

                if (queryString.Contains(Constants.travel_sector.ToLower()) || queryString.Contains(Constants.all.ToLower()))
                {
                    IEnumerable<Item> ChildItems = dataSource.Children.Where(x => x.Name.ToLower() == Constants.travel_sector.ToLower()).ToList();
                    List<TravelSector> MasterDropdownListData = new List<TravelSector>();
                    if (ChildItems != null && ChildItems.Count() >= 0)
                    {
                        foreach (Item collapsechild in ChildItems)
                        {
                            IEnumerable<Item> countrychild = collapsechild.Children;
                            foreach (Item item in countrychild)
                            {
                                if (item != null)
                                {
                                    MasterDropdownListData.Add(GetTravelSector(item));
                                }
                            }
                            masterListModel.TravelSector = MasterDropdownListData;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetMasterList() gives -> " + ex.Message);
            }



            return masterListModel;
        }

        /// <summary>
        /// this method has been written to retun country list data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>

        public CountryList GetCountryData(Item item)
        {
            CountryList country = new CountryList();
            if (item != null)
            {
                try
                {
                    country.Id = item[Constants.Id];
                    country.CountryName = item[Constants.CountryName];
                    country.DialCode = item[Constants.DialCode];
                    country.ISO3 = item[Constants.ISO3];
                    country.ISO2 = item[Constants.ISO2];
                    country.CurrencyName = item[Constants.CurrencyName];
                    country.CurrencyCode = item[Constants.CurrencyCode];
                    country.UNTERMEnglish = item[Constants.UNTERMEnglish];
                    country.RegionName = item[Constants.RegionName];
                    country.Capital = item[Constants.Capital];
                    country.Continent = item[Constants.Continent];
                    country.TLD = item[Constants.TLD];
                    country.Languages = item[Constants.Languages];
                    country.CountryFlagImage = item.Fields[Constants.FlagImage] != null ? _helper.GetImageURL(item, Constants.FlagImage) : String.Empty;
                    CheckboxField checkbox = item.Fields[Constants.IsDeleted];
                    if (checkbox != null)
                        country.IsDeleted = checkbox.Checked;
                }
                catch (Exception ex)
                {
                    _logRepository.Error("Master List GetCountryData() gives -> " + ex.Message);
                }
            }
            return country;
        }

        /// <summary>
        /// Airport master list
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public AirportList GetAirportList(Item item)
        {
            AirportList airport = new AirportList();
            if (item != null)
            {
                try
                {
                    airport.AirportID = item[Constants.AirportID];
                    airport.AirportName = item[Constants.AirportName];
                    airport.IATACode = item[Constants.IATACode];
                    CheckboxField IsActivecheckbox = item.Fields[Constants.IsActive];
                    if (IsActivecheckbox != null)
                        airport.IsActive = IsActivecheckbox.Checked;
                    CheckboxField IsDomesticcheckbox = item.Fields[Constants.IsDomestic];
                    if (IsDomesticcheckbox != null)
                        airport.IsDomestic = IsDomesticcheckbox.Checked;
                    airport.CountryName = item[Constants.CountryName];
                    airport.CountryCode = item[Constants.CountryCode];
                    airport.CityCode = item[Constants.CityCode];
                    airport.CityName = item[Constants.CityName];
                    airport.AirportCode = item[Constants.AirportCode];
                    CheckboxField IsPranaambox = item.Fields[Constants.IsPranaam];
                    if (IsActivecheckbox != null)
                        airport.IsPranaam = IsPranaambox.Checked;
                    CheckboxField isPopularbox = item.Fields[Constants.IsPopular];
                    if (IsDomesticcheckbox != null)
                        airport.IsPopular = isPopularbox.Checked;

                    List<string> keywordsList = new List<string>();
                    IEnumerable<Item> keywordstItems = item.Children.Where(x => x.TemplateID == Templates.AirportListCollection.KeywordsFolderTemplateID).ToList();
                    if (keywordstItems != null && keywordstItems.Count() > 0)
                    {
                        foreach (Sitecore.Data.Items.Item keyWords in keywordstItems)
                        {
                            IEnumerable<Item> keywordtItem = keyWords.Children.Where(x => x.TemplateID == Templates.AirportListCollection.KeywordTemplateID).ToList();
                            if (keywordtItem != null && keywordtItem.Count() > 0)
                            {
                                keywordsList.AddRange(from Item keyword in keywordtItem
                                                      let keywords = keyword.Fields["keyword"]?.Value
                                                      select keywords);
                            }
                            airport.Keywords = keywordsList;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logRepository.Error("Master List GetAirportList() gives -> " + ex.Message);
                }
            }
            return airport;
        }

        public PranaamAirportList GetPranaamAirportList(Item item)
        {
            PranaamAirportList airport = new PranaamAirportList();
            if (item != null)
            {
                try
                {
                    airport.AirportID = item[Constants.AirportID];
                    airport.AirportName = item[Constants.AirportName];
                    airport.IATACode = item[Constants.IATACode];
                    CheckboxField IsActivecheckbox = item.Fields[Constants.IsActive];
                    if (IsActivecheckbox != null)
                        airport.IsActive = IsActivecheckbox.Checked;
                    CheckboxField IsDomesticcheckbox = item.Fields[Constants.IsDomestic];
                    if (IsDomesticcheckbox != null)
                        airport.IsDomestic = IsDomesticcheckbox.Checked;
                    airport.AirportMaster = item[Constants.AirportMaster];
                    airport.City = item[Constants.City];
                    airport.ChangeServiceSLA = item[Constants.ChangeServiceSLA];
                    airport.Description = item[Constants.Description];
                    CheckboxField HasPranaamServicecheckbox = item.Fields[Constants.HasPranaamService];
                    if (IsDomesticcheckbox != null)
                        airport.HasPranaamService = HasPranaamServicecheckbox.Checked;
                    airport.Name_2 = item[Constants.Name_2];
                    CheckboxField IsActive_2checkbox = item.Fields[Constants.IsActive_2];
                    if (IsDomesticcheckbox != null)
                        airport.IsActive_2 = IsActive_2checkbox.Checked;
                    airport.PerDayBooking = item[Constants.PerDayBooking];
                    airport.ServiceBookingSLA = item[Constants.ServiceBookingSLA];
                    airport.ServiceTimeSLA = item[Constants.ServiceTimeSLA];
                    airport.Lat = item[Constants.Lat];
                    airport.Lng = item[Constants.Lng];



                }
                catch (Exception ex)
                {
                    _logRepository.Error("Master List GetAirportList() gives -> " + ex.Message);
                }
            }
            return airport;
        }

        /// <summary>
        /// this method has been witten to return Nationality list data
        /// </summary>
        /// <param name="datasource"></param>
        /// <returns></returns>
        public NationalityList GetNationalityData(Item item)
        {
            NationalityList nationalityMaster = new NationalityList();
            try
            {

                nationalityMaster.CountryName = item[Constants.name];
                nationalityMaster.Nationality = item[Constants.value];
                nationalityMaster.CountryFlagImage = item.Fields[Constants.Image] != null ? _helper.GetImageURL(item, Constants.Image) : String.Empty;


            }
            catch (Exception ex)
            {
                _logRepository.Error(" NationalityService GetNationalityData gives -> " + ex.Message);
            }
            return nationalityMaster;
        }

        public MasterDropdown GetDropDownList(Item item)
        {
            MasterDropdown dropdownlist = new MasterDropdown();
            if (item != null)
            {
                try
                {
                    dropdownlist.Id = item[Constants.Id];
                    dropdownlist.Label = item[Constants.label];
                    dropdownlist.Order = item[Constants.order];

                }
                catch (Exception ex)
                {
                    _logRepository.Error("Master List GetCountryData() gives -> " + ex.Message);
                }
            }
            return dropdownlist;
        }

        public Salutation GetSalutation(Item item)
        {
            Salutation dropdownlist = new Salutation();
            if (item != null)
            {
                try
                {
                    dropdownlist.Id = item[Constants.Id];
                    dropdownlist.Label = item[Constants.label];
                    dropdownlist.Order = item[Constants.order];
                    dropdownlist.Is_Adult = item[Constants.is_adult];
                    dropdownlist.Is_Child = item[Constants.is_child];
                    dropdownlist.Is_Infant = item[Constants.is_infant];

                }
                catch (Exception ex)
                {
                    _logRepository.Error("Master List GetCountryData() gives -> " + ex.Message);
                }
            }
            return dropdownlist;
        }

        public State GetState(Item item)
        {
            State dropdownlist = new State();
            if (item != null)
            {
                try
                {
                    dropdownlist.Id = item[Constants.Id];
                    dropdownlist.Import = item[Constants.Import];
                    dropdownlist.Name = item[Constants.Name];
                    dropdownlist.Country_Master = item[Constants.CountryMaster];
                    dropdownlist.Country_Code = item[Constants.CountryCode];
                    dropdownlist.State_Code = item[Constants.StateCode];
                    dropdownlist.Latitude = item[Constants.Latitude];
                    dropdownlist.Longitude = item[Constants.Longitude];
                    dropdownlist.Updated_State_Code = item[Constants.UpdatedStateCode];

                }
                catch (Exception ex)
                {
                    _logRepository.Error("Master List GetCountryData() gives -> " + ex.Message);
                }
            }
            return dropdownlist;
        }

        public TravelSector GetTravelSector(Item item)
        {
            TravelSector dropdownlist = new TravelSector();
            if (item != null)
            {
                try
                {
                    dropdownlist.Id = item[Constants.Id];
                    dropdownlist.Label = item[Constants.label];
                    dropdownlist.Order = item[Constants.order];
                    dropdownlist.Is_Transit = item[Constants.is_transit];


                }
                catch (Exception ex)
                {
                    _logRepository.Error("Master List GetCountryData() gives -> " + ex.Message);
                }
            }
            return dropdownlist;
        }
    }
}