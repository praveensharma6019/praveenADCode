using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Models
{
    public class OfferFilters_old
    {
        public string AirportCode { get; set; }
        public string SearchText { get; set; }
        public string language { get; set; }
        public string Code { get; set; }
        public string StoreType { get; set; }
        public string appType { get; set; }
        public string OfferUniqueID { get; set; }
        public string tab { get; set; }
        public bool isBankOffer { get; set; } = false;
        public string LOB { get; set; }
        //Ticket No 17128
        public bool isUnlockOffer { get; set; } = false;
        public bool airportsEnabled { get; set; } = false;
        public bool airlinesEnabled { get; set; } = false;
        public bool isInternational { get; set; } = false;

    }
}