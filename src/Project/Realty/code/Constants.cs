using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Realty.Website
{
    public struct Constants
    {
        public struct PayUResponseStatus
        {
            public static readonly string Success = "Success";
            public static readonly string Failure = "Failure";
            public static readonly string Pending = "Pending";
        }

        public struct PaymentResponse
        {
            public static readonly string Success = "Success";
            public static readonly string Failure = "Failure";
            public static readonly string Pending = "Pending";
            public static readonly string Initiated = "Initiated";
        }

        public struct BillDeskResponse
        {
            public static readonly string SuccessCode = "0300";
        }
    }
}