using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniCapital.Website.Models
{
    public class LastPaymentsbyCustomer
    {
        public int Amount
        {
            get;
            set;
        }

        public string BankName
        {
            get;
            set;
        }

        public string BounceDate
        {
            get;
            set;
        }

        public string BounceReason
        {
            get;
            set;
        }

        public string InstrumentDate
        {
            get;
            set;
        }

        public string InstrumentNumber
        {
            get;
            set;
        }

        public string InstrumentType
        {
            get;
            set;
        }

        public string PaymentDate
        {
            get;
            set;
        }

        public string PaymentMode
        {
            get;
            set;
        }

        public string ReferenceNumber
        {
            get;
            set;
        }

        public string TransactionDate
        {
            get;
            set;
        }
    }
    public class LastNBounces
    {
        public string BounceDate
        {
            get;
            set;
        }

        public string InstallmentAmount
        {
            get;
            set;
        }
    }
}