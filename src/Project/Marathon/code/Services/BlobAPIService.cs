using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Sitecore.Marathon.Website.Services
{
    public class BlobAPIService
    {
        public string BlobAPI(HttpPostedFileBase UploadedFile)
        {
            try
            {
                Log.Info("Marathon BlobAPIService call start", UploadedFile.FileName);

                using (var client = new HttpClient())
                {
                    var apiUrl = DictionaryPhraseRepository.Current.Get("/Blob Storage/API", "");
                    string BlobStorageName = DictionaryPhraseRepository.Current.Get("/Blob Storage/BlobStorageName", "");
                    string ContainerName = DictionaryPhraseRepository.Current.Get("/Blob Storage/ContainerName", "");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    using (var content = new MultipartFormDataContent())
                    {
                        UploadedFile.InputStream.Position = 0;
                        var fileContent = new StreamContent(UploadedFile.InputStream);
                        fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                        {
                            Name = "File",
                            FileName = UploadedFile.FileName
                        };
                        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(UploadedFile.ContentType);
                        content.Add(fileContent);
                        content.Add(new StringContent(UploadedFile.FileName), "Filename");
                        content.Add(new StringContent(BlobStorageName), "BlobStorageName");
                        content.Add(new StringContent(ContainerName), "ContainerName");
                        var response = client.PostAsync(apiUrl, content).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = response.Content.ReadAsStringAsync().Result.ToString();
                            return responseContent;
                        }
                    }
                    return null;
                }
            }
            catch(Exception ex)
            {
                Log.Error("Marathon BlobAPIService Exception ", UploadedFile.FileName);
                return null;
            }
            
           
        }
    }
}