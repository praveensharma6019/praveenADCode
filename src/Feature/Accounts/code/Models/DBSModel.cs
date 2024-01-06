using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Web.Mvc;
using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Linq;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace Sitecore.Feature.Accounts.Models
{
    //[Serializable]
    //public class DBSModel
    //{
    //    public string TransactionId { get; set; }
    //    public string UPIReference { get; set; }
    //    public string RefNumber { get; set; }
    //    public string AccountNumber { get; set; }
    //    public string Email { get; set; }
    //    public string Mobile { get; set; }
    //    public string OrderId { get; set; }
    //    public string ResponseStatus { get; set; }
    //    public string Responsecode { get; set; }
    //    public string AmountPayable { get; set; }
    //    public string Remark { get; set; }
    //    public string PaymentType { get; set; }
    //    public string TransactionDate { get; set; }
    //    public string Gateway { get; set; }
    //    public string PaymentMode { get; set; }
    //}

    public class DBSResponse
    {
        public Header header { get; set; }
        public TxnResponse txnInfo { get; set; }

        public string PaymentType { get; set; }
        public string PaymentMode { get; set; }
        public string AccountNumber { get; set; }
        public string merchantCode { get; set; }
        public string paymentMethod { get; set; }
        public string transactionDate { get; set; }
        public string txnId { get; set; }
        public string refNumber { get; set; }
        public string amount { get; set; }
        public string txnStatus { get; set; }
        public string till { get; set; }
        public string payerName { get; set; }
        public string ResponseMessage { get; set; }
    }

    public class Header
    {
        public string msgId { get; set; }
        public string orgId { get; set; }
        public string timeStamp { get; set; }
    }

    public class Sender
    {
        public string name { get; set; }
        public string accountNo { get; set; }
        public string vpa { get; set; }
        public string vbankClearingCodepa { get; set; }
    }

    public class Receiver
    {
        public string name { get; set; }
        public string vpa { get; set; }
    }

    public class TxnResponse
    {
        public string customerReference { get; set; }
        public string txnType { get; set; }
        public string txnCcy { get; set; }
        public string txnAmount { get; set; }
        public string refId { get; set; }
        public string txnRefId { get; set; }
        public Sender senderParty { get; set; }
        public Receiver receivingParty { get; set; }
    }
}
