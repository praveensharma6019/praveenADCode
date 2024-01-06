using Azure.Storage.Blobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Sitecore.Diagnostics;
using System;
using System.Configuration;
using System.IO;
using System.Web;
using static Sitecore.Feature.BlobStorage.Models.AzureBlobStorageModel;

namespace Sitecore.Feature.BlobStorage.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        public byte[] GetFileFromAzure(string fileName, string BlobStorageName, string ContainerName)
        {
            if (!string.IsNullOrEmpty(BlobStorageName) && !string.IsNullOrEmpty(ContainerName))
            {                
                string connection = ConfigurationManager.ConnectionStrings[BlobStorageName].ConnectionString;                
                string containerString = ContainerName;
                BlobServiceClient blobServiceClient = new BlobServiceClient(connection);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerString);
                BlobClient blobClient = containerClient.GetBlobClient(fileName);
                if (blobClient.ExistsAsync().Result)
                {
                    using (var ms = new MemoryStream())
                    {
                        blobClient.DownloadTo(ms);
                        return ms.ToArray();
                    }
                }
            }
            return null;  // returns empty array
        }
        public UploadFileReponseModel UploadFileToBlobBtyes(byte[] UploadedFile, string fileName, string contentType, string BlobStorageName, string ContainerName)
        {
            Log.Info("Inside UploadFileToBlobBtyes", this);
            UploadFileReponseModel uploadFileReponse = new UploadFileReponseModel();
            try
            {
                StorageServices blobstorage = new StorageServices();
                Log.Info("Created Storage service object blobstorage " + blobstorage, this);
                CloudBlobContainer blobContainer = blobstorage.GetCloudBlobContainer(BlobStorageName, ContainerName);
                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(fileName);
                Log.Info("Get Block of Blob Reference blob " + blob, this);
                if (blob.Exists())
                {
                    Log.Info("File name already exists.", this);
                    uploadFileReponse.StatusMsg = "File name already exists.";
                    uploadFileReponse.Status = false;
                    return uploadFileReponse;
                }
                else
                {

                    blob.Properties.ContentType = contentType;
                    var status = blob.UploadFromByteArrayAsync(UploadedFile, 0, UploadedFile.Length);
                    Log.Info("UploadFromByteArrayAsync status" + status, this);
                    Log.Info("UploadFromByteArrayAsync status" + status.IsCompleted, this);
                    status.Wait(2000);
                    if (status.IsCompleted)
                    {
                        string fileBlobURL = "https://adanistagesa.blob.core.windows.net/" + ContainerName + "/";
                        //uploadFileReponse.URL = string.Concat(new string[] { "https://adanistagesa.blob.core.windows.net/agel-safety-portal/", fileName });
                        uploadFileReponse.URL = string.Concat(new string[] { fileBlobURL, fileName });
                        uploadFileReponse.Status = true;
                        Log.Info("Uplod URL " + uploadFileReponse.URL, this);
                        return uploadFileReponse;
                    }
                    else
                    {
                        uploadFileReponse.StatusMsg = "File upload failed. Status: " + status.IsFaulted + " | " + status.IsCanceled;
                        uploadFileReponse.Status = false;
                        Log.Info("Uplod failed", this);
                        return uploadFileReponse;
                    }
                }
            }
            catch (Exception ex)
            {
                uploadFileReponse.StatusMsg = "Something has been wrong. Exception: " + ex.Message;
                uploadFileReponse.Status = false;
                Log.Error("exception " + ex, this);
                return uploadFileReponse;
            }
        }
        public UploadFileReponseModel UploadFileToBlob(HttpPostedFileBase UploadedFile, string contentType, string BlobStorageName, string ContainerName)
        {
            UploadFileReponseModel uploadFileReponse = new UploadFileReponseModel();
            try
            {
                Log.Info("Azure UploadFileToBlob Start: ", this);

                string uploadedfile_extension = Path.GetExtension(UploadedFile.FileName);
                string uploadedfilewith_extension = Path.GetFileNameWithoutExtension(UploadedFile.FileName);
                string uploadedfile_Name = UploadedFile.FileName;
                string fileName = uploadedfilewith_extension.Trim().Replace(" ", "-") + "-" + DateTime.Now.ToString("yyyyMMddHHmmsss") + uploadedfile_extension;
                Log.Info("Azure UploadFileToBlob File Name: " + fileName, this);
                StorageServices blobstorage = new StorageServices();
                CloudBlobContainer blobContainer = blobstorage.GetCloudBlobContainer(BlobStorageName, ContainerName);
                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(fileName);

                if (blob.Exists())
                {
                    Log.Info("Inside blob.Exists() IF", this);
                    uploadFileReponse.StatusMsg = "File name already exists.";
                    uploadFileReponse.Status = false;
                    Log.Info("uploadFileReponse: " + uploadFileReponse, this);
                    return uploadFileReponse;
                }
                else
                {
                    Log.Info("Inside blob.Exists() Else", this);
                    blob.Properties.ContentType = contentType;
                    var status = blob.UploadFromStreamAsync(UploadedFile.InputStream);
                    Log.Info("Status: " + status, this);
                    status.Wait();
                    if (status.IsCompleted)
                    {
                        Log.Info("Status Completed" + status.IsCompleted, this);
                        uploadFileReponse.URL = string.Concat(new string[] { "https://adanistagesa.blob.core.windows.net/agel-safety-portal/", fileName });
                        uploadFileReponse.Status = true;
                        Log.Info("uploadFileReponse" + uploadFileReponse, this);
                        return uploadFileReponse;
                    }
                    else
                    {
                        Log.Info("Status Completed" + status.IsCompleted, this);
                        uploadFileReponse.StatusMsg = "File upload failed. Status: " + status.IsFaulted + " | " + status.IsCanceled;
                        uploadFileReponse.Status = false;
                        Log.Info("uploadFileReponse" + uploadFileReponse, this);
                        return uploadFileReponse;
                    }
                }
            }
            catch (Exception ex)
            {
                uploadFileReponse.StatusMsg = "Something has been wrong. Exception: " + ex.Message + " StackTrace" + ex.StackTrace;
                uploadFileReponse.Status = false;
                return uploadFileReponse;
            }
        }

    }

    public class StorageServices
    {
        public CloudBlobContainer GetCloudBlobContainer(string BlobStorageName, string ContainerName)
        {
            
            string connection = ConfigurationManager.ConnectionStrings[BlobStorageName].ConnectionString;
            string containerString = ContainerName;
            if (!string.IsNullOrEmpty(connection) && !string.IsNullOrEmpty(containerString))
            {
                CloudStorageAccount storage = CloudStorageAccount.Parse(connection);
                CloudBlobClient client = storage.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(containerString);
                return container;
            }
            return null;
            
        }
    }
}