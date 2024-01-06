using System.Collections.Generic;

namespace SapPiService.Domain
{
    public class ComplaintRegistrationResponse
    {
        public string Error { get; set; }
        public string Message { get; set; }
        public string AccountNumber { get; set; }
        public string TATInfo { get; set; }
        public string ComplaintNumber { get; set; }
        public string ComplaintStatus { get; set; }
        public string LTHTInfo { get; set; }
        public string AUFNR { get; set; }
        public bool IsRegistered { get; set; }
    }
}