using System.Collections.Generic;

namespace SapPiService.Domain
{
    public class PaymentHistory
    {
        public List<PaymentHistoryRecord> PaymentHistoryList { get; set; }
        public PaymentHistory()
        {
            PaymentHistoryList = new List<PaymentHistoryRecord>();
        }
    }

    public class PaymentHistoryRecord
    {
        public string PaymentDate { get; set; }
        public string Center { get; set; }
        public string Amount { get; set; }
        public string PaymentMode { get; set; }
        public string Receipt { get; set; }
        public string ChequeNo { get; set; }
    }
}