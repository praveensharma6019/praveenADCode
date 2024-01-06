using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System.IO;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using System.Web;

namespace Sitecore.ICAN.Website.Services
{
    public class BlobStorageService
    {
        public CloudBlobContainer GetCloudBlobContainer()
        {
            string connection = ConfigurationManager.ConnectionStrings["ICANConnection"].ConnectionString;
            string fileName = "ICANVideos";
            string containerString = "ican";
            CloudStorageAccount storage = CloudStorageAccount.Parse(connection);
            CloudBlobClient client = storage.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(containerString);
            CloudBlockBlob blob = container.GetBlockBlobReference(fileName);
            return container;
            }
       
    }
}