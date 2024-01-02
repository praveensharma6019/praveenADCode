using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Models
{
    public class NameandValue
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
    public class NameValueCollectionList
    {
        public string ListName { get; set; }
        public List<NameandValue> Collection { get; set; }

    }
}