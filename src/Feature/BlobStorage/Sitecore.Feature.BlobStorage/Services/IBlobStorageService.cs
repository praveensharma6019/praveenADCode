using static Sitecore.Feature.BlobStorage.Models.AzureBlobStorageModel;

namespace Sitecore.Feature.BlobStorage.Services
{
    public interface IBlobStorageService
    {
        UploadFileReponseModel UploadFileToBlobBtyes(byte[] UploadedFile, string fileName, string contentType, string BlobStorageName, string ContainerName);
    }
}