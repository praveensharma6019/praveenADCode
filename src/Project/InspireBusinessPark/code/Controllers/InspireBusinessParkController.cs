using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Sitecore.InspireBusinessPark.Website.Models;

namespace Sitecore.InspireBusinessPark.Website.Controllers
{
    public class InspireBusinessParkController : Controller
    {

        // GET: InspireBusinessPark
        public ActionResult Index()
        {
            return View();
        }

        public bool IsReCaptchValid(string reResponse)
        {
            bool flag = false;
            string str = reResponse;
            //string str1 = DictionaryPhraseRepository.Current.Get("/CaptachaKey/SecretKey", "6Lcql9QZAAAAAOSIsZ-9gNRiZaVPrDmSrbKSe3y2");
            string str2 = string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", "6Ld6YqIgAAAAAOds8DbPikMFP9xYrzM1hRyPVCYu", str);
            using (WebResponse response = ((HttpWebRequest)WebRequest.Create(str2)).GetResponse())
            {
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    flag = (JObject.Parse(streamReader.ReadToEnd()).Value<bool>("success") ? true : false);
                }
            }
            return flag;
        }

        public  bool IsReCaptchValidV3(string reResponse)
        {
            HttpClient httpClient = new HttpClient();

            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret=6Ld6YqIgAAAAAOds8DbPikMFP9xYrzM1hRyPVCYu&response={reResponse}").Result;

            if (res.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);

            if (JSONdata.success != "true" || JSONdata.score <= 0.5)
            {
                return false;
            }

            return true;
        }
        [HttpPost]
        public ActionResult Insertcontactdetail(Inspire m)
        {
            var result = new { status = "1" };
            try
            {
                var flag = this.IsReCaptchValidV3(m.reResponse);
                if (flag == false)
                {
                    result = new { status = "2" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                InspireBPFormDataContext rdb = new InspireBPFormDataContext();
                InspireBPFormData r = new InspireBPFormData();

                r.FirstName = m.first_name;
                r.LastName = m.last_name;
                r.Mobile = m.mobile;
                r.Email = m.email;
                r.City = m.City;
                r.Lead_Source = m.LeadSource;
                r.PropertyType = "INSPIRE BUSINESS PARK";
                r.UTMSource = m.UTMSource;
                r.FormType = m.FormType;
                r.PageInfo = m.PageInfo;
                r.PropertyType = "INSPIRE BUSINESS PARK";
                r.Lead_Source = "Web To Lead";
                r.FormSubmitOn = DateTime.Now;// m.FormSubmitOn;
               



                Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Start first_name:" + m.first_name, this);
                Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB last_name:" + m.last_name, this);
                Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Mobile: " + m.mobile, this);
                Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB email: " + m.email, this);
               
                Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB PageInfo: " + m.PageInfo, this);

                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation starting", this);
                SalesForceWrapper SalesForceWrapperObj = new SalesForceWrapper();
                //Get project details
                Sitecore.Diagnostics.Log.Info("SalesForceWrapperObj created with token", this);
                LeadObject obj = new LeadObject
                {
                    Firstname = m.first_name,
                    FormType = m.FormType,
                    PageInfo = m.PageInfo,
                    LastName = m.last_name,
                    Email = m.email,
                    Mobile = m.mobile,
                    LeadSource = "Web To Lead",
                    RecordType = "0122v000001uO6j",
                    MasterProjectID = "a4F2v000000IEFN",
                    Projectintrested = "Commercial",
                    Project = "INSPIRE BUSINESS PARK",
                    AssignmentCity =  "Ahmedabad"
                };

                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead", this);
                var leadResult = SalesForceWrapperObj.GenerateLead(obj);
                r.Comment = leadResult;
                #region Insert to DB
                rdb.InspireBPFormDatas.InsertOnSubmit(r);

                rdb.SubmitChanges();
            }
            catch (Exception ex)
            {
                result = new { status = "fail" };
                Console.Write(ex);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}