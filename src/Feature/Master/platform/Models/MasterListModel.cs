using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Master.Platform.Models
{
    public class  MasterListModel
    {
        public List<CountryList> Country { get; set; }
        public List<AirportList> Airports { get; set; }
        public List<PranaamAirportList> PranaamAirports { get; set; }
        public List<NationalityList> Nationality { get; set; }
        public List<MasterDropdown> BookingStatus { get; set; }
        public List<MasterDropdown> FlyingClass { get; set; }
        public List<MasterDropdown> PassengerType { get; set; }
        public List<MasterDropdown> ServiceType { get; set; }
        public List<Salutation> Salutation { get; set; }
        public List<State> State { get; set; }
        public List<TravelSector> TravelSector { get; set; }
    }

    public class AirportList
    {
        public string AirportID { get; set; }
        public string IATACode { get; set; }
        public string AirportName { get; set; }
        public bool IsDomestic { get; set; }
        public bool IsActive { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string AirportCode { get; set; }
        public bool IsPranaam { get; set; } = false;
        public bool IsPopular { get; set; } = false;
        public List<string> Keywords { get; set; }
    }

    public class PranaamAirportList
    {
        public string AirportID { get; set; }
        public string IATACode { get; set; }
        public string AirportName { get; set; }
        public bool IsDomestic { get; set; }
        public bool IsActive { get; set; }
        public string AirportMaster { get; set; }
        public string Name_2 { get; set; }
        public string Description { get; set; }
        public bool HasPranaamService { get; set; }
        public string ServiceTimeSLA { get; set; }
        public string ServiceBookingSLA { get; set; }
        public string ChangeServiceSLA { get; set; }
        public string HelpdeskPhoneNumber { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string City { get; set; }
        public bool IsActive_2 { get; set; }
        public string PerDayBooking { get; set; }
        
    }

    /// <summary>
    /// this methos has been witten to return country list with all the required fields
    /// </summary>
    public class CountryList
    {
        public string CountryName { get; set; }

        public string DialCode { get; set; }
        public string ISO3 { get; set; }

        public string ISO2 { get; set; }
        public string CurrencyName { get; set; }

        public string CurrencyCode { get; set; }
        public string UNTERMEnglish { get; set; }

        public string RegionName { get; set; }

        public string Capital { get; set; }

        public string Continent { get; set; }
        public string TLD { get; set; }
        public string Languages { get; set; }

        public bool IsDeleted { get; set; }

        public string Id { get; set; }

        public string CountryFlagImage { get; set; }
    }

    /// <summary>
    /// this method has been witten to return Nationality list with all the required fields
    /// </summary>
    public class NationalityList
    {
        public string CountryName { get; set; }

        public string Nationality { get; set; }

        public string CountryFlagImage { get; set; }
    }

    /// <summary>
    /// this method has been witten to return master dropdown list with all the required fields
    /// </summary>
    public class MasterDropdown
    {
        public string Label { get; set; }

        public string Order { get; set; }

        public string Id { get; set; }
    }

    /// <summary>
    /// this method has been witten to return Salutation list with all the required fields
    /// </summary>
    public class Salutation
    {
        public string Label { get; set; }

        public string Order { get; set; }

        public string Id { get; set; }

        public string Is_Adult { get; set; }

        public string Is_Child { get; set; }

        public string Is_Infant { get; set; }
    }

    /// <summary>
    /// this method has been witten to return state list with all the required fields
    /// </summary>
    public class State
    {
        public string Id { get; set; }

        public string Import { get; set; }

        public string Name { get; set; }
        public string Country_Master { get; set; }
        public string Country_Code { get; set; }
        public string State_Code { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Updated_State_Code { get; set; }
    }

    /// <summary>
    /// this method has been witten to return TravelSector list with all the required fields
    /// </summary>
    public class TravelSector
    {
        public string Label { get; set; }

        public string Order { get; set; }

        public string Id { get; set; }

        public string Is_Transit { get; set; }
    }
}