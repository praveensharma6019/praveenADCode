using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    [Serializable]
    public class DashboardModel
    {
        public string cycleNumber { get; set; }
        public string userType { get; set; }
        public string primaryAccountNumber { get; set; }

        public string AccountNumber { get; set; }
        public string OrderId { get; set; }

        public string UserName { get; set; }
        public string SessionId { get; set; }
    }
}