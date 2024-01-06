using System.Collections.Generic;

namespace SapPiService.Domain
{
    public class ComplaintDetailsIGR
    {
        public string AUART { get; set; }
        public string AUFNR { get; set; }
        public string Complaint_Status { get; set; }
        public string ERDate { get; set; }
        public string GLTRP { get; set; }
        public string ILART { get; set; }
        public string IPHAS { get; set; }
        public string Complaint_Type { get; set; }
    }

    public class ComplaintDetailsStatus
    {
        public string CreatedDate { get; set; }
        public string OrderId { get; set; }
        public string AccountNumber { get; set; }
        public string Complaint_Status { get; set; }
        public string CompletionDate { get; set; }
        public string TATDate { get; set; }
        public string ZoneName { get; set; }
        //public string Division { get; set; }
        public string PMActivityType { get; set; }
    }
}