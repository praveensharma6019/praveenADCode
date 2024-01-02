using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.FAQ.Models
{
    public class FAQResultData
    {
        public string airportName { get; set; }
        public FAQData faqData { get; set; }
    }
    
    public class FAQResponseData
    {
        public bool status { get; set; }
        public ResultData data { get; set; }
    }

    public class ResultData
    {
        public Dictionary<string, FAQResultData> flightBookingDictionary { get; set; }
        public Dictionary<string, FAQResultData> pranaamDictionary { get; set; }
        public Dictionary<string, FAQResultData> dutyFreeDictionary { get; set; }
    }
}