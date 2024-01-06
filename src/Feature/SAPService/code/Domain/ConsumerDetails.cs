using SI_Contactlog_WebService_website;
using System.Collections.Generic;

namespace SapPiService.Domain
{
    public class ConsumerDetails
    {
        public string Name { get; set; }    //BP_NAME
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string CANumber { get; set; }
        public string MeterNumber { get; set; }//DEVICE_SR_NUMBER
        public string ConnectionType { get; set; }//RATE_CAT

        public string HouseNumber { get; set; }//HOUSE_NUMBER
        public string Street { get; set; }//STREET
        public string Street2 { get; set; }//Area
        public string Street3 { get; set; }//Landmark
        public string City { get; set; }
        public string PinCode { get; set; }
        public string Vertrag_Contract { get; set; }
        public string MOVEOUTFLAG { get; set; }

        public string BP_Number { get; set; }
        public string BP_Type { get; set; }
        public string CON_OBJ_NO { get; set; }
        public string MRU { get; set; }
        public string FUNC_DESCR { get; set; }
        public string DESC_CON_OBJECT { get; set; }
        public string REGSTRGROUP { get; set; }
    }

    public class UpdatePANGSTDetailsResult
    {
        public string FLAG { get; set; }
        public string LGNUM { get; set; }
        public string MESSAGE { get; set; }
    }

    public class ConsumerPANGSTDetails
    {
        public string CANumber { get; set; }
        public string PANNumber { get; set; }
        public string GSTNumber { get; set; }
    }

    public class ContactLogWebServiceResponse
    {
        public string CONTACT { get; set; }
        public string MESSAGE { get; set; }
        public SI_Contactlog_WebService_website.DT_Contactlog_respNOTICETEXT[] NOTICETEXT { get; set; }
        public SI_Contactlog_WebService_website.DT_Contactlog_respRETURN[] RETURN { get; set; }
        public SI_Contactlog_WebService_website.DT_Contactlog_respCONTACTOBJECTS_WITH_ROLE[] CONTACTOBJECTS_WITH_ROLE { get; set; }
    }

    public class ConsumerDetailsForSolar : ConsumerDetails
    {
        public string TATA_CONSUMER { get; set; }
        public string MOVE_OUT_DATE { get; set; }
        public string BILL_CLASS { get; set; }
        public string OUTSTANDING_AMOUNT { get; set; }
        public string SANCTIONED_LOAD { get; set; }
        public string Rate_Category { get; set; }
    }

    public class ConsumerOutstandingDetails
    {
        public string OUTSTANDING_AMOUNT { get; set; }
        public string SANCTIONED_LOAD { get; set; }
    }
}