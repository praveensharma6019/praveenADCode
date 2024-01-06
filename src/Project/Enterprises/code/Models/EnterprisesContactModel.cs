using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Enterprises.Website.Models
{
    public class EnterprisesContactModel
    {
        public string Name { set; get; }
        public string Email { set; get; }
        public string Mobile { set; get; }
        public string MessageType { set; get; }
        public string Message { set; get; }
        public string reResponse { set; get; }
        public string FormType { set; get; }
        public string PageInfo { get; set; }
        public DateTime FormSubmitOn { set; get; }
        public string OTP { get; set; }
    }
}