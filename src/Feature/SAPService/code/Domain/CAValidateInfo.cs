using System.Collections.Generic;

namespace SapPiService.Domain
{
    public class CAValidateInfo : AccountBase
    {
        public string NAME_CustomerName { get;set;}
        public string MOBILE_NO { get; set; }
        public string SMTP_ADDR_Email { get; set; }
        public string TARIFTYP_Ratecategory { get; set; }
        public string BSTATUS { get; set; }
        public string OVERDUE_AMT { get; set; }
        public string INVALIDCAFLAG { get; set; }
        public string MOVEOUTFLAG { get; set; }
        public string DISCONNECTEDFLAG { get; set; }
        public string VIGILANCEFLAG { get; set; }
        public string NO_LAST_BILL_FLG { get; set; }
        public string LAST_BIL_AMND_FL { get; set; }
        public string CHNG_OVER_FLAG { get; set; }
        public string AKLASSE { get; set; }
        public string VAPLZ_WORK_CENTER { get; set; }
        public string DivisionName { get; set; }
        public string ZoneName { get; set; }
    }
}