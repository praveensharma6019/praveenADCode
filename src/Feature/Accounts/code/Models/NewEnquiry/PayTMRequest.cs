using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    public class PayTMRequest
    {
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Amount { get; set; }
    }
}