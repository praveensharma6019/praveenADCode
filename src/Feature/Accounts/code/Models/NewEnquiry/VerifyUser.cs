using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    public class VerifyUser
    {
        public string User_ID { get; set; }
        public string Ev_Msg_Flag { get; set; }
        public string Ev_Message { get; set; }
    }
}