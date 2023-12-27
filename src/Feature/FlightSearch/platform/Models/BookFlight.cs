using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Models
{
   
    public class BookFlight
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public List<NameValueCollectionList> nameValueCollection { get; set; }
    }


}