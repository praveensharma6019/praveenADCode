using Sitecore.ContentSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.ElectricityNew.Website.Model
{
    public class PaymentSearchResultItem : Sitecore.ContentSearch.SearchTypes.SearchResultItem
    {
        [IndexField("account_number_s")]
        public string AccountNumber { get; set; }

        [IndexField("userid_s")]
        public string UserId { get; set; }

        [IndexField("order_id_s")]
        public string OrderId { get; set; }

        [IndexField("amount_s")]
        public string Amount { get; set; }

        [IndexField("transaction_id_s")]
        public string TransactionId { get; set; }

        [IndexField("gateway_type_s")]
        public string PaymentGateWay { get; set; }

        [IndexField("payment_mode_s")]
        public string PaymentMode { get; set; }

        [IndexField("payment_type_s")]
        public string PaymentType { get; set; }

        [IndexField("status_s")]
        public string Status { get; set; }

        [IndexField("user_type_s")]
        public string UserType { get; set; }

        [IndexField("response_time_dt")]
        public DateTime ResponseTime { get; set; }

        [IndexField("request_time_dt")]
        public DateTime RequestTime { get; set; }
    }

    public class PaymentSearchResult
    {
        public string AccountNumber { get; set; }
        public string TransactionId { get; set; }
        public string OrderId { get; set; }
        public string Amount { get; set; }
        public string Status { get; set; }
        public string PaymentGateWay { get; set; }
        public string PaymentType { get; set; }
        public string PaymentMode { get; set; }
        public string UserType { get; set; }
        public string UserId { get; set; }
        public Nullable<DateTime> RequestTime { get; set; }
        public Nullable<DateTime> ResponseTime { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public string ResponseMsg { get; set; }
    }
}