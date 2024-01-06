using System.Collections.Generic;

namespace SapPiService.Domain
{
    public class EMIBillingInfo : AccountBase
    {
        public string ConsumerName { get; set; }
        public string InvoiveNumber { get; set; }
        public string BillMonth { get; set; }
        public decimal TotalOutstanding { get; set; }
        public decimal EMIInstallmentAmount { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}