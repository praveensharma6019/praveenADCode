using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;

namespace Sitecore.Ports.Website.Services
{
    public class BlobAPIService
    {
        public string BlobAPI(HttpPostedFileBase File)
        {

            
            using (var client = new HttpClient())
            {
                var address = System.Web.HttpContext.Current.Request.Url;
                var hostAddress = address.Scheme + "://" + address.Host;
                var apiUrl = hostAddress + "/api/BlobStorage/UploadFileToBlobStorage";
                string BlobStorageName = "GreenTalkBlobStorage";
                string ContainerName = "agel-safety-portal";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               
                using (var content = new MultipartFormDataContent())
                {
                    // Create the file content using StreamContent
                    var fileContent = new StreamContent(File.InputStream);

                    // Set the 'Content-Disposition' header with the file name and parameter name "File"
                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                    {
                        Name = "File",
                        FileName = File.FileName
                    };

                    // Set the 'Content-Type' header for the file content
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(File.ContentType);

                    // Add the file content to the multi-part form data
                    content.Add(fileContent);

                    // Add the customer name as another form field
                    content.Add(new StringContent(File.FileName), "Filename");
                    content.Add(new StringContent(BlobStorageName), "BlobStorageName");
                    content.Add(new StringContent(ContainerName), "ContainerName");

                    // Make the API call and pass the 'content' in the request
                    var response = client.PostAsync(apiUrl, content).Result;

                    // Handle the API response as needed.
                    if (!response.IsSuccessStatusCode == false)
                    { 
                        var responseContent = response.Content.ReadAsStringAsync().Result.ToString();
                        return responseContent;
                    }
                }
            }
            return null;
        }
    }
}