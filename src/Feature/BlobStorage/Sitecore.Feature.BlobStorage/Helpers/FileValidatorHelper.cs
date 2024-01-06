using Sitecore.Diagnostics;
using Sitecore.Feature.BlobStorage.Models;
using System.Text.RegularExpressions;

namespace Sitecore.Feature.BlobStorage.Helpers
{
    public class FileValidatorHelper
    {
        public string GetValidFileName(string fileName)
        {
            Log.Info("Inside GetValidFileName Start" + fileName, this);
            // remove any invalid character from the filename.  
            string ret = Regex.Replace(fileName.Trim(), "[^A-Za-z0-9_. ]+", "");
            return ret.Replace(" ", "-");
        }
        public string GetMimeTypeByWindowsRegistry(string fileNameOrExtension)
        {
            Log.Info("Inside GetMimeTypeByWindowsRegistry Start" + fileNameOrExtension, this);
            string mimeType = "";
            string ext = (fileNameOrExtension.Contains(".")) ? System.IO.Path.GetExtension(fileNameOrExtension).ToLower() : "." + fileNameOrExtension;
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null) mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }
    }
}