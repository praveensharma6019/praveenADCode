using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.CabVendor.Platform.Models
{
    public class ResponseFilters
    {
        public bool status { get; set; }
        public ResultFilters data { get; set; }
        public Error error { get; set; }
        public Warning warning { get; set; }
    }
    public class ResultFilters
    {
        // public int count { get; set; }
        public Object result { get; set; }
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
}