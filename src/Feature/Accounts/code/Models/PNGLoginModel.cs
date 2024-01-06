using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    [Serializable]
    public class PNGLoginModel
    {
        public string userType { get; set; }
        public bool IsLoggedIn { get; set; }
        public string LoginName { get; set; }
        public string CustomerID { get; set; }
        public string Password { get; set; }
        public string Contract_No { get; set; }
        public string Partner { get; set; }
        public string Inst_No { get; set; }
        public string Meter_Fromdt { get; set; }
        public string Meter_Uptodt { get; set; }
        public string Meter_SerialNumber { get; set; }
        public string DeviceId { get; set; }
        public string RegNo { get; set; }
        public string ReadingUnit { get; set; }
        public string CustomerType { get; set; }
        public string UserIP { get; set; }
        public string AuthToken { get; set; }
    }
}