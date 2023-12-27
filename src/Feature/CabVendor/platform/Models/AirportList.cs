using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.CabVendor.Platform.Models
{
    public class AirportList
    {
        public List<AirportItem> ListOfAirports { get; set; }
    }

    public class AirportItem
    {
        public string AirportName { get; set; }
        public string AirportCode { get; set; }
        public string AirportID { get; set; }
        public string City { get; set; }
        public string Details { get; set; }
        public string AirportImage { get; set; }
        public string AirportThumbnailImage { get; set; }
        public string AirportAddress { get; set; }
        public string TerminalList { get; set; }
        public List<Terminals> TerminalsList { get; set; }
        public string ColorCode { get; set; }
        public string AirportIcon { get; set; }
        public ContactDetailsItems contactDetails { get; set; }
        public string AirportPrefixName { get; set; }
        public bool IsTheme { get; set; }
        public string PostFix { get; set; }
        public string AppBarBackgroundColor { get; set; }
        public string AppBarSubtitleColor { get; set; }
        public string AppBarTitleColor { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string placeID { get; set; }
        public bool IsCabEnabled { get; set; }

        public bool IsNonAdaniAirport { get; set; }
        public string StickyWidgetBackgroundColor { get; set; }
        public string StickyWidgetTextColor { get; set; }
        public string StickyWidgetIconColor { get; set; }
        public bool lightStatusBar { get; set; }
        public int dutyFreeAgeLimit { get; set; }
        public bool PranaamServiceAvailable { get; set; } = false;
        public bool PorterServiceAvailable { get; set; } = false;
        public List<string> Keywords { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaImage { get; set; }
        public string MetaUrl { get; set; }
    }
    public class Terminals
    {
        public string TerminalName { get; set; }
        public string TerminalAddress { get; set; }
        public bool PranaamServiceAvailable { get; set; } = false;
        public string TerminalCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string placeID { get; set; }
        public bool IsCabEnabled { get; set; } = false;
        public bool DutyFreeAvailable { get; set; } = false;
        public List<Gate> gates { get; set; } = new List<Gate>();
        public List<string> Keywords { get; set; }

    }
    public class Gate
    {
        public string gateType { get; set; }
        public string gate { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string pickupAddress { get; set; }
        public bool fnbAvailable { get; set; }
        public bool retailAvailable { get; set; }
        public bool dutyFreeAvailable { get; set; }
        public string placeID { get; set; }
        public bool IsCabEnabled { get; set; } = false;
    }
    public class ContactDetails
    {
        public string name { get; set; }
        public string title { get; set; }
        public string richText { get; set; }
    }
    public class ContactDetailsItems
    {
        public ContactDetails phone { get; set; }
        public ContactDetails email { get; set; }
    }

}