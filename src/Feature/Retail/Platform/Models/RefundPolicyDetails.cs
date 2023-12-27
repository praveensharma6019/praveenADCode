using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Retail.Platform.Models
{
    public class RefundPolicyDetails
    {
        public string Title { get; set; }
        public List<PolicyData> WebDescription { get; set; }
    }
    public class PolicyData
    {
        public string List { get; set; }
    }
}