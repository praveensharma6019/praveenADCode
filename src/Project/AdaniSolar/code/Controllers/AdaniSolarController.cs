using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using System.Web;
using Sitecore.AdaniSolar.Website.Models;
using System.Web.Mvc;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Sitecore.AdaniSolar.Website.Services;
using SelectPdf;
using System.Web.Script.Serialization;

namespace Sitecore.AdaniSolar.Website.Controllers
{
    public class AdaniSolarController : Controller
    {

        // GET: AdaniSolar
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult WarrantyCertificate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult WarrantyCertificate(AdaniSolarWarranty model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    SolarWarrantyService solarWarrantyService = new SolarWarrantyService();
                    AdaniSolarWarrantyInvoiceRoot jsonroot = new AdaniSolarWarrantyInvoiceRoot();
                    Invoice invoice = new Invoice();

                    invoice.Invoice_No_Date = model.AdaniInvoiceDate.ToString("yyyyMMdd");
                    invoice.Invoice_No = model.AdaniInvoiceNumber;
                    jsonroot.Record = invoice;
                    var jsonobject = JsonConvert.SerializeObject(jsonroot);

                    var apiResponse = solarWarrantyService.GetCertificatedata(jsonobject);
                    SolarWarrantyResponseModel responsemodel = JsonConvert.DeserializeObject<SolarWarrantyResponseModel>(apiResponse);
                    if (responsemodel == null)
                    {
                        var data = new { status = "2", message = "Data did not match, get in touch with support" };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else if (responsemodel.Record != null)
                    {
                        var data = new { status = "1", list = responsemodel.Record };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }

        }

        public ActionResult ShowCertificate()
        {

            return View();
        }

        [HttpPost]
        public ActionResult SubmitAction()
        {
            SelectPdf.GlobalProperties.LicenseKey = "c1hCU0FGQlNCU0FAXUNTQEJdQkFdSkpKSg==";
            SelectPdf.GlobalProperties.EnableRestrictedRenderingEngine = true;


            SolarWarrantyService solarWarrantyService = new SolarWarrantyService();
            string htmlString = Request.Form["HtmlString"];
            htmlString = !string.IsNullOrEmpty(HttpUtility.UrlDecode(htmlString)) ? HttpUtility.UrlDecode(htmlString) : solarWarrantyService.RenderRazorViewToString(this, "ShowCertificate");
            // read parameters from the webpage
            HtmlToPdf converter = new HtmlToPdf();
            string HtmlContent = htmlString;
            //string HtmlContent = Request.Form["HtmlString"];
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/styles/AdaniSolar/adani-solar.css";
       
            converter.Options.DisplayHeader = false;
            converter.Options.DisplayFooter = false;
            PdfDocument doc = converter.ConvertHtmlString(HtmlContent, baseUrl);
            // save pdf document
            //doc.RemovePageAt(doc.Pages.Count-1);
            byte[] pdf = doc.Save();

            // close pdf document
            doc.Close();


            // return resulted pdf document
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = "Document.pdf";
            //byte[] fileData = System.IO.File.ReadAllBytes(doc);
            //var binaryData = Encoding.UTF8.GetString(pdf.ToArray());
            //return binaryData;
            var pdfstring = System.Convert.ToBase64String(pdf);

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var data = new
            {
                sendbillresponse = pdfstring
            };
            var result = new ContentResult
            {
                Content = serializer.Serialize(data),
                ContentType = "application/json"
            };
            return result;

        }

        [HttpPost]
        public ActionResult ShowCertificate(string cerinput)
        {
            var cerdata = Request.Form["cerinput"];
            var listdata = JsonConvert.DeserializeObject<List<CerData>>(cerdata);
            ViewBag.cerdata = listdata;
            return new RedirectResult("/show-certificate");
        }

        private string GenerateRandomOTP(int iOTPLength)
        {
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };

            string sOTP = string.Empty;

            string sTempChars = string.Empty;

            Random rand = new Random();
            for (int i = 0; i < iOTPLength; i++)
            {

                int p = rand.Next(0, saAllowedCharacters.Length);

                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                sOTP += sTempChars;

            }
            return sOTP;

        }

        [HttpPost]
        public ActionResult GenerateOTP(SolarConsumerModel model)
        {
            // Generate OTP
            string otp = GenerateRandomOTP(6);
            AdaniSolarOTPHistoryDataContext pgd = new AdaniSolarOTPHistoryDataContext();
            var data = 0;

            try
            {
                var currdate = DateTime.Now;
                var befortime = currdate.AddMinutes(-60);
                data = pgd.AdaniSolarOTPHistories.Where(x => x.OTPFor == model.Mobile && x.CreatedDate >= befortime && x.CreatedDate <= currdate).Count();
                if (data >= 4)
                {
                    return Json(new { status = "3" });
                }

            }
            catch (Exception ex)
            {

                return Json(new { status = "0" , message = ex.Message });
            }

            AdaniSolarOTPHistory entity = new AdaniSolarOTPHistory()
            {
                Id = Guid.NewGuid(),
                OTPType = "Warranty Certificate",
                OTPFor = model.Mobile,
                OTP = otp,
                Status = false,
                CreatedDate = System.DateTime.Now
            };
            pgd.AdaniSolarOTPHistories.InsertOnSubmit(entity);
            pgd.SubmitChanges();

            // Save OTP to session for verification later
            Session["OTP"] = otp;

            // Send OTP via SMS
            try
            {
                MobileOtp mobileOtp = new MobileOtp();
                OtpData otpData = new OtpData();
                otpData.customerId = DictionaryPhraseRepository.Current.Get("/SolarWarrantySendOtp/CustomerID", "Mundra_Sol_UCfX90mqgQjwe5JLD30I");
                otpData.destinationAddress = model.Mobile;
                otpData.message = string.Format("Dear Customer, Your One Time Password (OTP) to download warranty certificate is {0}. Do not share OTP with anyone. Thanks, Adani Solar Team", otp);
                otpData.sourceAddress = DictionaryPhraseRepository.Current.Get("/SolarWarrantySendOtp/SourceAddress", "MSPVLI");
                otpData.messageType = DictionaryPhraseRepository.Current.Get("/SolarWarrantySendOtp/MessageType", "SERVICE_IMPLICIT");
                otpData.dltTemplateId = DictionaryPhraseRepository.Current.Get("/SolarWarrantySendOtp/DltTemplateID", "1007162775205964920");
                otpData.entityId = DictionaryPhraseRepository.Current.Get("/SolarWarrantySendOtp/EntityID", "1001458564483286180");
                String ApiUrl = DictionaryPhraseRepository.Current.Get("/SolarWarrantySendOtp/Api Url", "https://openapi.airtel.in/gateway/airtel-iq-sms-utility/sendSmsWithMultipleRequests");
                mobileOtp.OtpData = otpData;
                var jsonobject = JsonConvert.SerializeObject(otpData);
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);
                request.Headers.Add("Authorization", "Basic R2xvYmVUZWNoOkdsb2JlIzI0NTJA");
                var content = new StringContent("[" + jsonobject + "]", null, "application/json");
                request.Content = content;
                var response = client.SendAsync(request);
                var jsonresponse = response.Result.Content.ReadAsStringAsync().Result;
                if (jsonresponse == "SUCCESS")
                {
                    return Json(new { status = "1" });
                }
                else
                {
                    return Json(new { status = "0" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "0", success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public Boolean SaveConsumerData(SolarConsumerModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SolarWarrantyService saveCustomerData = new SolarWarrantyService();
                    bool saveResult = saveCustomerData.SaveData(model);
                    if (saveResult)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpPost]
        public Boolean SaveDownloadHistoryData(DownloadHistoryModel model)
        {
            try
            {
       
                    DownloadHistoryService saveHistoryData = new DownloadHistoryService();
                    bool saveResult = saveHistoryData.SaveData(model);
                    if (saveResult)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
             
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        [HttpPost]
        public ActionResult VerifyOTP(SolarConsumerModel model)
        {
            bool checaptch = true;
            AdaniSolarOTPHistoryDataContext pgd = new AdaniSolarOTPHistoryDataContext();
            AdaniSolarOTPHistory entity = new AdaniSolarOTPHistory();

            string _captcha = string.Empty;

            if (Session["Captcha"] == null)
            {
                _captcha = "";
            }
            else
            {
                _captcha = Session["Captcha"].ToString();
            }


            if (_captcha != model.Response)
            {
                checaptch = IsReCaptchValid(model.Response);
            }

            if (checaptch)
            {
                Session["Captcha"] = model.Response;
                var currdate = DateTime.Now;
                var befortime = currdate.AddMinutes(-60);

                var data2 = pgd.AdaniSolarOTPHistories.Where(x => x.OTPFor == model.Mobile && x.CreatedDate >= befortime && x.CreatedDate <= currdate && x.attempt > 0).FirstOrDefault();
                if (data2 != null)
                {
                    if (data2.attempt > 3)
                    {
                        var data = new { status = "4" };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }

                string userOTP = Request.Form["userOTP"];
                if (!string.IsNullOrEmpty(userOTP))
                {
                    string otp = !string.IsNullOrEmpty(Session["OTP"].ToString()) ? Session["OTP"].ToString() : string.Empty;
                    if (otp == userOTP)
                    {
                        // OTP matches, so do something
                        var IsDataSaved = SaveConsumerData(model);
                        if (IsDataSaved)
                        {
                            SolarWarrantyService solarWarrantyService = new SolarWarrantyService();
                            ConsumerInvoice jsonroot = new ConsumerInvoice();
                            Consumer consumer = new Consumer();

                            consumer.Serial_No = Request.Form["Serial_No"];
                            jsonroot.Record = consumer;
                            var jsonobject = JsonConvert.SerializeObject(jsonroot);

                            var apiResponse = solarWarrantyService.GetCertificatedata(jsonobject);
                            if (!apiResponse.Contains("["))
                            {
                                SolarWarrantyResponseModelNew responsemodel = JsonConvert.DeserializeObject<SolarWarrantyResponseModelNew>(apiResponse);
                                if (responsemodel == null)
                                {
                                    var data = new { status = "2", message = "Data did not match, get in touch with support" };
                                    return Json(data, JsonRequestBehavior.AllowGet);
                                }
                                else if (responsemodel.Record != null)
                                {
                                    var data1 = pgd.AdaniSolarOTPHistories.Where(x => x.OTPFor == model.Mobile).ToList();
                                    data1.ForEach(a => a.attempt = 0);
                                    pgd.SubmitChanges();

                                    var data = new { status = "1", list = responsemodel.Record };
                                    return Json(data, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                SolarWarrantyResponseModel responsemodel = JsonConvert.DeserializeObject<SolarWarrantyResponseModel>(apiResponse);
                                if (responsemodel == null)
                                {
                                    var data = new { status = "2", message = "Data did not match, get in touch with support" };
                                    return Json(data, JsonRequestBehavior.AllowGet);
                                }
                                else if (responsemodel.Record != null)
                                {
                                    var data1 = pgd.AdaniSolarOTPHistories.Where(x => x.OTPFor == model.Mobile).ToList();
                                    data1.ForEach(a => a.attempt = 0);
                                    pgd.SubmitChanges();

                                    var data = new { status = "1", list = responsemodel.Record };
                                    return Json(data, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }

                    }
                    else
                    {

                        var data1 = pgd.AdaniSolarOTPHistories.Where(x => x.OTPFor == model.Mobile).FirstOrDefault();

                        if (data1.attempt == null)
                        {
                            data1.attempt = 0;
                        }
                        data1.attempt = data1.attempt + 1;
                        pgd.SubmitChanges();

                        var data = new { status = "3" };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                var data = new { status = "0" };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return View();
        }


        [HttpPost]
        public ActionResult VerifySerialNumber(SolarConsumerModel model)
        {
            try
            {
                SolarWarrantyService solarWarrantyService = new SolarWarrantyService();
                ConsumerInvoice jsonroot = new ConsumerInvoice();
                Consumer consumer = new Consumer();

                consumer.Serial_No = Request.Form["Serial_No"];
                jsonroot.Record = consumer;
                var jsonobject = JsonConvert.SerializeObject(jsonroot);

                var apiResponse = solarWarrantyService.GetCertificatedata(jsonobject);


                if (!apiResponse.Contains("["))
                {
                    SolarWarrantyResponseModelNew responsemodel = JsonConvert.DeserializeObject<SolarWarrantyResponseModelNew>(apiResponse);
                    if (responsemodel == null)
                    {
                        var data = new { status = "2", message = "Data did not match, get in touch with support" };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else if (responsemodel.Record != null)
                    {
                        var data = new { status = "1", list = responsemodel.Record };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else if (responsemodel.Record == null)
                    {
                        var data = new { status = "2", message = "Data did not match, get in touch with support" };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    SolarWarrantyResponseModel responsemodel = JsonConvert.DeserializeObject<SolarWarrantyResponseModel>(apiResponse);
                    if (responsemodel == null)
                    {
                        var data = new { status = "2", message = "Data did not match, get in touch with support" };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else if (responsemodel.Record != null)
                    {
                        var data = new { status = "1", list = responsemodel.Record };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else if (responsemodel.Record == null)
                    {
                        var data = new { status = "2", message = "Data did not match, get in touch with support" };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }

            return View();
        }


        [HttpPost]
        public ActionResult SendMailAskExpert(AdaniSolarContactModel m)
        {
            var response = m.Response;
            var result = new { status = "1" };
            if (IsReCaptchValid(m.Response))
            {
                string regx = @"^[^*|\"":<>[\]{}`\\()';=@&$]+$";
                string MobileRegex = @"^[0-9]{10,10}$";
                string Emailregx = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Sitecore.Diagnostics.Log.Info("Adani Solar CRM lead generation - SendMailAskExpert:" + m.Name + "," + m.emailMessage + "," + m.Mobile + "," + m.Message + "," + m.MessageType + "," + m.Category + "," + m.State + "," + m.ProjectCategory + "," + m.CountryCode + "," + m.Region, this);

                if (string.IsNullOrEmpty(m.Email) || string.IsNullOrWhiteSpace(m.Email) || !Regex.IsMatch(m.Email, Emailregx))
                {
                    result = new { status = "0" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(m.Name) || string.IsNullOrWhiteSpace(m.Name) || !Regex.IsMatch(m.Name, regx))
                {
                    result = new { status = "0" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(m.Region) || string.IsNullOrWhiteSpace(m.Region) || !Regex.IsMatch(m.Region, regx))
                {
                    result = new { status = "0" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(m.Category) || string.IsNullOrWhiteSpace(m.Category) || !Regex.IsMatch(m.Category, regx))
                {
                    result = new { status = "0" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(m.State) || string.IsNullOrWhiteSpace(m.State) || !Regex.IsMatch(m.State, regx))
                {
                    result = new { status = "0" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(m.Mobile) || string.IsNullOrWhiteSpace(m.Mobile) || !Regex.IsMatch(m.Mobile, MobileRegex))
                {
                    result = new { status = "0" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                //generate CRM lead
                bool isLeadGenerated = GenerateCRMLead(m);

                Sitecore.Diagnostics.Log.Info("Adani Solar CRM lead generation - SendMailAskExpert lead generated success:" + m.Name + "," + m.emailMessage + "," + m.Mobile + "," + m.Message + "," + m.MessageType + "," + m.Category + "," + m.State + "," + m.ProjectCategory + "," + m.CountryCode + "," + m.Region, this);

                //send email
                var itemData = Sitecore.Context.Database.GetItem(m.emailMessage);
                var dataItem = "{C1F06695-E331-471E-B528-181E85668AD9}";
                var getEmailToItem = Sitecore.Context.Database.GetItem(dataItem);
                var getEmailTo = getEmailToItem.Fields["Value"].ToString();
                var getEmailText = getEmailToItem.Fields["Text"].ToString();

                string getMessage = "";
                string getEmailSubject = "";
                string from = "";
                //result = new { status = "1" };
                try
                {
                    getMessage = itemData.Fields["Body"].Value;
                    getEmailSubject = itemData.Fields["Subject"].Value;
                    from = itemData.Fields["From"].Value;

                    getMessage = getMessage.Replace("#UserName", m.Name);
                    getMessage = getMessage.Replace("#Email", m.Email);
                    getMessage = getMessage.Replace("#ContactNumber", m.Mobile);
                    getMessage = getMessage.Replace("#Category", m.Category);
                    getMessage = getMessage.Replace("#State ", m.State);
                    getMessage = getMessage.Replace("#ProjectCategory ", m.ProjectCategory);
                    getMessage = getMessage.Replace("#Region", m.Region);
                    getMessage = getMessage.Replace("#Message", m.Message);
                    getMessage = getMessage.Replace("#PageType", m.PageInfo);
                    getMessage = getMessage.Replace("#FormType", m.FormType);
                    getMessage = getMessage.Replace("#FormSubmitOn", m.FormSubmitOn.ToString());

                    getMessage = "Hello " + getEmailText + ",<br><br>" + getMessage;

                    bool results = sendEmail(getEmailTo, getEmailSubject, getMessage, from);

                    if (results)
                    {
                        Log.Error("Email Sent to specific Message Subject type- ", "");
                    }

                }
                catch (Exception ex)
                {
                    result = new { status = "0" };
                    Log.Error("Failed to get email from subject", ex.ToString());
                }
            }
            else
            {
                result = new { status = "0" };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendMailContactUs(AdaniSolarContactModel m)
        {
            var response = m.Response;
            var result = new { status = "1" };
            if (IsReCaptchValid(response))
            {
                string regx = @"^[^*|\"":<>[\]{}`\\()';=@&$]+$";
                string MobileRegex = @"^[0-9]{10,10}$";
                string Emailregx = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Sitecore.Diagnostics.Log.Info("Adani Solar CRM lead generation - SendMailContactUs:" + m.Name + "," + m.emailMessage + "," + m.Mobile + "," + m.Message + "," + m.MessageType + "," + m.Category + "," + m.State + "," + m.ProjectCategory + "," + m.CountryCode + "," + m.Region, this);
                if (string.IsNullOrEmpty(m.Email) || string.IsNullOrWhiteSpace(m.Email) || !Regex.IsMatch(m.Email, Emailregx))
                {
                    result = new { status = "0" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(m.Name) || string.IsNullOrWhiteSpace(m.Name) || !Regex.IsMatch(m.Name, regx))
                {
                    result = new { status = "0" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(m.Region) || string.IsNullOrWhiteSpace(m.Region) || !Regex.IsMatch(m.Region, regx))
                {
                    result = new { status = "0" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(m.Category) || string.IsNullOrWhiteSpace(m.Category) || !Regex.IsMatch(m.Category, regx))
                {
                    result = new { status = "0" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(m.State) || string.IsNullOrWhiteSpace(m.State) || !Regex.IsMatch(m.State, regx))
                {
                    result = new { status = "0" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(m.Mobile) || string.IsNullOrWhiteSpace(m.Mobile) || !Regex.IsMatch(m.Mobile, MobileRegex))
                {
                    result = new { status = "0" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                //generate CRM lead
                bool isLeadGenerated = GenerateCRMLead(m);

                Log.Info("Adani Solar CRM lead generation - SendMailContactUs lead generated success:" + m.Name + "," + m.emailMessage + "," + m.Mobile + "," + m.Message + "," + m.MessageType + "," + m.Category + "," + m.State + "," + m.ProjectCategory + "," + m.CountryCode + "," + m.Region, this);

                //send email
                var itemData = Sitecore.Context.Database.GetItem(m.emailMessage);
                var dataItem = "{C1F06695-E331-471E-B528-181E85668AD9}";
                var getEmailToItem = Sitecore.Context.Database.GetItem(dataItem);
                var getEmailTo = getEmailToItem.Fields["Value"].ToString();
                var getEmailText = getEmailToItem.Fields["Text"].ToString();
                string getMessage = "";
                string getEmailSubject = "";
                string from = "";
                try
                {

                    getMessage = itemData.Fields["Body"].Value;
                    getEmailSubject = itemData.Fields["Subject"].Value;
                    from = itemData.Fields["From"].Value;

                    getMessage = getMessage.Replace("#UserName", m.Name);
                    getMessage = getMessage.Replace("#Email", m.Email);
                    getMessage = getMessage.Replace("#ContactNumber", m.Mobile);
                    getMessage = getMessage.Replace("#PageType", m.PageInfo);
                    getMessage = getMessage.Replace("#CountryCode ", m.CountryCode);
                    getMessage = getMessage.Replace("#State ", m.State);
                    getMessage = getMessage.Replace("#ProjectCategory ", m.ProjectCategory);
                    getMessage = getMessage.Replace("#Category", m.Category);
                    getMessage = getMessage.Replace("#Region", m.Region);
                    getMessage = getMessage.Replace("#Message", m.Message);
                    getMessage = getMessage.Replace("#FormType", m.FormType);
                    getMessage = getMessage.Replace("#FormSubmitOn", m.FormSubmitOn.ToString());

                    getMessage = getMessage.Replace("#SubjectOfMessageText", m.SubjectOfMessageText);
                    getMessage = getMessage.Replace("#TextCategory", m.CategoryText);
                    getMessage = getMessage.Replace("#StateText", m.StateText);

                    getMessage = "Hello " + getEmailText + ",<br><br>" + getMessage;

                    bool results = sendEmail(getEmailTo, getEmailSubject, getMessage, from);
                    if (results)
                    {
                        Log.Error("Email Sent to specific Message Subject type- ", "");
                    }

                }
                catch (Exception ex)
                {
                    result = new { status = "0" };
                    Log.Error("Failed to get email from subject", ex.ToString());
                }
            }
            else
            {
                result = new { status = "0" };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendMailSubscribe(AdaniSolarContactModel m)
        {
            var itemData = Sitecore.Context.Database.GetItem(m.emailMessage);
            var dataItem = "{C1F06695-E331-471E-B528-181E85668AD9}";
            var getEmailToItem = Sitecore.Context.Database.GetItem(dataItem);
            var getEmailTo = getEmailToItem.Fields["Value"].ToString();
            var getEmailText = getEmailToItem.Fields["Text"].ToString();

            string getMessage = "";
            string getEmailSubject = "";
            string from = "";
            var result = new { status = "1" };
            try
            {
                getMessage = itemData.Fields["Body"].Value;
                getEmailSubject = itemData.Fields["Subject"].Value;
                from = itemData.Fields["From"].Value;

                getMessage = getMessage.Replace("#Email", m.Email);
                getMessage = getMessage.Replace("#PageType", m.PageInfo);
                getMessage = getMessage.Replace("#FormType", m.FormType);
                getMessage = getMessage.Replace("#FormSubmitOn", m.FormSubmitOn.ToString());

                getMessage = "Hello " + getEmailText + ",<br><br>" + getMessage;

                bool results = sendEmail(getEmailTo, getEmailSubject, getMessage, from);

                if (results)
                {
                    Log.Error("Email Sent to specific Message Subject type- ", "");
                }

            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Log.Error("Failed to get email from subject", ex.ToString());
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public bool sendEmail(string to, string subject, string body, string from)
        {
            bool status = false;
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.From = new MailAddress(from);
                MainUtil.SendMail(mail);
                status = true;
                return status;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message, "sendEmail - ");
                Log.Error(ex.Message, "sendEmail - ");
                Log.Error(ex.InnerException.ToString(), "sendEmail - ");
                return status;
            }
        }

        public bool IsReCaptchValid(string responseData)
        {
            var result = false;
            var captchaResponse = responseData;
            var secretKey = DictionaryPhraseRepository.Current.Get("/AdaniSolarCaptachaKey/SecretKey", "6Le7Ma0UAAAAAIvvn5DelsF7ptv6jnsWQj9vmFCv"); //"6Le7Ma0UAAAAAIvvn5DelsF7ptv6jnsWQj9vmFCv";
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    result = (isSuccess) ? true : false;
                }
            }
            return result;
        }

        public ActionResult GetRegion_Territories()
        {
            try
            {
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");

                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.SalesForceCRMSettings.Id.ToString()));
                var client_id = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.ConsumerKey].Value;// "be41119b-9aff-4103-a89c-0b6f9920029a";
                var client_secret = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.ConsumerSecret].Value;// "2hV[enYR-u?P67UVa[5c5GFQ3m/U0uM.";

                var IsSandboxUser = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.IsSandboxUser].Value.ToLower();
                var TokenUrl = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.TokenURL].Value; //"https://login.microsoftonline.com/04c72f56-1848-46a2-8167-8e5d36510cbc/oauth2/token";

                string resourceSandbox = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.Insta_SfdcSandboxEndPoint].Value; //"https://adanisolardev.crm8.dynamics.com/";
                string resourceProd = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.Insta_SfdcProductionEndPoint].Value; //"https://adanisolardev.crm8.dynamics.com/";

                var resource = IsSandboxUser.Equals("true", StringComparison.CurrentCultureIgnoreCase)               //string resource = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.Insta_SfdcProductionEndPoint].Value; //"https://adanisolardev.crm8.dynamics.com/";
                           ? resourceSandbox           //"https://test.salesforce.com/services/oauth2/token"
                           : resourceProd;       //https://login.salesforce.com/services/oauth2/token";


                var data = $"grant_type=client_credentials&client_id={client_id}&resource={resource}&client_secret={client_secret}";
                var postArray = Encoding.ASCII.GetBytes(data);
                var req = (HttpWebRequest)WebRequest.Create(TokenUrl);
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = postArray.Length;
                req.Accept = "application/json";
                req.Method = "POST";
                var reqStream = req.GetRequestStream();
                reqStream.Write(postArray, 0, postArray.Length);
                reqStream.Close();

                var resp = req.GetResponse();
                var responseStream = resp.GetResponseStream();
                if (responseStream == null) throw new Exception("Response stream is null");
                var streamReader = new StreamReader(responseStream);
                var responseText = streamReader.ReadToEnd();
                var responseObj = JsonConvert.DeserializeObject<dynamic>(responseText);
                if (responseObj.error != null) throw new Exception(responseObj.error_description);
                var Token = responseObj.access_token;
                var EndPoint = responseObj.instance_url;
                var request = (HttpWebRequest)WebRequest.Create(resource + "api/data/v9.0/territories?$select=name");
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Bearer " + Token);
                request.Accept = "application/json";
                request.Method = "GET";
                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var responseObj1 = JsonConvert.DeserializeObject<dynamic>(responseString);
                if (responseObj1.error != null) throw new Exception(responseObj1.error_description);
                JArray array = JArray.Parse(responseObj1.value.ToString());
                List<Territories> result = array.ToObject<List<Territories>>();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error("Adani Solar CRM lead generation - get countries error:" + e.InnerException.Message + "," + e.Message, this);
            }
            return null;
        }

        public ActionResult GetRegion_Countries()
        {
            try
            {
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");

                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.SalesForceCRMSettings.Id.ToString()));
                var client_id = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.ConsumerKey].Value;// "be41119b-9aff-4103-a89c-0b6f9920029a";
                var client_secret = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.ConsumerSecret].Value;// "2hV[enYR-u?P67UVa[5c5GFQ3m/U0uM.";

                var IsSandboxUser = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.IsSandboxUser].Value.ToLower();
                var TokenUrl = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.TokenURL].Value; //"https://login.microsoftonline.com/04c72f56-1848-46a2-8167-8e5d36510cbc/oauth2/token";

                string resourceSandbox = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.Insta_SfdcSandboxEndPoint].Value; //"https://adanisolardev.crm8.dynamics.com/";
                string resourceProd = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.Insta_SfdcProductionEndPoint].Value; //"https://adanisolardev.crm8.dynamics.com/";

                var resource = IsSandboxUser.Equals("true", StringComparison.CurrentCultureIgnoreCase)               //string resource = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.Insta_SfdcProductionEndPoint].Value; //"https://adanisolardev.crm8.dynamics.com/";
                           ? resourceSandbox           //"https://test.salesforce.com/services/oauth2/token"
                           : resourceProd;       //https://login.salesforce.com/services/oauth2/token";


                var data = $"grant_type=client_credentials&client_id={client_id}&resource={resource}&client_secret={client_secret}";
                var postArray = Encoding.ASCII.GetBytes(data);
                var req = (HttpWebRequest)WebRequest.Create(TokenUrl);
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = postArray.Length;
                req.Accept = "application/json";
                req.Method = "POST";
                var reqStream = req.GetRequestStream();
                reqStream.Write(postArray, 0, postArray.Length);
                reqStream.Close();

                var resp = req.GetResponse();
                var responseStream = resp.GetResponseStream();
                if (responseStream == null) throw new Exception("Response stream is null");
                var streamReader = new StreamReader(responseStream);
                var responseText = streamReader.ReadToEnd();
                var responseObj = JsonConvert.DeserializeObject<dynamic>(responseText);
                if (responseObj.error != null) throw new Exception(responseObj.error_description);
                var Token = responseObj.access_token;
                var EndPoint = responseObj.instance_url;

                var requestc = (HttpWebRequest)WebRequest.Create(resource + "api/data/v9.0/ispl_countries?$select=ispl_name");
                requestc.ContentType = "application/json";
                requestc.Headers.Add("Authorization", "Bearer " + Token);
                requestc.Accept = "application/json";
                requestc.Method = "GET";
                var responsec = (HttpWebResponse)requestc.GetResponse();

                var responseString1 = new StreamReader(responsec.GetResponseStream()).ReadToEnd();
                var responseObj11 = JsonConvert.DeserializeObject<dynamic>(responseString1);
                if (responseObj11.error != null) throw new Exception(responseObj11.error_description);
                JArray arracy = JArray.Parse(responseObj11.value.ToString());
                List<Countries> resultc = arracy.ToObject<List<Countries>>();
                return Json(resultc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error("Adani Solar CRM lead generation - get countries error:" + e.InnerException.Message + "," + e.Message, this);
            }
            return null;
        }

        public bool GenerateCRMLead(AdaniSolarContactModel m)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("Adani Solar CRM lead generation - generate lead start:" + m.Name + "," + m.Email + "," + m.Mobile + "," + m.Message + "," + m.MessageType + "," + m.Category + "," + m.CountryCode + "," + m.Region, this);
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");

                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.SalesForceCRMSettings.Id.ToString()));
                var client_id = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.ConsumerKey].Value;// "be41119b-9aff-4103-a89c-0b6f9920029a";
                var client_secret = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.ConsumerSecret].Value;// "2hV[enYR-u?P67UVa[5c5GFQ3m/U0uM.";

                var IsSandboxUser = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.IsSandboxUser].Value.ToLower();
                var TokenUrl = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.TokenURL].Value; //"https://login.microsoftonline.com/04c72f56-1848-46a2-8167-8e5d36510cbc/oauth2/token";

                string resourceSandbox = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.Insta_SfdcSandboxEndPoint].Value; //"https://adanisolardev.crm8.dynamics.com/";
                string resourceProd = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.Insta_SfdcProductionEndPoint].Value; //"https://adanisolardev.crm8.dynamics.com/";

                var resource = IsSandboxUser.Equals("true", StringComparison.CurrentCultureIgnoreCase)               //string resource = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.Insta_SfdcProductionEndPoint].Value; //"https://adanisolardev.crm8.dynamics.com/";
                           ? resourceSandbox           //"https://test.salesforce.com/services/oauth2/token"
                           : resourceProd;       //https://login.salesforce.com/services/oauth2/token";


                var data = $"grant_type=client_credentials&client_id={client_id}&resource={resource}&client_secret={client_secret}";

                var postArray = Encoding.ASCII.GetBytes(data);
                var req = (HttpWebRequest)WebRequest.Create(TokenUrl);
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = postArray.Length;
                req.Accept = "application/json";
                req.Method = "POST";
                var reqStream = req.GetRequestStream();
                reqStream.Write(postArray, 0, postArray.Length);
                reqStream.Close();

                var resp = req.GetResponse();
                var responseStream = resp.GetResponseStream();
                if (responseStream == null) throw new Exception("Response stream is null");
                var streamReader = new StreamReader(responseStream);
                var responseText = streamReader.ReadToEnd();
                var responseObj = JsonConvert.DeserializeObject<dynamic>(responseText);
                if (responseObj.error != null) throw new Exception(responseObj.error_description);
                var Token = responseObj.access_token;
                var EndPoint = responseObj.instance_url;
                Sitecore.Diagnostics.Log.Info("Adani Solar CRM lead generation - generate lead token:" + Token + "," + m.Name + "," + m.Email + "," + m.Mobile + "," + m.Message + "," + m.MessageType + "," + m.Category + "," + m.CountryCode + "," + m.State + "," + m.ProjectCategory + "," + m.Region, this);

                CRMObject t = new CRMObject();
                t.emailaddress1 = m.Email;
                t.firstname = m.Name;
                t.ispl_category = m.Category;
                t.ispl_websitecountry = m.CountryCode;
                t.ispl_websitemodulecategory = m.ProjectCategory;
                t.ispl_websitestate = m.State;
                t.ispl_websiteregion = m.Region;
                t.leadsourcecode = "8";
                t.mobilephone = m.Mobile;
                t.subject = m.topic;
                t.description = m.Message;

                var req1 = (HttpWebRequest)WebRequest.Create(resource + "api/data/v9.0/leads");
                req1.ContentType = "application/json";
                req1.Headers.Add("Authorization", "Bearer " + Token);
                req1.Accept = "application/json";
                req1.Method = "POST";

                Sitecore.Diagnostics.Log.Info("Adani Solar CRM lead generation - generate lead start after getting token:" + m.Name + "," + m.Email + "," + m.Mobile + "," + m.Message + "," + m.MessageType + "," + m.Category + "," + m.CountryCode + "," + m.Region, this);

                using (var streamWriter = new StreamWriter(req1.GetRequestStream()))
                {
                    string jsonStr = JsonConvert.SerializeObject(t);
                    streamWriter.Write(jsonStr);
                }

                var httpResponse1 = (HttpWebResponse)req1.GetResponse();
                using (var streamReader1 = new StreamReader(httpResponse1.GetResponseStream()))
                {
                    string result1 = streamReader1.ReadToEnd();
                    Sitecore.Diagnostics.Log.Info("Adani Solar CRM lead generation - generate lead success result:" + result1, this);
                }
                return true;
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error("Adani Solar CRM lead generation - generate lead error:" + e.InnerException.Message + "," + e.Message + "," + m.Name + "," + m.emailMessage + "," + m.Mobile + "," + m.Message + "," + m.MessageType + "," + m.Category + "," + m.CountryCode + "," + m.Region, this);
                return false;
            }
        }
    }
}
