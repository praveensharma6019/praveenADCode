using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniCapital.Website.Models
{
    public class UserLoginSessionModel
    {
        public string MobileNo { get; set; }
        public string SessionId { get; set; }
        public string UserIP { get; set; }
        public DateTime CurrentLoginDateTime { get; set; }
        public DateTime LastLoginDateTime { get; set; }
    }
}