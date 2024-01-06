using System.Collections.Generic;

namespace SapPiService.Domain
{
    public class ITDeclarationInfo
    {
        public string AccountNumber { get; set; }
        public string CANumber { get; set; }
        public string Source { get; set; }
        public string DeclarationType { get; set; }
        public string MobileNumber { get; set; }
        public string PANNumber { get; set; }
        public string AadharNumber { get; set; }
        public string AgreeOption { get; set; }
        public string FY_1 { get; set; }
        public string FY_1AcknowledgementNumber { get; set; }
        public string FY_1DateOfFilingReturn { get; set; }
        public string FY_2 { get; set; }
        public string FY_2AcknowledgementNumber { get; set; }
        public string FY_2DateOfFilingReturn { get; set; }
        public string FY_3 { get; set; }
        public string FY_3AcknowledgementNumber { get; set; }
        public string FY_3DateOfFilingReturn { get; set; }
        public string FY_3DateOfFilingRet { get; set; }
        public string BILL_PAYABLE_AMT { get; set; }
        public string PAYMENT_DATE { get; set; }
        public string PAYMENT_AMT { get; set; }
        public string TDS_RATE { get; set; }
        public string TDS_AMOUNT { get; set; }
        public string DOCUMENT_NO { get; set; }
        public string POSTING_DATE { get; set; }
    }

    public class ITDeclarationResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}