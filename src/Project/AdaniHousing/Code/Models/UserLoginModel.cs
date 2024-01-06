using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniHousing.Website.Models
{
    [Serializable]
    public class UserLoginModel
    {
        public bool IsLoggedIn { get; set; }
        public string LoginName { get; set; }
        public string LoanAccountNumber { get; set; }
        public string OTP { get; set; }
        public string LoginUser { get; set; }
        public string MobileNo { get; set; }
        //public string DeviceId { get; set; }
        public string UserIP { get; set; }
        public string AuthToken { get; set; }
    }
}