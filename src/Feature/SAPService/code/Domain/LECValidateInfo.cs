using System.Collections.Generic;

namespace SapPiService.Domain
{
    public class LECValidateInfo 
    {
        public string LEC_License_NO { get; set; }
        public string LEC_Name { get;set;}
        public string LEC_MOBILE_NO { get; set; }
        public string LEC_Email { get; set; }
        public string LEC_Validity_Info { get; set; }
        public string Message { get; set; }
        public bool IsValid { get; set; }
        public bool IsUpdated { get; set; }
    }
}