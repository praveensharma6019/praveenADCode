using Sitecore.AdaniConneX.Website.Helpers;
using Sitecore.AdaniConneX.Website.Models;
using Sitecore.AdaniConneX.Website.Provider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Sitecore.IO;
using Sitecore.Foundation.Dictionary.Repositories;

namespace Sitecore.AdaniConneX.Website.Providers
{
    public class JoinUsFormProvider : Controller
    {
        public async Task<string> JoinUsForm(AdaniConnex_JoinUsForm_Model m)
        {
            
            string EncryptionKey = "Tl;jld@456763909QPwOeiRuTy873XY7";
            string EncryptionIV = "CEIVRAJWquG8iiMw";

            string result = "1";
            AdaniConnexContactFormDataContext rdb = new AdaniConnexContactFormDataContext();
            AdaniConnex_JoinusForm r = new AdaniConnex_JoinusForm();
            r.Id = Guid.NewGuid();
            r.Name = m.Name;
            r.Email = EncryptionServiceHelper.EncryptString(EncryptionKey, m.Email, EncryptionIV);
            r.Contact = EncryptionServiceHelper.EncryptString(EncryptionKey, m.Contact, EncryptionIV);

            #region Blob Storage API Call               
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
                    var fileContent = new StreamContent(m.CVFile.InputStream);

                    // Set the 'Content-Disposition' header with the file name and parameter name "File"
                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                    {
                        Name = "File",
                        FileName = m.CVFile.FileName
                    };

                    // Set the 'Content-Type' header for the file content
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(m.CVFile.ContentType);

                    // Add the file content to the multi-part form data
                    content.Add(fileContent);

                    // Add the customer name as another form field
                    content.Add(new StringContent(m.CVFile.FileName), "Filename");
                    content.Add(new StringContent(BlobStorageName), "BlobStorageName");
                    content.Add(new StringContent(ContainerName), "ContainerName");

                    // Make the API call and pass the 'content' in the request
                    var response = client.PostAsync(apiUrl, content).Result;

                    // Handle the API response as needed.
                    if (response.IsSuccessStatusCode == false)
                    {
                        result = "406";
                        return result;
                    }
                    else
                    {
                        var responseContent = response.Content.ReadAsStringAsync().Result.ToString();
                        r.CVName = responseContent;
                    }
                }                
            }
            #endregion
            r.CVFile = new byte[0];
            r.FormType = m.FormType;
            r.FormUrl = m.FormUrl;
            r.SubmittedDate = DateTime.Now;
            rdb.AdaniConnex_JoinusForms.InsertOnSubmit(r);
            rdb.SubmitChanges();
            //Send Email to user
            if (m.SendEmail == "true")
            {
                SendThankyouEmailService emailThankyouNotification = new SendThankyouEmailService();
                #region Notification Email For User
                /*
                Sitecore.Data.Items.Item ThankyouTemplate = Context.Database.GetItem(Templates.EmailTemplate.Datasource.ThankyouTemplate);
                string messagebody = ThankyouTemplate.Fields[Templates.EmailTemplate.DatasourceFields.Body].Value;
                messagebody = messagebody.Replace("[Name]", m.Name);

                var emailServiceStatus = await emailThankyouNotification.ThankyouNotification(m.Email, messagebody, ThankyouTemplate);
                if (!System.Convert.ToBoolean(emailServiceStatus))
                {
                    result = "409";
                    return result;
                }
                 */
                #endregion
                #region Lead Data Send To Business
                Sitecore.Data.Items.Item LeadDataTemplate = Context.Database.GetItem(Templates.EmailTemplate.Datasource.LeadDataTemplate);
                string leadDataBody = LeadDataTemplate.Fields[Templates.EmailTemplate.DatasourceFields.Body].Value;
                leadDataBody = leadDataBody.Replace("[Name]", m.Name);
                leadDataBody = leadDataBody.Replace("[Email]", m.Email);
                leadDataBody = leadDataBody.Replace("[Contact]", m.Contact);
                leadDataBody = leadDataBody.Replace("[CV]", r.CVName);
                leadDataBody = leadDataBody.Replace("[FormType]", m.FormType);
                leadDataBody = leadDataBody.Replace("[Company]", "");
                leadDataBody = leadDataBody.Replace("[Message]", "");
                leadDataBody = leadDataBody.Replace("[Country]", "");

                var isEmailSent1 = await emailThankyouNotification.ThankyouNotification(m.Email, leadDataBody, LeadDataTemplate);
                if (!System.Convert.ToBoolean(isEmailSent1))
                {
                    result = "409";
                    return result;
                }
                #endregion
            }
            return result;
        }
    }
}