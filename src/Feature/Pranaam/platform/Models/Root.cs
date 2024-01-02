using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Models
{
    public class Root
    {
            public string UserId { get; set; }
            public string ServiceTypeId { get; set; }
            public string ServiceType { get; set; }
            public string TravelSector { get; set; }
            public string OriginAirport { get; set; }
            public string DestinationAirport { get; set; }
            public int BookingType { get; set; }
            public int OldBookingId { get; set; }
            public string ServiceDateTime { get; set; }
    }
}