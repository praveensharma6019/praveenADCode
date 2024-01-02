using System;
using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Models
{
    public class ResponseData
    {
        public bool status { get; set; }
        public ResultData data { get; set; }
        public Error error { get; set; }
        public Warning warning { get; set; }
    }

    public class ResponseDataOld
    {
        public bool status { get; set; }
        public ResultDataOld data { get; set; }
        public Error error { get; set; }
        public Warning warning { get; set; }
    }

    public class Error
    {
        public string statuscode { get; set; }
        public string description { get; set; }
        public string errorCode { get; set; }
        public string source { get; set; }
    }

    public class Warning
    {
        public string code { get; set; }
        public string description { get; set; }
        public string source { get; set; }
    }

    public class ResultData
    {
        public int count { get; set; }
        public List<Object> result { get; set; }
        public List<Object> airlines { get; set; }
        public List<Object> airports { get; set; }
        public List<Object> similar { get; set; }
        public List<Object> buyTogether { get; set; }
    }

    public class ResultDataOld
    {
        public int count { get; set; }
        public List<OfferMapping_old> result { get; set; }
        public List<MetaInformation> metaInformation { get; set; }
    }
   
}