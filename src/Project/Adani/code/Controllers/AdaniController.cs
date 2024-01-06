using RestSharp;
using Sitecore.Adani.Website.Models;
using Sitecore.Adani.Website.Providers;
using Sitecore.Adani.Website.Services;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Adani.Website.Controllers
{
    public class AdaniController : Controller
    {

        private AdaniRepository adanirepo = new AdaniRepository();
        public AdaniController()
        {

        }

        [HttpPost]
        public ActionResult CreateOTP(AdaniContactModal model)
        {
            ActionResult actionResult;
            EnquiryDataContext enquiryDataContext = new EnquiryDataContext();
            var variable = new { status = "0" };
            try
            {
                this.adanirepo.DeleteOldOtp(model.Mobile);
                string str = this.adanirepo.StoreGeneratedOtp(model);
                try
                {
                    string str1 = string.Format("https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=adtrotpi&subEnterpriseid=adtrotpi&pusheid=adtrotpi&pushepwd=adtrotpi22&msisdn={0}&sender=ADANIR&msgtext=Dear%20customer,%20please%20enter%20the%20verification%20code%20{1}%20to%20submit%20your%20web%20enquiry.", model.Mobile, str);
                    HttpClient httpClient = new HttpClient()
                    {
                        BaseAddress = new Uri(str1)
                    };
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (!httpClient.GetAsync(str1).Result.IsSuccessStatusCode)
                    {
                        Log.Error("OTP Api call failed. https://otp2.maccesssmspush.com", this);
                    }
                    else
                    {
                        Log.Error("OTP Api call success. https://otp2.maccesssmspush.com", this);
                    }
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    Log.Error(string.Format("{0}", 0), exception, this);
                }
                variable = new { status = "1" };
                actionResult = base.Json(variable, 0);
            }
            catch (Exception exception3)
            {
                Exception exception2 = exception3;
                Log.Error(string.Format("{0}", 0), exception2, this);
                actionResult = base.Json(variable, 0);
            }
            return actionResult;
        }

        public ActionResult Index()
        {
            return base.View();
        }

        [HttpPost]
        public ActionResult InsertContactdetail(AdaniContactModal m)
        {
            bool Validated = false;
            Log.Error("Insertcontactdetail ", "Start");
            var result = new
            {
                status = "1"
            };
            try
            {
                Recaptchav2Provider recaptchav2 = new Recaptchav2Provider();
                Validated = recaptchav2.IsReCaptchValid(m.reResponse);

            }
            catch (Exception ex)
            {
                Exception exception = ex;
                Log.Info(string.Concat("ContactUsForm Failed to validate auto script : ", ex.ToString()), this);
                result = new { status = "0" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            if (Validated)
            {
                Log.Info("\n-- Captcha Validation started", this);
                try
                {
                    AdaniContactFormRecordDataContext rdb = new AdaniContactFormRecordDataContext();
                    AdaniContactFormRecord r = new AdaniContactFormRecord();
                    r.Name = m.Name;
                    r.Email = m.Email;
                    r.Mobile = m.Mobile;
                    r.SubjectType = m.SubjectType;
                    r.Message = m.Message;
                    r.FormType = m.FormType;
                    r.PageInfo = m.PageInfo;
                    r.FormSubmitOn = m.FormSubmitOn;
                    rdb.AdaniContactFormRecords.InsertOnSubmit(r);
                    rdb.SubmitChanges();

                }
                catch (Exception ex2)
                {
                    result = new
                    {
                        status = "0"
                    };
                    Log.Error("\n-- Failed to submit in DB", ex2.ToString());
                    Console.WriteLine(ex2);
                }
                try
                {
                    string to = DictionaryPhraseRepository.Current.Get("/ContactUs/EmailTo", "");
                    string from = DictionaryPhraseRepository.Current.Get("/ContactUs/EmailFrom", "");
                    string emailSubject = DictionaryPhraseRepository.Current.Get("/ContactUs/EmailSubject", "");
                    string emailText = DictionaryPhraseRepository.Current.Get("/ContactUs/EmailText", "");
                    string message = "";
                    message = "Hello<br><br>" + emailText + "<br><br>Name: " + m.Name;
                    message = message + "<br>Email: " + m.Email + "<br>Contact Number: " + m.Mobile + "<br>Message Subject Type: " + m.SubjectType + "<br>Message: " + m.Message + "<br><br>Thanks";
                    EmailServicesProvider emailServicesProvider = new EmailServicesProvider();
                    if (emailServicesProvider.sendEmail(to, emailSubject, message, from))
                    {
                        Log.Info("Email Sent- ", "");
                    }
                }
                catch (Exception ex)
                {
                    result = new
                    {
                        status = "2",
                };
                //return Json(new
                //{
                //    status = "2",
                //    message = ex.Message
                //}, 0);
                Log.Error("Failed to sent Email", ex.ToString());
                }
            }
            else
            {
                result = new { status = "3" };
                Log.Info("\n-- Failed to validate captcha", this);
                Console.WriteLine("Captcha Validation Failed!!!!");
            }

            return base.Json(result, 0);
        }


        [HttpPost]
        public ActionResult VerifyOTP(AdaniContactModal model)
        {
            var variable = new { status = "0" };
            if (string.Equals(this.adanirepo.GetOTP(model.Mobile), model.OTP))
            {
                variable = new { status = "1" };
            }
            return base.Json(variable, 0);
        }

        //[HttpPost]
        //public ActionResult MarketCap()
        //{
        //   var usd = "https://www.google.com/search?q=1+usd+in+inr&rlz=1C1CHBF_enIN927IN927&oq=1+usd+in";

        //    var AEL = "https://www.google.com/search?q=adani+enterprises+share&rlz=1C1CHBF_enIN927IN927&oq=adani+enter&aqs=chrome.0.0i131i433l3j69i57j0i131i433l6.7820j1j7&sourceid=chrome&ie=UTF-8";
        //    var ATL = "https://www.google.com/search?q=adani+transmission+share&rlz=1C1CHBF_enIN927IN927&sxsrf=ALeKk02plNgZIM3E0t83-8-Gtnx-KtDNwg%3A1616742337078&ei=wYddYIibBM2F4-EP5p6z-AE&oq=adani+tr&gs_lcp=Cgdnd3Mtd2l6EAEYADIICAAQsQMQgwEyCAgAELEDEIMBMggIABCxAxCDATICCAA6BwgAEEcQsAM6BwgjEOoCECc6BAgjECc6BAgAEEM6BwgjECcQnQI6CggAELEDEIMBEEM6DAgjECcQnQIQRhD6AVDHB1iCImC8MWgBcAN4AoABtwWIAcwckgELMC41LjYuMS4xLjGYAQCgAQGqAQdnd3Mtd2l6sAEKyAEIwAEB&sclient=gws-wiz";
        //    var APL = "https://www.google.com/search?q=adani+power+share+price&rlz=1C1CHBF_enIN927IN927&oq=adani+poe&aqs=chrome.1.69i57j0i10i131i433j0i10j0i10i131i433l2j0i10j0i10i131i433l4.5879j1j7&sourceid=chrome&ie=UTF-8";
        //    var AGEL = "https://www.google.com/search?q=adani+green+energy+share+price&rlz=1C1CHBF_enIN927IN927&sxsrf=ALeKk03Z7ctF6GsclWiIdwwu7_Awv0xMhA%3A1616742377438&ei=6YddYKesGtKa4-EPrKyJsAI&oq=adani+green+e&gs_lcp=Cgdnd3Mtd2l6EAMYATINCAAQhwIQsQMQgwEQFDINCAAQhwIQsQMQgwEQFDILCAAQsQMQgwEQkQIyBQgAEJECOgcIABBHELADOgcIABCwAxBDOgcIIxAnEJ0COgQIIxAnOgQIABBDOgoIABCxAxCDARBDOgwIIxAnEJ0CEEYQ-gE6BQgAELEDOggIABCxAxCDAToHCAAQAhDLAToSCAAQhwIQsQMQgwEQFBBGEPoBUPOvAVjkygFg5NwBaAFwAngBgAH0A4gB0xWSAQowLjExLjQtMS4xmAEAoAEBqgEHZ3dzLXdpesgBCcABAQ&sclient=gws-wiz";
        //    var APSEZ = "https://www.google.com/search?q=Adani+Ports+and+Special+Economic+Zone+Ld+share+price&rlz=1C1CHBF_enIN927IN927&sxsrf=ALeKk03xUsMDOVBHtxBVv0EQy-nanxLF6w%3A1616755767592&ei=N7xdYJzXI5Od4-EP8ZuHyA8&oq=Adani+Ports+and+Special+Economic+Zone+Ld+share+price&gs_lcp=Cgdnd3Mtd2l6EAMyAggAOgcIABCwAxBDOgYIABAHEB46CAgAEAcQChAeOggIABAHEAUQHjoHCAAQsQMQQzoFCAAQkQI6CwgAELEDEIMBEJECOggIABCxAxCDAToNCAAQhwIQsQMQgwEQFFCdNljlP2DiRGgCcAJ4AIAB0gGIAZQJkgEFMC42LjGYAQCgAQGgAQKqAQdnd3Mtd2l6yAEKwAEB&sclient=gws-wiz&ved=0ahUKEwjcztzJ5M3vAhWTzjgGHfHNAfkQ4dUDCA0&uact=5";
        //    var ATGL = "https://www.google.com/search?q=adani+total+share+price&rlz=1C1CHBF_enIN927IN927&sxsrf=ALeKk00mCtuB-QZghsS7uCMkZ2RsSHjUIw%3A1616742053559&ei=pYZdYJ_NIdGd4-EPz9Gk0Ag&oq=adani+total+share+price&gs_lcp=Cgdnd3Mtd2l6EAMyAggAMgIIADIGCAAQFhAeMgYIABAWEB4yBggAEBYQHjIGCAAQFhAeMgYIABAWEB46BwgAEEcQsANQkk9YildguVloAXACeACAAcsBiAHXCZIBBTAuNi4xmAEAoAEBqgEHZ3dzLXdpesgBCMABAQ&sclient=gws-wiz&ved=0ahUKEwjfs66-sc3vAhXRzjgGHc8oCYoQ4dUDCA0&uact=5";

        //    return 0;
        //}

        [HttpGet]
        public ActionResult AdaniMarketCapital()
        {
            Log.Info("Adani.com market Cap start ", this);
            AdaniMarketCapListModel CapList = new AdaniMarketCapListModel();
            try
            {
                var datasource = Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering.DataSource;
                var CompanyList = Sitecore.Context.Database.GetItem(datasource);
                if (CompanyList != null)
                {
                    if (CompanyList.HasChildren)
                    {
                        foreach (Item item in CompanyList.GetChildren())
                        {
                            var client = new RestClient(DictionaryPhraseRepository.Current.Get("/MarketCap/PriceAPI-URL", "https://www.advaiya.com/mkt/mktCapApi.php?url=") + item.Fields["Key"].Value);
                            client.Timeout = -1;
                            var request = new RestRequest(Method.GET);
                            IRestResponse response = client.Execute(request);
                            Newtonsoft.Json.Linq.JObject jResponse = Newtonsoft.Json.Linq.JObject.Parse(response.Content);
                            AdaniMarketCapModel model = new AdaniMarketCapModel();
                            model.MarketCapID = item.Fields["Key"].Value;
                            model.MarketCapValue = jResponse.Value<string>("mktcap");
                            CapList.MarketList.Add(model);
                        }
                        foreach (Item item in CompanyList.GetChildren())
                        {
                            var client = new RestClient(DictionaryPhraseRepository.Current.Get("/MarketCap/USDAPI-URL", "https://www.advaiya.com/mkt/getUsd.php?url=") + item.Fields["Key"].Value);
                            client.Timeout = -1;
                            var request = new RestRequest(Method.GET);
                            IRestResponse response = client.Execute(request);
                            Newtonsoft.Json.Linq.JObject jResponse = Newtonsoft.Json.Linq.JObject.Parse(response.Content);
                            AdaniMarketCapModel model = new AdaniMarketCapModel();
                            model.MarketCapID = item.Fields["Key"].Value + "-USD";
                            model.MarketCapValue = jResponse.Value<string>("USD");
                            CapList.MarketList.Add(model);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Info("AEL Market Cap cannot be fetched :" + e.Message, this);
            }
            return View(CapList);
        }

    }
}