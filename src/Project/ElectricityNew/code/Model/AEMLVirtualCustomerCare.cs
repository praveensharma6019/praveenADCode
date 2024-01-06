namespace Sitecore.ElectricityNew.Website.Model
{
    using System;

    [Serializable]
    public class AEMLVirtualCustomerCare
    {
        public string CANumber { get; set; }
        public string Captcha { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
        public string CAType { get; set; }
        public string Priority { get; set; }
        public string Preferred_Language { get; set; }
        public string Interaction_Type { get; set; }
        public string UCID { get; set; }
        public string VCCOTP { get; set; }
        public string Message { get; set; }
    }



    public class VCCResponse
    {
        public string actionType { get; set; }
        public object announcement { get; set; }
        public object menuAction { get; set; }
        public Exitaction exitAction { get; set; }
        public object queue { get; set; }
        public object errorMessage { get; set; }
        public object menuID { get; set; }
        public Calldata callData { get; set; }
    }

    public class Exitaction
    {
        public string exitNode { get; set; }
    }

    public class Calldata
    {
        public object dnis { get; set; }
        public object callerID { get; set; }
        public object language { get; set; }
        public Sessionproperties sessionProperties { get; set; }
        public object userId { get; set; }
    }

    public class Sessionproperties
    {
        public string EmailID { get; set; }
        public string sessionData { get; set; }
        public string Interaction_Type { get; set; }
        public string rawResponse { get; set; }
        public string ModuleName { get; set; }
        public string Channel_Info { get; set; }
        public string errorCode { get; set; }
        public string Repeat { get; set; }
        public string CDN { get; set; }
        public string UCID { get; set; }
        public string CA_Type { get; set; }
        public string HostStatus { get; set; }
        public string SessionId { get; set; }
        public string CA { get; set; }
        public string SlotDetails { get; set; }
        public string Priority { get; set; }
        public string Channel { get; set; }
        public string Mobile { get; set; }
        public string Preferred_Language { get; set; }
        public string Skill { get; set; }
        public string NextNodeID { get; set; }
        public string customerid { get; set; }
        public string isSessionDataUsed { get; set; }
    }

    public class AdhocVccModal
    {
        public int slno { get; set; }
        public string skillsetid { get; set; }
        public string skillset { get; set; }
        public int agentavl { get; set; }
        public int agentinsrv { get; set; }
        public int agentonsktcalls { get; set; }
        public int agentnotready { get; set; }
        public int callwaiting { get; set; }
        public int expectedtime { get; set; }
        public int skillsetstate { get; set; }
        public int agentonothersktcall { get; set; }
    }

}