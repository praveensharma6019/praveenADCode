using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Model
{
    public class FeedbackFormResponse
    {
       
            public bool Status { get; set; }
            public ResultData Data { get; set; }
            public string Message { get; set; }

            public Error Error { get; set; }
            public Warning Warning { get; set; }     

        
    }

    public class Error
    {
        public string Statuscode { get; set; }
        public string Description { get; set; }
        public string ErrorCode { get; set; }
        public string Source { get; set; }
    }

    public class Warning
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
    }

    public class ResultData
    {
        public Incident IncidentDetail { get; set; }
    }

    public class Incident
    {
        public string IncidentId { get; set; }
    }
}