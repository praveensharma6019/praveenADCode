using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    public class GasContactModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string MessageType { get; set; }
        public string Mobile { get; set; }
        public string Message { get; set; }
        public string Address { get; set; }
        public string CustomerId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string FormType { get; set; }
        public string PageInfo { get; set; }
        public DateTime FormSubmitOn { get; set; }
        public string reResponse { get; set; }
        public string OtherCity { get; set; }
       

        public string emailMessage { get; set; }

        public string FromEmail { get; set; }
    }
}