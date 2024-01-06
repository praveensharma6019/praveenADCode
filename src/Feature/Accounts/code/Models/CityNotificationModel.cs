using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Web.Mvc;
using System;

namespace Sitecore.Feature.Accounts.Models
{
    [Serializable]
    public class CityNotificationModel
    {
        public PushNotificationToSSGModel PushNotificationToSSG { get; set; }
    }

    public class PushNotificationToSSGModel
    {
        public string TxnRefNo { get; set; }
        public string OrderNo { get; set; }
        public string NPCITxnId { get; set; }
        public string TimeStamp { get; set; }
        public string TranAuthDate { get; set; }
        public string StatusCode { get; set; }
        public string AccountNumber { get; set; }
        public string StatusDesc { get; set; }
        public string RespCode { get; set; }
        public string SettlementAmount { get; set; }
        public string SettlementCurrency { get; set; }
        public string ResponseMessage { get; set; }
        public string PaymentType { get; set; }
        public string PaymentMode { get; set; }
    }
}