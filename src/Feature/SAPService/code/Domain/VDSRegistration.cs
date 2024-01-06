using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SapPiService.Domain
{
    public class VDSRegistration
    {
        public string AccountNumber { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string PANNumber { get; set; }
        public string Amount { get; set; }
        public string TransactionId { get; set; }
        public string ResultFlag { get; set; }
        public string DateOfTransaction { get; set; }
    }
}