using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Hotels.Platform.Models
{
    public class Response
    {
        public bool status { get; set; }
        public Result data { get; set; }
        public Error error { get; set; }
        public Warning warning { get; set; }
    }
    public class Result
    {
        public AllPolicies allPolicies { get; set; }
        public TermsConditions termsAndConditions { get; set; }
        public ContactDetailsItem contactDetail { get; set; }
        public ImportantInfo importantInfo { get; set; }
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