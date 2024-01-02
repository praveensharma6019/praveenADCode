using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.File;
using Sitecore.ExperienceForms.Data;
using Sitecore.ExperienceForms.Data.Entities;
using Sitecore.IO;

namespace Adani.BAU.AdaniWelspunSXA.Project.FileStorageProviders
{
    public class AzureBlobStorageFileStorageProvider : IFileStorageProvider
    {
        private const string OriginalFileNameKey = "originalFileName";
        private const string CommitedKey = "commited";

        public string ConnectionString => Sitecore.Configuration.Settings.GetSetting("AzureBlobStorageFileStorageProvider.ConnectionString");
        public string BlobContainer => Sitecore.Configuration.Settings.GetSetting("AzureBlobStorageFileStorageProvider.BlobContainer");
        public string Folder => Sitecore.Configuration.Settings.GetSetting("AzureBlobStorageFileStorageProvider.Folder");

        public Guid StoreFile(Stream file, string fileName)
        {
            var id = Guid.NewGuid();
            string extension = Path.GetExtension(fileName);
            string contentType = GetMimeTypeByWindowsRegistry(extension);          
            var fileReference = GetFileReference(id);      
            var storageAccount = CloudStorageAccount.Parse(ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(BlobContainer);
            var blockBlob = blobContainer.GetBlockBlobReference(fileReference);
            var url = string.Concat(new string[] { "https://adanistagesa.blob.core.windows.net/agel-safety-portal/", id.ToString() });
            blockBlob.Properties.ContentType = contentType;
            file.Position = 0;          
            byte[] bindata = ReadFully(file);
            string filecontent = System.Convert.ToBase64String(bindata);
            if (filecontent.StartsWith("JVBER") == false && filecontent.StartsWith("UEsDB") == false && filecontent.StartsWith("0M8R4") == false)
            {
                id = new Guid("{00000000-0000-0000-0000-000000000000}");
                return id;
            }
            // var status = blockBlob.UploadFromByteArrayAsync(UploadedFile, 0, UploadedFile.Length);
            blockBlob.UploadFromByteArray(bindata,0, bindata.Length);
            blockBlob.Metadata.Add(OriginalFileNameKey, fileName);
            blockBlob.Metadata.Add(CommitedKey, bool.FalseString);
            blockBlob.SetMetadata();
            return id;
        }

        public static byte[] ReadFully(Stream input)
        {
            try
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        public static string GetMimeTypeByWindowsRegistry(string fileNameOrExtension)
        {
            string mimeType = "";
            string ext = (fileNameOrExtension.Contains(".")) ? System.IO.Path.GetExtension(fileNameOrExtension).ToLower() : "." + fileNameOrExtension;
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null) mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        public Sitecore.ExperienceForms.Data.Entities.StoredFile GetFile(Guid fileId)
        {
            var blockBlob = GetCloudBlockBlob(fileId);
            var storedFile = new Sitecore.ExperienceForms.Data.Entities.StoredFile
            {
                FileInfo = new StoredFileInfo
                {
                    FileId = fileId,
                    FileName = blockBlob.Metadata[OriginalFileNameKey]
                },
                File = GetFileStream(blockBlob)
            };
            return storedFile;
        }

        public void DeleteFiles(IEnumerable<Guid> fileIds)
        {
            foreach (var fileId in fileIds)
            {
                var blockBlob = GetCloudBlockBlob(fileId);
                blockBlob.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots);
            }
        }

        public void CommitFiles(IEnumerable<Guid> fileIds)
        {
            foreach (var fileId in fileIds)
            {
                var blockBlob = GetCloudBlockBlob(fileId);
                blockBlob.Metadata[CommitedKey] = bool.TrueString;
                blockBlob.SetMetadata();
            }
        }

        public void Cleanup(TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }

        private string GetFileReference(Guid id)
        {
            //return Path.Combine(Folder, id.ToString());
            return Path.Combine(id.ToString());
        }

        private static Stream GetFileStream(CloudBlob blockBlob)
        {
            var fileStream = new MemoryStream();
            blockBlob.DownloadToStream(fileStream);
            fileStream.Position = 0;
            return fileStream;
        }

        private CloudBlockBlob GetCloudBlockBlob(Guid fileId)
        {
            var storageAccount = CloudStorageAccount.Parse(ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(BlobContainer);
            var blockBlob = blobContainer.GetBlockBlobReference(Path.Combine(fileId.ToString()));
            blockBlob.FetchAttributes();
            return blockBlob;
        }
    }
}