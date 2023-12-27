using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Avatar.Models
{
    public class AvatarResultData
    {
        public bool status { get; set; }
        public ResultData data { get; set; }
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
        public List<AvatarData> result { get; set; }
    }
}