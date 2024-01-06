using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Sitecore.LucknowAirport.Website.Models
{
    [Table("LKO_AirportFlights")]
    public class LucknowFlight
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
    }
}