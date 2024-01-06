
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Sitecore.Feature.Accounts.Models
{
    public class UserStatistics
    {
        public string CANumber { get; set; }

        public string RegisteredDate { get; set; }

        public string ProfileUpdateDate { get; set; }

        public string LastLoginDate { get; set; }

        public string UserName { get; set; }
    }

    public class UserStatisticsdata
    {
        public List<UserStatistics> userdata { get; set; }
        public string message { get; set; }
     }

    public class UserCADetails
    {
        public string AccountNumber { get; set; }
    }

    public class UserCAList
    {
        public List<UserCADetails> userdata { get; set; }
        public string CustomerName { get; set; }
        public string message { get; set; }
    }
}