using Azure.Storage.Blobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using static Sitecore.AGELPortal.Website.Models.AzureBlobStorageModel;

namespace Sitecore.AGELPortal.Website.Helpers
{
    public class AzureBlobStorageServices
    {
        public byte[] GetFileFromAzure(string fileName)
        {
            string connection = ConfigurationManager.ConnectionStrings["AGELSafetyPortalABSConnection"].ConnectionString;
            //string DirectoryName = "AGELSafetyPortalDocuments";
            string containerString = "agel-safety-portal";
          //  string connection = "DefaultEndpointsProtocol=https;AccountName=adanistagesa;AccountKey=LSeI5Vz2sNGFqtd2vTTBIZ9Bjc29iekcZM8pGiLF6qoaIk+7wLZPwd4GRdO/dvz5zqeEkFZcLExGoO0ci2dpEw==;EndpointSuffix=core.windows.net";//ConfigurationManager.ConnectionStrings["AGELSafetyPortalABSConnection"].ConnectionString;
         //   string containerString = "agel-safety-portal";
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
            return null;  // returns empty array
        }
        public UploadFileReponseModel UploadFileToBlobBtyes(byte[] UploadedFile, string fileName, string contentType)
        {
            UploadFileReponseModel uploadFileReponse = new UploadFileReponseModel();
            try
            {
                /* string fileName = UploadedFile.FileName + "-" + DateTime.Now.ToString("yyyyMMddHHmmsss"); //change it to filename+yyyyMMddHHmmsss+extension*/
                StorageServices blobstorage = new StorageServices();
                CloudBlobContainer blobContainer = blobstorage.GetCloudBlobContainer(fileName);
                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(fileName);
                if (blob.Exists())
                {
                    uploadFileReponse.StatusMsg = "File name already exists.";
                    uploadFileReponse.Status = false;
                    return uploadFileReponse;
                }
                else
                {
                    blob.Properties.ContentType = contentType;
                    var status = blob.UploadFromByteArrayAsync(UploadedFile, 0, UploadedFile.Length);
                    status.Wait(2000);
                    if (status.IsCompleted)
                    {
                        uploadFileReponse.URL = string.Concat(new string[] { "https://adanistagesa.blob.core.windows.net/agel-safety-portal/", fileName });
                        uploadFileReponse.Status = true;
                        return uploadFileReponse;
                    }
                    else
                    {
                        uploadFileReponse.StatusMsg = "File upload failed. Status: " + status.IsFaulted + " | " + status.IsCanceled;
                        uploadFileReponse.Status = false;
                        return uploadFileReponse;
                    }
                }
            }
            catch (Exception ex)
            {
                uploadFileReponse.StatusMsg = "Something has been wrong. Exception: " + ex.Message;
                uploadFileReponse.Status = false;
                return uploadFileReponse;
            }
        }
        public UploadFileReponseModel UploadFileToBlob(HttpPostedFileBase UploadedFile,string contentType)
        {
            UploadFileReponseModel uploadFileReponse = new UploadFileReponseModel();
            try
            {
                Log.Info("Azure UploadFileToBlob Start: ", this);
                //added by neeraj yadav - updating filename to filename+yyyyMMddHHmmsss+extension
                //string fileName = UploadedFile.FileName + "-" + DateTime.Now.ToString("yyyyMMddHHmmsss"); //change it to filename+yyyyMMddHHmmsss+extension
                string uploadedfile_extension = Path.GetExtension(UploadedFile.FileName);
                string uploadedfilewith_extension = Path.GetFileNameWithoutExtension(UploadedFile.FileName);
                string uploadedfile_Name = UploadedFile.FileName;               
                string fileName = uploadedfilewith_extension.Trim().Replace(" ","-") + "-" + DateTime.Now.ToString("yyyyMMddHHmmsss") + uploadedfile_extension;
                Log.Info("Azure UploadFileToBlob File Name: "+ fileName, this);
                StorageServices blobstorage = new StorageServices();
                CloudBlobContainer blobContainer = blobstorage.GetCloudBlobContainer(fileName);
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
        public DeleteFileReponseModel DeleteFileBlob(string fileName)
        {
            DeleteFileReponseModel deleteFileReponse = new DeleteFileReponseModel();
            try
            {
                StorageServices blobstorage = new StorageServices();
                CloudBlobContainer blobContainer = blobstorage.GetCloudBlobContainer(fileName);
                var blob = blobContainer.GetBlockBlobReference(fileName);
                try
                {
                    if (blob.Exists())
                    {
                        if (blob.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots))
                        {
                            deleteFileReponse.StatusMsg = "File deleted successfully.";
                            deleteFileReponse.Status = true;
                            return deleteFileReponse;
                        }
                        else
                        {
                            deleteFileReponse.StatusMsg = "File delete failed.";
                            deleteFileReponse.Status = false;
                            return deleteFileReponse;
                        }

                    }
                    else
                    {
                        deleteFileReponse.StatusMsg = "File does not exist.";
                        deleteFileReponse.Status = false;
                        return deleteFileReponse;
                    }

                }
                catch (Exception ex)
                {
                    deleteFileReponse.StatusMsg = "Something has been wrong. Exception: " + ex.Message;
                    deleteFileReponse.Status = false;
                    return deleteFileReponse;
                }
            }
            catch (Exception ex)
            {
                deleteFileReponse.StatusMsg = "Something has been wrong. Exception: " + ex.Message;
                deleteFileReponse.Status = false;
                return deleteFileReponse;
            }
        }
        public RenameFileReponseModel RenameFileBlob(string oldFileName, string newFileName)
        {
            RenameFileReponseModel renameFileReponse = new RenameFileReponseModel();
            try
            {
                StorageServices blobstorage = new StorageServices();
                CloudBlobContainer blobContainer = blobstorage.GetCloudBlobContainer(oldFileName);
                var newBlob = blobContainer.GetBlockBlobReference(newFileName);
                if (!newBlob.Exists())
                {
                    CloudBlockBlob oldBlob = blobContainer.GetBlockBlobReference(oldFileName);

                    if (oldBlob.Exists())
                    {
                        renameFileReponse.IsNewCreated = !string.IsNullOrEmpty(newBlob.StartCopyFromBlob(oldBlob)) ? true : false;
                        renameFileReponse.IsExistingDeleted = oldBlob.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots);
                        renameFileReponse.URL = string.Concat(new string[] { "https://adanistagesa.blob.core.windows.net/agel-safety-portal/", newFileName });
                        if (renameFileReponse.IsNewCreated && renameFileReponse.IsExistingDeleted)
                        {
                            renameFileReponse.StatusMsg = "Renamed Successfully.";
                            renameFileReponse.Status = true;
                            return renameFileReponse;
                        }
                        else
                        {
                            renameFileReponse.StatusMsg = "File rename failed.";
                            renameFileReponse.Status = false;
                            return renameFileReponse;
                        }
                    }
                    else
                    {
                        renameFileReponse.StatusMsg = "File does not exist.";
                        renameFileReponse.Status = false;
                        return renameFileReponse;
                    }
                }
                else
                {
                    renameFileReponse.StatusMsg = "File name already exists.";
                    renameFileReponse.Status = false;
                    return renameFileReponse;
                }
            }
            catch (Exception ex)
            {
                renameFileReponse.StatusMsg = "Something has been wrong. Exception: " + ex.Message;
                renameFileReponse.Status = false;
                return renameFileReponse;
            }
        }        
    }
    public class StorageServices
    {
        public CloudBlobContainer GetCloudBlobContainer(string filename)
        {
            string connection = ConfigurationManager.ConnectionStrings["AGELSafetyPortalABSConnection"].ConnectionString;
            //string DirectoryName = "AGELSafetyPortalDocuments";
            string containerString = "agel-safety-portal";
            CloudStorageAccount storage = CloudStorageAccount.Parse(connection);
            CloudBlobClient client = storage.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(containerString);
            // CloudBlockBlob blob = container.GetBlockBlobReference(filename);
            return container;
        }
    }
}