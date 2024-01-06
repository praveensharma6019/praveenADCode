using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Bunkering.Website.Models
{
    public class BunkeringContactModel
    {
        public string Name { set; get; }
        public string Email { set; get; }
        public string Mobile { set; get; }
        public string MessageType { set; get; }
        public string Message { set; get; }
        public string reResponse { get; set; }
        public string FormType { set; get; }
        public string PageInfo { get; set; }
        public DateTime FormSubmitOn { set; get; }
        public string OTP { get; set; }
    }
}