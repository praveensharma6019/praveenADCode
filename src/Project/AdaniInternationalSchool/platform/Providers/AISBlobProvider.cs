using Project.AdaniInternationalSchool.Website.Helpers;
using Sitecore.Diagnostics;
using Sitecore.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Project.AdaniInternationalSchool.Website.Providers
{
    public class AISBlobProvider
    {
        AzureBlobStorageServices objazureBlobStorageServices = new AzureBlobStorageServices();

        public string UploadFileToBlob(byte[] fileData, string Filename)
        {
            Log.Info("UploadFileToBlob start for file" + Filename, this);

            var documentFileName = GetValidFileName(Filename);
            string extension1 = Path.GetExtension(documentFileName);
            string contentType = GetMimeTypeByWindowsRegistry(extension1);
            string uploadedfile_extension = Path.GetExtension(documentFileName);
            string uploadedfilewith_extension = Path.GetFileNameWithoutExtension(documentFileName);

            string uploadedfilefileName = uploadedfilewith_extension.Replace(" ", "-") + "-" + DateTime.Now.ToString("yyyyMMddHHmmsss") + uploadedfile_extension;
            var isUploaded = objazureBlobStorageServices.UploadFileToBlobBtyes(fileData, uploadedfilefileName, contentType);
            Log.Info("Azure UploadFileToBlob response: " + isUploaded, this);
            if (isUploaded.Status)
            {
                Log.Info("Inside If", this);
                Log.Info("Azure UploadFileToBlob Response Status: " + isUploaded.Status, this);
                Log.Info("Azure UploadFileToBlob Response Status Message: " + isUploaded.StatusMsg, this);
                return isUploaded.URL;
            }
            else
            {
                Log.Info("Inside Else", this);
                Log.Info("Azure UploadFileToBlob Response Status: " + isUploaded.Status, this);
                Log.Info("Azure UploadFileToBlob Response Status Message: " + isUploaded.StatusMsg, this);
                return "failed";
            }

        }
        public static string GetValidFileName(string fileName)
        {
            // remove any invalid character from the filename.   

            string ret = Regex.Replace(fileName.Trim(), "[^A-Za-z0-9_. ]+", "");
            return ret.Replace(" ", "-");
        }
        public static string GetMimeTypeByWindowsRegistry(string fileNameOrExtension)
        {
            string mimeType = "";
            string ext = (fileNameOrExtension.Contains(".")) ? System.IO.Path.GetExtension(fileNameOrExtension).ToLower() : "." + fileNameOrExtension;
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null) mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }
    }

}