using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Sitecore.TrivandrumAirport.Website.Models
{
    [Table("TRV_AirportFlights")]
    public class TrivandrumFlight
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