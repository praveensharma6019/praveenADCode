using System.Collections.Generic;
using System.Net;

namespace Sitecore.Farmpik.Website.Models
{
    public class KnowledgeHubTabsModel
    {
        public int Count { get; set; }
        public bool Status { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public List<KnowledgeHubTabModel> Payload { get; set; }
    }
    public class KnowledgeHubTabModel
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }
}