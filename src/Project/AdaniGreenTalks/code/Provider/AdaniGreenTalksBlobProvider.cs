using Sitecore.AdaniGreenTalks.Website.Helpers;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Sitecore.AdaniGreenTalks.Website.Provider
{
    public class AdaniGreenTalksBlobProvider
    {
        AzureBlobStorageServices objazureBlobStorageServices = new AzureBlobStorageServices();
        public static string GetMimeTypeByWindowsRegistry(string fileNameOrExtension)
        {
            string mimeType = "";
            string ext = (fileNameOrExtension.Contains(".")) ? System.IO.Path.GetExtension(fileNameOrExtension).ToLower() : "." + fileNameOrExtension;
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null) mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        public string UploadFileToBlob(HttpPostedFileBase file, string Filename)
        {
            Log.Info("UploadFileToBlob start for file"+Filename,this);

            BinaryReader b = new BinaryReader(file.InputStream);
            byte[] bindata = b.ReadBytes(file.ContentLength);
            string filecontent = System.Convert.ToBase64String(bindata);

            var documentFileName = GetValidFileName(Filename);
            string extension1 = Path.GetExtension(documentFileName);
            string contentType = GetMimeTypeByWindowsRegistry(extension1);



            string uploadedfile_extension = Path.GetExtension(documentFileName);
            string uploadedfilewith_extension = Path.GetFileNameWithoutExtension(documentFileName);
            string uploadedfile_Name = documentFileName;
            string uploadedfilefileName = uploadedfilewith_extension.Replace(" ", "-") + "-" + DateTime.Now.ToString("yyyyMMddHHmmsss") + uploadedfile_extension;
            var isUploaded = objazureBlobStorageServices.UploadFileToBlobBtyes(bindata, uploadedfilefileName, contentType);
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
            b.Close();

        }
        public static string GetValidFileName(string fileName)
        {
            // remove any invalid character from the filename.   

            string ret = Regex.Replace(fileName.Trim(), "[^A-Za-z0-9_. ]+", "");
            return ret.Replace(" ", "-");
        }
    }

}