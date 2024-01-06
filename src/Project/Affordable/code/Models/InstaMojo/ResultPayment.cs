using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Affordable.Website.Models.InstaMojo
{
    public class ResultPayment
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }

        public ResultPayment()
        {
            IsSuccess = false;
            Message = "Payment not done!";
        }
    }
}