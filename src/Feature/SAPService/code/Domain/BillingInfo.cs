using System.Collections.Generic;

namespace SapPiService.Domain
{
    public class BillingInfo : AccountBase
    {
        public string BillMonth {get;set;}
        public string DateDue { get; set; }
        public string TariffSlab { get; set; }
        public string UnitsConsumed { get; set; }
        public string BillAmount { get; set; }
        public string TotalCharges { get; set; }
        public string CurrentMonthCharge { get; set; }
        public string BroughtForward { get; set; }
        public string TotalBillAmount { get; set; }
        public decimal AmountPayable { get; set; }
        public bool IsOutstanding { get; set; }
        public BillingStatus BillingStatus { get; set; }
        public string[] MeterNumbers { get; set; }
        public string Message { get; set; }
        public string Flag { get; set; }
    }

    public class BillingDetailsList
    {
        public List<BillingDetailsRecord> BillingDetails { get; set; }

        public bool IsError { get; set; }
        public string Message { get; set; }

        public BillingDetailsList()
        {
            BillingDetails = new List<BillingDetailsRecord>();
        }
    }

    public class BillingDetailsRecord
    {
        public string InvoiceID { get; set; }
        public string AccountID { get; set; }
        public string ContractAccountID { get; set; }
        public string Currency { get; set; }
        public string AmountDue { get; set; }
        public string AmountPaid { get; set; }
        public string AmountRemaining { get; set; }
        public string InvoiceDescription { get; set; }
        public string InvoiceStatusID { get; set; }
        public string InvoiceDate { get; set; }
        public string Bill_Posting_Date { get; set; }
        public string Bill_Fiscal_Year { get; set; }
        public string Bill_ID { get; set; }
        public string Bill_Type { get; set; }
        public string Print_Document_No { get; set; }
        public string BillDate { get; set; }
        public string DueDate { get; set; }
    }
}