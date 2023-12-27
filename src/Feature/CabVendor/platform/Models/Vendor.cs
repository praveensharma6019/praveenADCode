﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.CabVendor.Platform.Models
{
    public class Vendor
    {
        public string title { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public string mobileImage { get; set; }
        public string vendorLogo { get; set; }
        //public List<Airports> airports { get; set; }
        public Information information { get; set; }
        public List<ServiceCategory> services { get; set; }
        public List<cancellationDetails> cancellationText { get; set; }

        public Vendor()
        {
            //airports = new List<Airports>();
            information = new Information();
            services = new List<ServiceCategory>();
            cancellationText = new List<cancellationDetails>();
        }
    }
    //public class Airports
    //{
    //    public string city { get; set; }
    //    public string airportName { get; set; }
    //    public string airportCode { get; set; }
    //    //public List<Terminals> terminals { get; set; }
    //    //public Airports()
    //    //{
    //    //    terminals = new List<Terminals>();
    //    //}
    //}

    //public class Terminals
    //{
    //    public string TerminalName { get; set; }
    //    public string terminalCode { get; set; }
    //}

    public class ServiceCategory
    {
        public string title { get; set; }
        public string subTitle { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public string mobileImage { get; set; }
    }

    public class Information
    {
        public string title { get; set; }
        public List<string> info { get; set; }

        public Information()
        {
            title = "";
            info = new List<string>();
        }
    }

    public class VendorDetails
    {
        public string vendorName { get; set; }
        public string airportName { get; set; }
        public string cityName { get; set; }
        public string terminalName { get; set; }
        public string gate { get; set; }
        public string cabLocation { get; set; }
        public string cancellationCTA { get; set; }
        public string cancellationCTAText { get; set; }
        public string cancellationCTAWeb { get; set; }
        public string cancellationCTAWebText { get; set; }
        public List<string> rideNowText { get; set; }
        public List<string> rideLaterText { get; set; }
        public List<string> stepsToBoard { get; set; }
        public string shareMessage { get; set; }
        public string policeHelpline { get; set; }
        public ContactDetailsItem contactDetail { get; set; }
        public string scheduleCabDetailShareTime { get; set; }
        public string trackingPrecautionMessage { get; set; }
        public string tripType { get; set; }
        public string cabSchedule { get; set; }
        public string cabBookingType { get; set; }
        public ContentJSONList infoDetails { get; set; }
        public List<BookingSteps> BookingStep { get; set; }
    }

    public class ContactDetail
    {
        public string name { get; set; }
        public string title { get; set; }
        public string richText { get; set; }
    }
    public class ContactDetailsItem
    {
        public ContactDetail phone { get; set; }
        public ContactDetail email { get; set; }
    }
    public class BookingSteps
    {
        public string stepTitle { get; set; }
        public string stepImage { get; set; }
        public string stepDescription { get; set; }
    }

    public class cancellationDetails
    {
        public string title { get; set; }
        public string description { get; set; }
        public string autoId { get; set; }

        public cancellationDetails()
        {
            title = string.Empty;
            description = string.Empty;
            autoId = string.Empty;
        }
    }
}