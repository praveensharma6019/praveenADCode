using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    public class CreateUserSet
    {
        public string UserID { get; set; }
        public string New_Password { get; set; }
        public string InquiryNo { get; set; }
        public string Ev_Msg_Flag { get; set; }
        public string Ev_Message { get; set; }
    }
}