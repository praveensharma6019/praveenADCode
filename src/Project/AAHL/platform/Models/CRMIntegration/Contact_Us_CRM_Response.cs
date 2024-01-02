using Newtonsoft.Json;

namespace Project.AAHL.Website.Models.CRMIntegration
{
    public class Contact_Us_CRM_Response
    {
        [JsonProperty("Case Number")]
        public string CaseNumber { get; set; }
        public string Message { get; set; }
    }
}