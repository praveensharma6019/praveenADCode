using System.Web;

namespace Sitecore.Feature.BlobStorage.Models
{
    public class BlobStorageModel
    {
        public string Filename { get; set; }
        public HttpPostedFileBase File { get; set; }
        public string BlobStorageName { get; set; }
        public string ContainerName { get; set; }
    }
}