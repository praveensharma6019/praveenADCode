using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    public class DoucmentDataGas
    {
        public string documentType { get; set; }
        public string documentName { get; set; }
        public HttpPostedFileBase Documentdata { get; set; }
    }
}