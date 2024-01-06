using System.Collections.Generic;

namespace SapPiService.Domain
{
    public class BankAccountDetails
    {
        public string Flag { get; set; }
        public string BANK_COUNTRY { get; set; }
        public string BANK_KEY { get; set; }
        public string CREATED_ON { get; set; }
        public string CREATED_BY { get; set; }
        public string BANK_NAME { get; set; }
        public string REGION { get; set; }
        public string STREET { get; set; }
        public string CITY { get; set; }
        public string SWIFT_CODE { get; set; }
        public string BANK_BRANCH { get; set; }
        public string Message { get; set; }
    }

}