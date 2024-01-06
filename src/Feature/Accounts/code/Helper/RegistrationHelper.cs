using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;

namespace Sitecore.Feature.Accounts.Helper
{
    public static class RegistrationHelper
    {
        public static Models.NewRegistration GetEnquiry(string EnquiryNo)
        {
            string url = ConfigurationHelper.oDataGetInquiry;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                var authenticationBytes = Encoding.ASCII.GetBytes(Helper.ConfigurationHelper.ServiceUserName + ":" + Helper.ConfigurationHelper.ServicePassword);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                HttpResponseMessage response = client.GetAsync(string.Format(url, EnquiryNo)).Result;


                var strResponse = response.Content.ReadAsStringAsync().Result;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(strResponse);
                XmlNodeList list = doc.GetElementsByTagName("entry");

                Models.NewRegistration newRegistration = new Models.NewRegistration();
                newRegistration.COMPNO = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[0].InnerText;
                newRegistration.PARTNER = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[1].InnerText;
                newRegistration.ZZCONNECTION_OBJ = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[2].InnerText;
                newRegistration.STATUS = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[3].InnerText;

                newRegistration.QMNUM = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[4].InnerText;
                newRegistration.TYPE = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[5].InnerText;
                newRegistration.BPKIND = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[6].InnerText;
                newRegistration.PLTXT = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[7].InnerText;
                newRegistration.NAME1 = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[8].InnerText;
                newRegistration.HOUSE_NUM1 = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[9].InnerText;
                newRegistration.STREET = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[10].InnerText;
                newRegistration.STR_SUPPL1 = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[11].InnerText;
                newRegistration.STR_SUPPL2 = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[12].InnerText;
                newRegistration.STR_SUPPL3 = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[13].InnerText;
                newRegistration.LOCATION = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[14].InnerText;
                newRegistration.Date = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[15].InnerText;
                newRegistration.CITY1 = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[16].InnerText;
                newRegistration.POST_CODE1 = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[17].InnerText;
                newRegistration.COUNTRY = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[18].InnerText;
                newRegistration.TEL_NUMBER = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[19].InnerText;
                newRegistration.MOBILE = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[20].InnerText;
                newRegistration.FAX_NUMBER = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[21].InnerText;
                newRegistration.REGIOGROUP = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[22].InnerText;
                newRegistration.DESCRIPT = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[23].InnerText;
                newRegistration.CTYPE = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[24].InnerText;
                newRegistration.AGNAM = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[25].InnerText;
                newRegistration.PLANT = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[26].InnerText;
                newRegistration.EMAIL = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[27].InnerText;
                newRegistration.NAME_FIRST = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[28].InnerText;
                newRegistration.NAME_LAST = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[29].InnerText;
                newRegistration.BU_SORT1 = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[30].InnerText;
                newRegistration.BU_SORT2 = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[31].InnerText;
                newRegistration.TITLE_MEDI = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[32].InnerText;
                newRegistration.REGION = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[33].InnerText;
                newRegistration.CREATION_GROUP = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[34].InnerText;
                newRegistration.AUGRP = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[35].InnerText;
                newRegistration.SENDCONTROL_GP = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[36].InnerText;
                newRegistration.MsgFlag = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[48].InnerText;
                newRegistration.Message = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[49].InnerText;

                return newRegistration;
            }
        }


        public static string CreateUser(Models.CreateUserSet CreateUserSet)
        {
            using (HttpClient client = new HttpClient())
            {
                var authenticationBytes = Encoding.ASCII.GetBytes(Helper.ConfigurationHelper.ServiceUserName + ":" + Helper.ConfigurationHelper.ServicePassword);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add("X-CSRF-TOKEN", "fetch");
                var url = string.Format(Helper.ConfigurationHelper.oDataCreateuser);
                var uri = new Uri(url);

                HttpResponseMessage response = client.GetAsync(uri).Result;

                var oTokal = response.Headers.GetValues("X-CSRF-TOKEN").FirstOrDefault();
                var cookies = response.Headers.GetValues("Set-Cookie");

                var department = CreateUserSet;
                string json = JsonConvert.SerializeObject(department, Newtonsoft.Json.Formatting.None);
                var content = new StringContent(json);

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("X-CSRF-TOKEN", oTokal);
                client.DefaultRequestHeaders.Add("Cookie", cookies);
                response = client.PostAsJsonAsync<Models.CreateUserSet>(url, department).Result;
                var resultContent = response.Headers.Location;
                var dd = response.Content.ReadAsStringAsync().Result;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(dd);
                return "";
            }


        }

        public static string CreateUser(string UserName, string Password, string EnquiryNo)
        {
            string url = ConfigurationHelper.oDataCreateuser;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                var authenticationBytes = Encoding.ASCII.GetBytes(Helper.ConfigurationHelper.ServiceUserName + ":" + Helper.ConfigurationHelper.ServicePassword);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                HttpResponseMessage response = client.GetAsync(string.Format(url, UserName, Password, EnquiryNo)).Result;

                var strResponse = response.Content.ReadAsStringAsync().Result;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(strResponse);
                XmlNodeList list = doc.GetElementsByTagName("entry");
                return list[0].ChildNodes[5].ChildNodes[0].ChildNodes[4].InnerText;
            }
        }

        public static Models.VerifyUser VerifyUser(string UserName)
        {
            string url = ConfigurationHelper.oDataVerifyUser;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                var authenticationBytes = Encoding.ASCII.GetBytes(Helper.ConfigurationHelper.ServiceUserName + ":" + Helper.ConfigurationHelper.ServicePassword);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                HttpResponseMessage response = client.GetAsync(string.Format(url, UserName)).Result;

                var strResponse = response.Content.ReadAsStringAsync().Result;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(strResponse);
                XmlNodeList list = doc.GetElementsByTagName("entry");
                Models.VerifyUser verifyUsername = new Models.VerifyUser();
                verifyUsername.Ev_Msg_Flag = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[1].InnerText;
                verifyUsername.Ev_Message = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[2].InnerText;

                return verifyUsername;
            }
        }

        public static bool VerifyPassword(string UserName, string Password)
        {
            string url = ConfigurationHelper.oDataVerifyUser;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                HttpResponseMessage response = client.GetAsync(string.Format(url, UserName)).Result;

                var strResponse = response.Content.ReadAsStringAsync().Result;
                if (strResponse.Contains("Logon Error Message"))
                    return false;
                else return true;
            }
        }

        public static List<Models.Documents> GetDocumentList()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                var authenticationBytes = Encoding.ASCII.GetBytes(Helper.ConfigurationHelper.ServiceUserName + ":" + Helper.ConfigurationHelper.ServicePassword);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                HttpResponseMessage response = client.GetAsync(ConfigurationHelper.oDataGetDocumentListMethodName).Result;
                List<Models.Documents> DocumentList = new List<Models.Documents>();
                var strresponse = response.Content.ReadAsStringAsync().Result;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(strresponse);
                XmlNodeList list = doc.GetElementsByTagName("entry");
                foreach (XmlNode dr in list)
                {
                    Models.Documents Document = new Models.Documents();
                    Document.Catagory = dr.ChildNodes[5].ChildNodes[0].ChildNodes[0].InnerText;
                    Document.DocNum = dr.ChildNodes[5].ChildNodes[0].ChildNodes[1].InnerText;
                    Document.DocType = dr.ChildNodes[5].ChildNodes[0].ChildNodes[2].InnerText;
                    Document.DocText = dr.ChildNodes[5].ChildNodes[0].ChildNodes[3].InnerText;

                    DocumentList.Add(Document);
                }

                return DocumentList;
            }
        }




        public static System.Net.HttpWebRequest UploadFileSOAPWebRequest(string authorization)
        {
            System.Net.HttpWebRequest Req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(Helper.ConfigurationHelper.DocumentSaveMethodName);
            Req.Headers.Add(@"SOAPAction:http://tempuri2.org/UploadFile");
            Req.ContentType = "text/xml;charset=\"utf-8\"";
            Req.Accept = "text/xml";
            Req.Method = "POST";
            Req.Credentials = new NetworkCredential(ConfigurationHelper.VAPTServiceUserName, ConfigurationHelper.ServicePassword);
            Req.Headers["Authorization"] = "Basic " + authorization;
            return Req;
        }

        public static string InvokeService(string Filepath, string EnquiryNo, string FileName, string Extension)
        {
            //VATP Code
            var authenticationBytes64Value = Helper.ServiceAuthenticationHelper.AddAuthorizationMethod();
            var FetchedTokenValue = Helper.ServiceAuthenticationHelper.FetchTokenValue(authenticationBytes64Value);

            byte[] imageArray = System.IO.File.ReadAllBytes(Filepath);
            string base64ImageRepresentation = System.Convert.ToBase64String(imageArray);

            System.Net.HttpWebRequest request = UploadFileSOAPWebRequest(authenticationBytes64Value);
            XmlDocument SOAPReqBody = new XmlDocument();
            var soapEnvelope = string.Empty;
            soapEnvelope = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            soapEnvelope += "<soap12:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\">";
            soapEnvelope += "<soap12:Body>";
            soapEnvelope += "<UploadFile xmlns=\"http://tempuri2.org/\">";
            soapEnvelope += "<Token>" + FetchedTokenValue + "</Token>";
            soapEnvelope += "<Base64>" + base64ImageRepresentation + "</Base64>";
            soapEnvelope += "<FileName>" + FileName + "</FileName>";
            soapEnvelope += "<FileType>" + Extension + "</FileType>";
            soapEnvelope += "<SAPID>" + EnquiryNo + "</SAPID>";
            soapEnvelope += "</UploadFile>";
            soapEnvelope += "</soap12:Body>";
            soapEnvelope += "</soap12:Envelope>";

            SOAPReqBody.LoadXml(soapEnvelope);
            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }

            using (System.Net.WebResponse Serviceres = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                {
                    var ServiceResult = rd.ReadToEnd();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(ServiceResult);
                    return doc.ChildNodes[1].ChildNodes[0].InnerText;
                }
            }
        }


        public static bool SaveInqDocuments(Models.DocumentUpload NewInq)
        {
            var authenticationBytes64Value = Helper.ServiceAuthenticationHelper.AddAuthorizationMethod();
            var FetchedTokenValue = Helper.ServiceAuthenticationHelper.FetchTokenValue(authenticationBytes64Value);

            using (HttpClient client = new HttpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationBytes64Value);
                client.DefaultRequestHeaders.Add("Token", FetchedTokenValue);
                var uri = new Uri(ConfigurationHelper.oDataSaveInqDocument);
                HttpResponseMessage response = client.PostAsJsonAsync<Models.DocumentUpload>(uri.ToString(), NewInq).Result;
                //HttpResponseMessage response = client.PostAsJsonAsync<Models.DocumentUpload>(ConfigurationHelper.oDataSaveInqDocument, NewInq).Result;
                var resultContent = response.Headers.Location;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }

        public static bool AddDocumentsInExistingEnquiry(Models.AddDocumentInExistingEnquiry Documents)
        {
            var authenticationBytes64Value = Helper.ServiceAuthenticationHelper.AddAuthorizationMethod();
            var FetchedTokenValue = Helper.ServiceAuthenticationHelper.FetchTokenValue(authenticationBytes64Value);

            using (HttpClient client = new HttpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationBytes64Value);
                client.DefaultRequestHeaders.Add("Token", FetchedTokenValue);
                var uri = new Uri(ConfigurationHelper.AddDocumentInExistingList);
                HttpResponseMessage response = client.PostAsJsonAsync<Models.AddDocumentInExistingEnquiry>(string.Format(uri.ToString(), Documents.INQID), Documents).Result;
                var resultContent = response.Headers.Location;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
                //HttpResponseMessage response = client.PostAsJsonAsync<Models.AddDocumentInExistingEnquiry>(string.Format(ConfigurationHelper.AddDocumentInExistingList, Documents.INQID), Documents).Result;
            }
        }

        public static bool DeleteDocument(Models.DeleteDocument Documents)
        {
            var authenticationBytes64Value = Helper.ServiceAuthenticationHelper.AddAuthorizationMethod();
            var FetchedTokenValue = Helper.ServiceAuthenticationHelper.FetchTokenValue(authenticationBytes64Value);

            using (HttpClient client = new HttpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationBytes64Value);
                client.DefaultRequestHeaders.Add("Token", FetchedTokenValue);
                var uri = new Uri(ConfigurationHelper.AddDocumentInExistingList);
                HttpResponseMessage response = client.PostAsJsonAsync<Models.DeleteDocument>(string.Format(uri.ToString()), Documents).Result;
                //HttpResponseMessage response = client.PostAsJsonAsync<Models.DeleteDocument>(string.Format(ConfigurationHelper.AddDocumentInExistingList), Documents).Result;
                var resultContent = response.Headers.Location;
                if (response.IsSuccessStatusCode)
                    return true;
                else return false;
            }
        }

        public static List<Models.Schemes> GetSchemes()
        {
            using (HttpClient client = new HttpClient())
            {
                List<Models.Schemes> SchemesList = new List<Models.Schemes>();
                var response = oDataHelper.GetDatabyUrl(string.Format(ConfigurationHelper.Schemes));
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);
                XmlNodeList list = doc.GetElementsByTagName("entry");
                foreach (XmlNode dr in list)
                {
                    Models.Schemes Scheme = new Models.Schemes();
                    Scheme.Scheme = dr.ChildNodes[5].ChildNodes[0].ChildNodes[2].InnerText;
                    Scheme.SchemeDes = dr.ChildNodes[5].ChildNodes[0].ChildNodes[3].InnerText;
                    Scheme.StampDutyReg = dr.ChildNodes[5].ChildNodes[0].ChildNodes[4].InnerText;
                    Scheme.FormChargesReg = dr.ChildNodes[5].ChildNodes[0].ChildNodes[5].InnerText;
                    Scheme.ConndepRefundReg = dr.ChildNodes[5].ChildNodes[0].ChildNodes[6].InnerText;
                    Scheme.SchemeName = dr.ChildNodes[5].ChildNodes[0].ChildNodes[7].InnerText;
                    Scheme.GasdepRefundReg = dr.ChildNodes[5].ChildNodes[0].ChildNodes[8].InnerText;
                    Scheme.AddtTaxChrgReg = dr.ChildNodes[5].ChildNodes[0].ChildNodes[9].InnerText;
                    Scheme.TotalPaymentReg = dr.ChildNodes[5].ChildNodes[0].ChildNodes[10].InnerText;
                    Scheme.ConndepRefundPost = dr.ChildNodes[5].ChildNodes[0].ChildNodes[11].InnerText;
                    Scheme.GasdepRefundPost = dr.ChildNodes[5].ChildNodes[0].ChildNodes[12].InnerText;
                    Scheme.AddtTaxChrgPost = dr.ChildNodes[5].ChildNodes[0].ChildNodes[13].InnerText;
                    Scheme.TotalPaymentPost = dr.ChildNodes[5].ChildNodes[0].ChildNodes[14].InnerText;
                    SchemesList.Add(Scheme);
                }

                return SchemesList;
            }
        }

        public static List<string> GetNewDocumentList()
        {
            List<string> list = new List<string>();
            list.Add("Recent Electricity Bill");
            list.Add("Recent BSNL Telephone Bill");
            list.Add("Passport");
            list.Add("Driving License");
            list.Add("Ration Card");
            list.Add("Society Certificate");
            list.Add("Voter ID/Ration card No");
            list.Add("PAN No");
            list.Add("A.M.C Tax Bill");
            list.Add("Index");
            list.Add("Regd. Sale Deed");
            list.Add("Soc. Share Deed");
            list.Add("Builder Allotment Letter");
            list.Add("Other (If Any)");
            list.Add("Aadhar No.");
            return list;
        }

        public static string InvokePaymentScheme(string EnquiryNo, string Fname, string Email, string txnid, string amount, string UDF1)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://hiveless-blowers.000webhostapp.com/newhashgen.php");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    txnid = txnid,
                    amount = amount,
                    pinfo = "GasBill",
                    fname = Fname,
                    email = Email,
                    udf1 = UDF1
                });
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var result = string.Empty;
            var Finalresult = string.Empty;
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                //{"success":"56977d27bfa2da0b057a195e4bad5c6950345244cbbabb15bb57e7eaa244f8d335cb97f0ad3d335c8dcfc51436a6068d4ad8438614e7c537239c6255fbb796ef"}
                result = streamReader.ReadToEnd();

                var dd = JsonConvert.DeserializeObject<HashClass>(result);
                Finalresult = dd.success;
                //result.Replace("", "");
                //Finalresult = result.Split(spearator).ToString();

            }
            return Finalresult;
        }

        public class HashClass
        {
            public string success { get; set; }
        }


        public static string SetActualBP(string EnquiryNo)
        {
            string url = ConfigurationHelper.SetActualBPSet;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                var authenticationBytes = Encoding.ASCII.GetBytes(Helper.ConfigurationHelper.ServiceUserName + ":" + Helper.ConfigurationHelper.ServicePassword);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                HttpResponseMessage response = client.GetAsync(string.Format(url, EnquiryNo)).Result;

                var strResponse = response.Content.ReadAsStringAsync().Result;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(strResponse);
                XmlNodeList list = doc.GetElementsByTagName("entry");
                //return "";
                Models.ActualBpSet actualBpSet = new Models.ActualBpSet();

                return list[0].ChildNodes[5].ChildNodes[0].ChildNodes[1].InnerText;
            }
        }
        public static Models.ActualBpSet GetActualBP(string SchemeName, string Partner)
        {
            string url = ConfigurationHelper.GetActualBPSet;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                var authenticationBytes = Encoding.ASCII.GetBytes(Helper.ConfigurationHelper.ServiceUserName + ":" + Helper.ConfigurationHelper.ServicePassword);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                HttpResponseMessage response = client.GetAsync(string.Format(url, Partner, SchemeName)).Result;

                var strResponse = response.Content.ReadAsStringAsync().Result;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(strResponse);
                XmlNodeList list = doc.GetElementsByTagName("entry");
                //return "";
                Models.ActualBpSet actualBPSet = new Models.ActualBpSet();
                actualBPSet.ActualBpNo = list[0].ChildNodes[5].ChildNodes[0].ChildNodes[2].InnerText;
                actualBPSet.ContractAcc= list[0].ChildNodes[5].ChildNodes[0].ChildNodes[3].InnerText;

                return actualBPSet;
            }
        }

        //Paytm Code start
        public static string InvokePaymentSchemePaytm(string EnquiryNo, string Fname, string txnid, string amount)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://germinant-visibilit.000webhostapp.com/checksum.php");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    MID = "ADANIA30158054352006",
                    ORDER_ID = txnid,
                    CUST_ID = Fname,
                    CHANNEL_ID = "WAP",
                    TXN_AMOUNT = amount,
                    WEBSITE = "WEBSTAGING",
                    INDUSTRY_TYPE_ID = "Retail",
                    CALLBACK_URL = "https://securegw-stage.paytm.in/theia/paytmCallback?ORDER_ID=",//"https://securegw-stage.paytm.in/theia/paytmCallback.jsp"
                    CHECKSUMHASH = ""
                });
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var result = string.Empty;
            var Finalresult = string.Empty;
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();

                var dd = JsonConvert.DeserializeObject<PaytmHashClass>(result);
                Finalresult = dd.CHECKSUMHASH;
            }
            return Finalresult;
        }

        public class PaytmHashClass
        {
            public string CHECKSUMHASH { get; set; }
        }
        //Paytm Code End


        public static string BillDeskCheckSum()
        {
            string docno = "";
             string url = "https://13.235.191.185/Odata.svc/billdeskchecksum";
            using (HttpClient client = new HttpClient())
            {
                BillDeskModal billdesk = new BillDeskModal();
                billdesk.msg = "ADANIENRTS|1000017595|NA|1.00|NA|NA|NA|INR|NA|R|adanienrts|NA|NA|F|1000017595|20180423|BillPayment|R3WRAFLLVX44OF3|-|1000017595|NA|http://208.85.249.167:8030/Enquiry/PaymentSuccess";
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                HttpResponseMessage response = client.PostAsJsonAsync<BillDeskModal>(url,billdesk).Result;
                var resultContent = response.Headers.Location;

                var strResponse = response.Content.ReadAsStringAsync().Result;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(strResponse);
                XmlNodeList list = doc.GetElementsByTagName("entry");
                docno= list[0].ChildNodes[6].ChildNodes[0].ChildNodes[0].InnerText;

            }

            using (HttpClient client = new HttpClient())
            {
                url = "https://13.235.191.185/Odata.svc/billdeskchecksum(" + docno + ")";
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                HttpResponseMessage response = client.GetAsync(url).Result;

                var strResponse = response.Content.ReadAsStringAsync().Result;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(strResponse);
                XmlNodeList list = doc.GetElementsByTagName("entry");
                return list[0].ChildNodes[6].ChildNodes[0].ChildNodes[3].InnerText;
            }
        }

        public class BillDeskModal
        {
            public string msg { get; set; }
            public string cp_msg { get; set; }
        }
    }
}