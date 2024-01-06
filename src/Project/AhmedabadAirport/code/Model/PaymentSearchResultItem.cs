using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Runtime.CompilerServices;

namespace Sitecore.AhmedabadAirport.Website.Model
{
    public class PaymentSearchResultItem : SearchResultItem
    {
        [IndexField("account_number_s")]
        public string AccountNumber
        {
            get;
            set;
        }

        [IndexField("amount_s")]
        public string Amount
        {
            get;
            set;
        }

        [IndexField("order_id_s")]
        public string OrderId
        {
            get;
            set;
        }

        [IndexField("gateway_type_s")]
        public string PaymentGateWay
        {
            get;
            set;
        }

        [IndexField("payment_mode_s")]
        public string PaymentMode
        {
            get;
            set;
        }

        [IndexField("payment_type_s")]
        public string PaymentType
        {
            get;
            set;
        }

        [IndexField("request_time_dt")]
        public DateTime RequestTime
        {
            get;
            set;
        }

        [IndexField("response_time_dt")]
        public DateTime ResponseTime
        {
            get;
            set;
        }

        [IndexField("status_s")]
        public string Status
        {
            get;
            set;
        }

        [IndexField("transaction_id_s")]
        public string TransactionId
        {
            get;
            set;
        }

        [IndexField("userid_s")]
        public string UserId
        {
            get;
            set;
        }

        [IndexField("user_type_s")]
        public string UserType
        {
            get;
            set;
        }

        public PaymentSearchResultItem()
        {
        }
    }
}