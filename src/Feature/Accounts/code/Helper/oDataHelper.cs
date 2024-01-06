using Sitecore.Feature.Accounts.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Xml;

namespace Sitecore.Feature.Accounts.Helper
{
    public static class oDataHelper
    {
        public static string GetData(string MethodUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                var authenticationBytes = Encoding.ASCII.GetBytes(Helper.ConfigurationHelper.ServiceUserName + ":" + Helper.ConfigurationHelper.ServicePassword);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                HttpResponseMessage response = client.GetAsync(ConfigurationHelper.ServiceBaseUrl + MethodUrl).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public static string GetDatabyUrl(string Url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                var authenticationBytes = Encoding.ASCII.GetBytes(Helper.ConfigurationHelper.ServiceUserName + ":" + Helper.ConfigurationHelper.ServicePassword);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                HttpResponseMessage response = client.GetAsync(Url).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public static string SaveEnquiry(Models.NewInq NewInq)
        {
            using (HttpClient client = new HttpClient())
            {
                var authenticationBytes = Encoding.ASCII.GetBytes(Helper.ConfigurationHelper.ServiceUserName + ":" + Helper.ConfigurationHelper.ServicePassword);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add("X-CSRF-TOKEN", "fetch");
                var url = Helper.ConfigurationHelper.oDataSaveInquiry;
                var uri = new Uri(url);

                HttpResponseMessage response = client.GetAsync(uri).Result;

                var oTokal = response.Headers.GetValues("X-CSRF-TOKEN").FirstOrDefault();
                var cookies = response.Headers.GetValues("Set-Cookie");

                var department = NewInq;
                string json = JsonConvert.SerializeObject(department, Newtonsoft.Json.Formatting.None);
                var content = new StringContent(json);

                var CompanyInfoData = Newtonsoft.Json.JsonConvert.DeserializeObject<NewInq>(json);

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("X-CSRF-TOKEN", oTokal);
                client.DefaultRequestHeaders.Add("Cookie", cookies);
                response = client.PostAsJsonAsync<NewInq>(url, department).Result;
                var resultContent = response.Headers.Location;
                var dd = response.Content.ReadAsStringAsync().Result;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(dd);
                return doc.ChildNodes[1].ChildNodes[5].ChildNodes[0].ChildNodes[23].InnerText;
            }


        }

        public static bool SaveEnquirySecond(Models.NewEnquirySecond NewEnquiry)
        {
            var authenticationBytes64Value = Helper.ServiceAuthenticationHelper.AddAuthorizationMethod();
            var FetchedTokenValue = Helper.ServiceAuthenticationHelper.FetchTokenValue(authenticationBytes64Value);

            using (HttpClient client = new HttpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationBytes64Value);
                client.DefaultRequestHeaders.Add("Token", FetchedTokenValue);
                var uri = new Uri(ConfigurationHelper.NewEnquirySecond);
                HttpResponseMessage response = client.PostAsJsonAsync<Models.NewEnquirySecond>(uri.ToString(), NewEnquiry).Result;
                var resultContent = response.Headers.Location;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }

        public static bool SaveEnquirySecondCNG(Models.NewInqSecondCNG NewEnquiry)
        {
            var authenticationBytes64Value = Helper.ServiceAuthenticationHelper.AddAuthorizationMethod();
            var FetchedTokenValue = Helper.ServiceAuthenticationHelper.FetchTokenValue(authenticationBytes64Value);

            using (HttpClient client = new HttpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationBytes64Value);
                client.DefaultRequestHeaders.Add("Token", FetchedTokenValue);
                var uri = new Uri(ConfigurationHelper.NewEnquirySecond);
                HttpResponseMessage response1 = client.PostAsJsonAsync<Models.NewInqSecondCNG>(uri.ToString(), NewEnquiry).Result;
                var resultContent = response1.Headers.Location;
                if (response1.IsSuccessStatusCode)
                    return true;
                else return false;
            }
        }

        public static NewEnquirySecond SecondEnquiryDetails(string EnquiryNo)
        {
            //VATP Code
            var authenticationBytes64Value = Helper.ServiceAuthenticationHelper.AddAuthorizationMethod();
            var FetchedTokenValue = Helper.ServiceAuthenticationHelper.FetchTokenValue(authenticationBytes64Value);

            using (HttpClient client = new HttpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                //VATP Code
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationBytes64Value);
                client.DefaultRequestHeaders.Add("Token", FetchedTokenValue);
                var uri = new Uri(ConfigurationHelper.NewEnquirySecondDetails);
                HttpResponseMessage response = client.GetAsync(string.Format(uri.ToString(), EnquiryNo)).Result;
                var resultContent = response.Headers.Location;
                if (response.IsSuccessStatusCode)
                {
                    //do nothing   
                }
                //HttpResponseMessage response = client.GetAsync(string.Format(ConfigurationHelper.NewEnquirySecondDetails, EnquiryNo)).Result;
                XmlDocument doc = new XmlDocument();
                var responsestring = response.Content.ReadAsStringAsync().Result;
                doc.LoadXml(responsestring);
                XmlNodeList list = doc.GetElementsByTagName("entry");
                Models.NewEnquirySecond newInqSecond = new NewEnquirySecond();
                var icount = list.Count - 1;
                newInqSecond.INQID = System.Convert.ToInt32(list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[0].InnerText);
                newInqSecond.TASKGRP = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[21].InnerText;
                newInqSecond.INQDATETM = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[1].InnerText;
                newInqSecond.FSOID = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[2].InnerText;
                newInqSecond.TITAL = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[3].InnerText;
                newInqSecond.CUSTNAME = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[4].InnerText;
                newInqSecond.CITY = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[5].InnerText;
                newInqSecond.POSTCODE = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[6].InnerText;
                newInqSecond.SOCIETY = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[7].InnerText;
                newInqSecond.AREA = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[8].InnerText;
                newInqSecond.HFNO = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[9].InnerText;
                newInqSecond.ADDR1 = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[10].InnerText;
                newInqSecond.ADDR2 = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[11].InnerText;
                newInqSecond.ESTATUS = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[12].InnerText;
                newInqSecond.INQTYPE = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[16].InnerText;
                newInqSecond.MOBNO = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[17].InnerText;
                newInqSecond.EMAILID = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[18].InnerText;
                newInqSecond.REMARK = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[13].InnerText;
                newInqSecond.SAPINQID = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[22].InnerText;
                newInqSecond.TASKID = System.Convert.ToInt32(list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[19].InnerText);
                newInqSecond.TASKNAME = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[20].InnerText;
                return newInqSecond;
            }
        }

        public static AdditionDataSave AdditionalDataGet(string EnquiryNo)
        {
            //VATP Code
            var authenticationBytes64Value = Helper.ServiceAuthenticationHelper.AddAuthorizationMethod();
            var FetchedTokenValue = Helper.ServiceAuthenticationHelper.FetchTokenValue(authenticationBytes64Value);

            using (HttpClient client = new HttpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                //VATP Code
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationBytes64Value);
                client.DefaultRequestHeaders.Add("Token", FetchedTokenValue);
                var uri = new Uri(ConfigurationHelper.AdditionDataGet);
                HttpResponseMessage response = client.GetAsync(string.Format(uri.ToString(), EnquiryNo)).Result;
                var resultContent = response.Headers.Location;
                if (response.IsSuccessStatusCode)
                {
                    //do nothing   
                }
                //HttpResponseMessage response = client.GetAsync(string.Format(ConfigurationHelper.AdditionDataGet, EnquiryNo)).Result;
                XmlDocument doc = new XmlDocument();
                var responsestring = response.Content.ReadAsStringAsync().Result;
                doc.LoadXml(responsestring);
                XmlNodeList list = doc.GetElementsByTagName("entry");
                if (list.Count > 0)
                {
                    Models.AdditionDataSave AdditionDataSave = new AdditionDataSave();
                    var icount = list.Count - 1;
                    AdditionDataSave.SAPINQID = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[0].InnerText;
                    AdditionDataSave.LPGCONSNO = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[1].InnerText;
                    AdditionDataSave.LPGCONSVALUE = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[2].InnerText;
                    AdditionDataSave.COMPANYNO = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[3].InnerText;
                    AdditionDataSave.COMPANYVALUE = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[4].InnerText;
                    AdditionDataSave.BATHROOMPOINT = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[5].InnerText;
                    AdditionDataSave.BATHROOMPOINTVALUE = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[6].InnerText;
                    AdditionDataSave.KITCHENPOINT = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[7].InnerText;
                    AdditionDataSave.KITCHENPOINTVALUE = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[8].InnerText;
                    AdditionDataSave.BUILDER = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[9].InnerText;
                    AdditionDataSave.BUILDERVALUE = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[10].InnerText;
                    AdditionDataSave.INDIVIDUALVALUE = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[11].InnerText;
                    AdditionDataSave.BULK = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[12].InnerText;
                    AdditionDataSave.BULKVALUE = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[13].InnerText;
                    AdditionDataSave.REAMRK = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[14].InnerText;
                    AdditionDataSave.CREATEDATE = list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[15].InnerText;


                    return AdditionDataSave;
                }
                else return null;
            }
        }

        public static bool UpdateEnquiryStatus(Models.NewEnquirySecond updateEnquiryStatus, int INQID)
        {
            var authenticationBytes64Value = Helper.ServiceAuthenticationHelper.AddAuthorizationMethod();
            var FetchedTokenValue = Helper.ServiceAuthenticationHelper.FetchTokenValue(authenticationBytes64Value);

            using (HttpClient client = new HttpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationBytes64Value);
                client.DefaultRequestHeaders.Add("Token", FetchedTokenValue);
                var uri = new Uri(ConfigurationHelper.UpdateInquiryStatus);
                HttpResponseMessage response = client.PostAsJsonAsync<Models.NewEnquirySecond>(string.Format(uri.ToString(), INQID), updateEnquiryStatus).Result;
                var resultContent = response.Headers.Location;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            //    HttpResponseMessage response = client.PutAsJsonAsync<Models.NewEnquirySecond>(string.Format(ConfigurationHelper.UpdateInquiryStatus, INQID), updateEnquiryStatus).Result;
        }


        public static bool AddEnquiryStatus(Models.AddInqStatus AddInqStatus)
        {
            var authenticationBytes64Value = Helper.ServiceAuthenticationHelper.AddAuthorizationMethod();
            var FetchedTokenValue = Helper.ServiceAuthenticationHelper.FetchTokenValue(authenticationBytes64Value);

            using (HttpClient client = new HttpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationBytes64Value);
                client.DefaultRequestHeaders.Add("Token", FetchedTokenValue);
                var uri = new Uri(ConfigurationHelper.AddInqStatus);
                HttpResponseMessage response = client.PostAsJsonAsync<Models.AddInqStatus>(uri.ToString(), AddInqStatus).Result;
                //HttpResponseMessage response = client.PostAsJsonAsync<Models.AddInqStatus>(ConfigurationHelper.AddInqStatus, AddInqStatus).Result;
                var resultContent = response.Headers.Location;
                if (response.IsSuccessStatusCode)
                    return true;
                else return false;
            }
        }

        public static string SaveDomesticAdditionalData(Models.DomesticAdditionData DomesticAdditionData)
        {
            using (HttpClient client = new HttpClient())
            {
                var authenticationBytes = Encoding.ASCII.GetBytes(Helper.ConfigurationHelper.ServiceUserName + ":" + Helper.ConfigurationHelper.ServicePassword);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add("X-CSRF-TOKEN", "fetch");
                var url = Helper.ConfigurationHelper.DomesticAdditionDetailsSave;
                var uri = new Uri(url);

                HttpResponseMessage response = client.GetAsync(uri).Result;

                var oTokal = response.Headers.GetValues("X-CSRF-TOKEN").FirstOrDefault();
                var cookies = response.Headers.GetValues("Set-Cookie");

                var department = DomesticAdditionData;
                string json = JsonConvert.SerializeObject(department, Newtonsoft.Json.Formatting.None);
                var content = new StringContent(json);

                var CompanyInfoData = Newtonsoft.Json.JsonConvert.DeserializeObject<NewInq>(json);

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("X-CSRF-TOKEN", oTokal);
                client.DefaultRequestHeaders.Add("Cookie", cookies);
                response = client.PostAsJsonAsync<DomesticAdditionData>(url, department).Result;
                var resultContent = response.Headers.Location;
                var dd = response.Content.ReadAsStringAsync().Result;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(dd);
                return "";
                //return doc.ChildNodes[1].ChildNodes[5].ChildNodes[0].ChildNodes[22].InnerText;
            }


        }


        public static string InquiryStatusSetSave(Models.InquiryStatusSet InquiryStatusSet)
        {
            using (HttpClient client = new HttpClient())
            {
                var authenticationBytes = Encoding.ASCII.GetBytes(Helper.ConfigurationHelper.ServiceUserName + ":" + Helper.ConfigurationHelper.ServicePassword);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add("X-CSRF-TOKEN", "fetch");
                var url = string.Format(Helper.ConfigurationHelper.InquiryStatusSet);
                var uri = new Uri(url);

                HttpResponseMessage response = client.GetAsync(uri).Result;

                var oTokal = response.Headers.GetValues("X-CSRF-TOKEN").FirstOrDefault();
                var cookies = response.Headers.GetValues("Set-Cookie");

                var department = InquiryStatusSet;
                string json = JsonConvert.SerializeObject(department, Newtonsoft.Json.Formatting.None);
                var content = new StringContent(json);

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("X-CSRF-TOKEN", oTokal);
                client.DefaultRequestHeaders.Add("Cookie", cookies);
                response = client.PostAsJsonAsync<InquiryStatusSet>(url, department).Result;
                var resultContent = response.Headers.Location;
                var dd = response.Content.ReadAsStringAsync().Result;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(dd);
                return "";
            }


        }

        public static string UpdatePaymentStatus(Models.UpdatePaymentStatus UpdatePaymentStatus)
        {
            using (HttpClient client = new HttpClient())
            {
                var authenticationBytes = Encoding.ASCII.GetBytes(Helper.ConfigurationHelper.ServiceUserName + ":" + Helper.ConfigurationHelper.ServicePassword);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add("X-CSRF-TOKEN", "fetch");
                var url = string.Format(Helper.ConfigurationHelper.InquiryStatusSet);
                var uri = new Uri(url);

                HttpResponseMessage response = client.GetAsync(uri).Result;

                var oTokal = response.Headers.GetValues("X-CSRF-TOKEN").FirstOrDefault();
                var cookies = response.Headers.GetValues("Set-Cookie");

                var department = UpdatePaymentStatus;
                string json = JsonConvert.SerializeObject(department, Newtonsoft.Json.Formatting.None);
                var content = new StringContent(json);

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("X-CSRF-TOKEN", oTokal);
                client.DefaultRequestHeaders.Add("Cookie", cookies);
                response = client.PostAsJsonAsync<UpdatePaymentStatus>(url, department).Result;
                var resultContent = response.Headers.Location;
                var dd = response.Content.ReadAsStringAsync().Result;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(dd);
                return "";
            }


        }

        public static Models.InquiryStatusSet InquiryStatusSetGet(string InquiryNo)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                var authenticationBytes = Encoding.ASCII.GetBytes(Helper.ConfigurationHelper.ServiceUserName + ":" + Helper.ConfigurationHelper.ServicePassword);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                HttpResponseMessage response = client.GetAsync(string.Format(ConfigurationHelper.InquiryStatusSetGet, InquiryNo)).Result;
                List<Models.Documents> DocumentList = new List<Models.Documents>();
                var strresponse = response.Content.ReadAsStringAsync().Result;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(strresponse);
                XmlNodeList list = doc.GetElementsByTagName("entry");
                InquiryStatusSet inquirystatusset = new InquiryStatusSet();
                inquirystatusset.InquiryNo = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[0].InnerText;
                inquirystatusset.InquiryDate = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[1].InnerText;
                inquirystatusset.ProspectNo = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[2].InnerText;
                inquirystatusset.ProspectDate = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[3].InnerText;
                inquirystatusset.ActualbpNo = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[4].InnerText;
                inquirystatusset.ContractAcc = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[5].InnerText;
                inquirystatusset.ActualbpDate = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[6].InnerText;
                inquirystatusset.UserID = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[7].InnerText;
                inquirystatusset.DocumentUpload = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[8].InnerText;
                inquirystatusset.DocumentApproved = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[9].InnerText;
                inquirystatusset.AdditionalData = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[10].InnerText;
                inquirystatusset.SchemeName = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[11].InnerText;
                inquirystatusset.Amount = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[12].InnerText;
                inquirystatusset.Payment = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[13].InnerText;
                inquirystatusset.InstallationDate = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[14].InnerText;
                inquirystatusset.ConnectionDate = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[15].InnerText;
                inquirystatusset.Feedback = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[16].InnerText;
                inquirystatusset.REMARKDATE = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[17].InnerText;
                inquirystatusset.REMARK = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[18].InnerText;
                inquirystatusset.ConvDate = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[19].InnerText;
                inquirystatusset.OEMId = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[20].InnerText;
                inquirystatusset.OEMName = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[21].InnerText;
                inquirystatusset.OEMDate = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[22].InnerText;
                inquirystatusset.CardGiven = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[23].InnerText;

                return inquirystatusset;
            }
        }

        public static string GetEnquiryStatus(string EnquiryNo)
        {
            var authenticationBytes64Value = Helper.ServiceAuthenticationHelper.AddAuthorizationMethod();
            var FetchedTokenValue = Helper.ServiceAuthenticationHelper.FetchTokenValue(authenticationBytes64Value);

            using (HttpClient client = new HttpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                //VATP Code
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationBytes64Value);
                client.DefaultRequestHeaders.Add("Token", FetchedTokenValue);
                var uri = new Uri(ConfigurationHelper.GetInquiryStatus);
                HttpResponseMessage response = client.GetAsync(string.Format(uri.ToString(), EnquiryNo)).Result;
                var resultContent = response.Headers.Location;
                if (response.IsSuccessStatusCode)
                {
                    //do nothing   
                }
                //HttpResponseMessage response = client.GetAsync(string.Format(ConfigurationHelper.GetInquiryStatus, EnquiryNo)).Result;
                XmlDocument doc = new XmlDocument();
                var responsestring = response.Content.ReadAsStringAsync().Result;
                doc.LoadXml(responsestring);
                XmlNodeList list = doc.GetElementsByTagName("entry");
                if (list.Count > 0)
                {
                    var icount = list.Count - 1;
                    return list[icount].ChildNodes[6].ChildNodes[0].ChildNodes[4].InnerText;
                }
                else return null;
            }
        }

        public static bool IsPaidEnquiryStatus(string EnquiryNo)
        {
            //VATP Code
            var authenticationBytes64Value = Helper.ServiceAuthenticationHelper.AddAuthorizationMethod();
            var FetchedTokenValue = Helper.ServiceAuthenticationHelper.FetchTokenValue(authenticationBytes64Value);

            using (HttpClient client = new HttpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                //VATP Code
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationBytes64Value);
                client.DefaultRequestHeaders.Add("Token", FetchedTokenValue);
                var uri = new Uri(ConfigurationHelper.GetInquiryPaidStatus);
                HttpResponseMessage response = client.GetAsync(string.Format(uri.ToString(), EnquiryNo)).Result;
                var resultContent = response.Headers.Location;
                if (response.IsSuccessStatusCode)
                {
                    //do nothing   
                }
                //HttpResponseMessage response = client.GetAsync(string.Format(ConfigurationHelper.GetInquiryPaidStatus, EnquiryNo)).Result;
                XmlDocument doc = new XmlDocument();
                var responsestring = response.Content.ReadAsStringAsync().Result;
                doc.LoadXml(responsestring);
                XmlNodeList list = doc.GetElementsByTagName("entry");
                if (list.Count > 0)
                    return true;
                else return false;
            }
        }


        public static List<Models.AddDocumentInExistingEnquiry> GetUploadedDocuments(string EnquiryNo)
        {
            var authenticationBytes64Value = Helper.ServiceAuthenticationHelper.AddAuthorizationMethod();
            var FetchedTokenValue = Helper.ServiceAuthenticationHelper.FetchTokenValue(authenticationBytes64Value);

            using (HttpClient client = new HttpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                //VATP Code
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationBytes64Value);
                client.DefaultRequestHeaders.Add("Token", FetchedTokenValue);
                var uri = new Uri(ConfigurationHelper.GetUploadedDocuments);
                HttpResponseMessage response = client.GetAsync(string.Format(uri.ToString(), EnquiryNo)).Result;
                var resultContent = response.Headers.Location;
                if (response.IsSuccessStatusCode)
                {
                    //do nothing   
                }
                //HttpResponseMessage response = client.GetAsync(string.Format(ConfigurationHelper.GetUploadedDocuments, EnquiryNo)).Result;
                XmlDocument doc = new XmlDocument();
                var responsestring = response.Content.ReadAsStringAsync().Result;
                doc.LoadXml(responsestring);
                XmlNodeList list = doc.GetElementsByTagName("entry");
                List<Models.AddDocumentInExistingEnquiry> documents = new List<Models.AddDocumentInExistingEnquiry>();
                foreach (XmlNode dr in list)
                {
                    Models.AddDocumentInExistingEnquiry document = new AddDocumentInExistingEnquiry();

                    document.FileExt = dr.ChildNodes[7].ChildNodes[0].ChildNodes[6].InnerText;
                    document.Documnet = dr.ChildNodes[7].ChildNodes[0].ChildNodes[5].InnerText;
                    document.FILENAME = dr.ChildNodes[7].ChildNodes[0].ChildNodes[4].InnerText;
                    document.DocName = dr.ChildNodes[7].ChildNodes[0].ChildNodes[3].InnerText;
                    document.DocType = dr.ChildNodes[7].ChildNodes[0].ChildNodes[2].InnerText;
                    document.DocStatus = dr.ChildNodes[7].ChildNodes[0].ChildNodes[10].InnerText;
                    document.CREATEDATE = dr.ChildNodes[7].ChildNodes[0].ChildNodes[9].InnerText;
                    document.ROWID = System.Convert.ToInt32(dr.ChildNodes[7].ChildNodes[0].ChildNodes[1].InnerText);

                    documents.Add(document);
                }
                return documents;
            }
        }

        public static bool SaveAdditionalData(Models.AdditionDataSave AdditionDataSave)
        {
            var authenticationBytes64Value = Helper.ServiceAuthenticationHelper.AddAuthorizationMethod();
            var FetchedTokenValue = Helper.ServiceAuthenticationHelper.FetchTokenValue(authenticationBytes64Value);

            using (HttpClient client = new HttpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                //VATP Code
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationBytes64Value);
                client.DefaultRequestHeaders.Add("Token", FetchedTokenValue);
                var uri = new Uri(ConfigurationHelper.AdditionDataSave);
                HttpResponseMessage response = client.PostAsJsonAsync<Models.AdditionDataSave>(uri.ToString(), AdditionDataSave).Result;
                //HttpResponseMessage response = client.PostAsJsonAsync<Models.AdditionDataSave>(ConfigurationHelper.AdditionDataSave, AdditionDataSave).Result;
                var resultContent = response.Headers.Location;
                if (response.IsSuccessStatusCode)
                    return true;
                else return false;
            }
        }

        public static bool UpdateAdditionalData(Models.AdditionDataSave AdditionDataSave)
        {
            var authenticationBytes64Value = Helper.ServiceAuthenticationHelper.AddAuthorizationMethod();
            var FetchedTokenValue = Helper.ServiceAuthenticationHelper.FetchTokenValue(authenticationBytes64Value);

            using (HttpClient client = new HttpClient())
            {
                Models.AdditionDataUpdate update = new AdditionDataUpdate();

                update.SAPINQID = AdditionDataSave.SAPINQID;
                update.LPGCONSNO = AdditionDataSave.LPGCONSNO;
                update.LPGCONSVALUE = AdditionDataSave.LPGCONSVALUE;
                update.COMPANYNO = AdditionDataSave.COMPANYNO;
                update.COMPANYVALUE = AdditionDataSave.COMPANYVALUE;
                update.BATHROOMPOINT = AdditionDataSave.BATHROOMPOINT;
                update.BATHROOMPOINTVALUE = AdditionDataSave.BATHROOMPOINTVALUE;
                update.KITCHENPOINT = AdditionDataSave.KITCHENPOINT;
                update.KITCHENPOINTVALUE = AdditionDataSave.KITCHENPOINTVALUE;
                update.BUILDERVALUE = AdditionDataSave.BUILDERVALUE;
                update.INDIVIDUAL = AdditionDataSave.INDIVIDUAL;
                update.INDIVIDUALVALUE = AdditionDataSave.INDIVIDUALVALUE;
                update.BULK = AdditionDataSave.BULK;
                update.BULKVALUE = AdditionDataSave.BULKVALUE;
                update.REAMRK = AdditionDataSave.REAMRK;
                update.UPDATEDATE = AdditionDataSave.CREATEDATE;

                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                //VATP Code
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationBytes64Value);
                client.DefaultRequestHeaders.Add("Token", FetchedTokenValue);
                var uri = new Uri(ConfigurationHelper.AdditionDataUpdate);
                HttpResponseMessage response = client.PostAsJsonAsync<Models.AdditionDataUpdate>(uri.ToString(), update).Result;
                //HttpResponseMessage response = client.PostAsJsonAsync<Models.AdditionDataUpdate>(ConfigurationHelper.AdditionDataUpdate, update).Result;
                var resultContent = response.Headers.Location;
                if (response.IsSuccessStatusCode)
                    return true;
                else return false;
            }
        }

        //public static string SetActualBP(string EnquiryNo)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        var authenticationBytes = Encoding.ASCII.GetBytes(Helper.ConfigurationHelper.ServiceUserName + ":" + Helper.ConfigurationHelper.ServicePassword);
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
        //        client.DefaultRequestHeaders.Add("X-CSRF-TOKEN", "fetch");
        //        var url = Helper.ConfigurationHelper.SetActualBPSet;
        //        var uri = new Uri(url);

        //        HttpResponseMessage response = client.GetAsync(uri).Result;

        //        var oTokal = response.Headers.GetValues("X-CSRF-TOKEN").FirstOrDefault();
        //        var cookies = response.Headers.GetValues("Set-Cookie");

        //        var department = EnquiryNo;
        //        string json = JsonConvert.SerializeObject(department, Newtonsoft.Json.Formatting.None);
        //        var content = new StringContent(json);

        //        var CompanyInfoData = Newtonsoft.Json.JsonConvert.DeserializeObject<NewInq>(json);

        //        client.DefaultRequestHeaders.Clear();
        //        client.DefaultRequestHeaders.Add("X-CSRF-TOKEN", oTokal);
        //        client.DefaultRequestHeaders.Add("Cookie", cookies);
        //        response = client.PostAsJsonAsync<string>(url, department).Result;
        //        var resultContent = response.Headers.Location;
        //        var dd = response.Content.ReadAsStringAsync().Result;
        //        XmlDocument doc = new XmlDocument();
        //        doc.LoadXml(dd);
        //        return "";
        //        //return doc.ChildNodes[1].ChildNodes[5].ChildNodes[0].ChildNodes[22].InnerText;
        //    }

        //    return "";

        //}


        public static List<VehicleType> GetVehicleType()
        {
            //VATP Code
            var authenticationBytes64Value = Helper.ServiceAuthenticationHelper.AddAuthorizationMethod();
            var FetchedTokenValue = Helper.ServiceAuthenticationHelper.FetchTokenValue(authenticationBytes64Value);

            using (HttpClient client = new HttpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                //VATP Code
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationBytes64Value);
                client.DefaultRequestHeaders.Add("Token", FetchedTokenValue);
                var uri = new Uri(ConfigurationHelper.GetVehicleType);
                HttpResponseMessage response = client.GetAsync(uri.ToString()).Result;
                var resultContent = response.Headers.Location;
                if (response.IsSuccessStatusCode)
                {
                    //do nothing   
                }
                //HttpResponseMessage response = client.GetAsync(ConfigurationHelper.GetVehicleType).Result;
                XmlDocument doc = new XmlDocument();
                var responsestring = response.Content.ReadAsStringAsync().Result;
                doc.LoadXml(responsestring);
                XmlNodeList list = doc.GetElementsByTagName("entry");
                List<VehicleType> ListVehicleType = new List<VehicleType>();
                foreach (XmlNode dr in list)
                {
                    VehicleType vehicletype = new VehicleType();
                    vehicletype.ID = dr.ChildNodes[6].ChildNodes[0].ChildNodes[0].InnerText;
                    vehicletype.Vehical_Type1 = dr.ChildNodes[6].ChildNodes[0].ChildNodes[0].InnerText;
                    ListVehicleType.Add(vehicletype);
                }
                return ListVehicleType.ToList();
            }
        }

        public static List<VehicleType> GetVehicleMake(string VehicleType)
        {
            //VATP Code
            var authenticationBytes64Value = Helper.ServiceAuthenticationHelper.AddAuthorizationMethod();
            var FetchedTokenValue = Helper.ServiceAuthenticationHelper.FetchTokenValue(authenticationBytes64Value);

            using (HttpClient client = new HttpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                //VATP Code
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationBytes64Value);
                client.DefaultRequestHeaders.Add("Token", FetchedTokenValue);
                var uri = new Uri(ConfigurationHelper.GetVehicleMake);
                HttpResponseMessage response = client.GetAsync(string.Format(uri.ToString(), VehicleType)).Result;
                var resultContent = response.Headers.Location;
                if (response.IsSuccessStatusCode)
                {
                    //do nothing   
                }
                //HttpResponseMessage response = client.GetAsync(string.Format(ConfigurationHelper.GetVehicleMake, VehicleType)).Result;
                XmlDocument doc = new XmlDocument();
                var responsestring = response.Content.ReadAsStringAsync().Result;
                doc.LoadXml(responsestring);
                XmlNodeList list = doc.GetElementsByTagName("entry");
                List<VehicleType> ListVehicleType = new List<VehicleType>();
                foreach (XmlNode dr in list)
                {
                    string str = dr.ChildNodes[6].ChildNodes[0].ChildNodes[2].InnerText; ;
                    if (ListVehicleType.Where(p => p.ID.Equals(str)).Count() == 0)
                    {
                        VehicleType vehicletype = new VehicleType();
                        vehicletype.ID = str;
                        vehicletype.Vehical_Type1 = str;
                        ListVehicleType.Add(vehicletype);
                    }
                }
                return ListVehicleType.ToList();
            }
        }

        public static List<VehicleType> GetVehicleModel(string VehicleType, string VehicleMake)
        {
            //VATP Code
            var authenticationBytes64Value = Helper.ServiceAuthenticationHelper.AddAuthorizationMethod();
            var FetchedTokenValue = Helper.ServiceAuthenticationHelper.FetchTokenValue(authenticationBytes64Value);

            using (HttpClient client = new HttpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                //VATP Code
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationBytes64Value);
                client.DefaultRequestHeaders.Add("Token", FetchedTokenValue);
                var uri = new Uri(ConfigurationHelper.GetVehicleModel);
                HttpResponseMessage response = client.GetAsync(string.Format(uri.ToString(), VehicleType, VehicleMake)).Result;
                var resultContent = response.Headers.Location;
                if (response.IsSuccessStatusCode)
                {
                    //do nothing   
                }
                //HttpResponseMessage response = client.GetAsync(string.Format(ConfigurationHelper.GetVehicleModel, VehicleType, VehicleMake)).Result;
                XmlDocument doc = new XmlDocument();
                var responsestring = response.Content.ReadAsStringAsync().Result;
                doc.LoadXml(responsestring);
                XmlNodeList list = doc.GetElementsByTagName("entry");
                List<VehicleType> ListVehicleType = new List<VehicleType>();
                foreach (XmlNode dr in list)
                {
                    VehicleType vehicletype = new VehicleType();
                    vehicletype.ID = dr.ChildNodes[6].ChildNodes[0].ChildNodes[3].InnerText;
                    vehicletype.Vehical_Type1 = dr.ChildNodes[6].ChildNodes[0].ChildNodes[3].InnerText;
                    ListVehicleType.Add(vehicletype);
                }
                return ListVehicleType.ToList();
            }
        }

        public static string SaveEnquiryCNG(Models.NewInquiryCNG NewInq)
        {
            using (HttpClient client = new HttpClient())
            {
                var authenticationBytes = Encoding.ASCII.GetBytes(Helper.ConfigurationHelper.ServiceUserName + ":" + Helper.ConfigurationHelper.ServicePassword);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add("X-CSRF-TOKEN", "fetch");
                var url = Helper.ConfigurationHelper.CNGInquirySave;
                var uri = new Uri(url);

                HttpResponseMessage response = client.GetAsync(uri).Result;

                var oTokal = response.Headers.GetValues("X-CSRF-TOKEN").FirstOrDefault();
                var cookies = response.Headers.GetValues("Set-Cookie");

                var department = NewInq;
                string json = JsonConvert.SerializeObject(department, Newtonsoft.Json.Formatting.None);
                var content = new StringContent(json);

                var CompanyInfoData = Newtonsoft.Json.JsonConvert.DeserializeObject<NewInq>(json);

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("X-CSRF-TOKEN", oTokal);
                client.DefaultRequestHeaders.Add("Cookie", cookies);
                response = client.PostAsJsonAsync<NewInquiryCNG>(url, department).Result;
                var resultContent = response.Headers.Location;
                var dd = response.Content.ReadAsStringAsync().Result;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(dd);
                return doc.ChildNodes[1].ChildNodes[5].ChildNodes[0].ChildNodes[25].InnerText;
            }


        }
    }
}