using System.Collections.Generic;

namespace Adani.Foundation.Messaging.Models
{
    public class FarmpikSMSModel
    {
        public string countryCode { get; set; }
        public string mobileNo { get; set; }
        public List<Data> data { get; set; }
    }

    public class Data
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
