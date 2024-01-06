using Sitecore.Diagnostics;
using Sitecore.Feature.BlobStorage.Helpers;
using Sitecore.Feature.BlobStorage.Models;
using Sitecore.Feature.BlobStorage.Services;
using System;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;
using Sitecore.IO;

namespace Sitecore.Feature.BlobStorage.Controllers
{
    public class BlobStorageController : Controller
    {
        readonly IBlobStorageService _blobStorageService;
        readonly FileValidatorHelper fileValidatorHelper = new FileValidatorHelper();

        public ActionResult Index()
        {
            return View();
        }


        public BlobStorageController(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        [HttpPost]
        public string UploadFileToBlobStorage(BlobStorageModel blobstoragemodel)
        {
            Log.Info("UploadFileToBlobStorage Start" + blobstoragemodel.Filename + blobstoragemodel.File, this);
            #region Payload
            /*
             {
                "Filename";
                "File";
                "BlobStorageName";
                "ContainerName";
             }
            */
            #endregion

            Stream fs = blobstoragemodel.File.InputStream;
            byte[] UploadedFile = new byte[0];
            using (BinaryReader br = new BinaryReader(fs))
            {
                UploadedFile = br.ReadBytes((Int32)fs.Length);
            }
                
            string documentFileName = fileValidatorHelper.GetValidFileName(blobstoragemodel.Filename);
            string extension = Path.GetExtension(documentFileName);
            string contentType = fileValidatorHelper.GetMimeTypeByWindowsRegistry(extension);
            string uploadedfile_extension = Path.GetExtension(documentFileName);
            string uploadedfilewith_extension = Path.GetFileNameWithoutExtension(documentFileName);
            string uploadedfilefileName = uploadedfilewith_extension.Replace(" ", "-") + "-" + DateTime.Now.ToString("yyyyMMddHHmmsss") + uploadedfile_extension;

            #region Using Azure Blob Storage Service
            var isUploaded = _blobStorageService.UploadFileToBlobBtyes(UploadedFile, uploadedfilefileName, contentType, blobstoragemodel.BlobStorageName, blobstoragemodel.ContainerName);
            if (isUploaded.Status)
            {
                Log.Info("Azure UploadFileToBlob Response Status: " + isUploaded.Status, this);
                return isUploaded.URL;
            }
            else
            {
                Log.Info("Azure UploadFileToBlob Response Status: " + isUploaded.Status, this);
                return "failed";
            }
            #endregion          
        }
    }
}