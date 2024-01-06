using System;
using System.Runtime.CompilerServices;

namespace Sitecore.TrivandrumAirport.Website.Model
{
    public class PaymentSearchResult
    {
        public string AccountNumber
        {
            get;
            set;
        }

        public string Amount
        {
            get;
            set;
        }

        public DateTime? CreateDate
        {
            get;
            set;
        }

        public string OrderId
        {
            get;
            set;
        }

        public string PaymentGateWay
        {
            get;
            set;
        }

        public string PaymentMode
        {
            get;
            set;
        }

        public string PaymentType
        {
            get;
            set;
        }

        public DateTime? RequestTime
        {
            get;
            set;
        }

        public string ResponseMsg
        {
            get;
            set;
        }

        public DateTime? ResponseTime
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public string TransactionId
        {
            get;
            set;
        }

        public string UserId
        {
            get;
            set;
        }

        public string UserType
        {
            get;
            set;
        }

        public PaymentSearchResult()
        {
        }
    }
}