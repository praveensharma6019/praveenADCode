using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SapPiService.Domain
{
    public class NoSupplyComplaintStatus
    {
        public string AccountNumber { get; set; }
        public string ComplaintNumber { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public string Captcha { get; set; }
        public string ServiceCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}