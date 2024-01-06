using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Sitecore.JaipurAirport.Website.Models
{
    [Table("JAI_AirportFlights")]
    public class JaipurFlight
    {

        public DateTime OriginDate;
        public string FlightNumber;
        public string ArrivingFrom;
        public string DepartureFrom;
        public string Scheduled;
        public string Airline;
        public string terminal;
        public string Status;
        public string OperationalStatus;
        public string ServiceType;
        public string AssociatedFlightLegSchedule;
        public string EstimationTime;
        public DateTime ScheduledTime;
        public DateTime ActualTime;
        public string SpecialAction;
        public string Flightkind;
        public string FlightType;

    }
}