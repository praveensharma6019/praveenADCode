using System.Collections.Generic;

namespace Adani.Foundation.Messaging.Models
{
    public class AmbujaSMSServicePayloadModel
    {
        public List<AmbujaSMSServicePayload> messages { get; set; }
        public TrackingDetails tracking { get; set; }
    }
    public class AmbujaSMSServicePayload
    {
        public string from { get; set; }
        public List<Recepiants> destinations { get; set; }
        public string text { get; set; }
        public DltParameterDetails regional { get; set; }
    }

    public class Recepiants 
    {
        public string to { get; set; }
    }

    public class DltParameterDetails
    {
        public DltParameters indiaDlt { get; set; }
    }
    public class DltParameters
    {
        public string principalEntityId { get; set; }
        public string contentTemplateId { get; set; }
    }
    public class TrackingDetails
    {
        public string track { get; set; }
    }
}
